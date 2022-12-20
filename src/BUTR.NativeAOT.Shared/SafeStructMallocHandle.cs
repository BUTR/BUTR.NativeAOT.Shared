﻿#region License
// MIT License
//
// Copyright (c) Bannerlord's Unofficial Tools & Resources
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

#nullable enable
#if !BUTR_NATIVEAOT_ENABLE_WARNING
#pragma warning disable
#endif

namespace BUTR.NativeAOT.Shared
{
    using global::System;
    using global::System.Runtime.InteropServices;
    using global::Microsoft.Win32.SafeHandles;

    internal unsafe class SafeStructMallocHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public static SafeStructMallocHandle<TStruct> Create<TStruct>(TStruct* ptr) where TStruct : unmanaged => new(ptr);

        protected SafeStructMallocHandle(): base(true) { }
        protected SafeStructMallocHandle(IntPtr handle) : base(true)
        {
            this.handle = handle;
            var b = false;
            DangerousAddRef(ref b);
        }

        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
                NativeMemory.Free(handle.ToPointer());
            return true;
        }
    }

    internal sealed unsafe class SafeStructMallocHandle<TStruct> : SafeStructMallocHandle where TStruct : unmanaged
    {
        public static implicit operator TStruct*(SafeStructMallocHandle<TStruct> handle) => (TStruct*) handle.handle.ToPointer();

        public TStruct* Value => this;

        public bool IsNull => Value == null;

        public void ValueAsVoid()
        {
            if (typeof(TStruct) != typeof(return_value_void))
                throw new Exception();

            var ptr = (return_value_void*) Value;
            if (ptr->Error is null)
            {
                return;
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public SafeStringMallocHandle ValueAsString()
        {
            if (typeof(TStruct) != typeof(return_value_string))
                throw new Exception();

            var ptr = (return_value_string*) Value;
            if (ptr->Error is null)
            {
                return new SafeStringMallocHandle(ptr->Value);
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public SafeStringMallocHandle ValueAsJson()
        {
            if (typeof(TStruct) != typeof(return_value_json))
                throw new Exception();

            var ptr = (return_value_json*) Value;
            if (ptr->Error is null)
            {
                return new SafeStringMallocHandle(ptr->Value);
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public bool ValueAsBool()
        {
            if (typeof(TStruct) != typeof(return_value_bool))
                throw new Exception();

            var ptr = (return_value_bool*) Value;
            if (ptr->Error is null)
            {
                return ptr->Value;
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public uint ValueAsUInt32()
        {
            if (typeof(TStruct) != typeof(return_value_uint32))
                throw new Exception();

            var ptr = (return_value_uint32*) Value;
            if (ptr->Error is null)
            {
                return ptr->Value;
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public int ValueAsInt32()
        {
            if (typeof(TStruct) != typeof(return_value_int32))
                throw new Exception();

            var ptr = (return_value_int32*) Value;
            if (ptr->Error is null)
            {
                return ptr->Value;
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public void* ValueAsPointer()
        {
            if (typeof(TStruct) != typeof(return_value_ptr))
                throw new Exception();

            var ptr = (return_value_ptr*) Value;
            if (ptr->Error is null)
            {
                return ptr->Value;
            }

            using var hError = new SafeStringMallocHandle(ptr->Error);
            throw new NativeCallException(new string(hError));
        }

        public SafeStructMallocHandle(): base(IntPtr.Zero) { }
        public SafeStructMallocHandle(TStruct* param): base(new IntPtr(param)) { }
    }
}
#nullable restore
#if !BUTR_NATIVEAOT_ENABLE_WARNING
#pragma warning restore
#endif