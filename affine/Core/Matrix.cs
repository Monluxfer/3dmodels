using System;
namespace Render
{
	public class Matrix {
		private double[][] _matr;
		private int _m;
		private int _n;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Render.Matrix"/> class.
		/// </summary>
		/// <param name="n"> Кол-во строк</param>
		/// <param name="m"> Кол-во столбцов</param>
		public Matrix(int n, int m) {
			if (n < 0 || m < 0)
				throw new Exception("size>0");
			_m = m;
			_n = n;
			_matr = new double[_m][];
			for (int i = 0; i < _m; i++)
				_matr[i] = new double[_n];
		}
		// Вывод на консоль матрицы
		public void Print(){
			for (int i = 0; i < _m; i++){
				for (int j = 0; j < _n; j++){
					Console.Write(_matr[i][j]);
					Console.Write(" ");
				}
				Console.WriteLine();
			}

		}
		// Задать значение элементу значение
		/// <summary>
		/// Задать значение элементу
		/// </summary>
		/// <param name="x">Значение</param>
		/// <param name="i">Значение по  N (строка)</param>
		/// <param name="j">Значение по M (столбец)</param>
		public void Set(double x, int i, int j){
			if (i > _m || j > _n || i < 0 || j < 0)
				throw new Exception();
			_matr[i][j] = x;
		}
		// Получить значение элемента
		/// <summary>
		/// Получение значение элемента
		/// </summary>
		/// <param name="i">Значение по  N (строка)</param>
		/// <param name="j">Значение по M (столбец)</param>
		public double Get(int i, int j) {
			if (i > _m || j > _n || i < 0 || j < 0)
				throw new Exception();
			return _matr[i][j];
		}
		// Получить 
		public int M() {
			return _m;
		}
		public int N() {
			return _n;
		}

		// Перемножение матриц
		public Matrix Mult(Matrix m2) {
			Matrix res = new Matrix(m2._n, _m);

			for (int i = 0; i < _m; i++) {
				for (int j = 0; j < m2._n; j++) {
					for (int k = 0; k < m2._m; k++) {
						res._matr[i][j] += _matr[i][k] * m2._matr[k][j];
					}
				}
			}

			return res;
		}
		public Point3d ToPoint() {
			if (_m >= 4)
				return new Point3d(_matr[0][0] / _matr[3][0], _matr[1][0] / _matr[3][0], _matr[2][0] / _matr[3][0]);

			if (_n >= 4)
				return new Point3d (_matr [0] [0] / _matr [0] [3], _matr [0] [1] / _matr [0] [3], _matr [0] [2] / _matr [0] [3]);

			if (_m == 3) {
				return new Point3d(_matr[0][0] / _matr[2][0], _matr[1][0] / _matr[2][0]);
			}
			else {
				return new Point3d(_matr[0][0] / _matr[0][2], _matr[0][1] / _matr[0][2]);
			}
		}
		/// <summary>
		/// Получение матрицы смещения для двумерного случая
		/// </summary>
		/// <param name="x"Смещение по х</param>
		/// <param name="y">Смещение по у</param>
		public static Matrix Smestchenie(double x, double y) {
			Matrix p = new Matrix(3, 3);
			p.Set(1, 0, 0);
			p.Set(1, 1, 1);
			p.Set(1, 2, 2);
			p.Set(x, 2, 0);
			p.Set(y, 2, 1);
			return p;
		}
		/// <summary>
		/// Получение матрицы для трехмерного случая
		/// </summary>
		/// <param name="x">Смещение по x</param>
		/// <param name="y">Смещение по у</param>
		/// <param name="z">Смещение по z</param>
		public static Matrix Smestchenie(double x, double y, double z) {
			Matrix p = new Matrix(4, 4);
			p.Set(1, 0, 0);
			p.Set(1, 1, 1);
			p.Set(1, 2, 2);
			p.Set(1, 3, 3);
			p.Set(x, 3, 0);
			p.Set(y, 3, 1);
			p.Set(z, 3, 2);
			return p;

		}
		/// <summary>
		/// Получение матрицы поворота
		/// </summary>
		/// <returns>Матрица</returns>
		/// <param name="x">Угол, на который повернуть</param>
		public static Matrix Rotate2D(double x){
			Matrix p = new Matrix(3, 3);
			p.Set(Math.Cos(x), 0, 0);
			p.Set(Math.Sin(x), 0, 1);
			p.Set(Math.Sin(x) * (-1), 1, 0);
			p.Set(Math.Cos(x), 1, 1);
			p.Set(1, 2, 2);
			return p;
		}
		/// <summary>
		/// Получение матрицы поворота по X
		/// </summary>
		/// <param name="x">K.</param>
		public static Matrix Rotate3Dx(double x){
			Matrix res = new Matrix(4,4);
			res.Set(1, 0, 0);
			res.Set(1, 3, 3);
			res.Set(Math.Cos(x), 1, 1);
			res.Set(Math.Sin(x) * (-1), 1, 2);
			res.Set(Math.Sin(x), 2, 1);
			res.Set(Math.Cos(x), 2, 2);
			return res;
		}
		/// <summary>
		/// Получение матрицы поворота по Y
		/// </summary>
		/// <returns>Матрица</returns>
		/// <param name="x">Угол поворота<param>
		public static Matrix Rotate3Dy(double x) {
			Matrix res = new Matrix(4, 4);
			res.Set(Math.Cos(x), 0, 0);
			res.Set(Math.Sin(x), 0, 2);
			res.Set(1, 1, 1);
			res.Set((-1) * Math.Sin(x), 2, 0);
			res.Set(Math.Cos(x), 2, 2);
			res.Set(1, 3, 3);
			return res;
		}
		public static Matrix Rotate3Dz(double x) {
			Matrix res = new Matrix(4, 4);
			res.Set(Math.Cos(x), 0, 0);
			res.Set((-1)*Math.Sin(x), 0, 1);
			res.Set(Math.Sin(x), 1, 0);
			res.Set(Math.Cos(x), 1, 1);
			res.Set(1, 2, 2);
			res.Set(1, 3, 3);
			return res;
		}

		public static Matrix Size(double k)
		{
			Matrix p = new Matrix(3, 3);
			p.Set(k, 0, 0);
			p.Set(k, 1, 1);
			p.Set(1, 2, 2);
			return p;
		}
		/// <summary>
		/// Получение матрицы для изменния размера
		/// </summary>
		/// <returns>The d.</returns>
		/// <param name="x">Изменение размера по X</param>
		/// <param name="y">Изменение размера по Y</param>
		/// <param name="z">Изменение размера по Z</param>
		public static Matrix Size3D(double x, double y, double z) {
			Matrix res = new Matrix(4, 4);
			res.Set(x, 0, 0);
			res.Set(y, 1, 1);
			res.Set(z, 2, 2);
			res.Set(1, 3, 3);
			return res;
		}
		public double Det(){
			double det = 1;
			const double EPS = 1E-9;
			double[][] b = new double[1][];
			b[0] = new double[_n];
			//проходим по строкам
			for (int i=0; i<_n; ++i) {
				//присваиваем k номер строки
				int k = i;
				//идем по строке от i+1 до конца
				for (int j=i+1; j<_n; ++j)
					//проверяем
					if (Math.Abs(_matr[j] [i]) > Math.Abs(_matr[k][i]))
						//если равенство выполняется то k присваиваем j
						k = j;
				//если равенство выполняется то определитель приравниваем 0 и выходим из программы
				if (Math.Abs(_matr[k] [i]) < EPS) {
					det = 0;
					break;
				}
				//меняем местами a[i] и a[k]
				b[0] = _matr[i];
				_matr[i] = _matr[k];
				_matr[k] = b[0];
				//если i не равно k
				if (i != k)
					//то меняем знак определителя
					det = -det;
				//умножаем det на элемент a[i][i]
				det *= _matr[i][i];
				//идем по строке от i+1 до конца
				for (int j=i+1; j<_n; ++j)
					//каждый элемент делим на a[i][i]
					_matr[i][j] /= _matr[i][i];
				//идем по столбцам
				for (int j=0; j<_n; ++j)
					//проверяем
					if ((j != i)&&(Math.Abs(_matr[j][i]) > EPS))
						//если да, то идем по k от i+1
						for (k = i+1; k < _n; ++k)
							_matr[j][k] -= _matr[i][k] * _matr[j][i];
			}
			return det;
		}
	}
}
