namespace whuts
{
    public class TileBits
    {
        P3D size;
        P3D size2;
        P3D sizeShift;

        bool[] bits;
        public TileBits(P3D size)
        {
            this.size = size;
            sizeShift = (0, Pow2(size.y), Pow2(size.z));
            size2 = (size.x, (1 << sizeShift.y), (1 << sizeShift.z));
            bits = new bool[size2.x * size2.y * size2.z];
        }

        public bool this[P3D p] {
            get { return bits[p.z + ((p.y + (p.x << sizeShift.y)) << sizeShift.z)]; }
            set { bits[p.z + ((p.y + (p.x << sizeShift.y)) << sizeShift.z)] = value; }
        }
        int Pow2(int size) {
            var i = 0;
            var p2 = 1;
            while (size > p2) {
                p2 <<= 1;
                i++;
            }
            return i;
        }
    }
}