using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintApp
{
    internal class MyShape : Shape
    {
        protected override Geometry DefiningGeometry => throw new NotImplementedException();
    }
}
