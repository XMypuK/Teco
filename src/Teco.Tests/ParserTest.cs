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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using Teco.Parsing;

namespace Teco.Tests {
    class ParserTest {
        [Test]
        public void PRS_Raw() {
            var testList = new TestRecord[] {
                new TestRecord("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", "", "", 0)
                }),
                new TestRecord("1234567890", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "1234567890", "", "", 0)
                }),
                new TestRecord("~!@#$%^&*()_+-=[]{}'\\\"|,./<>?:;", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "~!@#$%^&*()_+-=[]{}'\\\"|,./<>?:;", "", "", 0)
                }),
                new TestRecord("абвгдеёжзиклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "абвгдеёжзиклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ", "", "", 0)
                })
            };
            TestList(testList);
        }

        [Test]
        public void PRS_Text() {
            var testList = new TestRecord[] {
                new TestRecord("{text:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Text, "Var_1234567890", "", "", 0)
                }),
                new TestRecord("{text:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:}", "", "", 0)
                }),
                new TestRecord("{text:0_Var_1234567890}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:0_Var_1234567890}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890.Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Text, "Var_1234567890.Prop[\"Key\"].Something", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890.Prop[42].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Text, "Var_1234567890.Prop[42].Something", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890..Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890..Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890.0Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890.0Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890.Prop[].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890.Prop[].Something}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890.Prop[.Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890.Prop[.Something}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890:}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890:{}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890:{}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890:}}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{text:Var_1234567890:}}", "", "", 0)
                }),
                new TestRecord("{text:Var_1234567890:abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Text, "Var_1234567890", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;", "", 0)
                })
            };
            TestList(testList);
        }

        [Test]
        public void PRS_Html() {
            var testList = new TestRecord[] {
                new TestRecord("{html:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Html, "Var_1234567890", "", "", 0)
                }),
                new TestRecord("{html:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:}", "", "", 0)
                }),
                new TestRecord("{html:0_Var_1234567890}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:0_Var_1234567890}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890.Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Html, "Var_1234567890.Prop[\"Key\"].Something", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890.Prop[42].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Html, "Var_1234567890.Prop[42].Something", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890..Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890..Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890.0Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890.0Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890.Prop[].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890.Prop[].Something}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890.Prop[.Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890.Prop[.Something}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890:}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890:{}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890:{}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890:}}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{html:Var_1234567890:}}", "", "", 0)
                }),
                new TestRecord("{html:Var_1234567890:abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Html, "Var_1234567890", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;", "", 0)
                })
            };
            TestList(testList);
        }

        [Test]
        public void PRS_Url() {
            var testList = new TestRecord[] {
                new TestRecord("{url:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890", "", "", 0)
                }),
                new TestRecord("{url:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:}", "", "", 0)
                }),
                new TestRecord("{url:0_Var_1234567890}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:0_Var_1234567890}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890.Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890.Prop[\"Key\"].Something", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890.Prop[42].Something}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890.Prop[42].Something", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890..Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890..Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890.0Prop[\"Key\"].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890.0Prop[\"Key\"].Something}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890.Prop[].Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890.Prop[].Something}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890.Prop[.Something}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890.Prop[.Something}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890:}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890:}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890:{}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890:{}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890:}}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890:}}", "", "", 0)
                }),
                new TestRecord("{url:Var_1234567890:abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url:Var_1234567890:abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+-=[]'\\\"|,./<>?:;}", "", "", 0)
                }),
                new TestRecord("{url/:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890", "", "", 0)
                }),
                new TestRecord("{url/p:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890", "", "p", 0)
                }),
                new TestRecord("{url/a:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890", "", "a", 0)
                }),
                new TestRecord("{url/pa:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Open, TokenKeyword.Url, "Var_1234567890", "", "pa", 0)
                }),
                new TestRecord("{url/x:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url/x:Var_1234567890}", "", "", 0)
                }),
                new TestRecord("{url/px:Var_1234567890}", new List<Token> {
                    new Token(TokenType.Raw, TokenKeyword.None, "{url/px:Var_1234567890}", "", "", 0)
                })
            };
            TestList(testList);
        }

        private void TestList(TestRecord[] testList) {
            Parser parser = new Parser();
            foreach (TestRecord record in testList) {
                IEnumerator<Token> expected = record.ExpectedTokens.GetEnumerator();
                IEnumerator<Token> actual = parser.GetTokenEnumerator(record.Template);
                Int32 diffIndex = AssertTokenSequence(expected, actual);
                if (diffIndex >= 0)
                    throw new AssertionException(String.Format("Template: \"{0}\". Incorrect token sequence at pos {1}.", record.Template, diffIndex));
            }
        }

        private Int32 AssertTokenSequence(IEnumerator<Token> expected, IEnumerator<Token> actual) {
            TokenComparer comparer = new TokenComparer();
            Int32 diffIndex = 0;
            Boolean bExpValid;
            Boolean bActValid;
            do {
                bExpValid = expected.MoveNext();
                bActValid = actual.MoveNext();
                if (!bExpValid || !bActValid)
                    break;

                if (!comparer.Equals(expected.Current, actual.Current))
                    return diffIndex;

                diffIndex++;
            }
            while (true);
            if (bExpValid == bActValid)
                return -1;

            return diffIndex;
        }

        class TestRecord {
            public String Template { get; private set; }
            public List<Token> ExpectedTokens { get; private set; }

            public TestRecord(String template, List<Token> expectedTokens) {
                this.Template = template;
                this.ExpectedTokens = expectedTokens;
            }
        }

        class TokenComparer: EqualityComparer<Token> {
            public override bool Equals(Token x, Token y) {
                if (x == null) {
                    return (y == null);
                }

                if (y == null)
                    return false;

                return x.Type == y.Type
                    && x.Keyword == y.Keyword
                    && x.Expression == y.Expression
                    && x.FormatOrLocalName == y.FormatOrLocalName
                    && x.Modifiers == y.Modifiers
                    && x.Position == y.Position;
            }

            public override int GetHashCode(Token obj) {
                Int32 hashCode = 0;
                if (obj != null) {
                    hashCode = obj.Type.GetHashCode();
                    hashCode ^= obj.Keyword.GetHashCode();
                    hashCode ^= obj.Expression != null ? obj.Expression.GetHashCode() : 0;
                    hashCode ^= obj.FormatOrLocalName != null ? obj.FormatOrLocalName.GetHashCode() : 0;
                    hashCode ^= obj.Modifiers != null ? obj.Modifiers.GetHashCode() : 0;
                    hashCode ^= obj.Position.GetHashCode();
                }
                return hashCode;
            }
        }

    }
}
