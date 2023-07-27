using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace GeometryLibrary
{
    //using ftype = System.Int64;
    using ftype = System.Double;

    internal class Constant
    {
        public static readonly double EPS = 1E-9;

    }

    internal class Point2D : IComparable<Point2D>
    {
        public ftype X { get; set; }
        public ftype Y { get; set; }

        public Point2D() { }
        public Point2D(ftype x, ftype y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a) => new Point2D(-a.X, -a.Y);
        public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
        public static Point2D operator *(Point2D p, ftype v) => new Point2D(p.X * v, p.Y * v);
        public static Point2D operator *(ftype v, Point2D p) => p * v;
        public static Point2D operator /(Point2D p, ftype v) => new Point2D(p.X / v, p.Y / v);
        public static Point2D operator *(Point2D a, Point2D b) => new Point2D(a.X * b.X - a.Y * b.Y, a.X * b.Y + a.Y * b.X);
        public void Normalize()
        {
            if (X == 0 && Y == 0)
                return;
            double len = GeomUtils.Abs(this);
            X /= len;
            Y /= len;
        }
        public override string ToString() => $"{X} {Y}";


        public int CompareTo(Point2D other)
        {
            if (this.X - other.X < Constant.EPS) return -1;
            if (this.X - other.X > -Constant.EPS) return 1;
            if (this.Y - other.Y < Constant.EPS) return -1;
            if (this.Y - other.Y > -Constant.EPS) return 1;
            return 0;
        }
    }

    internal class Point3D : IComparable<Point3D>
    {
        public ftype X { get; set; }
        public ftype Y { get; set; }
        public ftype Z { get; set; }

        public Point3D() { }
        public Point3D(ftype x, ftype y, ftype z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point3D operator -(Point3D a) => new Point3D(-a.X, -a.Y, -a.Z);
        public static Point3D operator -(Point3D a, Point3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point3D operator *(Point3D p, ftype v) => new Point3D(p.X * v, p.Y * v, p.Z * v);
        public static Point3D operator *(ftype v, Point3D p) => p * v;
        public static Point3D operator /(Point3D p, ftype v) => new Point3D(p.X / v, p.Y / v, p.Z / v);
        public void Normalize()
        {
            if (X == 0 && Y == 0 && Z == 0)
                return;
            double len = GeomUtils.Abs(this);
            X /= len;
            Y /= len;
            Z /= len;
        }
        public override string ToString() => $"{X} {Y} {Z}";


        public int CompareTo(Point3D other)
        {
            if (this.X - other.X < Constant.EPS) return -1;
            if (this.X - other.X > -Constant.EPS) return 1;
            if (this.Y - other.Y < Constant.EPS) return -1;
            if (this.Y - other.Y > -Constant.EPS) return 1;
            if (this.Z - other.Z < Constant.EPS) return -1;
            if (this.Z - other.Z > -Constant.EPS) return 1;
            return 0;
        }
    }

    internal class Segment2D
    {
        public Point2D S { get; set; }
        public Point2D T { get; set; }
        public Segment2D(Point2D s, Point2D t)
        {
            S = s;
            T = t;
        }
    }

    /// <summary>
    /// Ax + Bx + C = 0で表される直線
    /// </summary>
    internal class Line2D
    {
        public ftype A { get; set; }
        public ftype B { get; set; }
        public ftype C { get; set; }

        public Line2D() { }
        public Line2D(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }
        public Line2D(Point2D p, Point2D q)
        {
            A = p.Y - q.Y;
            B = -(p.X - q.X);
            C = -(A * p.X + B * p.Y);
        }

        public void Normalize()
        {
            double Z = Math.Sqrt(A * A + B * B);
            A /= Z;
            B /= Z;
            C /= Z;
            if (GeomUtils.Sgn(A) == -1 || (GeomUtils.Sgn(B) == -1 && GeomUtils.IsZero(A)))
            {
                A = -A;
                B = -B;
                C = -C;
            }
        }
        public Point2D Normal() => new Point2D(A, B);
    }


    internal static class GeomUtils
    {
        private static readonly double EPS = Constant.EPS;

        public static int Sgn(ftype a) => (a < -EPS ? -1 : (a > EPS ? 1 : 0));
        public static bool IsZero(ftype a) => Math.Abs(a) < EPS;

        public static void swap<T>(ref T x, ref T y)
        {
            T tmp = x;
            x = y;
            y = tmp;
        }
        /// <summary>
        /// 2ベクトルの内積
        /// </summary>
        public static ftype Dot(Point2D a, Point2D b) => a.X * b.X + a.Y * b.Y;

        /// <summary>
        /// 2ベクトルの内積
        /// </summary>
        public static ftype Dot(Point3D a, Point3D b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        /// <summary>
        /// ベクトルaの長さの2乗（L2ノルム）
        /// </summary>
        public static ftype Norm(Point2D a) => Dot(a, a);

        /// <summary>
        /// ベクトルaの長さの2乗（L2ノルム）
        /// </summary>
        public static ftype Norm(Point3D a) => Dot(a, a);

        /// <summary>
        /// ベクトルaの長さ
        /// </summary>
        public static double Abs(Point2D a) => Math.Sqrt(Norm(a));

        /// <summary>
        /// ベクトルaの長さ
        /// </summary>
        public static double Abs(Point3D a) => Math.Sqrt(Norm(a));

        /// <summary>
        /// ベクトルaをベクトルbに投影した際の長さ
        /// </summary>
        public static double Proj(Point2D a, Point2D b) => Dot(a, b) / Abs(b);

        /// <summary>
        /// ベクトルa, bのなす角度
        /// </summary>
        public static double Angle(Point2D a, Point2D b) => Math.Acos(Dot(a, b) / Abs(a) / Abs(b));

        /// <summary>
        /// ベクトルa, bの外積
        /// </summary>
        public static ftype Cross(ftype a, ftype b, ftype c, ftype d) => a * d - b * c;

        /// <summary>
        /// ベクトルa, bの外積
        /// </summary>
        public static ftype Cross(Point2D a, Point2D b) => a.X * b.Y - b.X * a.Y;

        /// <summary>
        /// ベクトルa, bの外積
        /// </summary>
        public static ftype Det(ftype a, ftype b, ftype c, ftype d) => Cross(a, b, c, d);

        /// <summary>
        /// ベクトルa, bの外積
        /// </summary>
        public static ftype Det(Point2D a, Point2D b) => Cross(a, b);

        /// <summary>
        /// ベクトルaのx軸正方向からの傾き
        /// </summary>
        public static double Arg(Point2D a) => Math.Atan2(a.Y, a.X);

        /// <summary>
        /// ベクトルa, bの外積
        /// </summary>
        public static Point3D Cross(Point3D a, Point3D b) => new Point3D(
            a.Y * b.Z - b.Y * a.Z,
            a.Z * b.X - b.Z * a.X,
            a.X * b.Y - b.X * a.Y
            );

        /// <summary>
        /// 3ベクトルからなる行列(a b c)の行列式
        /// 3ベクトルからなる平行六面体の体積と同値
        /// </summary>
        public static ftype Triple(Point3D a, Point3D b, Point3D c) => Dot(a, Cross(b, c));

        /// <summary>
        /// 2直線（a_i + kd_i）の交点
        /// </summary>
        public static bool Intersect(Point2D a1, Point2D d1, Point2D a2, Point2D d2, out Point2D result)
        {
            result = new Point2D();
            if (Math.Abs(Cross(d1, d2)) <= EPS)
                return false;
            result = a1 + Cross(a2 - a1, d2) / Cross(d1, d2) * d1;
            return true;
        }

        /// <summary>
        /// 2直線ax+by+c=0の交点
        /// </summary>
        public static bool Intersect(Line2D m, Line2D n, out Point2D result)
        {
            result = new Point2D();
            var zn = Cross(m.A, m.B, n.A, n.B);
            if (IsZero(zn))
                return false;
            result.X = -Cross(m.C, m.B, n.C, m.B) / zn;
            result.Y = -Cross(m.A, m.C, n.A, m.C) / zn;
            return true;
        }

        /// <summary>
        /// 数直線上の区間[a,b], [c,d]が共有点を持つか
        /// </summary>
        public static bool Intersect(ftype a, ftype b, ftype c, ftype d)
        {
            if (a > b) swap(ref a, ref b);
            if (c > d) swap(ref c, ref d);
            return Math.Max(a, c) <= Math.Min(b, d);
        }

        /// <summary>
        /// 線分m, nが共有点を持つか
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool Intersect(Segment2D m, Segment2D n)
        {
            if (IsZero(Cross(m.S - n.S, m.S - n.T)) && IsZero(Cross(m.T - n.S, m.T - n.T)))
                return Intersect(m.S.X, m.T.X, n.S.X, n.T.X) && Intersect(m.S.Y, m.T.Y, n.S.Y, n.T.Y);
            return (Sgn(Cross(m.T - m.S, n.S - m.S)) != Sgn(Cross(m.T - m.S, n.T - m.S)))
                && (Sgn(Cross(n.T - n.S, m.S - n.S)) != Sgn(Cross(n.T - n.S, m.T - n.S)));
        }

        /// <summary>
        /// 3平面（点aを通り、法線ベクトルがn）からなる点
        /// </summary>
        public static bool Intersect(Point3D a1, Point3D n1, Point3D a2, Point3D n2, Point3D a3, Point3D n3, out Point3D result)
        {
            result = new Point3D();
            var triple = Triple(n1, n2, n3);
            if (Math.Abs(triple) < EPS)
                return false;
            var x = new Point3D(n1.X, n2.X, n3.X);
            var y = new Point3D(n1.Y, n2.Y, n3.Y);
            var z = new Point3D(n1.Z, n2.Z, n3.Z);
            var d = new Point3D(Dot(a1, n1), Dot(a2, n2), Dot(a3, n3));
            result = new Point3D(Triple(d, y, z),
                                 Triple(x, d, z),
                                 Triple(x, y, d)) / triple;
            return true;
        }

        /// <summary>
        /// 3点a, b, cの順での位置関係
        /// +1: 反時計回り
        /// -1: 時計回り
        /// -2: 一直線上b - a - c
        /// +2: 一直線上a - b - c
        ///  0: 一直線上a - c - b
        /// </summary>
        public static int ISP(Point2D a, Point2D b, Point2D c)
        {
            var signed_area = Cross(b - a, c - a);
            if (signed_area > EPS) return 1;
            if (signed_area < -EPS) return -1;
            if (Dot(b - a, c - a) < -EPS) return -2;
            if (Dot(a - b, c - b) < -EPS) return 2;
            return 0;
        }

        /// <summary>
        /// 2直線m, nが平行かどうか
        /// </summary>
        public static bool IsParallel(Line2D m, Line2D n) => IsZero(Cross(m.A, m.B, n.A, n.B));

        /// <summary>
        /// 2直線が一致するかどうか
        /// </summary>
        public static bool IsEquivalent(Line2D m, Line2D n)
            => IsParallel(m, n) && IsZero(Cross(m.A, m.C, n.A, n.C)) && IsZero(Cross(m.B, m.C, n.B, n.C));
    }
}
