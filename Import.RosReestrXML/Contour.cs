namespace Import.RosReestrXML
{
    class Contour
    {
        private string numContour;
        private EntitySpatial es;

        public void SetNumContour(string numContour)
        {
            this.numContour = numContour;
        }
        public string GetNumContour()
        {
            return numContour;
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