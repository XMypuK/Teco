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
using System.Reflection.Emit;

namespace Teco.Generating {

    /// <summary>
    /// Holds context data of custom scope.
    /// </summary>
    class Scope {
        Scope _parent;
        CompilerScopeType _type;
        Boolean _isBroken;
        ScopeLocalCollection _locals;

        /// <summary>
        /// Returns type of the current scope.
        /// </summary>
        public CompilerScopeType Type { get { return _type; } }

        /// <summary>
        /// Returns parent scope if the current scope type is
        /// not equal Root. For Root scope returns null.
        /// </summary>
        public Scope Parent { get { return _parent; } }

        /// <summary>
        /// Returns root scope.
        /// </summary>
        public RootScope Root {
            get {
                if (Parent != null)
                    return Parent.Root;
                if (Type == CompilerScopeType.Root)
                    return (RootScope)this;
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of local variables assigned to
        /// current scope.
        /// </summary>
        protected ScopeLocalCollection Locals { get { return _locals; } }

        /// <summary>
        /// Returns or sets the flag indicating that there is a 
        /// problem with code generation of resolving expression 
        /// value (wrong member names in expression) for statement 
        /// in this or parent scope.
        /// </summary>
        public Boolean IsBroken {
            get { return _isBroken; }
            set {
                if (_parent != null && _parent.IsBroken) {
                    _isBroken = true;
                }
                else {
                    _isBroken = value;
                }
            }
        }

        /// <summary>
        /// Create new instance of scope.
        /// </summary>
        /// <param name="parent">Scope in which Each scope is located.</param>
        /// <param name="type">Type of scope.</param>
        public Scope(Scope parent, CompilerScopeType type) {
            this._parent = parent;
            this._type = type;
            this._isBroken = parent != null ? parent.IsBroken : false;
            this._locals = new ScopeLocalCollection();
        }

        /// <summary>
        /// Returns a local variable with the name specified
        /// from this scope or from one of the parent scopes.
        /// </summary>
        /// <param name="localName">The name of a local variable.</param>
        public LocalBuilder ResolveLocal(String localName) {
            LocalBuilder local = Locals[localName];
            if (local == null && _parent != null) {
                local = _parent.ResolveLocal(localName);
            }
            return local;
        }

        /// <summary>
        /// Looks at scope types up the scope hierarchy and returns
        /// the first scope the type of which is equal to the type 
        /// specified.
        /// </summary>
        /// <param name="type">The type of scope.</param>
        /// <param name="includeSelf">Flag indicating if this scope type should be also checked.</param>
        public Scope GetClosestScope(CompilerScopeType type, Boolean includeSelf) {
            Scope scope = includeSelf ? this : Parent;
            while (scope != null) {
                if (scope.Type == type)
                    return scope;
                scope = scope.Parent;
            }
            return null;
        }
    }
}
