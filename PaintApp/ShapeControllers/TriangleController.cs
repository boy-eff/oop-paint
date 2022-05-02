using PaintApp.Interfaces;
using PaintApp.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PaintApp.ShapeControllers
{
    internal class TriangleController : IShapeController
    {
        public Shape CreateShape()
        {
            Shape shape = new MyWeirdShape();
            return shape;
        }
    }
}
