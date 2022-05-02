using PaintApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PaintApp.ShapeControllers
{
    internal class RectangleController : IShapeController
    {
        public Shape CreateShape()
        {
            Shape shape = new Rectangle();
            return shape;
        }
    }
}
