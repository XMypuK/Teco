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

namespace Teco.Tests.Model {
    public class EachTokenTestModel {
        public List<Object> EmptyList = new List<Object>();
        public List<Int32> ValueList = new List<Int32> { 2, 1, 2, 8, 5, 0, 6 };
        public List<EachTokenElement1> ModelList = new List<EachTokenElement1> {
            new EachTokenElement1 { Text = "Item 1" },
            new EachTokenElement1 { Text = "Item 2" },
            new EachTokenElement1 { Text = "Item 3" }
        };
        public List<EachTokenElement2> ComplexModel = new List<EachTokenElement2> {
            new EachTokenElement2 { Children = new char[] { 't', 'o' } },
            new EachTokenElement2 { Children = new char[] { 'b', 'e' } },
            new EachTokenElement2 { Children = new char[] { 'o', 'r' } },
            new EachTokenElement2 { Children = new char[] { 'n', 'o', 't' } },
            new EachTokenElement2 { Children = new char[] { 't', 'o' } },
            new EachTokenElement2 { Children = new char[] { 'b', 'e' } }
        };
    }

    public class EachTokenElement1 {
        public String Text;
    }

    public class EachTokenElement2 {
        public Char[] Children;
    }
}
