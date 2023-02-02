using System.Collections.Generic;
using System.Drawing;

namespace Import.RosReestrXML
{
    internal class EntityPoint
    {
        public double x;
        public double y;
        public double radius;

        public EntityPoint(double x, double y, double radius)
        {
           this.x = x;
           this.y = y;
           this.radius = radius;
        }
    }
}