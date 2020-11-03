using System;
using System.Collections.Generic;

namespace Render
{
	public class Face
	{
		private List<Point3d> _points;

		public List<Point3d> Points (){ return _points; }

		public Face (List<Point3d> points){
			_points = points;
		}
		public Face (){
			_points = new List<Point3d> ();
		}
		public void Add(Point3d p){
			_points.Add (p);
		}
		public bool isFacial() { // является ли грань лицевой
			Point3d p1 = _points[1];
			Point3d p2 = _points[2];
			Point3d p3 = _points[0];
			double aX = p1.x - p3.x, aY = p1.y - p3.y, bX = p2.x - p3.x, bY = p2.y - p3.y;
			return (aY * bX - aX * bY < 0);
		}
		public bool Cont(Point3d p){
			for (int i = 0; i < _points.Count; i++) {
				if (_points [i].Equals (p))
					return true;
			}
			return false;
		}
		public virtual bool Equal(Face f){
			int c = 0;
			if (_points.Count != f._points.Count)
				return false;
			
			for (int i = 0; i < _points.Count; i++) {
				for (int j = 0; j < f._points.Count; j++) {
					if (_points [i].Equals (f._points [j])) {
						c++;
						break;
					}
				}
			}
			return c == _points.Count;
		}
		public bool isIn(Point3d p){
			Matrix m = new Matrix (3, 3);
			Point3d p1 = _points[0];
			Point3d p2 = _points[1];
			Point3d p3 = _points[2];

			m.Set (p.x - p1.x, 0, 0);
			m.Set (p.y - p1.y, 0, 1);
			m.Set (p.z - p1.z, 0, 2);
			m.Set (p2.x - p1.x, 1, 0);
			m.Set (p2.y - p1.y, 1, 1);
			m.Set (p2.z - p1.z, 1, 2);
			m.Set (p3.x - p1.x, 2, 0);
			m.Set (p3.y - p1.y, 2, 1);
			m.Set (p3.z - p1.z, 2, 2);

			return m.Det() == 0;

		}
		public void Println(){
			for (int i = 0; i < _points.Count; i++) {
				_points [i].Print ();
			}
			Console.WriteLine ();
		}
		public Point3d Normal(){
			Matrix m = new Matrix (3, 3);
			Point3d p1 = _points[0];
			Point3d p2 = _points[1];
			Point3d p3 = _points[2];

			m.Set (p2.x - p1.x, 1, 0);
			m.Set (p2.y - p1.y, 1, 1);
			m.Set (p2.z - p1.z, 1, 2);
			m.Set (p3.x - p1.x, 2, 0);
			m.Set (p3.y - p1.y, 2, 1);
			m.Set (p3.z - p1.z, 2, 2);

			// Почти уравнение плоскости
			var x = m.Get (1, 1) * m.Get (2, 2) - m.Get (1, 2) * m.Get (2, 1);
			var y = m.Get (0, 1) * m.Get (2, 2) - m.Get (1, 2) * m.Get (2, 0);
			var z = m.Get (1, 0) * m.Get (2, 1) - m.Get (1, 1) * m.Get (2, 0);

			Point3d res = new Point3d (x, y, z);
			return res;
		}

	}
}

