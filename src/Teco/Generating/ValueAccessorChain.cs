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
using System.Reflection;
using System.Reflection.Emit;

namespace Teco.Generating {
    class ValueAccessorChain {
        public ValueAccessor[] Accessors { get; private set; }
        public Boolean IsBroken { get; private set; }

        public ValueAccessorChain(InputParam modelParam, Scope scope, ValuePath valuePath) {
            Boolean empty = (valuePath.Fragments.Length == 0);
            if (empty) {
                IsBroken = true;
                return;
            }

            List<ValueAccessor> accessors = new List<ValueAccessor>();

            Int32 fragIndex = 0;

            ValuePathFragment frag = valuePath.Fragments[fragIndex];
            LocalBuilder local = scope.ResolveLocal(frag.Literal);
            if (local != null) {
                accessors.Add(new ValueAccessor(local));
                fragIndex++;
            }
            else {
                EachScope eachScope = (EachScope)scope.GetClosestScope(CompilerScopeType.Each, true);
                if (eachScope != null)
                    accessors.Add(new ValueAccessor(eachScope.LocThis));
                else
                    accessors.Add(new ValueAccessor(modelParam));
            }

            for (; fragIndex < valuePath.Fragments.Length && !IsBroken; fragIndex++) {
                frag = valuePath.Fragments[fragIndex];
                ValueAccessor lastAccessor = accessors.Last();
                switch (frag.Type) {
                    case ValuePathFragmentType.Name:
                        MemberInfo member = lastAccessor.ResultType.GetMember(frag.Literal, MemberTypes.Property | MemberTypes.Field, BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
                        if (member != null)
                            accessors.Add(new ValueAccessor(member));
                        else
                            IsBroken = true;
                        break;

                    case ValuePathFragmentType.StringIndexer:
                        PropertyInfo strIndexer = lastAccessor.ResultType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { typeof(String) }, null);
                        if (strIndexer != null)
                            accessors.Add(new ValueAccessor(strIndexer, frag.Literal, strIndexer.PropertyType));
                        else
                            IsBroken = true;
                        break;

                    case ValuePathFragmentType.IntegerIndexer:
                        if (lastAccessor.ResultType.IsArray) {
                            accessors.Add(new ValueAccessor(null, Int32.Parse(frag.Literal), lastAccessor.ResultType.GetElementType()));
                        }
                        else {
                            PropertyInfo intIndexer = lastAccessor.ResultType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { typeof(Int32) }, null);
                            if (intIndexer != null)
                                accessors.Add(new ValueAccessor(intIndexer, Int32.Parse(frag.Literal), intIndexer.PropertyType));
                            else
                                IsBroken = true;
                        }
                        break;
                }
            }

            this.Accessors = accessors.ToArray();
        }
    }
}
