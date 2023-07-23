using System;
using System.Diagnostics;

namespace QuickGridCTests
{
    public class MyGridItem
    {
        public string String { get; set; } = default!;
        public string? StringNullable { get; set; }
        public Guid Guid { get; set; }
        public Guid? GuidNullable { get; set; }
        public EnumTest Enum { get; set; } = default!;
        public EnumTest? EnumNullable { get; set; }
        public bool Bool { get; set; }
        public bool? BoolNullable { get; set; }
        public byte Byte { get; set; }
        public byte? ByteNullable { get; set; }
        public sbyte Sbyte { get; set; }
        public sbyte? SbyteNullable { get; set; }
        public char Char { get; set; }
        public char? CharNullable { get; set; }
        public decimal Decimal { get; set; }
        public decimal? DecimalNullable { get; set; }
        public double Double { get; set; }
        public double? DoubleNullable { get; set; }
        public float Float { get; set; }
        public float? FloatNullable { get; set; }
        public int Int { get; set; }
        public int? IntNullable { get; set; }
        public uint Uint { get; set; }
        public uint? UintNullable { get; set; }
        public long Long { get; set; }
        public long? LongNullable { get; set; }
        public ulong Ulong { get; set; }
        public ulong? UlongNullable { get; set; }
        public short Short { get; set; }
        public short? ShortNullable { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? DateTimeNullable { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public DateTimeOffset? DateTimeOffsetNullable { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public TimeSpan? TimeSpanNullable { get; set; }
        public DateOnly DateOnly { get; set; }
        public DateOnly? DateOnlyNullable { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public TimeOnly? TimeOnlyNullable { get; set; }
    }

    public enum EnumTest
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

}
