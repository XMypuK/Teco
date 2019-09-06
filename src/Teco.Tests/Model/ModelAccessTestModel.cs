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

namespace Teco.Tests.Model {
    public class ModelAccessTestModel {
        public String StrFld = "StrFldValue";

        public Int32 IntFld = 42;

        public Int32? NullableIntFld = 47;

        public Int32? NullableIntFldNull = null;

        public String[] StrArrFld = new String[3] { "StrArrFld0", "StrArrFld1", "StrArrFld2" };

        public String[] StrArrFldNull = null;

        public Int32[] IntArrFld = new Int32[] { 2, 12, 85, 0, 6 };

        public Int32[] IntArrFldNull = null;

        public String StrProp { get { return "StrPropValue"; } }

        public Int32 IntProp { get { return 7; } }

        public Int32? NullableIntProp { get { return 12; } }

        public Int32? NullableIntPropNull { get { return null; } }

        public RefType RefProp { get; private set; }

        public RefType RefPropNull { get { return null; } }

        public String[] StrArrProp { get; private set; }

        public String[] StrArrPropNull { get { return null; } }

        public Int32[] IntArrProp { get; private set; }

        public Int32[] IntArrPropNull { get { return null; } }

        public RefType2[] RefArrProp { get; private set; }

        public ValType2[] ValArrProp { get; private set; }

        public List<RefType2> RefListProp { get; private set; }

        public List<ValType2> ValListProp { get; private set; }

        public IEnumerable<RefType2> RefEnumProp { get; private set; }

        public IEnumerable<ValType2> ValEnumProp { get; private set; }

        public IndexedRefType IndexedRefProp { get; private set; }

        public IndexedValType IndexedValProp { get; private set; }

        public ModelAccessTestModel() {
            RefProp = new RefType();
            StrArrProp = new String[3] { "StrArrProp0", "StrArrProp1", "StrArrProp2" };
            IntArrProp = new Int32[] { 3, 15, 9, 20 };
            RefArrProp = new RefType2[] {
                new RefType2("a", 1),
                new RefType2("b", 2),
                new RefType2("c", 3)
            };
            ValArrProp = new ValType2[] {
                new ValType2("d", 7),
                new ValType2("e", 8),
                new ValType2("f", 9)
            };
            RefListProp = new List<RefType2> {
                new RefType2("x", 13),
                new RefType2("z", 7),
                new RefType2("y", 4)
            };
            ValListProp = new List<ValType2> {
                new ValType2("f", 8),
                new ValType2("h", 33),
                new ValType2("r", 41)
            };
            RefEnumProp = new List<RefType2> {
                new RefType2("t", 7),
                new RefType2("y", 5),
                new RefType2("u", 3)
            };
            ValEnumProp = new List<ValType2> {
                new ValType2("i", 8),
                new ValType2("o", 6),
                new ValType2("p", 4)
            };
            IndexedRefProp = new IndexedRefType();
            IndexedValProp = new IndexedValType();
        }

        public class RefType {
            public String StrFld = "RefType.StrFldValue";

            public Int32 IntFld = 52;

            public Int32? NullableIntFld = 57;

            public Int32? NullableIntFldNull = null;

            public String StrProp { get { return "RefType.StrPropValue"; } }

            public Int32 IntProp { get { return 27; } }

            public Int32? NullableIntProp { get { return 32; } }

            public Int32? NullableIntPropNull { get { return null; } }
        }

        public class RefType2 {

            public String StrValue { get; private set; }

            public Int32 IntValue { get; private set; }

            public RefType2(String strValue, Int32 intValue) {
                this.StrValue = strValue;
                this.IntValue = intValue;
            }
        }

        public struct ValType2 {

            public String StrValue { get; private set; }

            public Int32 IntValue { get; private set; }

            public ValType2(String strValue, Int32 intValue)
                : this() {

                StrValue = strValue;
                IntValue = intValue;
            }
        }

        public class IndexedRefType {
            public String this[String index] {
                get {
                    return "IRT" + index + "VALUE";
                }
            }

            public Int32 this[Int32 index] {
                get {
                    return index * 7;
                }
            }
        }

        public struct IndexedValType {
            public String this[String index] {
                get {
                    return "IVT" + index + "VALUE";
                }
            }

            public Int32 this[Int32 index] {
                get {
                    return index * 9;
                }
            }
        }
    }
}
