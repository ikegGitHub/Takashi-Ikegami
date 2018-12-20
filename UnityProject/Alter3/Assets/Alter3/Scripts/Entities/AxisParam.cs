using System;
using System.Collections.Generic;

namespace XFlag.Alter3Simulator
{
    public class AxisParam : IEquatable<AxisParam>
    {
        public int AxisNumber { get; set; }

        public double Value { get; set; }

        public int Priority { get; set; }

        public int Duration { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AxisParam);
        }

        public bool Equals(AxisParam other)
        {
            return other != null &&
                   AxisNumber == other.AxisNumber &&
                   Value == other.Value &&
                   Priority == other.Priority &&
                   Duration == other.Duration;
        }

        public override int GetHashCode()
        {
            var hashCode = 1490284165;
            hashCode = hashCode * -1521134295 + AxisNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Priority.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{nameof(AxisParam)}{{{nameof(AxisNumber)}={AxisNumber},{nameof(Value)}={Value},{nameof(Priority)}={Priority},{nameof(Duration)}={Duration}}}";
        }

        public static bool operator ==(AxisParam param1, AxisParam param2)
        {
            return EqualityComparer<AxisParam>.Default.Equals(param1, param2);
        }

        public static bool operator !=(AxisParam param1, AxisParam param2)
        {
            return !(param1 == param2);
        }
    }
}
