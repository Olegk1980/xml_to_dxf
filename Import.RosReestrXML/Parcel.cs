using System.Collections.Generic;

namespace Import.RosReestrXML
{
    class Parcel
    {
        private string kn;
        private string area;
        private string status;
        private string name;
        private string category;
        private string[] utilization;
        private string knEZ;
        private Parcel[] parcelEZ;
        private List<string> parentKN;
        private Contour[] contours;
        private EntitySpatial es;
        private SubParcel[] subParcel;        

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
        public void SetStatus(string status)
        {
            this.status = status;
        }
        public string GetStatus()
        {
            return this.status;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return this.name;
        }
        public void SetCategory(string category)
        {
            this.category = category;
        }
        public string GetCategory()
        {
            return this.category;
        }
        public void SetUtilization(string[] utilization)
        {
            this.utilization = utilization;
        }
        public string[] GetUtilization()
        {
            return this.utilization;
        }
        public void SetKadNumEZ(string knEZ)
        {
            this.knEZ = knEZ;
        }
        public string GetKadNumEZ()
        {
            return knEZ;
        }
        public void AddParentKN(List<string> parentKN)
        {
            this.parentKN = parentKN;
        }
        public List<string> GetParentKN()
        {
            return this.parentKN;
        }
        public void AddParcelEZ(Parcel[] parcelEZ)
        {
            this.parcelEZ = parcelEZ;
        }
        public Parcel[] GetParcelEZ()
        {
            return this.parcelEZ;
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
        public void AddSubParcels(SubParcel[] subParcels)
        {
            this.subParcel = subParcels;
        }
        public SubParcel[] GetSubParcels()
        {
            return subParcel;
        }
    }
}