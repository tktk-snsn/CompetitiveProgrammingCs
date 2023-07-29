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
            if (GeomUtils.Sgn(this.Y - other.Y) != 0)
                return GeomUtils.Sgn(this.Y - other.Y);
            if (GeomUtils.Sgn(this.X - other.X) != 0)
                return GeomUtils.Sgn(this.X - other.X);
            return 0;
            //if (this.X - other.X < Constant.EPS) return -1;
            //if (this.X - other.X > -Constant.EPS) return 1;
            //if (this.Y - other.Y < Constant.EPS) return -1;
            //if (this.Y - other.Y > -Constant.EPS) return 1;
            //return 0;
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
        public Point2D S { get; set; }
        public Point2D T { get; set; }

        public Line2D(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
            if (!GeomUtils.IsZero(b) && !GeomUtils.IsZero(a))
            {
                S = new Point2D(0, -C / B);
                T = new Point2D(-C / A, 0);
            }
            else if (GeomUtils.IsZero(a))
            {
                S = new Point2D(0, -C / B);
                T = new Point2D(1, -C / B);
            }
            else
            {
                S = new Point2D(-C / A, 0);
                T = new Point2D(-C / A, 1);
            }
        }
        public Line2D(Point2D p, Point2D q)
        {
            A = p.Y - q.Y;
            B = -(p.X - q.X);
            C = -(A * p.X + B * p.Y);
            S = p;
            T = q;
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

    internal class Polygon2D
    {
        public int N { get; }
        public List<Point2D> Points { get; }

        private double _area;
        public double Area { get { return _area; } }

        private bool _isClockwise;
        public bool IsClockwise { get { return _isClockwise; } }

        public Polygon2D(List<Point2D> points)
        {
            N = points.Count;
            Points = points;
            CalcArea();
        }

        private void CalcArea()
        {
            double area = GeomUtils.Cross(Points[N - 1], Points[0]);
            for (int i = 1; i < N; ++i)
                area += GeomUtils.Cross(Points[i - 1], Points[i]);
            _area = Math.Abs(area) * 0.5;
            _isClockwise = GeomUtils.Sgn(area) == -1;
        }
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
        public static bool Intersect(Segment2D m, Segment2D n)
        {
            if (IsZero(Cross(m.S - n.S, m.S - n.T)) && IsZero(Cross(m.T - n.S, m.T - n.T)))
                return Intersect(m.S.X, m.T.X, n.S.X, n.T.X) && Intersect(m.S.Y, m.T.Y, n.S.Y, n.T.Y);
            return (Sgn(Cross(m.T - m.S, n.S - m.S)) != Sgn(Cross(m.T - m.S, n.T - m.S)))
                && (Sgn(Cross(n.T - n.S, m.S - n.S)) != Sgn(Cross(n.T - n.S, m.T - n.S)));
        }

        /// <summary>
        /// 線分segと直線lineが共有点を持つか
        /// </summary>
        public static bool Intersect(Segment2D seg, Line2D line) => ISP(line.S, line.T, seg.S) * ISP(line.S, line.T, seg.T) == 1;

        /// <summary>
        /// 線分segと直線lineが共有点を持つか
        /// </summary>
        public static bool Intersect(Line2D line, Segment2D seg) => Intersect(seg, line);

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

        /// <summary>
        /// 点pと直線mの距離
        /// </summary>
        public static double Distance(Point2D p, Line2D m) => Math.Abs(Cross(m.T - m.S, p - m.S)) / Abs(m.T - m.S);

        /// <summary>
        /// 点pと直線mの距離
        /// </summary>
        public static double Distance(Line2D m, Point2D p) => Distance(p, m);

        /// <summary>
        /// 2直線の距離
        /// </summary>
        public static double Distance(Line2D m, Line2D n)
        {
            if (IsEquivalent(m, n))
                return 0;
            if (!IsParallel(m, n))
                return 0;
            return Distance(m.S, n);
        }

        /// <summary>
        /// 直線lineと線分segの距離
        /// </summary>
        public static double Distance(Line2D line, Segment2D seg)
        {
            if (Intersect(line, seg))
                return Math.Min(Distance(line, seg.S), Distance(line, seg.T));
            return 0;
        }

        /// <summary>
        /// 直線lineと線分segの距離
        /// </summary>
        public static double Distance(Segment2D seg, Line2D line) => Distance(line, seg);

        /// <summary>
        /// 線分segと点pの距離
        /// </summary>
        public static double Distance(Segment2D seg, Point2D p)
        {
            if (Sgn(Dot(seg.T - seg.S, p - seg.S)) == -1 || Sgn(Dot(seg.S - seg.T, p - seg.T)) == -1)
                return Math.Min(Abs(p - seg.S), Abs(p - seg.T));
            return Math.Abs(Cross(seg.T - seg.S, p - seg.S)) / Abs(seg.T - seg.S);
        }

        /// <summary>
        /// 線分segと点pの距離
        /// </summary>
        public static double Distance(Point2D p, Segment2D seg) => Distance(seg, p);

        /// <summary>
        /// 線分m, nの距離
        /// </summary>
        public static double Distance(Segment2D m, Segment2D n)
        {
            if (Intersect(m, n))
                return 0;
            double res = Math.Min(Distance(m, n.S), Distance(m, n.T));
            res = Math.Min(res, Distance(m.S, n));
            res = Math.Min(res, Distance(m.T, n));
            return res;
        }

        /// <summary>
        /// 多角形が凸か判定する
        /// </summary>
        public static bool IsConvex(Polygon2D poly)
        {
            int N = poly.N;
            bool cw = false;
            bool ccw = false;
            for (int i = 0; i < N; ++i)
            {
                int isp = ISP(poly.Points[i], poly.Points[(i + 1) % N], poly.Points[(i + 2) % N]);
                if (isp == 1) cw = true;
                if (isp == -1) ccw = true;
            }
            return !(cw && ccw);
        }

        /// <summary>
        /// 点pが多角形の内部にあれば2, 辺上にあれば1, 外部にあれば0を返す
        /// </summary>
        public static int IsInside(Polygon2D poly, Point2D p)
        {
            int n = poly.N;
            bool isInside = false;
            for (int i = 0; i < n; ++i)
            {
                var a = poly.Points[i] - p;
                var b = poly.Points[(i + 1) % n] - p;
                if (Sgn(Dot(a, b)) != 1 && Sgn(Cross(a, b)) == 0)
                    return 1;
                if (a.Y > b.Y)
                    swap(ref a, ref b);
                if (Sgn(a.Y) != 1 && Sgn(b.Y) == 1 && Sgn(Cross(a, b)) == -1)
                    isInside = !isInside;
            }
            return isInside ? 2 : 0;
        }

        /// <summary>
        /// 多角形polyの凸包を返す
        /// </summary>
        public static Polygon2D GetConvexHull(Polygon2D poly, bool isClockwise = true)
        {
            int ng = isClockwise ? 1 : -1;

            int n = poly.N;
            var points = poly.Points;
            points.Sort();

            List<Point2D> upper = new List<Point2D>();
            foreach (var p in points)
            {
                while (upper.Count >= 2 && ISP(upper[upper.Count - 2], upper[upper.Count - 1], p) == ng)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }
            upper.RemoveAt(upper.Count - 1);

            List<Point2D> lower = new List<Point2D>();
            points.Reverse();
            foreach (var p in points)
            {
                while (lower.Count >= 2 && ISP(lower[lower.Count - 2], lower[lower.Count - 1], p) == ng)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }
            lower.RemoveAt(lower.Count - 1);

            List<Point2D> hull = new List<Point2D>();
            foreach (var p in upper)
                hull.Add(p);
            foreach (var p in lower)
                hull.Add(p);
            return new Polygon2D(hull);
        }

        /// <summary>
        /// 凸包上の最遠点対の距離を求める
        /// </summary>
        /// <param name="hull"></param>
        /// <returns></returns>
        public static double RotatingCaliper(Polygon2D hull)
        {
            int n = hull.N;
            double dmax = double.MinValue;

            Point2D p;
            int i, j = 0;
            for (i = 0; i < n; ++i)
            {
                p = hull.Points[j] - hull.Points[i];
                while (true)
                {
                    if (Sgn(Abs(hull.Points[(j + 1) % n] - hull.Points[i]) - Abs(p)) != 1)
                        break;
                    j = (j + 1) % n;
                    p = hull.Points[j] - hull.Points[i];
                }
                if (Abs(p) > dmax)
                {
                    dmax = Abs(p);
                }
            }
            return dmax;
        }
    }
}
