namespace Import.RosReestrXML
{
    class OMSPoint
    {
        public string numPunkt;
        public string name;
        public string klass;
        public double ordX;
        public double ordY;
        
        public void SetNumPunkt(string numPunkt)
        {
            this.numPunkt = numPunkt;
        }
        public string GetNumPunkt()
        {
            return numPunkt;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }
        public void SetKlass(string klass)
        {
            this.klass = klass;
        }
        public string GetKlass()
        {
            return klass;
        }
        public void SetOrdX(double ordX)
        {
            this.ordX = ordX;
        }
        public double GetOrdX()
        {
            return this.ordX;
        }
        public void SetOrdY(double ordY)
        {
            this.ordY = ordY;
        }
        public double GetOrdY()
        {
            return this.ordY;
        }
    }
}