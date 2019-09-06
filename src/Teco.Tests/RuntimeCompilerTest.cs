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
using System.Linq;
using System.Text;
using Teco.Tests.Model;
using Teco.Utilities;

namespace Teco.Tests {
    class RuntimeCompilerTest {
        [Test]
        public void RC_ModelAccess() {
            ModelAccessTestModel model = new ModelAccessTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());

            Assert.AreEqual(model.StrFld, compiler.Compile<ModelAccessTestModel>("{text:StrFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IntFld.ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntFld.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntFld.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntFld.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntFldNull.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntFldNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntFldNull.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntFldNull.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.StrProp, compiler.Compile<ModelAccessTestModel>("{text:StrProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IntProp.ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntProp.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntProp.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntProp.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntPropNull.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntPropNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.NullableIntPropNull.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:NullableIntPropNull.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.StrFld, compiler.Compile<ModelAccessTestModel>("{text:RefProp.StrFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.IntFld.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.IntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntFld.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntFld.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntFld.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntFldNull.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntFldNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntFldNull.HasValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntFldNull.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.StrProp, compiler.Compile<ModelAccessTestModel>("{text:RefProp.StrProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.IntProp.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.IntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntProp.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefProp.NullableIntPropNull.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefProp.NullableIntPropNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual("", compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.StrFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.IntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32?).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntFld}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Boolean).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntFld.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32?).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntFldNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Boolean).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntFldNull.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual("", compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.StrProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.IntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32?).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntProp}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Boolean).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntProp.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32?).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntPropNull}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Boolean).ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefPropNull.NullableIntPropNull.HasValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.StrArrFld[1], compiler.Compile<ModelAccessTestModel>("{text:StrArrFld[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual("", compiler.Compile<ModelAccessTestModel>("{text:StrArrFldNull[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.StrArrProp[1], compiler.Compile<ModelAccessTestModel>("{text:StrArrProp[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual("", compiler.Compile<ModelAccessTestModel>("{text:StrArrPropNull[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.StrArrFld.Length.ToString(), compiler.Compile<ModelAccessTestModel>("{text:StrArrFld.Length}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IntArrFld[1].ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntArrFld[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32).ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntArrFldNull[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IntArrProp[1].ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntArrProp[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(default(Int32).ToString(), compiler.Compile<ModelAccessTestModel>("{text:IntArrPropNull[1]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefArrProp[2].StrValue, compiler.Compile<ModelAccessTestModel>("{text:RefArrProp[2].StrValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefArrProp[0].IntValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefArrProp[0].IntValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.ValArrProp[2].StrValue, compiler.Compile<ModelAccessTestModel>("{text:ValArrProp[2].StrValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.ValArrProp[0].IntValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:ValArrProp[0].IntValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.StrArrFld) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrFld}{text:this};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.StrArrProp) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrProp}{text:this};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(0, model.StrArrFld.Length).Select(n => n.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrFld}{text:thisIndex};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(0, model.StrArrProp.Length).Select(n => n.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrProp}{text:thisIndex};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(1, model.StrArrFld.Length).Select(n => n.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrFld}{text:thisNum};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(1, model.StrArrProp.Length).Select(n => n.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrProp}{text:thisNum};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(0, model.StrArrFld.Length).Select(n => model.StrArrFld.Length.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrFld}{text:thisCount};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", Enumerable.Range(0, model.StrArrProp.Length).Select(n => model.StrArrProp.Length.ToString()).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:StrArrProp}{text:thisCount};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.RefArrProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:RefArrProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.ValArrProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:ValArrProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefListProp[2].StrValue, compiler.Compile<ModelAccessTestModel>("{text:RefListProp[2].StrValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.RefListProp[0].IntValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:RefListProp[0].IntValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.ValListProp[2].StrValue, compiler.Compile<ModelAccessTestModel>("{text:ValListProp[2].StrValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.ValListProp[0].IntValue.ToString(), compiler.Compile<ModelAccessTestModel>("{text:ValListProp[0].IntValue}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.RefListProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:RefListProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.ValListProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:ValListProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.RefEnumProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:RefEnumProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(String.Join(";", model.ValEnumProp.Select(e => e.StrValue + "+" + e.IntValue).ToArray()) + ";", compiler.Compile<ModelAccessTestModel>("{each:ValEnumProp}{text:StrValue}+{text:IntValue};{/each}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IndexedRefProp["Test"], compiler.Compile<ModelAccessTestModel>("{text:IndexedRefProp[\"Test\"]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IndexedRefProp[3].ToString(), compiler.Compile<ModelAccessTestModel>("{text:IndexedRefProp[3]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IndexedValProp["tseT"], compiler.Compile<ModelAccessTestModel>("{text:IndexedValProp[\"tseT\"]}", OutputFormat.Text)(model, null));
            Assert.AreEqual(model.IndexedValProp[7].ToString(), compiler.Compile<ModelAccessTestModel>("{text:IndexedValProp[7]}", OutputFormat.Text)(model, null));
        }


        [Test]
        public void RC_TextToken() {
            var model = new TextTokenTestModel();
            var cultureArr = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo(1049) };
            foreach (CultureInfo culture in cultureArr) {
                RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
                Assert.AreEqual(model.TextProperty, compiler.Compile(model.GetType(), "{text:TextProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(model.HtmlProperty, compiler.Compile(model.GetType(), "{text:HtmlProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(EncodeUtility.HtmlEncode(model.HtmlProperty), compiler.Compile(model.GetType(), "{text:HtmlProperty}", OutputFormat.Html).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0}", model.IntProperty), compiler.Compile(model.GetType(), "{text:IntProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:0000.00}", model.IntProperty), compiler.Compile(model.GetType(), "{text:IntProperty:0000.00}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0}", model.FloatProperty), compiler.Compile(model.GetType(), "{text:FloatProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:0.##}", model.FloatProperty), compiler.Compile(model.GetType(), "{text:FloatProperty:0.##}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy}", model.DateTimeProperty), compiler.Compile(model.GetType(), "{text:DateTimeProperty:dd.MM.yyyy}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy HH:mm:ss}", model.DateTimeProperty), compiler.Compile(model.GetType(), "{text:DateTimeProperty:dd.MM.yyyy HH:mm:ss}", OutputFormat.Text).DynamicInvoke(model, culture));
            }
        }

        [Test]
        public void RC_HtmlToken() {
            var model = new TextTokenTestModel();
            var cultureArr = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo(1049) };
            foreach (CultureInfo culture in cultureArr) {
                RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
                Assert.AreEqual(model.TextProperty, compiler.Compile(model.GetType(), "{html:TextProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(model.HtmlProperty, compiler.Compile(model.GetType(), "{html:HtmlProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(model.HtmlProperty, compiler.Compile(model.GetType(), "{html:HtmlProperty}", OutputFormat.Html).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0}", model.IntProperty), compiler.Compile(model.GetType(), "{html:IntProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:0000.00}", model.IntProperty), compiler.Compile(model.GetType(), "{html:IntProperty:0000.00}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0}", model.FloatProperty), compiler.Compile(model.GetType(), "{html:FloatProperty}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:0.##}", model.FloatProperty), compiler.Compile(model.GetType(), "{html:FloatProperty:0.##}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy}", model.DateTimeProperty), compiler.Compile(model.GetType(), "{html:DateTimeProperty:dd.MM.yyyy}", OutputFormat.Text).DynamicInvoke(model, culture));
                Assert.AreEqual(String.Format(culture, "{0:dd.MM.yyyy HH:mm:ss}", model.DateTimeProperty), compiler.Compile(model.GetType(), "{html:DateTimeProperty:dd.MM.yyyy hh:mm:ss}", OutputFormat.Text).DynamicInvoke(model, culture));
            }
        }

        [Test]
        public void RC_UrlToken() {
            UrlTokenTestModel model = new UrlTokenTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());

            Assert.AreEqual("%22english%20text%2C%20%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9%20%D1%82%D0%B5%D0%BA%D1%81%D1%82%22", (String)compiler.Compile(model.GetType(), "{url/p:TextProperty}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("%22english%20text,%20%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B9%20%D1%82%D0%B5%D0%BA%D1%81%D1%82%22", (String)compiler.Compile(model.GetType(), "{url:TextProperty}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual(EncodeUtility.UrlEncodeAsUrl(model.UrlProperty, false), (String)compiler.Compile(model.GetType(), "{url:UrlProperty}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual(EncodeUtility.UrlEncode(model.UrlProperty, false), (String)compiler.Compile(model.GetType(), "{url/p:UrlProperty}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual(EncodeUtility.UrlEncodeAsUrl(model.UrlProperty, true), (String)compiler.Compile(model.GetType(), "{url/a:UrlProperty}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual(EncodeUtility.HtmlEncode(EncodeUtility.UrlEncodeAsUrl(model.UrlProperty, false)), (String)compiler.Compile(model.GetType(), "{url:UrlProperty}", OutputFormat.Html).DynamicInvoke(model, null));
            Assert.AreEqual(EncodeUtility.HtmlEncode(EncodeUtility.UrlEncodeAsUrl(model.UrlProperty, true)), (String)compiler.Compile(model.GetType(), "{url/a:UrlProperty}", OutputFormat.Html).DynamicInvoke(model, null));
        }

        [Test]
        public void RC_IfToken() {
            ConditionalTokenTestModel model = new ConditionalTokenTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:Bool_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:Int32_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:UInt32_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:Int16_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:UInt16_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:Int64_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:UInt64_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:String_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{if:Object_True}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Bool_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Int32_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:UInt32_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Int16_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:UInt16_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Int64_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:UInt64_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:String_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Object_False}condition text{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("[Level1][Level2][/Level2][/Level1]", compiler.Compile(model.GetType(), "{if:Object_True}[Level1]{if:Int32_True}[Level2][/Level2]{/if}[/Level1]{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("[Level1][/Level1]", compiler.Compile(model.GetType(), "{if:Object_True}[Level1]{if:Int32_False}[Level2][/Level2]{/if}[/Level1]{/if}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{if:Object_False}[Level1]{if:Int32_True}[Level2][/Level2]{/if}[/Level1]{/if}", OutputFormat.Text).DynamicInvoke(model, null));
        }

        [Test]
        public void RC_IfNotToken() {
            ConditionalTokenTestModel model = new ConditionalTokenTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Bool_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Int32_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:UInt32_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Int16_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:UInt16_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Int64_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:UInt64_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:String_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Object_True}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:Bool_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:Int32_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:UInt32_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:Int16_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:UInt16_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:Int64_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:UInt64_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:String_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("condition text", compiler.Compile(model.GetType(), "{ifnot:Object_False}condition text{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("[Level1][Level2][/Level2][/Level1]", compiler.Compile(model.GetType(), "{ifnot:Object_False}[Level1]{ifnot:Int32_False}[Level2][/Level2]{/ifnot}[/Level1]{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("[Level1][/Level1]", compiler.Compile(model.GetType(), "{ifnot:Object_False}[Level1]{ifnot:Int32_True}[Level2][/Level2]{/ifnot}[/Level1]{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{ifnot:Object_True}[Level1]{ifnot:Int32_False}[Level2][/Level2]{/ifnot}[/Level1]{/ifnot}", OutputFormat.Text).DynamicInvoke(model, null));
        }

        [Test]
        public void RC_EachToken() {
            EachTokenTestModel model = new EachTokenTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
            Assert.AreEqual("", compiler.Compile(model.GetType(), "{each:EmptyList}text{/each}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("(1/7)2 (2/7)1 (3/7)2 (4/7)8 (5/7)5 (6/7)0 (7/7)6 ", compiler.Compile(model.GetType(), "{each:ValueList}({text:thisNum}/{text:thisCount}){text:this} {/each}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("0:Item 1; 1:Item 2; 2:Item 3; ", compiler.Compile(model.GetType(), "{each:ModelList}{text:thisIndex}:{text:Text}; {/each}", OutputFormat.Text).DynamicInvoke(model, null));
            Assert.AreEqual("to be or not to be ", compiler.Compile(model.GetType(), "{each:ComplexModel}{each:Children}{text:this}{/each} {/each}", OutputFormat.Text).DynamicInvoke(model, null));
        }

        [Test]
        public void RC_Order() {
            OrderTestModel model = new OrderTestModel();
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());

            var cultureArr = new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo(1049) };
            foreach (CultureInfo culture in cultureArr) {
                String templateString =
@"
Dear {text:Customer}

Your order:
{each:OrderList}{text:thisNum}. {text:Title} x {text:Quantity} : {text:Total:0.##$}
{/each}
{if:Discount}Order total without discount: {text:OrderTotal:0.##$}
Discount: {text:Discount:0'%'}{/if}{ifnot:Discount}You should order more products to get discount.{/ifnot}
Total: {text:OrderTotalWithDiscount:0.##$}
";

                StringBuilder expectedMessage = new StringBuilder();
                expectedMessage.AppendFormat(culture, "\r\nDear {0}\r\n\r\nYour order:\r\n", model.Customer);
                Int32 itemNum = 0;
                foreach (OrderItem item in model.OrderList) {
                    expectedMessage.AppendFormat(culture, "{0}. {1} x {2} : {3:0.##$}\r\n", ++itemNum, item.Title, item.Quantity, item.Total);
                }
                if (model.Discount != 0.0) {
                    expectedMessage.AppendFormat(culture, "\r\nOrder total without discount: {0:0.##$}\r\n", model.OrderTotal);
                    expectedMessage.AppendFormat(culture, "Discount: {0:0'%'}\r\n", model.Discount);
                }
                else {
                    expectedMessage.AppendFormat(culture, "\r\nYou should order more products to get discount.\r\n");
                }
                expectedMessage.AppendFormat(culture, "Total: {0:0.##$}\r\n", model.OrderTotalWithDiscount);
                Assert.AreEqual(expectedMessage.ToString(), compiler.Compile(model.GetType(), templateString, OutputFormat.Text).DynamicInvoke(model, culture));
            }
        }

        [Test]
        public void RC_Syntax() {
            String[] errorTemplates = new String[] {
                "{if:Condition}",
                "{if:Condition}test{/if}{/if}",
                "{when:Something}{if:Condition}test{/if}{/when}",
                "{when:Something}{ifnot:Condition}test{/ifnot}{/when}",
                "{when:Something}{text:VarPath}{/when}",
                "{when:Something}{html:VarPath}{/when}",
                "{when:Something}{url:VarPath}{/when}",
                "{when:Something}{eq:@Const}{/eq}{each:Collection}{/each}{else}{/else}{/when}",
                "{when:Something}{when:Foo}{eq:@Const}test{/eq}{/when}{/when}",
                "{when:Something}test{/when}",
                "{eq:@Const}test{/eq}",
                "{begins:@Const}test{/begins}",
                "{ends:@Const}test{/ends}",
                "{contains:@Const}test{/contains}",
                "{else}test{/else}"
            };
            RuntimeCompiler compiler = new RuntimeCompiler(new CompilerOptions());
            foreach (String template in errorTemplates) {
                try {
                    compiler.Compile<Object>(template);
                    throw new AssertionException(String.Format("Syntax exception was expected, but it was not thrown. Template: \"{0}\".", template));
                }
                catch (SyntaxException) { }
            }
        }

    }
}
