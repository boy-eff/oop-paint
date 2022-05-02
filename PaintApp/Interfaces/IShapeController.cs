using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PaintApp.Interfaces
{
    internal interface IShapeController
    {
        public Shape CreateShape();
    }
}
