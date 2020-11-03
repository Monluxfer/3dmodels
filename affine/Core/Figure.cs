using System;
using System.Collections.Generic;

namespace Render {
	public class Figure {
		public List<Line> lines;

		public void Add(Line l) {
			lines.Add(l);
		}

		public Figure(){
			lines = new List<Line>();
		}
		public List<Line> GetLines(){
			return lines;
		}
		/// <summary>
		/// Поворот фигуры вокруг оси OX
		/// </summary>
		/// <param name="d">Угол поворота</param>
		public void RotateX(double d){
			for (int i=0; i< lines.Count; i++) {
				lines [i].Rotate3Dx (d);
			}
		}
		/// <summary>
		/// Поворот фигуры вокруг оси OY
		/// </summary>
		/// <param name="d">Угол поворота</param>
		public void RotateY(double d){
			for (int i=0; i< lines.Count; i++) {
				lines [i].Rotate3Dy (d);
			}
		}
		/// <summary>
		/// Поворот фигуры вокруг оси OZ
		/// </summary>
		/// <param name="d">Угол поворота</param>
		public void RotateZ(double d){
			for (int i=0; i< lines.Count; i++) {
				lines [i].Rotate3Dz (d);
			}
		}

		/// <summary>
		/// Поворот фигуры вокруг прямой _line на угол angle
		/// </summary>
		/// <param name="_line">Линия, вокруг которой будем поворачивать</param>
		/// <param name="angle">Угол поворота</param>
		public void Rotate(Line _line, double angle){
			Point3d normalize;

			{
				double x = _line.start.x - _line.end.x;
				double y = _line.start.y - _line.end.y;
				double z = _line.start.z - _line.end.z;
				double length = Math.Sqrt (x * x + y * y + z * z);
				normalize = new Point3d (x / length, y / length, z / length);
			}
			double d = Math.Sqrt (normalize.y * normalize.y + normalize.z * normalize.z);
			double alpha = -Math.Asin (normalize.y / d);
			double beta = Math.Asin (normalize.x);
			for (int i = 0; i < lines.Count; i++) {
				lines [i].Smestchenie ((-1) * _line.start.x, (-1) * _line.start.y, (-1) * _line.start.z);

				lines [i].Rotate3Dx (alpha);
				lines [i].Rotate3Dy (beta);
				lines [i].Rotate3Dz (angle);
				lines [i].Rotate3Dy (-beta);
				lines [i].Rotate3Dx (-alpha);

				lines [i].Smestchenie (_line.start.x, _line.start.y, _line.start.z);
			}

					
		}
		/// <summary>
		/// Смещение фигуры в трехмерном пространстве
		/// </summary>
		/// <param name="x">Смещение по х</param>
		/// <param name="y">Смещение по у</param>
		/// <param name="z">Смещение по z</param>
		public void Smestchenie(double x, double y, double z) {
			for (int i=0; i< lines.Count; i++) {
				lines [i].Smestchenie (x, y, z);
			}
		}
		/// <summary>
		/// Масштабирование в пространстве
		/// </summary>
		/// <param name="x">Новый масштаб по х</param>
		/// <param name="y">Новый масштаб по у</param>
		/// <param name="z">Новый масштаб по z</param>
		public void Scale(double x, double y, double z) {
			for (int i=0; i< lines.Count; i++) {
				lines [i].Scale (x, y, z);
			}
		}


		/// <summary>
		/// Сохранить фигуру в файл
		/// </summary>
		/// <param name="filename">Filename.</param>
		public void Save(string filename)
		{
			string res = "";
			for (int i = 0; i < lines.Count; i++)
			{
				res += lines[i].start.x + " " + lines[i].start.y + " " + lines[i].start.z + " " +
					lines[i].end.x + " " + lines[i].end.y + " " + lines[i].end.z + "\n";
			}
			System.IO.File.WriteAllText(filename, res);
		}
		/// <summary>
		/// Загрузить фигуру из файла
		/// </summary>
		/// <param name="filename">Filename.</param>
		public bool Load(string filename)
		{
			bool res = true;

			string[] lines = System.IO.File.ReadAllLines(filename);
			for (int i = 0; i < lines.Length; i++)
			{
				string[] data = lines[i].Split(' ');
				if (data.Length != 6)
					continue;
				double x1, x2, y1, y2, z1, z2;
				if (!Double.TryParse(data[0], out x1))
					res = false;
				if (!Double.TryParse(data[1], out y1))
					res = false;
				if (!Double.TryParse(data[2], out z1))
					res = false;
				if (!Double.TryParse(data[3], out x2))
					res = false;
				if (!Double.TryParse(data[4], out y2))
					res = false;
				if (!Double.TryParse(data[5], out z2))
					res = false;
				if (!res)
					return res;

				Point3d p1 = new Point3d(x1, y1, z1);
				Point3d p2 = new Point3d(x2, y2, z2);
				Line l = new Line(p1, p2);
				this.lines.Add(l);

			}
			return res;
		}

		//Example figure = Figure.Generate(((double x,double y) =>{ return x*x - y*y;}), -50,-50,0,0,1,1);
		/// <summary>
		/// Построение фигуры по функции
		/// </summary>
		/// <param name="f">Функция</param>
		/// <param name="x1">Начало по x</param>
		/// <param name="y1">Начало по y</param>
		/// <param name="x2">Конец по x</param>
		/// <param name="y2">Конец по y</param>
		/// <param name="x_step">Шаг по x</param>
		/// <param name="y_step">Шаг по y</param>
		public static Figure Generate(Func<double, double, double> f, double x1, double y1, double x2, double y2, double x_step, double y_step){
			if (x_step <= 0)
				throw new Exception ("step for x > 0");
			if (y_step <= 0)
				throw new Exception ("step for y > 0");
			if (x1 > x2)
				throw new Exception ("x1 <! x2");
			if (y1 > y2)
				throw new Exception ("y1 <! y2");
			
			Figure res = new Figure ();

			List<Line> prev = new List<Line> ();
			List<Line> th = new List<Line> ();
			Point3d pr = null;

			for (double y = y1; y <= y2; y+=y_step) {
				int i = 0;
				for (double x = x1; x <= x2; x += x_step) {
					double z = f (x, y);
					//Point3d tmp = new Point3d (x, y, z);
					Point3d tmp = new Point3d (z, y, x);
					//Point3d tmp = new Point3d (z, y, x);
					//Console.WriteLine (x);
					if (pr != null) {
						Line l = new Line (pr, tmp);
						th.Add (l);
						res.Add (l);
					}
					if (prev.Count > i) {
						Line tm = new Line (prev [i].start, tmp);
						res.Add (tm);
					} else if (prev.Count != 0) {
						Line tm = new Line (prev [i-1].end, tmp);
						res.Add (tm);
					}
					pr = tmp;
					i++;
				}

				prev = th;
				th = new List<Line> ();
				pr = null;
			}

			return res;
		}
		public Figure2 toVersion2(){
			Figure2 res = new Figure2 ();
			// Огранизуем базовые плоскости
			for (int i = 0; i < lines.Count; i++) {
				for (int j = i + 1; j < lines.Count; j++) {
					if (lines [i].start.Equals (lines [j].start)) {
						Face f = new Face ();
						f.Add (lines [i].end);
						f.Add (lines [i].start);
						f.Add (lines [j].end);
						res.Add (f);
					}
					if (lines [i].start.Equals (lines [j].end)) {
						Face f = new Face ();
						f.Add (lines [i].end);
						f.Add (lines [i].start);
						f.Add (lines [j].start);
						res.Add (f);
					}
					if (lines [i].end.Equals (lines [j].start)) {
						Face f = new Face ();
						f.Add (lines [i].start);
						f.Add (lines [i].end);
						f.Add (lines [j].end);
						res.Add (f);
					}
					if (lines [i].end.Equals (lines [j].end)) {
						Face f = new Face ();
						f.Add (lines [i].start);
						f.Add (lines [i].end);
						f.Add (lines [j].start);
						res.Add (f);
					}
				}
			}
			Figure2 tmp = res;
			res = new Figure2 ();
			// У нас много дублирующихся плоскостей из-за того, что одна
			// плоскость реализована 3 точками, а в плоскости их 
			// может быть больше
			for (int i = 0; i < tmp.Faces ().Count; i++) {
				Face f = tmp.Faces () [i];
				for (int j = i+1; j < tmp.Faces ().Count; j++) {
					Face t = tmp.Faces () [j];

					int eq = 0;
					int pos = -1;

					for (int k = 0; k < t.Points ().Count; k++) {
						if (f.Cont (t.Points () [k]))
							eq++;
						else if (f.isIn (t.Points () [k])) {
							pos = k;
						} else 
							eq--;
					}

					if ((eq+1) == t.Points ().Count) {
						if (pos != -1) {
							tmp._faces [i].Add (t.Points () [pos]);
						}
						tmp._faces.RemoveAt (j);
					}

				}
			}
			// У нас много дублирующихся плоскостей. Удаляем их
			res = tmp.DeleteDoublicete();


			//res.Print ();
			//Console.WriteLine (res.Faces ().Count);
			return res;
		}
	}
}
