using System.Collections.Generic;

namespace Import.RosReestrXML
{
    class Block
    {
        private string kn;
        private string area;
        private Parcel[] parcels;
        private ObjectRealty[] objectsRealty;
        private OMSPoint[] omsPoints;
        private Bound[] bounds;
        private Zone[] zones;
        private EntitySpatial es;

        public void SetKadNum(string kn)
        {
            this.kn = kn;
        }
        public string GetKadNum()
        {
            if (kn == null)
            {
                return "NoNameCadastre";
            }
            return kn.Replace(':', '_');
        }
        public void SetArea(string area)
        {
            this.area = area;
        }
        public string GetArea()
        {
            return this.area;
        }
        public void SetEntitySpatial(EntitySpatial es)
        {
            this.es = es;
        }
        public EntitySpatial GetEntitySpatial()
        {
            return es;
        }
        public void AddParcels(List<Parcel> parcels)
        {
            this.parcels = parcels.ToArray();
        }
        public Parcel[] GetParcels()
        {
            return this.parcels;
        }
        public void AddObjectsRealty(List<ObjectRealty> objectsRealty)
        {
            this.objectsRealty = objectsRealty.ToArray();
        }
        public ObjectRealty[] GetObjectsRealty()
        {
            return this.objectsRealty;
        }
        public void AddOMSPoints(List<OMSPoint> omsPoints)
        {
            this.omsPoints = omsPoints.ToArray();
        }
        public OMSPoint[] GetOMSPoints()
        {
            return this.omsPoints;
        }
        public void AddZones(List<Zone> zones)
        {
            this.zones = zones.ToArray();
        }
        public Zone[] GetZones()
        {
            return this.zones;
        }
        public void AddBounds(List<Bound> bounds)
        {
            this.bounds = bounds.ToArray();
        }
        public Bound[] GetBounds()
        {
            return this.bounds;
        }
    }
}