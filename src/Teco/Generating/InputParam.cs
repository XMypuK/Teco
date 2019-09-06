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
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Teco.Generating {
    public class InputParam {
        public InputParamSource Source { get; private set; }
        public Int32 Index { get; private set; }
        public String Name { get; private set; }
        public Type Type { get; private set; }
        public LocalBuilder Local { get; private set; }
        public FieldInfo Field { get; private set; }

        public InputParam(ParameterBuilder paramBuilder, Type type, Boolean instanceMethod) {
            Source = InputParamSource.Param;
            Name = paramBuilder.Name;
            Index = instanceMethod ? paramBuilder.Position : (paramBuilder.Position - 1);
            Type = type;
            Local = null;
            Field = null;
        }

        public InputParam(ParameterInfo paramInfo) {
            Source = InputParamSource.Param;
            Name = paramInfo.Name;
            Index = paramInfo.Position;
            Type = paramInfo.ParameterType;
            Local = null;
            Field = null;
        }

        public InputParam(LocalBuilder local) {
            Source = InputParamSource.Local;
            Name = null;
            Index = local.LocalIndex;
            Type = local.LocalType;
            Local = local;
            Field = null;
        }

        public InputParam(FieldInfo fieldInfo) {
            Source = InputParamSource.Field;
            Name = fieldInfo.Name;
            Type = fieldInfo.FieldType;
            Index = -1;
            Local = null;
            Field = fieldInfo;
        }

    }
}
