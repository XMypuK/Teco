#region License
// Copyright (c) 2019 Zeiler Evgenii, https://github.com/XMypuK
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Teco.Generating;
using Teco.Parsing;

namespace Teco {
    public class CompilerBase {
        protected IParser Parser { get; private set; }
        protected CompilerOptions Options { get; private set; }

        public CompilerBase(IParser parser, CompilerOptions options) {
            if (parser == null)
                throw new ArgumentNullException("parser");

            if (options == null)
                throw new ArgumentNullException("options");

            Parser = parser;
            Options = options;
        }

        protected void Compile(String template, Generator generator) {
            generator.GenProlog();
            CompilerScope scope = new CompilerScope(null, CompilerScopeType.Root);
            IEnumerator<Token> tokenEnumerator = Parser.GetTokenEnumerator(template);
            while (tokenEnumerator.MoveNext()) {
                Token token = tokenEnumerator.Current;
                ValidateScope(scope, token);
                ProcessToken(generator, token);
                scope = GetAfterScope(scope, token);
            }
            EnsureScopeIs(scope, CompilerScopeType.Root, null);
            generator.GenEpilog();
        }

        private void ValidateScope(CompilerScope scope, Token token) {
            switch (token.Type) {
                case TokenType.Open:
                    switch (token.Keyword) {
                        case TokenKeyword.Each:
                        case TokenKeyword.Html:
                        case TokenKeyword.If:
                        case TokenKeyword.IfNot:
                        case TokenKeyword.Text:
                        case TokenKeyword.Url:
                        case TokenKeyword.When: EnsureScopeIsNot(scope, CompilerScopeType.When, token); break;
                        case TokenKeyword.Begins:
                        case TokenKeyword.Contains:
                        case TokenKeyword.Ends:
                        case TokenKeyword.Eq: EnsureScopeIs(scope, CompilerScopeType.When, token); break;
                        default: throw new SyntaxException(String.Format("Unexpected opening keyword '{0}' at pos {1}.", token.Keyword, token.Position));
                    }
                    break;

                case TokenType.Close:
                    switch (token.Keyword) {
                        case TokenKeyword.Begins: EnsureScopeIs(scope, CompilerScopeType.Begins, token); break;
                        case TokenKeyword.Contains: EnsureScopeIs(scope, CompilerScopeType.Contains, token); break;
                        case TokenKeyword.Each: EnsureScopeIs(scope, CompilerScopeType.Each, token); break;
                        case TokenKeyword.Else: EnsureScopeIs(scope, CompilerScopeType.Else, token); break;
                        case TokenKeyword.Ends: EnsureScopeIs(scope, CompilerScopeType.Ends, token); break;
                        case TokenKeyword.Eq: EnsureScopeIs(scope, CompilerScopeType.Eq, token); break;
                        case TokenKeyword.If: EnsureScopeIs(scope, CompilerScopeType.If, token); break;
                        case TokenKeyword.IfNot: EnsureScopeIs(scope, CompilerScopeType.IfNot, token); break;
                        case TokenKeyword.When: EnsureScopeIs(scope, CompilerScopeType.When, token); break;
                        default: throw new SyntaxException(String.Format("Unexpected closing keyword '{0}' at pos {1}.", token.Keyword, token.Position));
                    }
                    break;

                case TokenType.Raw:
                    if (scope.Type == CompilerScopeType.When && !Regex.IsMatch(token.Expression, @"^\s*$"))
                        throw new SyntaxException(String.Format("Unexpected character sequence '{0}' at pos {1}.", token.Expression, token.Position));
                    break;

                default: throw new SyntaxException(String.Format("Unexpected token type '{0}' at pos {1}.", token.Type, token.Position));

            }
        }

        private void EnsureScopeIs(CompilerScope scope, CompilerScopeType type, Token token) {
            if (scope == null || scope.Type != type) {
                String scopeTypeName = scope != null ? scope.Type.ToString() : "null";
                if (token != null && token.Type == TokenType.Open)
                    throw new SyntaxException(String.Format("Opening keyword '{0}' is not expected within a scope '{1}' at {2}. Expected scope is '{3}'.", token.Keyword, scopeTypeName, token.Position, type));
                if (token != null && token.Type == TokenType.Close)
                    throw new SyntaxException(String.Format("Closing keyword '{0}' is not expected within a scope '{1}' at {2}. Expected scope is '{3}'.", token.Keyword, scopeTypeName, token.Position, type));

                String tokenPosition = token != null ? token.Position.ToString() : "unknown";
                throw new SyntaxException(String.Format("Scope '{0}' is expected but '{1}' has found at pos {2}.", type, scopeTypeName, tokenPosition));
            }
        }

        private void EnsureScopeIsNot(CompilerScope scope, CompilerScopeType type, Token token) {
            if (scope != null && scope.Type == type) {
                if (token != null && token.Type == TokenType.Open) 
                    throw new SyntaxException(String.Format("Opening keyword '{0}' is not expected within a scope '{1}' at {2}.", token.Keyword, scope.Type, token.Position));
                if (token != null && token.Type == TokenType.Close)
                    throw new SyntaxException(String.Format("Closing keyword '{0}' is not expected within a scope '{1}' at {2}.", token.Keyword, scope.Type, token.Position));

                String tokenPosition = token != null ? token.Position.ToString() : "unknown";
                throw new SyntaxException(String.Format("Scope '{0}' is not expected at pos {1}.", scope.Type, tokenPosition));
            }
        }

        private void ProcessToken(Generator generator, Token token) {
            switch (token.Type) {
                case TokenType.Raw: generator.GenRaw(token.Expression); break;
                case TokenType.Open:
                    switch (token.Keyword) {
                        case TokenKeyword.If: generator.GenOpenIfScope(token.Expression); break;
                        case TokenKeyword.IfNot: generator.GenOpenIfNotScope(token.Expression); break;
                        case TokenKeyword.Each: generator.GenOpenEachScope(token.Expression, token.FormatOrLocalName); break;
                        case TokenKeyword.When: generator.GenOpenWhenScope(token.Expression, token.FormatOrLocalName); break;
                        case TokenKeyword.Eq: generator.GenOpenEqScope(token.Expression, token.FormatOrLocalName, token.Modifiers.Contains("i")); break;
                        case TokenKeyword.Contains: generator.GenOpenContainsScope(token.Expression, token.FormatOrLocalName, token.Modifiers.Contains("i")); break;
                        case TokenKeyword.Begins: generator.GenOpenBeginsScope(token.Expression, token.FormatOrLocalName, token.Modifiers.Contains("i")); break;
                        case TokenKeyword.Ends: generator.GenOpenEndsScope(token.Expression, token.FormatOrLocalName, token.Modifiers.Contains("i")); break;
                        case TokenKeyword.Else: generator.GenOpenElseScope(); break;
                        case TokenKeyword.Text: generator.GenText(token.Expression, token.FormatOrLocalName); break;
                        case TokenKeyword.Html: generator.GenHtml(token.Expression, token.FormatOrLocalName); break;
                        case TokenKeyword.Url: generator.GenUrl(token.Expression, token.Modifiers.Contains("p"), token.Modifiers.Contains("a")); break;
                    }
                    break;
                case TokenType.Close:
                    switch (token.Keyword) {
                        case TokenKeyword.If: generator.GenCloseIfScope(); break;
                        case TokenKeyword.IfNot: generator.GenCloseIfNotScope(); break;
                        case TokenKeyword.Each: generator.GenCloseEachScope(); break;
                        case TokenKeyword.When: generator.GenCloseWhenScope(); break;
                        case TokenKeyword.Eq: generator.GenCloseEqScope(); break;
                        case TokenKeyword.Contains: generator.GenCloseContainsScope(); break;
                        case TokenKeyword.Begins: generator.GenCloseBeginsScope(); break;
                        case TokenKeyword.Ends: generator.GenCloseEndsScope(); break;
                        case TokenKeyword.Else: generator.GenCloseElseScope(); break;
                    }
                    break;
            }
        }

        private CompilerScope GetAfterScope(CompilerScope scope, Token token) {
            switch (token.Type) {
                case TokenType.Open:
                    switch (token.Keyword) {
                        case TokenKeyword.Begins: return new CompilerScope(scope, CompilerScopeType.Begins);
                        case TokenKeyword.Contains: return new CompilerScope(scope, CompilerScopeType.Contains);
                        case TokenKeyword.Each: return new CompilerScope(scope, CompilerScopeType.Each);
                        case TokenKeyword.Else: return new CompilerScope(scope, CompilerScopeType.Else);
                        case TokenKeyword.Ends: return new CompilerScope(scope, CompilerScopeType.Ends);
                        case TokenKeyword.Eq: return new CompilerScope(scope, CompilerScopeType.Eq);
                        case TokenKeyword.If: return new CompilerScope(scope, CompilerScopeType.If);
                        case TokenKeyword.IfNot: return new CompilerScope(scope, CompilerScopeType.IfNot);
                        case TokenKeyword.When: return new CompilerScope(scope, CompilerScopeType.When);
                    }
                    break;
                case TokenType.Close: return scope.Parent;
            }
            return scope;
        }
    }
}
