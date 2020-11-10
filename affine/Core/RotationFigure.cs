using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public class RotationFigure : Figure
    {
        private Axis _mainAxis;
        private Point2D[] _generatix;
        private float _rotation;
        private int _sides;

        public RotationFigure(Axis mainAxis, Point2D[] generatix, int sides) : base()
        {
            if (sides < 3) throw new ArgumentException("Count of sides must be more then 2");
            _mainAxis = mainAxis;
            _generatix = generatix;
            _rotation = 360f / sides;
            _sides = sides;
            Initialize();
        }

        private void Initialize()
        {
            Point2D topVertex = new Point2D(0, _generatix[0].Y);
            Point2D bottomVertex = new Point2D(0, _generatix.Last().Y);
            Point3d[] previous = new Point3d[_sides];

            int index = topVertex != _generatix[0] ? 0 : 1;

            //if (topVertex != _generatix[0])
            {
                float angle = 0;
                Point3d vertex = new Point3d(0, 0, topVertex.Y);
                for (int j = 0; j < _sides; ++j)
                {
                    double radAngle = angle * Math.PI / 180.0;
                    Point3d point = new Point3d(Math.Cos(radAngle) * _generatix[index].X, Math.Sin(radAngle) * _generatix[index].X, _generatix[index].Y);
                    Add(new Line(vertex, point));
                    previous[j] = point;
                    angle += _rotation;
                    if (j > 0)
                    {
                        Add(new Line(point, previous[j - 1]));
                    }
                    if (j == _sides - 1)
                    {
                        Add(new Line(point, previous[0]));
                    }
                }
            }

            ++index;
            // main cycle
            for (int i = index; i < _generatix.Length; ++i)
            {
                float angle = 0;
                for (int j = 0; j < _sides; ++j)
                {
                    double radAngle = angle * Math.PI / 180.0;
                    Point3d point = new Point3d(Math.Cos(radAngle) * _generatix[i].X, Math.Sin(radAngle) * _generatix[i].X, _generatix[i].Y);
                    Add(new Line(previous[j], point));
                    previous[j] = point;
                    angle += _rotation;
                    if (j > 0)
                    {
                        Add(new Line(point, previous[j - 1]));
                    }
                    if (j == _sides - 1)
                    {
                        Add(new Line(point, previous[0]));
                    }
                }
            }

            if (bottomVertex != _generatix.Last())
            {
                Point3d vertex = new Point3d(0, 0, bottomVertex.Y);
                for (int i = 0; i < _sides; ++i)
                {
                    Add(new Line(vertex, previous[i]));
                }
            }
        }
    }
}
