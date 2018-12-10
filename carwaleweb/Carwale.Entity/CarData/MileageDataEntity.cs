using System;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class MileageDataEntity : IComparable<MileageDataEntity>,IEquatable<MileageDataEntity>
    {
        public string FuelType { get; set; }

        public string MileageUnit { get; set; }

        public string Displacement { get; set; }

        public string Transmission { get; set; }

        public double Arai { get; set; }
        int compareArai(MileageDataEntity other)
        {
            if (other.Arai - this.Arai > 0.001)
            {
                return -1;
            }
            else if (Math.Abs(other.Arai - this.Arai) < 0.001)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        int IComparable<MileageDataEntity>.CompareTo(MileageDataEntity other)
        {
            return this.compareArai(other);
        }
        public bool Equals(MileageDataEntity other)
        {
            return this.compareArai(other) == 0;
        }
        public override int GetHashCode()
        {
            return this.Arai.GetHashCode();
        }
    }
}
