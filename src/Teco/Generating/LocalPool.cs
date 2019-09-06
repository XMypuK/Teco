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
using System.Linq;
using System.Reflection.Emit;

namespace Teco.Generating {

    /// <summary>
    /// Local variable pool which can provide a variable of
    /// specific type on demand.
    /// </summary>
    class LocalPool {
        class LocalDesc {
            public Boolean Reserved;
            public LocalBuilder Local;
        }

        ILGenerator _il;
        Dictionary<Type, List<LocalDesc>> _pool;

        /// <summary>
        /// Creates new instance of local variable pool.
        /// </summary>
        public LocalPool(ILGenerator il) {
            _il = il;
            _pool = new Dictionary<Type, List<LocalDesc>>();
        }

        /// <summary>
        /// In private storage searches a free local variable with
        /// the type specified. If no sutable variable is found it
        /// allocates new variable. The variable is marked as
        /// reserved and is returned.
        /// </summary>
        /// <param name="type">Demanded type of variable.</param>
        public LocalBuilder Reserve(Type type) {
            List<LocalDesc> localList;
            if (!_pool.TryGetValue(type, out localList)) {
                localList = new List<LocalDesc>();
                _pool.Add(type, localList);
            }
            LocalDesc descToReserve = localList.FirstOrDefault(desc => !desc.Reserved);
            if (descToReserve == null) {
                descToReserve = new LocalDesc { Local = _il.DeclareLocal(type) };
                localList.Add(descToReserve);
            }
            descToReserve.Reserved = true;
            return descToReserve.Local;
        }

        /// <summary>
        /// Marks a variable as free so it can be reused later.
        /// </summary>
        /// <param name="local">Local variable to free.</param>
        public void Free(LocalBuilder local) {
            if (local == null)
                return;

            List<LocalDesc> localList;
            if (!_pool.TryGetValue(local.LocalType, out localList))
                return;

            LocalDesc descToFree = localList.FirstOrDefault(desc => desc.Local == local);
            descToFree.Reserved = false;
        }
    }
}
