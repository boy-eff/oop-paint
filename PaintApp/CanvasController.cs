using PaintApp.Interfaces;
using PaintApp.ShapeControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PaintApp
{
    internal class CanvasController : ICanvasController
    {
        public CanvasController()
        {
            EllipseController = new EllipseController();
            RectangleController = new RectangleController();
            LineController = new LineController();
            PolygonController = new PolygonController();
            CurrentShapeController = EllipseController;
            TriangleController = new TriangleController();

        }

        public IShapeController CurrentShapeController { get; set; }
        public EllipseController EllipseController { get; set; }
        public RectangleController RectangleController { get; set; }
        public LineController LineController { get; set; }
        public PolygonController PolygonController { get; set; }
        public TriangleController TriangleController { get; set; }
        public Shape CreateShape()
        {
            return CurrentShapeController.CreateShape();
        }
    }
}
