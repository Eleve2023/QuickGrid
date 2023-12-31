﻿using System.Collections.Generic;

namespace Shared
{
    public class MyService
    {
        public static ICollection<MyAllType> GetMyAllTypes()
        {
            return new MyAllType[]
            {
                new() {Bool = true, BoolNullable = true, Byte= 1, ByteNullable = 1, Char='a', CharNullable = 'a', DateOnly = new(2023,01,01), DateOnlyNullable = new(2023,01,01), DateTime = new(2023,01,01), DateTimeNullable =new(2023,01,01), DateTimeOffset= new(2023,01,01,0,0,0,new()), DateTimeOffsetNullable= new(2023,01,01,0,0,0,new()), Decimal = 1.00m, DecimalNullable = 1.00m, Double= 1.00, DoubleNullable = 1.00, Float = 1.00f, FloatNullable = 1.00f, Int = 1, IntNullable = 1, Long = 1, LongNullable = 1, Sbyte = 1, SbyteNullable = 1, Short = 1, ShortNullable= 1, String = "abc", StringNullable = "abc", TimeOnly = new(12,0,0), TimeOnlyNullable = new(12,0,0), TimeSpan= new(12,0,0), TimeSpanNullable = new(12, 0, 0), Uint = 1, UintNullable = 1, Ulong =1, UlongNullable = 1, Guid = Guid.Empty, GuidNullable = Guid.Empty, Enum = EnumTest.One, EnumNullable = EnumTest.One } ,
                new() {Bool = true, BoolNullable = null, Byte= 1, ByteNullable = null, Char='a', CharNullable = null, DateOnly = new(2023,01,01), DateOnlyNullable = null, DateTime = new(2023,01,01), DateTimeNullable =null, DateTimeOffset= new(2023,01,01,0,0,0,new()), DateTimeOffsetNullable= null, Decimal = 1.00m, DecimalNullable = null, Double= 1.00, DoubleNullable = null, Float = 1.00f, FloatNullable = null, Int = 1, IntNullable = 1, Long = 1, LongNullable = null, Sbyte = 1, SbyteNullable = null, Short = 1, ShortNullable= null, String = "abc", StringNullable = null, TimeOnly = new(12,0,0), TimeOnlyNullable = null, TimeSpan= new(12,0,0), TimeSpanNullable = null, Uint = 1, UintNullable = null, Ulong =1, UlongNullable = null,Guid = Guid.Empty, GuidNullable = null, Enum = EnumTest.One, EnumNullable = null  } ,
            };
        }
        public static ICollection<MyExpModele1> MyExpModele1s = new MyExpModele1[]
        {  
            new () {String1 = "AAA", String2 = "aaa", Int1 = 1, Bool1 = true, Enum1 = EnumTest.One}, 
            new () {String1 = "AAA", String2 = "aab", Int1 = 2, Bool1 = false, Enum1 = EnumTest.Two}, 
            new () {String1 = "AAB", String2 = "aab", Int1 = 2, Bool1 = false, Enum1 = EnumTest.Three}, 
            new () {String1 = "AAC", String2 = "abb", Int1 = 3, Bool1 = true, Enum1 = EnumTest.Four}, 
            new () {String1 = "AAC", String2 = "abb", Int1 = 4, Bool1 = true, Enum1 = EnumTest.Five}, 
        };
    }
}
