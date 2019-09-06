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
using System.Reflection;
using System.Reflection.Emit;

namespace Teco.Generating {
    class ValueAccessor {
        public LocalBuilder Local { get; private set; }
        public InputParam Param { get; private set; }
        public MemberInfo Member { get; private set; }
        public Object Index { get; private set; }
        private Type ElementType { get; set; }

        public Boolean IsLocal {
            get { return Local != null; }
        }

        public Boolean IsParam {
            get { return Param != null; }
        }

        public Boolean IsField {
            get { return Member != null && Member.MemberType == MemberTypes.Field; }
        }

        public Boolean IsProperty {
            get { return Member != null && Member.MemberType == MemberTypes.Property; }
        }

        public Boolean IsIndexer {
            get { return Index != null; }
        }

        public Type ResultType {
            get {
                if (IsLocal)
                    return Local.LocalType;
                if (IsParam)
                    return Param.Type;
                if (IsField)
                    return ((FieldInfo)Member).FieldType;
                if (IsProperty)
                    return ((PropertyInfo)Member).PropertyType;
                if (IsIndexer)
                    return ElementType;
                return null;
            }
        }

        public ValueAccessor(LocalBuilder local) {
            this.Local = local;
        }

        public ValueAccessor(InputParam @param) {
            this.Param = @param;
        }

        public ValueAccessor(MemberInfo member) : this(member, null, null) { }
        public ValueAccessor(MemberInfo member, Object index, Type elementType) {
            this.Member = member;
            this.Index = index;
            this.ElementType = elementType;
        }
    }
}
