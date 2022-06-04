﻿namespace Fornax.Compiler.WasmEmitter;

public enum NumberType : byte
{
    I32 = 0x7f,
    I64 = 0x7e,
    F32 = 0x7d,
    F64 = 0x7c
}