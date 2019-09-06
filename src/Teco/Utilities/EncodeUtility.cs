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
using System.Globalization;
using System.IO;
using System.Text;

namespace Teco.Utilities {
    public static class EncodeUtility {
        const UInt64 SEG_ENC_FLAGS_1 = 0xFFFFFFFFFFF9003FUL;
        const UInt64 SEG_ENC_FLAGS_2 = 0x8000001E8000001DUL;
        const UInt64 URL_ENC_FLAGS_1 = 0xFFFFFFFFA000000AUL;
        const UInt64 URL_ENC_FLAGS_2 = 0x0000000A8000001DUL;

        static String[] SEG_ENC_TABLE;
        static String[] URL_ENC_TABLE;

        static EncodeUtility() {
            InitSegEncTable();
            InitUrlEncTable();
        }

        static void InitSegEncTable() {
            String[] encTable = new String[128];
            for (Int32 i = 0; i < 64; i++) {
                if ((SEG_ENC_FLAGS_1 & (0x8000000000000000UL >> i)) != 0UL) {
                    encTable[i] = "%" + i.ToString("X2");
                }
            }
            for (Int32 i = 64, j = 0; i < 128; i++, j++) {
                if ((SEG_ENC_FLAGS_2 & (0x8000000000000000UL >> j)) != 0UL) {
                    encTable[i] = "%" + i.ToString("X2");
                }
            }
            SEG_ENC_TABLE = encTable;
        }

        static void InitUrlEncTable() {
            String[] encTable = new String[128];
            for (Int32 i = 0; i < 64; i++) {
                if ((URL_ENC_FLAGS_1 & (0x8000000000000000UL >> i)) != 0UL) {
                    encTable[i] = "%" + i.ToString("X2");
                }
            }
            for (Int32 i = 64, j = 0; i < 128; i++, j++) {
                if ((URL_ENC_FLAGS_2 & (0x8000000000000000UL >> j)) != 0UL) {
                    encTable[i] = "%" + i.ToString("X2");
                }
            }
            URL_ENC_TABLE = encTable;
        }

        /// <summary>
        /// Applies percent encoding to the characters of the value, 
        /// which are not included in reserved and unreserved 
        /// character sets according to RFC 3986.
        /// </summary>
        /// <param name="value">A value to encode.</param>
        /// <param name="skipUnicode">If true then unicode characters will not be encoded.</param>
        /// <returns>Percent-encoded value.</returns>
        public static String UrlEncodeAsUrl(String value, Boolean skipUnicode) {
            if (String.IsNullOrEmpty(value))
                return value;

            StringBuilder result = new StringBuilder(3 * value.Length);
            using (StringWriter output = new StringWriter(result)) {
                Boolean skipSeq = false;
                Int32 seqPos = -1;
                for (Int32 i = 0; i < value.Length; i++) {
                    Char @char = value[i];
                    Boolean skip;
                    if (@char < 0x40) {
                        skip = (URL_ENC_FLAGS_1 & (0x8000000000000000UL >> @char)) == 0UL;
                    }
                    else if (@char < 0x80) {
                        skip = (URL_ENC_FLAGS_2 & (0x8000000000000000UL >> (@char & 0x3F))) == 0UL;
                    }
                    else {
                        skip = skipUnicode;
                    }

                    if (skip) {
                        if (seqPos != -1 && !skipSeq) {
                            PercentEncodeAsUTF8(value.Substring(seqPos, i - seqPos), output);
                            seqPos = i;
                            skipSeq = true;
                        }
                        else if (seqPos == -1) {
                            seqPos = i;
                            skipSeq = true;
                        }
                    }
                    else {
                        if (seqPos != -1 && skipSeq) {
                            output.Write(value.Substring(seqPos, i - seqPos));
                            seqPos = -1;
                        }
                        if (@char < 128) {
                            if (seqPos != -1 && !skipSeq) {
                                PercentEncodeAsUTF8(value.Substring(seqPos, i - seqPos), output);
                                seqPos = -1;
                            }
                            output.Write(URL_ENC_TABLE[@char]);
                        }
                        else if (seqPos == -1) {
                            seqPos = i;
                            skipSeq = false;
                        }
                    }
                }
                if (seqPos != -1) {
                    if (skipSeq) {
                        output.Write(value.Substring(seqPos));
                    }
                    else {
                        PercentEncodeAsUTF8(value.Substring(seqPos), output);
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Applies percent encoding to the characters of the value, 
        /// which are not included in unreserved character set 
        /// according to RFC 3986.
        /// </summary>
        /// <param name="value">A value to encode.</param>
        /// <param name="skipUnicode">If true then unicode characters will not be encoded.</param>
        /// <returns>Percent-encoded value.</returns>
        public static String UrlEncode(String value, Boolean skipUnicode) {
            if (String.IsNullOrEmpty(value))
                return value;

            StringBuilder result = new StringBuilder(3 * value.Length);
            using (StringWriter output = new StringWriter(result)) {
                Boolean skipSeq = false;
                Int32 seqPos = -1;
                for (Int32 i = 0; i < value.Length; i++) {
                    Char @char = value[i];
                    Boolean skip;
                    if (@char < 0x40) {
                        skip = (SEG_ENC_FLAGS_1 & (0x8000000000000000UL >> @char)) == 0UL;
                    }
                    else if (@char < 0x80) {
                        skip = (SEG_ENC_FLAGS_2 & (0x8000000000000000UL >> (@char & 0x3F))) == 0UL;
                    }
                    else {
                        skip = skipUnicode;
                    }

                    if (skip) {
                        if (seqPos != -1 && !skipSeq) {
                            PercentEncodeAsUTF8(value.Substring(seqPos, i - seqPos), output);
                            seqPos = i;
                            skipSeq = true;
                        }
                        else if (seqPos == -1) {
                            seqPos = i;
                            skipSeq = true;
                        }
                    }
                    else {
                        if (seqPos != -1 && skipSeq) {
                            output.Write(value.Substring(seqPos, i - seqPos));
                            seqPos = -1;
                        }
                        if (@char < 128) {
                            if (seqPos != -1 && !skipSeq) {
                                PercentEncodeAsUTF8(value.Substring(seqPos, i - seqPos), output);
                                seqPos = -1;
                            }
                            output.Write(SEG_ENC_TABLE[@char]);
                        }
                        else if (seqPos == -1) {
                            seqPos = i;
                            skipSeq = false;
                        }
                    }
                }
                if (seqPos != -1) {
                    if (skipSeq) {
                        output.Write(value.Substring(seqPos));
                    }
                    else {
                        PercentEncodeAsUTF8(value.Substring(seqPos), output);
                    }
                }
            }
            return result.ToString();
        }

        private static void PercentEncodeAsUTF8(String value, StringWriter output) {
            Byte[] utf8data = Encoding.UTF8.GetBytes(value);
            for (Int32 i = 0; i < utf8data.Length; i++) {
                output.Write('%');
                output.Write(utf8data[i].ToString("X2"));
            }
        }

        /// <summary>
        /// Replaces HTML special characters with corresponding character
        /// references.
        /// </summary>
        /// <param name="value">A value to encode.</param>
        /// <returns>Encoded string.</returns>
        public static String HtmlEncode(String value) {
            if (String.IsNullOrEmpty(value))
                return value;

            Int32 pos = IndexOfHtmlEncodingChar(value, 0);
            if (pos < 0)
                return value;

            StringBuilder result = new StringBuilder(value.Length + 5);
            Int32 begin = 0;
            do {
                Int32 len = pos - begin;
                if (len > 0) {
                    result.Append(value.Substring(begin, len));
                }
                Char @char = value[pos];
                switch (@char) {
                    case '<': result.Append("&lt;"); break;
                    case '>': result.Append("&gt;"); break;
                    case '"': result.Append("&quot;"); break;
                    case '&': result.Append("&amp;"); break;
                    default:
                        result.Append("&#");
                        result.Append(((UInt16)@char).ToString(NumberFormatInfo.InvariantInfo));
                        result.Append(";");
                        break;
                }
                begin = pos + 1;
                pos = IndexOfHtmlEncodingChar(value, begin);
            }
            while (pos >= 0);

            if ( begin < value.Length) {
                result.Append(value.Substring(begin));
            }
            return result.ToString();
        }

        private static Int32 IndexOfHtmlEncodingChar(String value, Int32 pos) {
            Int32 len = value.Length;
            while (pos < len) {
                Char @char = value[pos];
                if (@char <= '>') {
                    switch (@char) {
                        case '<':
                        case '>':
                        case '"':
                        case '&': return pos;
                    }
                }
                else if (@char >= 0x00A0 && @char <= 0x00FF) {
                    return pos;
                }
                pos++;
            }
            return -1;
        }
    }
}
