namespace Import.RosReestrXML
{
    class SubParcel
    {
        private string kn;
        private EntitySpatial es;

        public void SetKadNum(string kn)
        {
            this.kn = kn;
        }
        public string GetKadNum()
        {
            return kn;
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