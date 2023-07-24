using CompetitiveProgrammingCs.lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;



namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver();
            solver.solve(args);
        }
    }

    class Solver
    {
        private IO.StreamScanner sc;
        public Solver()
        {
            sc = new IO.StreamScanner(Console.OpenStandardInput());
        }


        public void solve(string[] args)
        {
            int px1 = sc.NextInt();
            int py1 = sc.NextInt();
            int px2 = sc.NextInt();
            int py2 = sc.NextInt();

            Point2D a = new Point2D(px1, py1);
            Point2D b = new Point2D(px2, py2);

            int q = sc.NextInt();
            for (int i = 0; i < q; ++i)
            {
                int px3 = sc.NextInt();
                int py3 = sc.NextInt();
                Point2D c = new Point2D(px3, py3);

                double proj = GeomUtils.Proj(c - a, b - a);
                Point2D d = a + (b - a) * (proj / GeomUtils.Abs(b - a));
                Point2D ans = c + 2 * (d - c);
                Console.WriteLine(ans);
            }
        }
    }
}






namespace Program.IO
{
    using System.IO;
    using System.Text;
    using System.Globalization;

    public class StreamScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer = new byte[1024];
        private int size, ptr;
        private bool isEOF = false;

        public StreamScanner(Stream stream)
        {
            this.stream = stream;
            ptr = 0;
            size = 0;
        }

        private byte read()
        {
            if (isEOF) return 0;
            if (ptr >= size)
            {
                ptr = 0;
                size = stream.Read(buffer, 0, 1024);
                if (size <= 0)
                {
                    isEOF = true;
                    return 0;
                }
            }
            return buffer[ptr++];
        }

        public char Char()
        {
            byte b = 0;
            do { b = read(); } while ((b < 33 || 126 < b) && !isEOF);
            return (char)b;
        }

        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)read())
                sb.Append(b);
            return sb.ToString();
        }

        public int NextInt() { return isEOF ? Int32.MinValue : Int32.Parse(Scan(), CultureInfo.InvariantCulture); }
        public long NextLong() { return isEOF ? Int64.MinValue : Int64.Parse(Scan(), CultureInfo.InvariantCulture); }
        public double NextDouble() { return isEOF ? Double.MinValue : Double.Parse(Scan(), CultureInfo.InvariantCulture); }
    }
}
