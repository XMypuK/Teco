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
using System.Reflection.Emit;

namespace Teco.Generating {

    /// <summary>
    /// A collection of local variables intended to be used
    /// in scopes.
    /// </summary>
    class ScopeLocalCollection {
        private Dictionary<String, LocalBuilder> _locals;

        /// <summary>
        /// Returns a local variable by its name or null if
        /// there is no variable with such name were found
        /// in collection. Adds new local variable into
        /// collection. When adding if a collection already
        /// contains a variable with such name then the
        /// InvalidOperationException will be throwned.
        /// </summary>
        /// <param name="localName">A name of variable.</param>
        public LocalBuilder this[String localName] {
            get {
                LocalBuilder local;
                if (_locals.TryGetValue(localName, out local))
                    return local;

                return null;
            }
            set {
                try {
                    _locals.Add(localName, value);
                }
                catch (Exception exc) {
                    throw new InvalidOperationException("Local variable with the same name is already presented in the collection.", exc);
                }                
            }
        }

        /// <summary>
        /// Creates new instaince of collection.
        /// </summary>
        public ScopeLocalCollection() {
            _locals = new Dictionary<String, LocalBuilder>();
        }

    }
}
