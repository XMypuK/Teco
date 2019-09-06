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

using NUnit.Framework;
using System;
using System.Globalization;
using Teco.Tests.Model;
using Teco.Utilities;

namespace Teco.Tests {
    class AssemblyCompilerTest {
        [Test]
        public void AC_TextToken() {
            TextTokenTestModel model = new TextTokenTestModel();
            AssemblyCompilerOptions compilerOptions = new AssemblyCompilerOptions() {
                ModelType = model.GetType()
            };
#if !NETCOREAPP
            compilerOptions.AssemblyDirectory = TestContext.CurrentContext.TestDirectory;
            compilerOptions.AllowSaveAssembly = true;
#endif
            AssemblyCompiler compiler = new AssemblyCompiler(compilerOptions);
            AssemblyCompilerTypeContext typeContext = compiler.AddType("TextTokenTest", typeof(ITextTokenTestResultModel));
            typeContext.AddProperty("TextProperty", "{text:TextProperty}", OutputFormat.Text);
            typeContext.AddProperty("HtmlProperty", "{text:HtmlProperty}", OutputFormat.Text);
            typeContext.AddProperty("EncodedHtmlProperty", "{text:HtmlProperty}", OutputFormat.Html);
            typeContext.AddProperty("IntProperty", "{text:IntProperty}", OutputFormat.Text);
            typeContext.AddProperty("FormattedIntProperty", "{text:IntProperty:0000.00}", OutputFormat.Text);
            typeContext.AddProperty("FloatProperty", "{text:FloatProperty}", OutputFormat.Text);
            typeContext.AddProperty("FormattedFloatProperty", "{text:FloatProperty:0.##}", OutputFormat.Text);
            typeContext.AddProperty("DateTimeProperty", "{text:DateTimeProperty:dd.MM.yyyy}", OutputFormat.Text);
            typeContext.AddProperty("FormattedDateTimeProperty", "{text:DateTimeProperty:dd.MM.yyyy HH:mm:ss}", OutputFormat.Text);
#if NETCOREAPP
            Type[] compiledTypes = compiler.GetCompiledTypes();
#else
            Type[] compiledTypes = compiler.GetCompiledTypes(true);
#endif
            Assert.IsNotNull(compiledTypes);
            Assert.AreEqual(1, compiledTypes.Length);
            Type type = compiledTypes[0];
            Assert.IsNotNull(type);
            Assert.IsNotNull(type.GetInterface(typeof(ITextTokenTestResultModel).FullName));
            CultureInfo[] cultureArr = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo(1049) };
            foreach (CultureInfo culture in cultureArr) {
                ITextTokenTestResultModel result = AssemblyCompiler.CreateTypeInstance<ITextTokenTestResultModel>(type, model, culture);
                Assert.AreEqual(model.TextProperty, result.TextProperty);
                Assert.AreEqual(model.HtmlProperty, result.HtmlProperty);
                Assert.AreEqual(EncodeUtility.HtmlEncode(model.HtmlProperty), result.EncodedHtmlProperty);
                Assert.AreEqual(String.Format(culture, "{0}", model.IntProperty), result.IntProperty);
                Assert.AreEqual(String.Format(culture, "{0:0000.00}", model.IntProperty), result.FormattedIntProperty);
                Assert.AreEqual(String.Format(culture, "{0}", model.FloatProperty), result.FloatProperty);
                Assert.AreEqual(String.Format(culture, "{0:0.##}", model.FloatProperty), result.FormattedFloatProperty);
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy}", model.DateTimeProperty), result.DateTimeProperty);
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy HH:mm:ss}", model.DateTimeProperty), result.FormattedDateTimeProperty);
            }
        }

        [Test]
        public void AC_HtmlToken() {
            TextTokenTestModel model = new TextTokenTestModel();
            AssemblyCompilerOptions compilerOptions = new AssemblyCompilerOptions() {
                ModelType = model.GetType()
            };
#if !NETCOREAPP
            compilerOptions.AssemblyDirectory = TestContext.CurrentContext.TestDirectory;
            compilerOptions.AllowSaveAssembly = true;
#endif
            AssemblyCompiler compiler = new AssemblyCompiler(compilerOptions);
            AssemblyCompilerTypeContext typeContext = compiler.AddType("TextTokenTest", typeof(ITextTokenTestResultModel));
            typeContext.AddProperty("TextProperty", "{html:TextProperty}", OutputFormat.Text);
            typeContext.AddProperty("HtmlProperty", "{html:HtmlProperty}", OutputFormat.Text);
            typeContext.AddProperty("EncodedHtmlProperty", "{html:HtmlProperty}", OutputFormat.Html);
            typeContext.AddProperty("IntProperty", "{html:IntProperty}", OutputFormat.Text);
            typeContext.AddProperty("FormattedIntProperty", "{html:IntProperty:0000.00}", OutputFormat.Text);
            typeContext.AddProperty("FloatProperty", "{html:FloatProperty}", OutputFormat.Text);
            typeContext.AddProperty("FormattedFloatProperty", "{html:FloatProperty:0.##}", OutputFormat.Text);
            typeContext.AddProperty("DateTimeProperty", "{html:DateTimeProperty:dd.MM.yyyy}", OutputFormat.Text);
            typeContext.AddProperty("FormattedDateTimeProperty", "{html:DateTimeProperty:dd.MM.yyyy HH:mm:ss}", OutputFormat.Text);
#if NETCOREAPP
            Type[] compiledTypes = compiler.GetCompiledTypes();
#else
            Type[] compiledTypes = compiler.GetCompiledTypes(true);
#endif
            Assert.IsNotNull(compiledTypes);
            Assert.AreEqual(1, compiledTypes.Length);
            Type type = compiledTypes[0];
            Assert.IsNotNull(type);
            Assert.IsNotNull(type.GetInterface(typeof(ITextTokenTestResultModel).FullName));
            CultureInfo[] cultureArr = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo(1049) };
            foreach (CultureInfo culture in cultureArr) {
                ITextTokenTestResultModel result = AssemblyCompiler.CreateTypeInstance<ITextTokenTestResultModel>(type, model, culture);
                Assert.AreEqual(model.TextProperty, result.TextProperty);
                Assert.AreEqual(model.HtmlProperty, result.HtmlProperty);
                Assert.AreEqual(model.HtmlProperty, result.EncodedHtmlProperty);
                Assert.AreEqual(String.Format(culture, "{0}", model.IntProperty), result.IntProperty);
                Assert.AreEqual(String.Format(culture, "{0:0000.00}", model.IntProperty), result.FormattedIntProperty);
                Assert.AreEqual(String.Format(culture, "{0}", model.FloatProperty), result.FloatProperty);
                Assert.AreEqual(String.Format(culture, "{0:0.##}", model.FloatProperty), result.FormattedFloatProperty);
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy}", model.DateTimeProperty), result.DateTimeProperty);
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy HH:mm:ss}", model.DateTimeProperty), result.FormattedDateTimeProperty);
            }
        }

    }
}
