using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace CompetitiveProgrammingCs.lib
{
    //using ftype = System.Int64;
    using ftype = System.Double;

    internal class Point2D
    {
        static readonly double Eps = 1E-9;
        public ftype x { get; set; }
        public ftype y { get; set; }

        public Point2D() { }
        public Point2D(ftype x, ftype y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point2D operator +(Point2D left, Point2D right) => new Point2D(left.x + right.x, left.y + right.y);
        public static Point2D operator -(Point2D left, Point2D right) => new Point2D(left.x - right.x, left.y - right.y);
        public static Point2D operator *(Point2D p, ftype v) => new Point2D(p.x * v, p.y * v);
        public static Point2D operator *(ftype v, Point2D p) => p * v;
        public static Point2D operator /(Point2D p, ftype v) => new Point2D(p.x / v, p.y / v);

        public override string ToString()
        {
            return $"{x} {y}";
        }
    }

    internal class Point3D
    {
        public ftype x { get; set; }
        public ftype y { get; set; }
        public ftype z { get; set; }

        public Point3D() { }
        public Point3D(ftype x, ftype y, ftype z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Point3D operator +(Point3D left, Point3D right) => new Point3D(left.x + right.x, left.y + right.y, left.z + right.z);
        public static Point3D operator -(Point3D left, Point3D right) => new Point3D(left.x - right.x, left.y - right.y, left.z - right.z);
        public static Point3D operator *(Point3D p, ftype v) => new Point3D(p.x * v, p.y * v, p.z * v);
        public static Point3D operator *(ftype v, Point3D p) => p * v;
        public static Point3D operator /(Point3D p, ftype v) => new Point3D(p.x / v, p.y / v, p.z / v);
        public override string ToString()
        {
            return $"{x} {y} {z}";
        }
    }

    internal static class GeomUtils
    {
        /// <summary>
        /// Inner product(Dot Product)
        /// </summary>
        public static ftype Dot(Point2D a, Point2D b) => a.x * b.x + a.y * b.y;
        /// <summary>
        /// Inner product(Dot Product)
        /// </summary>
        public static ftype Dot(Point3D a, Point3D b) => a.x * b.x + a.y * b.y + a.z * b.z;

        public static ftype Norm(Point2D a) => Dot(a, a);
        /// <summary>
        /// Length of a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Abs(Point2D a) => Math.Sqrt(Norm(a));

        /// <summary>
        /// Projection of a onto b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Proj(Point2D a, Point2D b) => Dot(a, b) / Abs(b);

        /// <summary>
        /// Angle between two vectors a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Angle(Point2D a, Point2D b) => Math.Acos(Dot(a, b) / Abs(a) / Abs(b));

        /// <summary>
        /// cross product
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ftype Cross(Point2D a, Point2D b) => a.x * b.y - b.x * a.y;
        public static Point3D Cross(Point3D a, Point3D b) => new Point3D(
            a.y * b.z - b.y * a.z,
            a.z * b.x - b.z * a.x,
            a.x * b.y - b.x * a.y
            );
        public static ftype Triple(Point3D a, Point3D b, Point3D c) => Dot(a, Cross(b, c));
    }
}
