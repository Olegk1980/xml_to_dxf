using System.Collections.Generic;
using System.Drawing;

namespace Import.RosReestrXML
{
    class EntitySpatial
    {        
        public struct SpelementUnit
        {            
            public string TypeUnit;
            public string SuNmb;
            public Ordinate ordinate;
            public double R;
        }
        public struct Ordinate
        {
            public double X;
            public double Y;
            public string OrdNmb;
            public string NumGeopoint;
            public string DeltaGeopoint;
        }
        
        private List<List<SpelementUnit>> spatialElement = new List<List<SpelementUnit>>();

        public void AddSpatialElement()
        {
            spatialElement.Add(new List<SpelementUnit>());
        }
        public void AddSpelementUnit(SpelementUnit su)
        {
            spatialElement[spatialElement.Count - 1].Add(su);
        }

        public List<List<SpelementUnit>> GetSpatialElement()
        {
            return spatialElement;
        }

        public List<EntityPoint> GetEntityPoints()
        {
            List<EntityPoint> points = new List<EntityPoint>();
            foreach (List<EntitySpatial.SpelementUnit> listSUnit in spatialElement)
            {
                /*EntityPoint point = new EntityPoint() { points3d = new List<Point3d>() }; 
                foreach (EntitySpatial.SpelementUnit sUnit in listSUnit)
                {
                    point.points3d.Add(new Point3d(sUnit.ordinate.X, sUnit.ordinate.Y, 0));
                    point.radius = sUnit.R;
                }
                points.Add(point);*/
                foreach (EntitySpatial.SpelementUnit sUnit in listSUnit)
                {
                    if (sUnit.R != 0)
                    {
                        points.Add(new EntityPoint(sUnit.ordinate.X, sUnit.ordinate.Y, sUnit.R));
                    }
                    else
                    {
                        points.Add(new EntityPoint(sUnit.ordinate.X, sUnit.ordinate.Y, 0));
                    }
                }
            }
            return points;
        }

        public List<EntityPoint> GetEntityPoints(List<EntitySpatial.SpelementUnit> spElement)
        {
            List<EntityPoint> points = new List<EntityPoint>();

                foreach (EntitySpatial.SpelementUnit sUnit in spElement)
                {
                    if (sUnit.R != 0)
                    {
                        points.Add(new EntityPoint(sUnit.ordinate.X, sUnit.ordinate.Y, sUnit.R));
                    }
                    else
                    {
                        points.Add(new EntityPoint(sUnit.ordinate.X, sUnit.ordinate.Y, 0));
                    }
                }
            return points;
        }

    }
}