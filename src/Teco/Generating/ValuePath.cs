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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teco.Generating {
    class ValuePath {
        String _path;
        ValuePathFragment[] _fragments;

        public ValuePathFragment[] Fragments {
            get {
                if (_fragments == null) {
                    ParseExpression();
                }
                return _fragments.ToArray();
            }
        }

        public ValuePath(String path) {
            _path = path;
            _fragments = null;
        }

        void ParseExpression() {
            List<ValuePathFragment> parts = new List<ValuePathFragment>();

            Int32 partStart = 0;
            while (partStart < _path.Length) {
                Int32 delimiterPos = _path.IndexOfAny(new char[] { '.', '[' }, partStart);
                Boolean isDot = false;
                Boolean isIndexer = false;
                if (delimiterPos >= 0) {
                    isDot = _path[delimiterPos] == '.';
                    isIndexer = !isDot;
                }
                Int32 partLen = delimiterPos >= 0
                    ? delimiterPos - partStart
                    : _path.Length - partStart;
                if (partLen > 0) {
                    // read variable or member name
                    String part = _path.Substring(partStart, partLen);
                    parts.Add(new ValuePathFragment(part, ValuePathFragmentType.Name));
                    partStart += partLen;
                }
                else if (isDot) {
                    // just skip dot
                    if (partStart + 1 >= _path.Length)
                        throw new InvalidOperationException();
                    if (_path[partStart + 1] == '.' || _path[partStart + 1] == '[')
                        throw new InvalidOperationException();
                    partStart++;
                }
                else if (isIndexer) {
                    // read something like ["Some text"]
                    if (partStart == 0)
                        throw new InvalidOperationException();
                    if (delimiterPos + 1 >= _path.Length)
                        throw new InvalidOperationException();
                    Boolean isStringIndexer = _path[delimiterPos + 1] == '"';
                    if (isStringIndexer) {
                        StringBuilder stringIndexer = new StringBuilder(_path.Length - delimiterPos);
                        Boolean escaping = false;
                        Int32 pos = delimiterPos + 2;
                        while (true) {
                            if (pos >= _path.Length)
                                throw new InvalidOperationException();
                            if (escaping) {
                                stringIndexer.Append(_path[pos]);
                                escaping = false;
                            }
                            else if (_path[pos] == '\\') {
                                escaping = true;
                            }
                            else if (_path[pos] == '"') {
                                break;
                            }
                            else {
                                stringIndexer.Append(_path[pos]);
                            }
                            pos++;
                        }
                        if (pos + 1 >= _path.Length || _path[pos + 1] != ']')
                            throw new InvalidOperationException();
                        parts.Add(new ValuePathFragment(stringIndexer.ToString(), ValuePathFragmentType.StringIndexer));
                        partStart = pos + 2;
                    }
                    else {
                        // read something like [42]
                        StringBuilder numericIndexer = new StringBuilder(_path.Length - delimiterPos);
                        Int32 pos = delimiterPos + 1;
                        while (true) {
                            if (pos >= _path.Length)
                                throw new InvalidOperationException();
                            Char @char = _path[pos];
                            if (@char == ']')
                                break;
                            else if (@char < '0' || @char > '9')
                                throw new InvalidOperationException();
                            numericIndexer.Append(@char);
                            pos++;
                        }
                        parts.Add(new ValuePathFragment(numericIndexer.ToString(), ValuePathFragmentType.IntegerIndexer));
                        partStart = pos + 1;
                    }
                }

                _fragments = parts.ToArray();
            }
        }
    }
}
