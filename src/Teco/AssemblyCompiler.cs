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
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Teco.Generating;
using Teco.Parsing;

namespace Teco {
    public class AssemblyCompiler : CompilerBase {
        static readonly Type T_DebuggableAttribute = typeof(DebuggableAttribute);
        static readonly ConstructorInfo CTOR_DebuggableAttribute_DebuggingModes = T_DebuggableAttribute.GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });

        AssemblyBuilder _assemblyBuilder;
        ModuleBuilder _moduleBuilder;
        List<AssemblyCompilerTypeContext> _types;

        protected internal new AssemblyCompilerOptions Options {
            get { return (AssemblyCompilerOptions)base.Options; }
        }

        public AssemblyCompiler(IParser parser, AssemblyCompilerOptions options) : base(parser, options) {
            Init();
        }

        public AssemblyCompiler(AssemblyCompilerOptions options) : this(new Parser(), options) { }

        public AssemblyCompilerTypeContext AddType(String typeName) {
            return AddType(Options.ModelType, typeName, null);
        }

        public AssemblyCompilerTypeContext AddType(String typeName, Type typeInterface) {
            return AddType(Options.ModelType, typeName, typeInterface);
        }

        public AssemblyCompilerTypeContext AddType(Type modelType, String typeName) {
            return AddType(modelType, typeName, null);
        }

        public AssemblyCompilerTypeContext AddType(Type modelType, String typeName, Type typeInterface) {
            AssemblyCompilerTypeContext typeContext = new AssemblyCompilerTypeContext(this, modelType, typeName, typeInterface);
            _types.Add(typeContext);
            return typeContext;
        }

#if !NETCOREAPP
        public Type[] GetCompiledTypes() {
            return GetCompiledTypes(false);
        }
#endif

        public Type[] GetCompiledTypes(
#if !NETCOREAPP
            Boolean saveAssembly
#endif
            ) {
            EnsureModuleBuilder();
            Type[] compiledTypes = new Type[_types.Count];
            for (Int32 i = 0; i < _types.Count; i++) {
                compiledTypes[i] = _types[i].CreateType();
            }
#if !NETCOREAPP
            if (saveAssembly) {
                _assemblyBuilder.Save(Options.AssemblyName + ".dll");
            }
#endif
            Init();
            return compiledTypes;
        }

        public static Object CreateTypeInstance(Type type, Object model, IFormatProvider formatProvider) {
            if (type == null)
                throw new ArgumentNullException("type");

            if (model == null)
                throw new ArgumentNullException("model");

            ConstructorInfo ctor = type.GetConstructor(new Type[] { model.GetType(), typeof(IFormatProvider) });
            if (ctor == null) {
                throw new InvalidOperationException(String.Format("The type \"{0}\" doesn't have a constructor with parameter types \"{1}\" and \"{2}\".",
                    type.Name,
                    model.GetType().Name,
                    typeof(IFormatProvider).Name));
            }

            Object typeInstance = ctor.Invoke(new Object[] { model, formatProvider });
            return typeInstance;
        }

        public static TResult CreateTypeInstance<TResult>(Type type, Object model, IFormatProvider formatProvider) {
            return (TResult)CreateTypeInstance(type, model, formatProvider);
        }

        private void Init() {
            _assemblyBuilder = null;
            _moduleBuilder = null;
            _types = new List<AssemblyCompilerTypeContext>();
        }

        internal ModuleBuilder GetModuleBuilder() {
            EnsureModuleBuilder();
            return _moduleBuilder;
        }

        internal void CompileInternal(String template, Generator generator) {
            Compile(template, generator);
        }

        private void EnsureModuleBuilder() {
            if (_moduleBuilder != null)
                return;

            if (String.IsNullOrEmpty(Options.AssemblyName))
                throw new ArgumentException("Invalid assembly name.");

#if NETCOREAPP
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Options.AssemblyName), AssemblyBuilderAccess.Run);
#else
            _assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(
                new AssemblyName { Name = Options.AssemblyName }, 
                Options.AllowSaveAssembly ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run, 
                Options.AssemblyDirectory);
#endif
            _assemblyBuilder.SetCustomAttribute(
                new CustomAttributeBuilder(CTOR_DebuggableAttribute_DebuggingModes, new Object[] {
                    DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.Default
                }));

#if NETCOREAPP
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(Options.AssemblyName + ".dll");
#else
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(Options.AssemblyName + ".dll", true);
#endif
        }
    }
}
