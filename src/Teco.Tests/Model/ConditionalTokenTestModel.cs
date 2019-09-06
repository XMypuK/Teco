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

namespace Teco.Tests.Model {
    public class ConditionalTokenTestModel {
        public Boolean Bool_True = true;
        public Boolean Bool_False = false;
        public Int32 Int32_True = 1;
        public Int32 Int32_False = 0;
        public UInt32 UInt32_True = 1;
        public UInt32 UInt32_False = 0;
        public Int16 Int16_True = 1;
        public Int16 Int16_False = 0;
        public UInt16 UInt16_True = 1;
        public UInt16 UInt16_False = 0;
        public Int64 Int64_True = 1;
        public Int64 Int64_False = 0;
        public UInt64 UInt64_True = 1;
        public UInt64 UInt64_False = 0;
        public String String_True = "a";
        public String String_False = "";
        public Object Object_True = new Object();
        public Object Object_False = null;
    }
}
