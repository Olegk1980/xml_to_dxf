
namespace Import.RosReestrXML
{
    class Bound
    {
        private string desc;
        private string accNum;
        private Contour[] contours;
        private EntitySpatial es;

        public void SetDesc(string desc)
        {
            this.desc = desc;
        }
        public string GetDesc()
        {
            return desc;
        }

        public void SetAccNum(string accNum)
        {
            this.accNum = accNum;
        }
        public string GetAccNum()
        {
            return accNum;
        }
        public void SetContours(Contour[] contours)
        {
            this.contours = contours;
        }
        public Contour[] GetContours()
        {
            return this.contours;
        }
        public void SetEntitySpatial(EntitySpatial es)
        {
            this.es = es;
        }
        public EntitySpatial GetEntitySpatial()
        {
            return es;
        }
    }
}