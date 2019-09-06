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

namespace Teco.Generating {
    /// <summary>
    /// Flags for describing collection type.
    /// </summary>
    [Flags]
    enum CollectionTypeFlags {
        /// <summary>
        /// Empty flag set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Type implements one of the generic interfaces.
        /// </summary>
        Generic = 1,

        /// <summary>
        /// Type implements IEnumerable or IEnumerable&lt;T&gt; interface.
        /// </summary>
        IEnumerate = 2,

        /// <summary>
        /// Type implements ICollection or ICollection&lt;T&gt; interface.
        /// </summary>
        ICollection = 4,

        /// <summary>
        /// Type implements IList or IList&lt;T&gt; interface.
        /// </summary>
        IList = 8,

        /// <summary>
        /// Type is an array type.
        /// </summary>
        Array = 16
    }
}
