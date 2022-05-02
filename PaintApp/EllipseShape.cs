using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintApp
{
    [Serializable]
    public class CustomShape
    {
        public CustomShape()
        {

        }
        public CustomShape(Point startPoint, Shape shape)
        {
            StartPoint = startPoint;
            Shape = shape;
        }

        public Point StartPoint { get; set; } = new Point(0,0);
        public Shape Shape { get; set; } = new Ellipse();
    }
}
