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

namespace Teco.Parsing {
    public class Token {
        /// <summary>
        /// Type of the token.
        /// </summary>
        public TokenType Type { get; private set; }

        /// <summary>
        /// Command keyword which is represented by the token.
        /// </summary>
        public TokenKeyword Keyword { get; private set; }

        /// <summary>
        /// A string representing either a path to value in 
        /// the model or a constant value.
        /// </summary>
        public String Expression { get; private set; }

        /// <summary>
        /// Either a string which describes a format of value
        /// for 'html' and 'text' commands or a name of a local
        /// variable for 'each' command.
        /// </summary>
        public String FormatOrLocalName { get; private set; }

        /// <summary>
        /// A string representing a set of flags which are
        /// applied to token.
        /// </summary>
        public String Modifiers { get; private set; }

        /// <summary>
        /// Index of token first character in the source template
        /// string.
        /// </summary>
        public Int32 Position { get; private set; }

        public Token(TokenType type, TokenKeyword keyword, 
            String expression, String formatOrlLocalName,
            String modifiers, Int32 position) {

            this.Type = type;
            this.Keyword = keyword;
            this.Expression = expression;
            this.FormatOrLocalName = formatOrlLocalName;
            this.Modifiers = modifiers;
            this.Position = position;
        }
    }
}
