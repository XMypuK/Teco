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
using Teco.Parsing;

namespace Teco {
    public class RuntimeCompiler: CompilerBase {

        public RuntimeCompiler(IParser parser, CompilerOptions options) : base(parser, options) { }

        public RuntimeCompiler(CompilerOptions options) : this(new Parser(), options) { }

        public Delegate Compile(Type modelType, String template) {
            return CompileInternal(modelType, template, Options.OutputFormat);
        }

        public Delegate Compile(Type modelType, String template, OutputFormat outputFormat) {
            return CompileInternal(modelType, template, outputFormat);
        }

        public Delegate Compile(String template, OutputFormat outputFormat) {
            return CompileInternal(Options.ModelType, template, outputFormat);
        }

        public Delegate Compile(String template) {
            return CompileInternal(Options.ModelType, template, Options.OutputFormat);
        }

        public Func<TModel, IFormatProvider, String> Compile<TModel>(String template, OutputFormat outputFormat) {
            return (Func<TModel, IFormatProvider, String>)CompileInternal(typeof(TModel), template, outputFormat);
        }

        public Func<TModel, IFormatProvider, String> Compile<TModel>(String template) {
            return (Func<TModel, IFormatProvider, String>)CompileInternal(typeof(TModel), template, Options.OutputFormat);
        }

        private Delegate CompileInternal(Type modelType, String template, OutputFormat outputFormat) {
            if (modelType == null)
                throw new ArgumentNullException("modelType");

            DynamicMethod genMethod = new DynamicMethod("Build", typeof(String), new[] { modelType, typeof(IFormatProvider) }, true);
            genMethod.InitLocals = true;
#if !NETCOREAPP
            genMethod.DefineParameter(1, ParameterAttributes.In, "model");
            genMethod.DefineParameter(2, ParameterAttributes.In, "formatProvider");
#endif
            ILGenerator il = genMethod.GetILGenerator();
            InputParam modelParam = new InputParam(genMethod.GetParameters()[0]);
            InputParam formatProviderParam = new InputParam(genMethod.GetParameters()[1]);
            Generator generator = new Generator(il, outputFormat, modelParam, formatProviderParam);
            Compile(template, generator);
            Delegate buildFunc = genMethod.CreateDelegate(typeof(Func<,,>).MakeGenericType(modelType, typeof(IFormatProvider), typeof(String)));
            return buildFunc;
        }
    }
}
