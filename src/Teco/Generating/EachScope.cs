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
using System.Reflection.Emit;

namespace Teco.Generating {

    /// <summary>
    /// Holds context data of Each scope.
    /// </summary>
    class EachScope : Scope {
        String _elementLocName;
        String _indexLocName;
        String _numLocName;
        String _countLocName;

        /// <summary>
        /// Label pointed to routine which checks if there
        /// is next element to process.
        /// </summary>
        public Label LabCheck { get; set; }

        /// <summary>
        /// Label pointed to the end of scope.
        /// </summary>
        public Label LabScopeEnd { get; set; }

        /// <summary>
        /// Local variable that holds an object which actually
        /// provides elements for the next iteration.
        /// </summary>
        public LocalBuilder LocCollection { get; set; }

        /// <summary>
        /// Local variable that holds an element of current
        /// iteration.
        /// </summary>
        public LocalBuilder LocThis {
            get { return Locals[_elementLocName]; }
            set { Locals[_elementLocName] = value; }
        }

        /// <summary>
        /// Local variable that holds an index of current 
        /// iteration.
        /// </summary>
        public LocalBuilder LocIndex {
            get { return Locals[_indexLocName]; }
            set { Locals[_indexLocName] = value; }
        }

        /// <summary>
        /// Local variable that holds a number (one-based)
        /// of current iteration.
        /// </summary>
        public LocalBuilder LocNum {
            get { return Locals[_numLocName]; }
            set { Locals[_numLocName] = value; }
        }

        /// <summary>
        /// Local variable that holds a count of elements in
        /// collection (if this information is available).
        /// </summary>
        public LocalBuilder LocCount {
            get { return Locals[_countLocName]; }
            set { Locals[_countLocName] = value; }
        }

        /// <summary>
        /// Creates new instance of Each scope.
        /// </summary>
        /// <param name="parent">Scope in which Each scope is located.</param>
        /// <param name="elementLocName">Name for local variable which will hold element of iteration. If null or empty then 'this' will be used.</param>
        public EachScope(Scope parent, String elementLocName)
            : base(parent, CompilerScopeType.Each) {

            if (String.IsNullOrEmpty(elementLocName)) {
                elementLocName = "this";
            }

            _elementLocName = elementLocName;
            _indexLocName = elementLocName + "Index";
            _numLocName = elementLocName + "Num";
            _countLocName = elementLocName + "Count";
        }
    }
}
