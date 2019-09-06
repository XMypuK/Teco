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
using System.Reflection;
using System.Reflection.Emit;
using Teco.Generating;

namespace Teco {
    public class AssemblyCompilerTypeContext {
        static readonly Type T_Object = typeof(Object);
        static readonly ConstructorInfo CTOR_Object = T_Object.GetConstructor(new Type[0]);

        AssemblyCompiler _compiler;
        Type _modelType;
        String _typeName;
        Type _typeInterface;
        Boolean _typeCreated;
        TypeBuilder _typeBuilder;
        InputParam _modelParam;
        InputParam _formatProviderParam;

        internal AssemblyCompilerTypeContext(AssemblyCompiler compiler, Type modelType, String typeName, Type typeInterface) {
            if (modelType == null)
                throw new ArgumentNullException("modelType");

            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentException("Invalid type name.", "typeName");

            if (typeInterface != null && !typeInterface.IsInterface)
                throw new ArgumentException("The type provided is not an interface type.", "typeInterface");

            _compiler = compiler;
            _modelType = modelType;
            _typeName = typeName;
            _typeInterface = typeInterface;
            _typeCreated = false;
        }

        internal Type CreateType() {
            _typeCreated = true;
            if (_typeBuilder != null)
                return _typeBuilder.CreateType();
            else
                return null;
        }

        public void AddMethod(String methodName, String template, OutputFormat outputFormat) {
            AddMethodInternal(methodName, template, outputFormat);
        }

        public void AddMethod(String methodName, String template) {
            AddMethodInternal(methodName, template, _compiler.Options.OutputFormat);
        }

        public void AddProperty(String propName, String template, OutputFormat outputFormat) {
            AddPropertyInternal(propName, template, outputFormat);
        }

        public void AddProperty(String propName, String template) {
            AddPropertyInternal(propName, template, _compiler.Options.OutputFormat);
        }

        private void AddMethodInternal(String methodName, String template, OutputFormat outputFormat) {
            if (_typeCreated)
                throw new InvalidOperationException("Can't add method to compiled type.");

            EnsureTypeBuilder();
            MethodBuilder methodBuilder = _typeBuilder.DefineMethod(methodName, 
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Virtual, 
                CallingConventions.HasThis, 
                typeof(String), 
                new Type[0]);
            methodBuilder.InitLocals = true;
            ILGenerator il = methodBuilder.GetILGenerator();
            Generator generator = new Generator(il, outputFormat, _modelParam, _formatProviderParam);
            _compiler.CompileInternal(template, generator);
        }

        private void AddPropertyInternal(String propName, String template, OutputFormat outputFormat) {
            if (_typeCreated)
                throw new InvalidOperationException("Can't add method to compiled type.");

            EnsureTypeBuilder();
            PropertyBuilder propBuilder = _typeBuilder.DefineProperty(propName,
                PropertyAttributes.None,
                CallingConventions.HasThis,
                typeof(String), 
                null, null, null, null, null);

            MethodBuilder getMethodBuilder = _typeBuilder.DefineMethod("get_" + propName,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Virtual,
                CallingConventions.HasThis,
                typeof(String),
                new Type[0]);

            propBuilder.SetGetMethod(getMethodBuilder);

            getMethodBuilder.InitLocals = true;
            ILGenerator il = getMethodBuilder.GetILGenerator();
            Generator generator = new Generator(il, outputFormat, _modelParam, _formatProviderParam);
            _compiler.CompileInternal(template, generator);
        }

        private void EnsureTypeBuilder() {
            if (_typeBuilder != null)
                return;

            ModuleBuilder moduleBuilder = _compiler.GetModuleBuilder();
            if (_typeInterface != null) {
                _typeBuilder = moduleBuilder.DefineType(_typeName, TypeAttributes.Public | TypeAttributes.Class, null, new[] { _typeInterface });
            }
            else {
                _typeBuilder = moduleBuilder.DefineType(_typeName, TypeAttributes.Public | TypeAttributes.Class);
            }
            InitModelField();
            InitFormatProviderField();
            InitConstuctor();
        }

        private void InitModelField() {
            FieldBuilder field = _typeBuilder.DefineField("_model", _modelType, FieldAttributes.Private | FieldAttributes.InitOnly);
            _modelParam = new InputParam(field);
        }

        private void InitFormatProviderField() {
            FieldBuilder field = _typeBuilder.DefineField("_formatProvider", typeof(IFormatProvider), FieldAttributes.Private | FieldAttributes.InitOnly);
            _formatProviderParam = new InputParam(field);
        }

        private void InitConstuctor() {
            ConstructorBuilder ctor = _typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, new Type[] { _modelParam.Type, _formatProviderParam.Type });
            ParameterBuilder modelArg = ctor.DefineParameter(1, ParameterAttributes.None, "model");
            ParameterBuilder formatProviderArg = ctor.DefineParameter(2, ParameterAttributes.None, "formatProvider");
            ILGenerator il = ctor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, CTOR_Object);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, _modelParam.Field);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, _formatProviderParam.Field);
            il.Emit(OpCodes.Ret);
        }
    }
}
