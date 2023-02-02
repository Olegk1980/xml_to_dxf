namespace Import.RosReestrXML
{
    class ObjectRealty
    {
        private string kn;
        private string area;
        private string areaType;
        private string objectType;
        private string assignationName;
        private Contour[] contours;
        private EntitySpatial es;

        public void SetKadNum(string kn)
        {
            this.kn = kn;
        }
        public string GetKadNum()
        {
            return kn;
        }
        public void SetArea(string area)
        {
            this.area = area;
        }
        public string GetArea()
        {
            return this.area;
        }
        public void SetAreaType(string areaType)
        {
            this.areaType = areaType;
        }
        public string GetAreaType()
        {
            return areaType;
        }
        public void SetObjectType(string objectType)
        {
            this.objectType = objectType;
        }
        public string GetObjectType()
        {
            return objectType;
        }
        public void SetAssignationName(string assignationName)
        {
            this.assignationName = assignationName;
        }
        public string GetAssignationName()
        {
            return assignationName;
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