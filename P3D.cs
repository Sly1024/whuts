namespace whuts
{
    // A 3D point structure that supports convenience operations like -p, p1+p2, p1 % p2 (modulo on all 3 dimensions)
    public struct P3D
    {
        public int x, y, z;

        public override string ToString()
        {
            return "[" + x + "," + y + "," + z + "]";
        }

        // convert a tuple (int,int,int) to P3D
        public static implicit operator P3D((int x, int y, int z) tuple) => new P3D { x = tuple.x, y = tuple.y, z = tuple.z };

        // negate a P3D (-p)
        public static P3D operator -(P3D p) => new P3D { x = -p.x, y = -p.y, z = -p.z };

        // add two P3D points
        public static P3D operator +(P3D p1, P3D p2) => new P3D { x = p1.x + p2.x, y = p1.y + p2.y, z = p1.z + p2.z };

        // positive remainder
        static int PosRem(int a, int b)
        {
            // a %= b;
            // return a < 0 ? a + b : a;
            while (a < 0) a += b;
            while (a >= b) a -= b;
            return a;
        }
        // integer remainder in all 3 dimensions, but always non-negative results
        public static P3D operator %(P3D p, P3D mod) => new P3D { x = PosRem(p.x, mod.x), y = PosRem(p.y, mod.y), z = PosRem(p.z, mod.z) };
    }
}