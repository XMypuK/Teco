#region License
// Copyright (c) 2019 Evgenii Zeiler, https://github.com/XMypuK
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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Teco.Parsing {
    /// <summary>
    /// Class intended to convert a template string into a series of
    /// tokens.
    /// 
    /// The token is either of:
    /// - one of substituion commands;
    /// - one of flow control commands;
    /// - raw piece of template string.
    /// 
    /// In general a command have next form: {n/m:e:p}
    /// where:
    ///     n is a name of command;
    ///     m is a set of modificators (optional with slash);
    ///     e is a variable or constant;
    ///     p is an extra parameter (format string or variable name).
    /// 
    /// Substitution commands are: text, html, url.
    /// Flow control commands are: each, /each, if, /if, ifnot, 
    /// /ifnot, when, /when, eq, /eq, begins, /begins, contains,
    /// /contains, ends, /ends, else, /else.
    /// 
    /// Modificators are flag parameters which can be used on
    /// some commands to change its behaviour. In token each
    /// modificator is represented by an alphabet character.
    /// 
    /// Variable is a path to a member in the model which is 
    /// expected to contain a value for the current command.
    /// The path is a set of member names separated by dot. Each
    /// member name must begins with an alphabet character or
    /// underscore and can contain any alphabet, digit or 
    /// underscore characters.
    /// 
    /// Constant is a string value which should be processed by
    /// command (instead of model member value). Constant starts 
    /// with ' @ ' symbol and can contain any characters but 
    /// characters ' { ', ' } ', ' : ' must be doubled.
    /// 
    /// Extra parameter sense and syntax depend on a command
    /// which it is applied to. Format string can contain any
    /// characters excepting ' { ' and ' } '. Variable name
    /// must be started with alphabet or underscore character
    /// and can contain alphabet, digit or underscore characters.
    /// </summary>
    public class Parser: IParser {
        const String RXSTR_NAME = @"[A-Za-z_][A-Za-z0-9_]*";
        const String RXSTR_INDEXER = @"\[(?: \d+ | ""(?:[^\\""]|\\\\|\\"")*"" )\]";
        const String RXSTR_VALUE_PATH = RXSTR_NAME + @"(?:\." + RXSTR_NAME + @"|" + RXSTR_INDEXER + @")*";
        const String RXSTR_CONSTANT = @"@(?:[^\{\}:]|\{\{|\}\}|::)*";
        const String RXSTR_EXPRESSION = RXSTR_VALUE_PATH + "|" + RXSTR_CONSTANT;
        const String RXSTR_FORMAT = @"[^\{\}]+";
        const String RXSTR_TOKEN = @"\{(?:
            (text|html):(" + RXSTR_VALUE_PATH + @")(?::(" + RXSTR_FORMAT + @"))? |
            (url)(/[pa]*)?:(" + RXSTR_VALUE_PATH + @") |
            (if|ifnot):(" + RXSTR_VALUE_PATH + @") |
            (each):(" + RXSTR_VALUE_PATH + @")(?::(" + RXSTR_NAME + @"))? |
            (when):(" + RXSTR_EXPRESSION + @")(?::(" + RXSTR_FORMAT + @"))? |
            (eq|begins|contains|ends)(/[i]*)?:(" + RXSTR_EXPRESSION + @")(?::(" + RXSTR_FORMAT + @"))? |
            (else) |
            (/(?:if|ifnot|each|when|eq|begins|contains|ends|else))
        )\}";
        static readonly Regex RX_TOKEN = new Regex(RXSTR_TOKEN, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);

        public IEnumerator<Token> GetTokenEnumerator(String template) {
            return new TokenEnumerator(template);
        }

        class TokenEnumerator : IEnumerator<Token> {
            String _template;
            Match _matchToCheck;
            Int32 _posToCheck;
            Token _token;

            public Token Current {
                get {
                    if (_token == null)
                        throw new InvalidOperationException();

                    return _token;
                }
            }

            object IEnumerator.Current {
                get { return Current; }
            }

            public TokenEnumerator(String template) {
                _template = template;
                Reset();
            }

            public Boolean MoveNext() {
                MoveToNextToken();
                return (_token != null);
            }

            public void Reset() {
                this._matchToCheck = null;
                this._posToCheck = 0;
                this._token = null;
            }

            public void Dispose() {
            }

            private void MoveToNextToken() {
                if (_matchToCheck == null || !_matchToCheck.Success) {
                    _matchToCheck = RX_TOKEN.Match(_template, _posToCheck);
                    Int32 nextPos = _matchToCheck.Success ? _matchToCheck.Index : _template.Length;
                    if (_posToCheck < nextPos) {
                        _token = CreateRawToken(_posToCheck, nextPos - _posToCheck);
                        _posToCheck = nextPos;
                    }
                    else if (_matchToCheck.Success) {
                        _token = CreateTokenFromMatch(_matchToCheck);
                        _posToCheck = _matchToCheck.Index + _matchToCheck.Length;
                        _matchToCheck = null;
                    }
                    else
                        _token = null;
                }
                else {
                    _token = CreateTokenFromMatch(_matchToCheck);
                    _posToCheck = _matchToCheck.Index + _matchToCheck.Length;
                    _matchToCheck = null;
                }
            }

            private Token CreateTokenFromMatch(Match match) {
                if (match.Groups[1].Success) {
                    return new Token(TokenType.Open,
                        match.Groups[1].Value == "html" ? TokenKeyword.Html : TokenKeyword.Text,
                        match.Groups[2].Value,
                        match.Groups[3].Success ? match.Groups[3].Value : "",
                        "",
                        match.Index);
                }
                if (match.Groups[4].Success) {
                    return new Token(TokenType.Open,
                        TokenKeyword.Url,
                        match.Groups[6].Value,
                        "",
                        match.Groups[5].Success ? match.Groups[5].Value.Remove(0, 1) : "",
                        match.Index);
                }
                if (match.Groups[7].Success) {
                    return new Token(TokenType.Open,
                        match.Groups[7].Value == "if" ? TokenKeyword.If : TokenKeyword.IfNot,
                        match.Groups[8].Value,
                        "",
                        "",
                        match.Index);
                }
                if (match.Groups[9].Success) {
                    return new Token(TokenType.Open,
                        TokenKeyword.Each,
                        match.Groups[10].Value,
                        match.Groups[11].Success ? match.Groups[11].Value : "",
                        "",
                        match.Index);
                }
                if (match.Groups[12].Success) {
                    return new Token(TokenType.Open,
                        TokenKeyword.When,
                        match.Groups[13].Value,
                        match.Groups[14].Success ? match.Groups[11].Value : "",
                        "",
                        match.Index);
                }
                if (match.Groups[15].Success) {
                    TokenKeyword keyword;
                    switch (match.Groups[15].Value) {
                        case "eq": keyword = TokenKeyword.Eq; break;
                        case "begins": keyword = TokenKeyword.Begins; break;
                        case "contains": keyword = TokenKeyword.Contains; break;
                        case "ends": keyword = TokenKeyword.Ends; break;
                        default: keyword = TokenKeyword.None; break;
                    }
                    return new Token(TokenType.Open,
                        keyword,
                        match.Groups[17].Value,
                        match.Groups[18].Success ? match.Groups[18].Value : "",
                        match.Groups[16].Success ? match.Groups[16].Value.Remove(0, 1) : "",
                        match.Index);
                }
                if (match.Groups[19].Success) {
                    return new Token(TokenType.Open,
                        TokenKeyword.Else,
                        "",
                        "",
                        "",
                        match.Index);
                }
                if (match.Groups[20].Success) {
                    TokenKeyword keyword;
                    switch (match.Groups[20].Value) {
                        case "/if": keyword = TokenKeyword.If; break;
                        case "/ifnot": keyword = TokenKeyword.IfNot; break;
                        case "/each": keyword = TokenKeyword.Each; break;
                        case "/when": keyword = TokenKeyword.When; break;
                        case "/eq": keyword = TokenKeyword.Eq; break;
                        case "/begins": keyword = TokenKeyword.Begins; break;
                        case "/contains": keyword = TokenKeyword.Contains; break;
                        case "/ends": keyword = TokenKeyword.Ends; break;
                        case "/else": keyword = TokenKeyword.Else; break;
                        default: keyword = TokenKeyword.None; break;
                    }
                    return new Token(TokenType.Close,
                        keyword,
                        "",
                        "",
                        "",
                        match.Index);
                }
                return null;
            }

            private Token CreateRawToken(Int32 pos, Int32 len) {
                return new Token(TokenType.Raw,
                    TokenKeyword.None,
                    _template.Substring(pos, len),
                    "",
                    "",
                    pos);
            }
        }
    }
}
