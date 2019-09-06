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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Teco.Utilities;

namespace Teco.Generating {
    public class Generator {
        static readonly Type T_Object = typeof(Object);
        static readonly Type T_String = typeof(String);
        static readonly Type T_StringBuilder = typeof(StringBuilder);
        static readonly Type T_StringComparison = typeof(StringComparison);
        static readonly Type T_EncodeUtility = typeof(EncodeUtility);
        static readonly Type T_Nullable_1 = typeof(Nullable<>);
        static readonly Type T_Int16 = typeof(Int16);
        static readonly Type T_Int32 = typeof(Int32);
        static readonly Type T_Int64 = typeof(Int64);
        static readonly Type T_UInt16 = typeof(UInt16);
        static readonly Type T_UInt32 = typeof(UInt32);
        static readonly Type T_UInt64 = typeof(UInt64);
        static readonly Type T_Byte = typeof(Byte);
        static readonly Type T_SByte = typeof(SByte);
        static readonly Type T_Single = typeof(Single);
        static readonly Type T_Double = typeof(Double);
        static readonly Type T_Boolean = typeof(Boolean);
        static readonly Type T_IEnumerable = typeof(IEnumerable);
        static readonly Type T_IEnumerable_1 = typeof(IEnumerable<>);
        static readonly Type T_IEnumerator = typeof(IEnumerator);
        static readonly Type T_ICollection = typeof(ICollection);
        static readonly Type T_ICollection_1 = typeof(ICollection<>);
        static readonly Type T_IList = typeof(IList);
        static readonly Type T_IList_1 = typeof(IList<>);
        static readonly Type T_IFormatProvider = typeof(IFormatProvider);
        static readonly Type T_CultureInfo = typeof(CultureInfo);
        static readonly Type T_ObjectArray = typeof(Object[]);
        static readonly MethodInfo IM_String_IndexOf_String_StringComparison = T_String.GetMethod("IndexOf", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_StringComparison }, null);
        static readonly MethodInfo IM_String_StartsWith_String_StringComparison = T_String.GetMethod("StartsWith", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_StringComparison }, null);
        static readonly MethodInfo IM_String_EndsWith_String_StringComparison = T_String.GetMethod("EndsWith", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_StringComparison }, null);
        static readonly MethodInfo SM_String_Format_IFormatProvider_String_ObjectArray = T_String.GetMethod("Format", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_IFormatProvider, T_String, T_ObjectArray }, null);
        static readonly MethodInfo IM_StringBuilder_Append_String = T_StringBuilder.GetMethod("Append", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String }, null);
        static readonly MethodInfo IM_StringBuilder_ToString = T_StringBuilder.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[0], null);
        static readonly MethodInfo IM_StringBuilder_AppendFormat_IFormatProvider_String_ObjectArray = T_StringBuilder.GetMethod("AppendFormat", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_IFormatProvider, T_String, T_ObjectArray }, null);
        static readonly MethodInfo IM_IEnumerable_GetEnumerator = T_IEnumerable.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[0], null);
        static readonly MethodInfo IM_IEnumerator_MoveNext = T_IEnumerator.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[0], null);
        static readonly MethodInfo SM_String_IsNullOrEmpty_String = T_String.GetMethod("IsNullOrEmpty", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String }, null);
        static readonly MethodInfo SM_String_Equals_String_String_StringComparison = T_String.GetMethod("Equals", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_String, T_StringComparison }, null);
        static readonly MethodInfo SM_EncodeUtility_HtmlEncode_String = T_EncodeUtility.GetMethod("HtmlEncode", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String }, null);
        static readonly MethodInfo SM_EncodeUtility_UrlEncodeAsUrl_String_Boolean = T_EncodeUtility.GetMethod("UrlEncodeAsUrl", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_Boolean }, null);
        static readonly MethodInfo SM_EncodeUtility_UrlEncode_String_Boolean = T_EncodeUtility.GetMethod("UrlEncode", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new[] { T_String, T_Boolean }, null);
        static readonly ConstructorInfo CTOR_StringBuilder = T_StringBuilder.GetConstructor(new Type[0]);

        ILGenerator _il;
        InputParam _modelParam;
        InputParam _formatProviderParam;
        OutputFormat _outputFormat;
        Scope _scope;
        LocalPool _locals;

        /// <summary>
        /// Creates new instance of generator.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="outputFormat">Determines generator output format: Html or Text. Using Html triggers applying of HTML encoding in some cases.</param>
        /// <param name="modelParam">A parameter with the model which will be used to build the result string from a template.</param>
        /// <param name="formatProviderParam"></param>
        public Generator(ILGenerator il, OutputFormat outputFormat, InputParam modelParam, InputParam formatProviderParam) {
            this._il = il;
            this._modelParam = modelParam;
            this._formatProviderParam = formatProviderParam;
            this._outputFormat = outputFormat;
            this._scope = null;
            this._locals = new LocalPool(il);
        }

        /// <summary>
        /// Extracts and returns an interface from a type implementing it.
        /// </summary>
        private Type ExtractInterface(Type type, Type interfaceType) {
            return (type.Name == interfaceType.Name && type.Namespace == interfaceType.Namespace)
                ? type
                : type.GetInterface(interfaceType.Namespace + "." + interfaceType.Name);
        }

        /// <summary>
        /// Determines if a type is a kind of generic Nullable<T> type.
        /// </summary>
        private Boolean IsNullableStruct(Type type) {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == T_Nullable_1;
        }

        private void GenLdRefToInputParam(InputParam param) {
            switch (param.Source) {
                case InputParamSource.Param: GenLdRefToMethodParam(param.Index, param.Type); break;
                case InputParamSource.Local: GenLdRefToLocal(param.Local); break;
                case InputParamSource.Field:
                    _il.Emit(OpCodes.Ldarg_0);
                    GenLdRefToField(param.Field);
                    break;
            }
        }

        private void GenLdInputParam(InputParam param) {
            switch (param.Source) {
                case InputParamSource.Param: GenLdMethodParam(param.Index); break;
                case InputParamSource.Local: GenLdLocal(param.Local); break;
                case InputParamSource.Field:
                    _il.Emit(OpCodes.Ldarg_0);
                    GenLdField(param.Field);
                    break;
            }
        }

        /// <summary>
        /// Produces optimal code to put a reference to param on the stack.
        /// </summary>
        private void GenLdRefToMethodParam(Int32 index, Type type) {
            if (type.IsValueType) {
                _il.Emit(OpCodes.Ldarga_S, (Byte)index);
            }
            else {
                GenLdMethodParam(index);
            }
        }

        /// <summary>
        /// Produces optimal code to put a param on the stack.
        /// </summary>
        private void GenLdMethodParam(Int32 index) {
            switch (index) {
                case 0: _il.Emit(OpCodes.Ldarg_0); break;
                case 1: _il.Emit(OpCodes.Ldarg_1); break;
                case 2: _il.Emit(OpCodes.Ldarg_2); break;
                case 3: _il.Emit(OpCodes.Ldarg_3); break;
                default:
                    if (index < 256)
                        _il.Emit(OpCodes.Ldarg_S, (Byte)index);
                    else
                        _il.Emit(OpCodes.Ldarg, index);
                    break;
            }
        }

        /// <summary>
        /// Produces optimal code to put a reference to local on the stack.
        /// </summary>
        private void GenLdRefToLocal(LocalBuilder local) {
            if (local.LocalType.IsValueType)
                _il.Emit(OpCodes.Ldloca, local);
            else
                GenLdLocal(local);
        }

        /// <summary>
        /// Produces optimal code to put a local on the stack.
        /// </summary>
        private void GenLdLocal(LocalBuilder local) {
            _il.Emit(OpCodes.Ldloc, local);
        }

        /// <summary>
        /// Produces optimal code to put a reference to field on the stack.
        /// </summary>
        private void GenLdRefToField(FieldInfo field) {
            if (field.FieldType.IsValueType)
                _il.Emit(OpCodes.Ldflda, field);
            else
                GenLdField(field);
        }

        /// <summary>
        /// Produces optimal code to put a field on the stack.
        /// </summary>
        private void GenLdField(FieldInfo field) {
            _il.Emit(OpCodes.Ldfld, field);
        }

        /// <summary>
        /// Produces optimal code to put an element of array on the stack.
        /// </summary>
        private void GenLdElem(Type elemType) {
            if (elemType.IsValueType) {
                if (elemType == T_Int32)
                    _il.Emit(OpCodes.Ldelem_I4);
                else if (elemType == T_Int16)
                    _il.Emit(OpCodes.Ldelem_I2);
                else if (elemType == T_Int64)
                    _il.Emit(OpCodes.Ldelem_I8);
                else if (elemType == T_Byte)
                    _il.Emit(OpCodes.Ldelem_U1);
                else if (elemType == T_UInt32)
                    _il.Emit(OpCodes.Ldelem_U4);
                else if (elemType == T_UInt16)
                    _il.Emit(OpCodes.Ldelem_U2);
                else if (elemType == T_SByte)
                    _il.Emit(OpCodes.Ldelem_I1);
                else if (elemType == T_Single)
                    _il.Emit(OpCodes.Ldelem_R4);
                else if (elemType == T_Double)
                    _il.Emit(OpCodes.Ldelem_R8);
                else
                    _il.Emit(OpCodes.Ldelem, elemType);
            }
            else {
                _il.Emit(OpCodes.Ldelem, elemType);
            }
        }

        /// <summary>
        /// Produces code intended to get the value of an expression in the current scope.
        /// Expression is being splitted on fragments. First fragment is either a reference to model
        /// or a reference to local variable of scope. Every subsequent fragment is either a member
        /// of type got from previous fragment or a indexer (by integer or string). The result value
        /// is stored to a local.
        /// It may happen that one or more fragments have a reference type which means that in
        /// runtime they can become null value. In this case the default value of corresponding 
        /// output type will be stored to result local. Output type is determined by the last
        /// fragment.
        /// </summary>
        /// <param name="expression">Expression which describes a path to the value.</param>
        /// <param name="scope">Current scope.</param>
        /// <returns>A reference to the result local.</returns>
        private LocalBuilder GenResolveExpressionValue(String expression, Scope scope) {
            ValuePath valuePath = new ValuePath(expression);
            ValueAccessorChain valueAccessorChain = new ValueAccessorChain(_modelParam, scope, valuePath);
            if (valueAccessorChain.IsBroken)
                return null;

            ValueAccessor last = valueAccessorChain.Accessors.Last();
            LocalBuilder resultLocal = _locals.Reserve(last.ResultType);
            List<LocalBuilder> tempLocals = new List<LocalBuilder>();

            Label labOnNull = _il.DefineLabel();
            Label labEnd = _il.DefineLabel();

            for (Int32 i = 0; i < valueAccessorChain.Accessors.Length; i++) {
                ValueAccessor cur = valueAccessorChain.Accessors[i];
                if (i == 0 && !cur.IsLocal && !cur.IsParam)
                    throw new InvalidOperationException();
                if (i > 0 && (cur.IsLocal || cur.IsParam))
                    throw new InvalidOperationException();
                Boolean isLast = (i + 1) == valueAccessorChain.Accessors.Length;
                // get the value on the stack
                if (cur.IsParam) {
                    if (isLast) GenLdInputParam(cur.Param);
                    else GenLdRefToInputParam(cur.Param);
                }
                else if (cur.IsLocal) {
                    if (isLast) GenLdLocal(cur.Local);
                    else GenLdRefToLocal(cur.Local);
                }
                else if (cur.IsField) {
                    if (isLast) GenLdField((FieldInfo)cur.Member);
                    else GenLdRefToField((FieldInfo)cur.Member);
                }
                else {
                    if (cur.IsProperty) {
                        PropertyInfo prop = (PropertyInfo)cur.Member;
                        if (cur.IsIndexer) {
                            ParameterInfo indexInfo = prop.GetIndexParameters()[0];
                            if (indexInfo.ParameterType == T_String)
                                _il.Emit(OpCodes.Ldstr, (String)cur.Index);
                            else if (indexInfo.ParameterType == T_Int32)
                                _il.Emit(OpCodes.Ldc_I4, (Int32)cur.Index);
                            else
                                throw new InvalidOperationException();
                        }
                        _il.Emit(
                            prop.DeclaringType.IsValueType ? OpCodes.Call : OpCodes.Callvirt,
                            prop.GetGetMethod());
                    }
                    else if (cur.IsIndexer) {
                        _il.Emit(OpCodes.Ldc_I4, (Int32)cur.Index);
                        GenLdElem(cur.ResultType);
                    }
                    else {
                        throw new InvalidOperationException();
                    }
                    // if there is next accessor to process and the value has a value type
                    // then we need a reference to the value rather than value itself
                    if (!isLast && cur.ResultType.IsValueType) {
                        LocalBuilder temp = _locals.Reserve(cur.ResultType);
                        tempLocals.Add(temp);
                        _il.Emit(OpCodes.Stloc, temp);
                        GenLdRefToLocal(temp);
                    }
                }
                // at this point the value of the current acessor is on the top of stack
                if (isLast) {
                    // store the result
                    _il.Emit(OpCodes.Stloc, resultLocal);
                    _il.Emit(OpCodes.Br, labEnd);
                }
                else if (!cur.ResultType.IsValueType) {
                    // if the value has a reference type then check it for null
                    _il.Emit(OpCodes.Dup); 
                    _il.Emit(OpCodes.Brfalse, labOnNull);
                }
            }
            // release temp locals for further use
            foreach (LocalBuilder local in tempLocals) {
                _locals.Free(local);
            }

            _il.MarkLabel(labOnNull);
            // remove null value from stack
            _il.Emit(OpCodes.Pop);
            // assign default value to result local
            if (last.ResultType.IsValueType) {
                GenLdRefToLocal(resultLocal);
                _il.Emit(OpCodes.Initobj, resultLocal.LocalType);
            }
            else {
                _il.Emit(OpCodes.Ldnull);
                _il.Emit(OpCodes.Stloc, resultLocal);
            }
            _il.MarkLabel(labEnd);

            return resultLocal;
        }

        /// <summary>
        /// Produces code which checks a local value for 'true' and jumps or not to
        /// a target label depending on checking result.
        /// Next values are treated as 'true':
        /// - Boolean type: true;
        /// - any integer type: not zero;
        /// - Nullable<T> type: HasValue property returns true;
        /// - String: not null and not empty;
        /// - any reference type: not null;
        /// - any other value type: always true.
        /// </summary>
        /// <param name="locCond">A local variable to check.</param>
        /// <param name="labTarget">Target label to jump to.</param>
        /// <param name="jumpValue">A value on which a jump will be performed.</param>
        private void GenCondJump(LocalBuilder locCond, Label labTarget, Boolean jumpValue) {
            if (locCond.LocalType == T_Boolean) {
                GenLdLocal(locCond);
                _il.Emit(jumpValue ? OpCodes.Brtrue : OpCodes.Brfalse, labTarget);
            }
            else if (locCond.LocalType == T_Int32 || locCond.LocalType == T_Int16 || locCond.LocalType == T_UInt32 || locCond.LocalType == T_UInt16) {
                GenLdLocal(locCond);
                _il.Emit(OpCodes.Ldc_I4_0);
                _il.Emit(jumpValue ? OpCodes.Bne_Un : OpCodes.Beq, labTarget);
            }
            else if (locCond.LocalType == T_Int64 || locCond.LocalType == T_UInt64) {
                GenLdLocal(locCond);
                _il.Emit(OpCodes.Ldc_I8, 0L);
                _il.Emit(jumpValue ? OpCodes.Bne_Un : OpCodes.Beq, labTarget);
            }
            else if (IsNullableStruct(locCond.LocalType)) {
                PropertyInfo hasValuePropInfo = locCond.LocalType.GetProperty("HasValue", BindingFlags.Instance | BindingFlags.Public);
                GenLdLocal(locCond);
                _il.Emit(OpCodes.Call, hasValuePropInfo.GetGetMethod());
                _il.Emit(jumpValue ? OpCodes.Brtrue : OpCodes.Brfalse, labTarget);
            }
            else if (locCond.LocalType == T_String) {
                GenLdLocal(locCond);
                _il.Emit(OpCodes.Call, SM_String_IsNullOrEmpty_String);
                _il.Emit(jumpValue ? OpCodes.Brfalse : OpCodes.Brtrue, labTarget);
            }
            else if (!locCond.LocalType.IsValueType) {
                GenLdLocal(locCond);
                _il.Emit(jumpValue ? OpCodes.Brtrue : OpCodes.Brfalse, labTarget);
            }
            else {
                if (jumpValue)
                    _il.Emit(OpCodes.Br, labTarget);
            }
        }

        /// <summary>
        /// Analysis the provided type of collection, determines which interfaces
        /// are implemented and extracts an element type of collection.
        /// </summary>
        /// <param name="type">Type of collection.</param>
        /// <param name="elementType">Element type of collection.</param>
        /// <returns>A set of flags corresponding to interfaces implemented by collection.</returns>
        private CollectionTypeFlags AnalyzeCollection(Type type, out Type elementType) {
            elementType = T_Object;
            if (type.IsArray) {
                elementType = type.GetElementType();
                return CollectionTypeFlags.Array | CollectionTypeFlags.IEnumerate;
            }
            Type genericIList = ExtractInterface(type, T_IList_1);
            if (genericIList != null) {
                elementType = genericIList.GetGenericArguments()[0];
                return CollectionTypeFlags.IList | CollectionTypeFlags.ICollection | CollectionTypeFlags.IEnumerate | CollectionTypeFlags.Generic;
            }
            Type genericICollection = ExtractInterface(type, T_ICollection_1);
            if (genericICollection != null) {
                elementType = genericICollection.GetGenericArguments()[0];
                return CollectionTypeFlags.ICollection | CollectionTypeFlags.IEnumerate | CollectionTypeFlags.Generic;
            }
            Type genericIEnumerable = ExtractInterface(type, T_IEnumerable_1);
            if (genericIEnumerable != null) {
                elementType = genericIEnumerable.GetGenericArguments()[0];
                return CollectionTypeFlags.IEnumerate | CollectionTypeFlags.Generic;
            }
            Type iList = ExtractInterface(type, T_IList);
            if (iList != null) {
                return CollectionTypeFlags.IList | CollectionTypeFlags.ICollection | CollectionTypeFlags.IEnumerate;
            }
            Type iCollection = ExtractInterface(type, T_ICollection);
            if (iCollection != null) {
                return CollectionTypeFlags.ICollection | CollectionTypeFlags.IEnumerate;
            }
            Type iEnumerable = ExtractInterface(type, T_IEnumerable);
            if (iEnumerable != null) {
                return CollectionTypeFlags.IEnumerate;
            }
            return CollectionTypeFlags.None;
        }

        /// <summary>
        /// Produces code which takes a local variable and applies String.Format
        /// method to it with the format specified. The result string will be on the
        /// stack.
        /// </summary>
        /// <param name="formatProviderParam"></param>
        /// <param name="locValue">A local variable to be formatted.</param>
        /// <param name="format">A format string to be applied to the variale.</param>
        private void GenFormatValue(InputParam formatProviderParam, LocalBuilder locValue, String format) {
            // Call static method String.Format(IFormatProvier, String, Object[])
            // IFormatProvider
            GenLdInputParam(formatProviderParam);
            // String
            _il.Emit(OpCodes.Ldstr, format != "" ? "{0:" + format + "}" : "{0}");
            // Object[]
            _il.Emit(OpCodes.Ldc_I4_1);
            _il.Emit(OpCodes.Newarr, T_Object);
            _il.Emit(OpCodes.Dup);
            _il.Emit(OpCodes.Ldc_I4_0);
            GenLdLocal(locValue);
            if (locValue.LocalType.IsValueType) {
                _il.Emit(OpCodes.Box, locValue.LocalType);
            }
            _il.Emit(OpCodes.Stelem_Ref);
            // Call
            _il.Emit(OpCodes.Call, SM_String_Format_IFormatProvider_String_ObjectArray);
        }

        /// <summary>
        /// Produces code which appends a local variable to the output string builder
        /// with AppendFormat method using the format specified.
        /// </summary>
        /// <param name="locStringBuilder">A local variable of output string builder.</param>
        /// <param name="formatProviderParam"></param>
        /// <param name="locValue">A local variable to be appended.</param>
        /// <param name="format">A format string to be applied to the variale.</param>
        private void GenAppendFormat(LocalBuilder locStringBuilder, InputParam formatProviderParam, LocalBuilder locValue, String format) {
            // Call instance method StringBuilder.AppendFormat(IFormatProvider, String, Object[])
            // Instance
            GenLdLocal(locStringBuilder);
            // IFormatProvider
            GenLdInputParam(formatProviderParam);
            // Instance
            _il.Emit(OpCodes.Ldstr, format != "" ? "{0:" + format + "}" : "{0}");
            // Object[]
            _il.Emit(OpCodes.Ldc_I4_1);
            _il.Emit(OpCodes.Newarr, T_Object);
            _il.Emit(OpCodes.Dup);
            _il.Emit(OpCodes.Ldc_I4_0);
            GenLdLocal(locValue);
            if (locValue.LocalType.IsValueType) {
                _il.Emit(OpCodes.Box, locValue.LocalType);
            }
            _il.Emit(OpCodes.Stelem_Ref);
            // Call
            _il.Emit(OpCodes.Callvirt, IM_StringBuilder_AppendFormat_IFormatProvider_String_ObjectArray);
            // Drop result
            _il.Emit(OpCodes.Pop);
        }

        /// <summary>
        /// Produces initial code which initializes a string builder 
        /// for collecting the result string.
        /// </summary>
        public void GenProlog() {
            RootScope rootScope = new RootScope();
            _scope = rootScope;

            rootScope.LocOutput = _locals.Reserve(T_StringBuilder);
            _il.Emit(OpCodes.Newobj, CTOR_StringBuilder);
            _il.Emit(OpCodes.Stloc, rootScope.LocOutput);
        }

        /// <summary>
        /// Produces final code which calls ToString() method of the
        /// string builder and returns its result.
        /// </summary>
        public void GenEpilog() {
            RootScope rootScope = (RootScope)_scope;
            _il.Emit(OpCodes.Ldloc, rootScope.LocOutput);
            _il.Emit(OpCodes.Callvirt, IM_StringBuilder_ToString);
            _il.Emit(OpCodes.Ret);
            _scope = null;
        }

        /// <summary>
        /// Produces code which appends a raw text to the result
        /// string builder.
        /// </summary>
        public void GenRaw(String text) {
            _il.Emit(OpCodes.Ldloc, _scope.Root.LocOutput);
            _il.Emit(OpCodes.Ldstr, text);
            _il.Emit(OpCodes.Callvirt, IM_StringBuilder_Append_String);
            _il.Emit(OpCodes.Pop);
        }

        /// <summary>
        /// Produces code corresponding to opening If statement. The code 
        /// checks the expression provided and performs nested instructions
        /// only if the check result is effectifely 'true' (see 
        /// <see cref="GenCondJump"/> method description).
        /// </summary>
        public void GenOpenIfScope(String expression) {
            IfScope ifScope = new IfScope(_scope);
            _scope = ifScope;
            if (ifScope.IsBroken)
                return;

            LocalBuilder locCond = GenResolveExpressionValue(expression, ifScope.Parent);
            if (locCond == null) {
                ifScope.IsBroken = true;
                return;
            }

            ifScope.LabScopeEnd = _il.DefineLabel();
            GenCondJump(locCond, ifScope.LabScopeEnd, false);
            _locals.Free(locCond);
        }

        /// <summary>
        /// Produces code corresponding to closing If statement. The code
        /// marks the end of the If scope.
        /// </summary>
        public void GenCloseIfScope() {
            IfScope ifScope = (IfScope)_scope;
            if (!ifScope.IsBroken) {
                _il.MarkLabel(ifScope.LabScopeEnd);
            }
            _scope = ifScope.Parent;
        }

        /// <summary>
        /// Produces code corresponding to opening IfNot statement. The code 
        /// checks the expression provided and performs nested instructions
        /// only if the check result is effectifely 'false' (see 
        /// <see cref="GenCondJump"/> method description).
        /// </summary>
        public void GenOpenIfNotScope(String expression) {
            IfNotScope ifNotScope = new IfNotScope(_scope);
            _scope = ifNotScope;
            if (ifNotScope.IsBroken)
                return;

            LocalBuilder locCond = GenResolveExpressionValue(expression, ifNotScope.Parent);
            if (locCond == null) {
                ifNotScope.IsBroken = true;
                return;
            }

            ifNotScope.LabScopeEnd = _il.DefineLabel();
            GenCondJump(locCond, ifNotScope.LabScopeEnd, true);
            _locals.Free(locCond);
        }

        /// <summary>
        /// Produces code corresponding to closing IfNot statement. The 
        /// code marks the end of the IfNot scope.
        /// </summary>
        public void GenCloseIfNotScope() {
            IfNotScope ifNotScope = (IfNotScope)_scope;
            if (!ifNotScope.IsBroken) {
                _il.MarkLabel(ifNotScope.LabScopeEnd);
            }
            _scope = ifNotScope.Parent;
        }

        /// <summary>
        /// Produces code corresponding to opening Each statement, which is
        /// intended to enumerate collections. The code resolves the value 
        /// of the expression given, then it initializes a set of local 
        /// variables such as current element of a collection, its zero-based
        /// index and one-based number, count of elements in collection (if 
        /// available). At each iteration the code checks if all the
        /// elements of collection were enumerated, and if it were then jumps
        /// at the end of scope. Otherwise it gets the next element.
        /// Names of the local variables depend on the name of the variable
        /// for current element. For example, if it called 'this', then next
        /// variable names also will be chosen: 'thisIndex', 'thisNum',
        /// 'thisCount'.
        /// </summary>
        /// <param name="elementLocalName">The name of a local variable which holds current element.</param>
        public void GenOpenEachScope(String expression, String elementLocalName) {
            EachScope eachScope = new EachScope(_scope, elementLocalName);
            _scope = eachScope;
            if (eachScope.IsBroken)
                return;

            LocalBuilder enumLocal = GenResolveExpressionValue(expression, eachScope.Parent);
            if (enumLocal == null) {
                eachScope.IsBroken = true;
                return;
            }

            eachScope.LabScopeEnd = _il.DefineLabel();
            Type elementType;
            CollectionTypeFlags collType = AnalyzeCollection(enumLocal.LocalType, out elementType);
            // check if there is a value to enumerate
            if (!enumLocal.LocalType.IsValueType) {
                GenLdLocal(enumLocal);
                _il.Emit(OpCodes.Brfalse, eachScope.LabScopeEnd);
            }
            else if (IsNullableStruct(enumLocal.LocalType)) {
                PropertyInfo hasValuePropInfo = enumLocal.LocalType.GetProperty("HasValue", BindingFlags.Public | BindingFlags.Instance);
                GenLdLocal(enumLocal);
                _il.Emit(OpCodes.Call, hasValuePropInfo.GetGetMethod());
                _il.Emit(OpCodes.Brfalse, eachScope.LabScopeEnd);
            }
            // get/store collection var
            if ((collType & (CollectionTypeFlags.Array | CollectionTypeFlags.IList)) != CollectionTypeFlags.None) {
                eachScope.LocCollection = enumLocal;
            }
            else if ((collType & CollectionTypeFlags.IEnumerate) != CollectionTypeFlags.None) {
                Type typeIEnumerable = ExtractInterface(
                    enumLocal.LocalType,
                    (collType & CollectionTypeFlags.Generic) != CollectionTypeFlags.None ? T_IEnumerable_1 : T_IEnumerable);
                MethodInfo methodGetEnumerator = typeIEnumerable.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[0], null);
                eachScope.LocCollection = _locals.Reserve(methodGetEnumerator.ReturnType);
                GenLdRefToLocal(enumLocal);
                _il.Emit(OpCodes.Callvirt, methodGetEnumerator);
                _il.Emit(OpCodes.Stloc, eachScope.LocCollection);
                _locals.Free(enumLocal);
            }
            else {
                // if reflected type is not enumerable
                // we can try to cast it at runtime
                GenLdLocal(enumLocal);
                _il.Emit(OpCodes.Isinst, T_IEnumerable);
                // jump out of the loop if an instance is not IEnumerable
                _il.Emit(OpCodes.Brfalse, eachScope.LabScopeEnd);
                GenLdLocal(enumLocal);
                _il.Emit(OpCodes.Castclass, T_IEnumerable);
                MethodInfo methodGetEnumerator = IM_IEnumerable_GetEnumerator;
                eachScope.LocCollection = _locals.Reserve(methodGetEnumerator.ReturnType);
                _il.Emit(OpCodes.Callvirt, methodGetEnumerator);
                _il.Emit(OpCodes.Stloc, eachScope.LocCollection);
                _locals.Free(enumLocal);
            }
            // create index var
            eachScope.LocIndex = _locals.Reserve(T_Int32);
            _il.Emit(OpCodes.Ldc_I4_0);
            _il.Emit(OpCodes.Stloc, eachScope.LocIndex);
            // create count var
            if ((collType & CollectionTypeFlags.Array) != CollectionTypeFlags.None) {
                eachScope.LocCount = _locals.Reserve(T_Int32);
                GenLdLocal(eachScope.LocCollection);
                _il.Emit(OpCodes.Ldlen);
                _il.Emit(OpCodes.Conv_I4);
                _il.Emit(OpCodes.Stloc, eachScope.LocCount);
            }
            else if ((collType & (CollectionTypeFlags.IList | CollectionTypeFlags.ICollection)) != CollectionTypeFlags.None) {
                eachScope.LocCount = _locals.Reserve(T_Int32);
                Type typeICollection = ExtractInterface(
                    eachScope.LocCollection.LocalType,
                    (collType & CollectionTypeFlags.Generic) != CollectionTypeFlags.None ? T_ICollection_1 : T_ICollection);
                PropertyInfo propCount = typeICollection.GetProperty("Count", BindingFlags.Instance | BindingFlags.Public);
                GenLdRefToLocal(eachScope.LocCollection);
                _il.Emit(OpCodes.Callvirt, propCount.GetGetMethod());
                _il.Emit(OpCodes.Stloc, eachScope.LocCount);
            }
            // check loop condition
            eachScope.LabCheck = _il.DefineLabel();
            _il.MarkLabel(eachScope.LabCheck);
            if ((collType & (CollectionTypeFlags.Array | CollectionTypeFlags.IList)) != CollectionTypeFlags.None) {
                // check if the index less than the count
                GenLdLocal(eachScope.LocIndex);
                GenLdLocal(eachScope.LocCount);
                _il.Emit(OpCodes.Bge, eachScope.LabScopeEnd);
            }
            else {
                // move to the first element and get the result
                GenLdRefToLocal(eachScope.LocCollection);
                _il.Emit(OpCodes.Callvirt, IM_IEnumerator_MoveNext);
                _il.Emit(OpCodes.Brfalse, eachScope.LabScopeEnd);
            }
            // calc num var
            eachScope.LocNum = _locals.Reserve(T_Int32);
            GenLdLocal(eachScope.LocIndex);
            _il.Emit(OpCodes.Ldc_I4_1);
            _il.Emit(OpCodes.Add);
            _il.Emit(OpCodes.Stloc, eachScope.LocNum);
            // get current element
            eachScope.LocThis = _locals.Reserve(elementType);
            if ((collType & CollectionTypeFlags.Array) != CollectionTypeFlags.None) {
                GenLdLocal(eachScope.LocCollection);
                GenLdLocal(eachScope.LocIndex);
                GenLdElem(eachScope.LocThis.LocalType);
                _il.Emit(OpCodes.Stloc, eachScope.LocThis);
            }
            else if ((collType & CollectionTypeFlags.IList) != CollectionTypeFlags.None) {
                PropertyInfo propItem = eachScope.LocCollection.LocalType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { T_Int32 }, null);
                GenLdRefToLocal(eachScope.LocCollection);
                GenLdLocal(eachScope.LocIndex);
                _il.Emit(OpCodes.Callvirt, propItem.GetGetMethod());
                _il.Emit(OpCodes.Stloc, eachScope.LocThis);
            }
            else {
                PropertyInfo propCurrent = eachScope.LocCollection.LocalType.GetProperty("Current", BindingFlags.Public | BindingFlags.Instance);
                GenLdRefToLocal(eachScope.LocCollection);
                _il.Emit(OpCodes.Callvirt, propCurrent.GetGetMethod());
                _il.Emit(OpCodes.Stloc, eachScope.LocThis);
            }
        }


        /// <summary>
        /// Produces code corresponding to closing Each scope. This code
        /// increments index and jumps to the next iteration checking block.
        /// Also it marks a label of scope end.
        /// </summary>
        public void GenCloseEachScope() {
            EachScope eachScope = (EachScope)_scope;
            if (!eachScope.IsBroken) {
                GenLdLocal(eachScope.LocIndex);
                _il.Emit(OpCodes.Ldc_I4_1);
                _il.Emit(OpCodes.Add);
                _il.Emit(OpCodes.Stloc, eachScope.LocIndex);
                _il.Emit(OpCodes.Br, eachScope.LabCheck);
                _il.MarkLabel(eachScope.LabScopeEnd);
                _locals.Free(eachScope.LocCollection);
                _locals.Free(eachScope.LocIndex);
                _locals.Free(eachScope.LocNum);
                _locals.Free(eachScope.LocThis);
                if (eachScope.LocCount != null) {
                    _locals.Free(eachScope.LocCount);
                }
            }
            _scope = eachScope.Parent;
        }

        /// <summary>
        /// Produces code corresponding to opening When scope which is
        /// a kind of switch-case statement. The code resolves the value
        /// of expression given, applies specified format and stores the 
        /// result as a local variable. Nested statements such as 'eq',
        /// 'begins', 'contains', 'ends' compare their resolved values
        /// with this variable and either perform code inside own scope
        /// or jump to the next sibling scope check routine.
        /// </summary>
        public void GenOpenWhenScope(String expression, String format) {
            WhenScope whenScope = new WhenScope(_scope);
            _scope = whenScope;
            if (whenScope.IsBroken)
                return;

            if (expression.StartsWith("@")) {
                whenScope.LabScopeEnd = _il.DefineLabel();
                whenScope.LocOperand = _locals.Reserve(T_String);
                _il.Emit(OpCodes.Ldstr, expression.Remove(0, 1));
                _il.Emit(OpCodes.Stloc, whenScope.LocOperand);
            }
            else { 
                LocalBuilder operandLocal = GenResolveExpressionValue(expression, whenScope.Parent);
                if (operandLocal == null) {
                    whenScope.IsBroken = true;
                    return;
                }
                whenScope.LabScopeEnd = _il.DefineLabel();
                whenScope.LocOperand = _locals.Reserve(T_String);
                GenFormatValue(_formatProviderParam, operandLocal, format);
                _il.Emit(OpCodes.Stloc, whenScope.LocOperand);
                _locals.Free(operandLocal);
            }
        }

        /// <summary>
        /// Produces code corresponding to closing When scope. The code
        /// marks labels of branch end and scope end.
        /// </summary>
        public void GenCloseWhenScope() {
            WhenScope whenScope = (WhenScope)_scope;
            if (!whenScope.IsBroken) {
                if (whenScope.LabNextBranch.HasValue) {
                    _il.MarkLabel(whenScope.LabNextBranch.Value);
                }
                _il.MarkLabel(whenScope.LabScopeEnd);
            }
            _scope = whenScope.Parent;
        }

        /// <summary>
        /// Produces code corresponding to opening Eq statement. The code resolves
        /// the value of an expression given, formats it according to the specified
        /// format string. Then it compares the result string and string from When
        /// scope and if they are match perfoms nested instructions. Otherwise it
        /// jumps to the next branch of the When scope.
        /// </summary>
        public void GenOpenEqScope(String expression, String format, Boolean ignoreCase) {
            WhenScope whenScope = (WhenScope)_scope;
            Scope scope = new Scope(_scope, CompilerScopeType.Eq);
            _scope = scope;
            if (scope.IsBroken)
                return;

            if (whenScope.LabNextBranch.HasValue) {
                _il.MarkLabel(whenScope.LabNextBranch.Value);
            }
            whenScope.LabNextBranch = _il.DefineLabel();

            if (expression.StartsWith("@")) {
                GenLdLocal(whenScope.LocOperand);
                _il.Emit(OpCodes.Ldstr, expression.Remove(0, 1));
            }
            else {
                LocalBuilder operandLocal = GenResolveExpressionValue(expression, whenScope);
                if (operandLocal == null) {
                    _il.Emit(OpCodes.Br, whenScope.LabNextBranch.Value);
                    scope.IsBroken = true;
                    return;
                }
                GenLdLocal(whenScope.LocOperand);
                GenFormatValue(_formatProviderParam, operandLocal, format);
                _locals.Free(operandLocal);
            }
            _il.Emit(OpCodes.Ldc_I4, (Int32)(ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            _il.Emit(OpCodes.Call, SM_String_Equals_String_String_StringComparison);
            _il.Emit(OpCodes.Brfalse, whenScope.LabNextBranch.Value);
        }

        /// <summary>
        /// Produces code corresponding to closing Eq statement. The code jumps
        /// to the end of When statement after a branch was successfully perfomed.
        /// </summary>
        public void GenCloseEqScope() {
            WhenScope whenScope = (WhenScope)_scope.Parent;

            if (!_scope.IsBroken) {
                _il.Emit(OpCodes.Br, whenScope.LabScopeEnd);
            }

            _scope = whenScope;
        }

        /// <summary>
        /// Produces code corresponding to opening Contains statement. The code 
        /// resolves the value of an expression given, formats it according to the
        /// specified format string. Then it check where the string from When scope
        /// contains the result string. If it does the nested instuctions are 
        /// perfomed. Otherwise it jumps to the next branch of the When scope.
        /// </summary>
        public void GenOpenContainsScope(String expression, String format, Boolean ignoreCase) {
            WhenScope whenScope = (WhenScope)_scope;
            Scope scope = new Scope(_scope, CompilerScopeType.Contains);
            _scope = scope;
            if (scope.IsBroken)
                return;

            if (whenScope.LabNextBranch.HasValue) {
                _il.MarkLabel(whenScope.LabNextBranch.Value);
            }
            whenScope.LabNextBranch = _il.DefineLabel();

            if (expression.StartsWith("@")) {
                GenLdLocal(whenScope.LocOperand);
                _il.Emit(OpCodes.Ldstr, expression.Remove(0, 1));
            }
            else {
                LocalBuilder operandLocal = GenResolveExpressionValue(expression, whenScope);
                if (operandLocal == null) {
                    _il.Emit(OpCodes.Br, whenScope.LabNextBranch.Value);
                    scope.IsBroken = true;
                    return;
                }
                GenLdLocal(whenScope.LocOperand);
                GenFormatValue(_formatProviderParam, operandLocal, format);
                _locals.Free(operandLocal);
            }
            _il.Emit(OpCodes.Ldc_I4, (Int32)(ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            _il.Emit(OpCodes.Callvirt, IM_String_IndexOf_String_StringComparison);
            _il.Emit(OpCodes.Ldc_I4_0);
            _il.Emit(OpCodes.Blt, whenScope.LabNextBranch.Value);
        }

        /// <summary>
        /// Produces code corresponding to closing Contains statement. The code jumps
        /// to the end of When statement after a branch was successfully perfomed.
        /// </summary>
        public void GenCloseContainsScope() {
            WhenScope whenScope = (WhenScope)_scope.Parent;

            if (!_scope.IsBroken) {
                _il.Emit(OpCodes.Br, whenScope.LabScopeEnd);
            }

            _scope = whenScope;
        }

        /// <summary>
        /// Produces code corresponding to opening Begins statement. The code 
        /// resolves the value of an expression given, formats it according to the
        /// specified format string. Then it check where the string from When scope
        /// starts with the result string. If it does the nested instuctions are 
        /// perfomed. Otherwise it jumps to the next branch of the When scope.
        /// </summary>
        public void GenOpenBeginsScope(String expression, String format, Boolean ignoreCase) {
            WhenScope whenScope = (WhenScope)_scope;
            Scope scope = new Scope(_scope, CompilerScopeType.Begins);
            _scope = scope;
            if (scope.IsBroken)
                return;

            if (whenScope.LabNextBranch.HasValue) {
                _il.MarkLabel(whenScope.LabNextBranch.Value);
            }
            whenScope.LabNextBranch = _il.DefineLabel();

            if (expression.StartsWith("@")) {
                GenLdLocal(whenScope.LocOperand);
                _il.Emit(OpCodes.Ldstr, expression.Remove(0, 1));
            }
            else {
                LocalBuilder operandLocal = GenResolveExpressionValue(expression, whenScope);
                if (operandLocal == null) {
                    _il.Emit(OpCodes.Br, whenScope.LabNextBranch.Value);
                    scope.IsBroken = true;
                    return;
                }
                GenLdLocal(whenScope.LocOperand);
                GenFormatValue(_formatProviderParam, operandLocal, format);
                _locals.Free(operandLocal);
            }

            _il.Emit(OpCodes.Ldc_I4, (Int32)(ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            _il.Emit(OpCodes.Callvirt, IM_String_StartsWith_String_StringComparison);
            _il.Emit(OpCodes.Brfalse, whenScope.LabNextBranch.Value);
        }

        /// <summary>
        /// Produces code corresponding to closing Begins statement. The code jumps
        /// to the end of When statement after a branch was successfully perfomed.
        /// </summary>
        public void GenCloseBeginsScope() {
            WhenScope whenScope = (WhenScope)_scope.Parent;

            if (!_scope.IsBroken) {
                _il.Emit(OpCodes.Br, whenScope.LabScopeEnd);
            }

            _scope = whenScope;
        }

        /// <summary>
        /// Produces code corresponding to opening Ends statement. The code 
        /// resolves the value of an expression given, formats it according to the
        /// specified format string. Then it check where the string from When scope
        /// ends with the result string. If it does the nested instuctions are 
        /// perfomed. Otherwise it jumps to the next branch of the When scope.
        /// </summary>
        public void GenOpenEndsScope(String expression, String format, Boolean ignoreCase) {
            WhenScope whenScope = (WhenScope)_scope;
            Scope scope = new Scope(_scope, CompilerScopeType.Ends);
            _scope = scope;
            if (scope.IsBroken)
                return;

            if (whenScope.LabNextBranch.HasValue) {
                _il.MarkLabel(whenScope.LabNextBranch.Value);
            }
            whenScope.LabNextBranch = _il.DefineLabel();

            if (expression.StartsWith("@")) {
                GenLdLocal(whenScope.LocOperand);
                _il.Emit(OpCodes.Ldstr, expression.Remove(0, 1));
            }
            else {
                LocalBuilder operandLocal = GenResolveExpressionValue(expression, whenScope);
                if (operandLocal == null) {
                    _il.Emit(OpCodes.Br, whenScope.LabNextBranch.Value);
                    scope.IsBroken = true;
                    return;
                }
                GenLdLocal(whenScope.LocOperand);
                GenFormatValue(_formatProviderParam, operandLocal, format);
                _locals.Free(operandLocal);
            }

            _il.Emit(OpCodes.Ldc_I4, (Int32)(ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            _il.Emit(OpCodes.Callvirt, IM_String_EndsWith_String_StringComparison);
            _il.Emit(OpCodes.Brfalse, whenScope.LabNextBranch.Value);
        }

        /// <summary>
        /// Produces code corresponding to closing Ends statement. The code jumps
        /// to the end of When statement after a branch was successfully perfomed.
        /// </summary>
        public void GenCloseEndsScope() {
            WhenScope whenScope = (WhenScope)_scope.Parent;

            if (!_scope.IsBroken) {
                _il.Emit(OpCodes.Br, whenScope.LabScopeEnd);
            }

            _scope = whenScope;
        }

        /// <summary>
        /// Produces code corresponding to opening Else statement. This branch
        /// of code is perfomed only if all the conditions of previous branches
        /// did not become 'true'.
        /// </summary>
        public void GenOpenElseScope() {
            WhenScope whenScope = (WhenScope)_scope;
            Scope scope = new Scope(whenScope, CompilerScopeType.Else);
            _scope = scope;

            if (scope.IsBroken) 
                return;

            if (whenScope.LabNextBranch.HasValue) {
                _il.MarkLabel(whenScope.LabNextBranch.Value);
            }
            whenScope.LabNextBranch = null; 
        }

        /// <summary>
        /// Produces code corresponding to closing Else statement. The code jumps
        /// to the end of When statement after a branch was successfully perfomed.
        /// </summary>
        public void GenCloseElseScope() {
            WhenScope whenScope = (WhenScope)_scope.Parent;

            if (!_scope.IsBroken) {
                _il.Emit(OpCodes.Br, whenScope.LabScopeEnd);
            }

            _scope = whenScope;
        }

        /// <summary>
        /// Produces code corresponding to Text statement. The code resolves the
        /// value of an expression, formats it if a format string specified and
        /// put to the output string builder. Depending on output generating 
        /// format the value appended may be escaped or not.
        /// </summary>
        public void GenText(String expression, String format) {
            if (_scope.IsBroken)
                return;

            LocalBuilder locValue = GenResolveExpressionValue(expression, _scope);
            if (locValue == null)
                return;

            if (_outputFormat == OutputFormat.Html) {
                GenLdLocal(_scope.Root.LocOutput);
                GenFormatValue(_formatProviderParam, locValue, format);
                _il.Emit(OpCodes.Call, SM_EncodeUtility_HtmlEncode_String);
                _il.Emit(OpCodes.Callvirt, IM_StringBuilder_Append_String);
                _il.Emit(OpCodes.Pop);
            }
            else {
                GenAppendFormat(_scope.Root.LocOutput, _formatProviderParam, locValue, format);
            }
            _locals.Free(locValue);
        }

        /// <summary>
        /// Produces code corresponding to Html statement. The code resolves the
        /// value of an expression, formats it if a format string specified and
        /// put to the output string builder.
        /// </summary>
        public void GenHtml(String expression, String format) {
            if (_scope.IsBroken)
                return;

            LocalBuilder locValue = GenResolveExpressionValue(expression, _scope);
            if (locValue == null)
                return;

            GenAppendFormat(_scope.Root.LocOutput, _formatProviderParam, locValue, format);
            _locals.Free(locValue);
        }

        /// <summary>
        /// Produces code corresponding to Url statement. The code resolves the
        /// value of an expression, formats it if a format string specified.
        /// Then the string value is being encoded with UrlPathEncode or 
        /// UrlEncode method. If the generating output format is HTML then
        /// the HTML encoding is also perfoming. The result string goes to
        /// the output string builder.
        /// </summary>
        public void GenUrl(String expression, Boolean paramEncoding, Boolean onlyAscii) {
            if (_scope.IsBroken)
                return;

            LocalBuilder locValue = GenResolveExpressionValue(expression, _scope);
            if (locValue == null)
                return;

            GenLdLocal(_scope.Root.LocOutput);
            GenFormatValue(_formatProviderParam, locValue, "");
            _locals.Free(locValue);
            _il.Emit(onlyAscii ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            if (paramEncoding) {
                _il.Emit(OpCodes.Call, SM_EncodeUtility_UrlEncode_String_Boolean);
            }
            else {
                _il.Emit(OpCodes.Call, SM_EncodeUtility_UrlEncodeAsUrl_String_Boolean);
            }

            if (_outputFormat == OutputFormat.Html) {
                _il.Emit(OpCodes.Call, SM_EncodeUtility_HtmlEncode_String);
            }
            _il.Emit(OpCodes.Callvirt, IM_StringBuilder_Append_String);
            _il.Emit(OpCodes.Pop);
        }
    }
}
