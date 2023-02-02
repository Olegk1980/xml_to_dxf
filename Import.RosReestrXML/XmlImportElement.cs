using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Import.RosReestrXML
{
    class XmlImportElement
    {
        private XmlReader reader;
        public XmlImportElement(XmlReader reader)
        {
            this.reader = reader;
        }

        public Parcel Parcel()
        {
            Parcel parcel = new Parcel();
            List<Contour> contours = new List<Contour>();
            XDocument doc = XDocument.Parse(reader.ReadOuterXml());
            XElement e = doc.Root;

            if (e.Name.LocalName == "Parcel")
            {
                parcel.SetKadNum(e.Attribute("CadastralNumber").Value);
                parcel.SetArea(e.Element(e.Name.Namespace + "Area").Element(e.Name.Namespace + "Area").Value);
                if (e.Element(e.Name.Namespace + "Name") != null)
                {
                    parcel.SetName(e.Element(e.Name.Namespace + "Name").Value);
                }
                if (e.Element(e.Name.Namespace + "Category") != null)
                {
                    parcel.SetCategory(e.Element(e.Name.Namespace + "Category").Value);
                }                
                if (e.Element(e.Name.Namespace + "Contours") != null)
                {
                    foreach (XElement elContour in e.Element(e.Name.Namespace + "Contours").Elements(e.Name.Namespace + "Contour"))
                    {
                        Contour contour = new Contour();
                        if (elContour.Attribute("NumberRecord") != null)
                        {
                            contour.SetNumContour(elContour.Attribute("NumberRecord").Value);
                        }
                        if (elContour.Attribute("Number_Record") != null)
                        {
                            contour.SetNumContour(elContour.Attribute("Number_Record").Value);
                        }
                        contour.SetEntitySpatial(ReadEntitySpatial(elContour));
                        contours.Add(contour);
                    }
                    parcel.SetContours(contours.ToArray());
                }

                if (e.Element(e.Name.Namespace + "EntitySpatial") != null || e.Element(e.Name.Namespace + "Entity_Spatial") != null)
                {
                    parcel.SetEntitySpatial(ReadEntitySpatial(e));
                }
            }
            else if (e.Name.LocalName == "land_record")
            {
                parcel.SetKadNum(e.Element("object").Element("common_data").Element("cad_number").Value);
                if (e.Element("object").Element("subtype") != null)
                {
                    parcel.SetName(e.Element("object").Element("subtype").Element("value").Value);
                }
                if (e.Element("params").Element("area") != null)
                {
                    parcel.SetArea(e.Element("params").Element("area").Element("value").Value);
                }
                if (e.Element("params").Element("category") != null)
                {
                    parcel.SetCategory(e.Element("params").Element("category").Element("type").Element("value").Value);
                }
                if (e.Element("contours_location") != null)
                {
                    if (e.Element("contours_location").Element("contours").Element("contour").Element("number_pp") != null)
                    {
                        foreach (XElement elContour in e.Element("contours_location").Element("contours").Elements("contour"))
                        {
                            Contour contour = new Contour();
                            if (elContour.Element("number_pp") != null)
                            {
                                contour.SetNumContour(elContour.Element("number_pp").Value);
                            }
                            contour.SetEntitySpatial(ReadEntitySpatial(elContour));
                            contours.Add(contour);
                        }
                        parcel.SetContours(contours.ToArray());
                    }
                    else
                    {
                        parcel.SetEntitySpatial(ReadEntitySpatial(e.Element("contours_location").Element("contours").Element("contour")));
                    }
                }
            }
            Console.Write(".");
            return parcel;
        }

        public ObjectRealty ObjectRealty()
        {
            ObjectRealty objRealty = new ObjectRealty();
            List<Contour> contours = new List<Contour>();
            XDocument doc = XDocument.Parse(reader.ReadOuterXml());
            XElement e = doc.Root;

            if (e.Name.LocalName == "ObjectRealty")
            {
                if (e.Element(e.Name.Namespace + "Building") != null)
                {
                    XElement elBuilding = e.Element(e.Name.Namespace + "Building");
                    objRealty.SetKadNum(elBuilding.Attribute("CadastralNumber").Value);
                    objRealty.SetObjectType(elBuilding.Element(elBuilding.Name.Namespace + "ObjectType").Value);
                    objRealty.SetAssignationName(elBuilding.Element(elBuilding.Name.Namespace + "AssignationBuilding").Value);
                    if (elBuilding.Element(elBuilding.Name.Namespace + "Area") != null)
                    {
                        objRealty.SetArea(elBuilding.Element(elBuilding.Name.Namespace + "Area").Value);
                    }
                    if (elBuilding.Element(elBuilding.Name.Namespace + "EntitySpatial") != null)
                    {
                        objRealty.SetEntitySpatial(ReadEntitySpatial(elBuilding));

                    }
                }
                if (e.Element(e.Name.Namespace + "Construction") != null)
                {
                    XElement elConstraction = e.Element(e.Name.Namespace + "Construction");
                    objRealty.SetKadNum(elConstraction.Attribute("CadastralNumber").Value);
                    objRealty.SetObjectType(elConstraction.Element(elConstraction.Name.Namespace + "ObjectType").Value);
                    if (elConstraction.Element(elConstraction.Name.Namespace + "AssignationName") != null)
                    {
                        objRealty.SetAssignationName(elConstraction.Element(elConstraction.Name.Namespace + "AssignationName").Value);
                    }
                    if (elConstraction.Element(elConstraction.Name.Namespace + "KeyParameters") != null)
                    {
                        StringBuilder typeParameter = new StringBuilder();
                        StringBuilder valueParameter = new StringBuilder();
                        XElement keyParameters = elConstraction.Element(elConstraction.Name.Namespace + "KeyParameters");
                        foreach (XElement keyParameter in keyParameters.Elements(keyParameters.Elements().First().Name.Namespace + "KeyParameter"))
                        {
                            typeParameter.Append(keyParameter.Attribute("Type").Value);
                            valueParameter.Append(keyParameter.Attribute("Value").Value);
                        }
                        objRealty.SetAreaType(typeParameter.ToString());
                        objRealty.SetArea(valueParameter.ToString());
                    }
                    if (elConstraction.Element(elConstraction.Name.Namespace + "EntitySpatial") != null)
                    {
                        objRealty.SetEntitySpatial(ReadEntitySpatial(elConstraction));
                    }
                }
                if (e.Element(e.Name.Namespace + "Uncompleted") != null)
                {
                    XElement elUncompleted = e.Element(e.Name.Namespace + "Uncompleted");
                    objRealty.SetKadNum(elUncompleted.Attribute("CadastralNumber").Value);
                    if (elUncompleted.Element(elUncompleted.Name.Namespace + "ObjectType") != null)
                    {
                        objRealty.SetObjectType(elUncompleted.Element(elUncompleted.Name.Namespace + "ObjectType").Value);
                    }
                    if (elUncompleted.Element(elUncompleted.Name.Namespace + "AssignationName") != null)
                    {
                        objRealty.SetAssignationName(elUncompleted.Element(elUncompleted.Name.Namespace + "AssignationName").Value);
                    }
                    if (elUncompleted.Element(elUncompleted.Name.Namespace + "KeyParameters") != null)
                    {
                        StringBuilder typeParameter = new StringBuilder();
                        StringBuilder valueParameter = new StringBuilder();
                        XElement keyParameters = elUncompleted.Element(elUncompleted.Name.Namespace + "KeyParameters");
                        foreach (XElement keyParameter in keyParameters.Elements(keyParameters.Elements().First().Name.Namespace + "KeyParameter"))
                        {
                            typeParameter.Append(keyParameter.Attribute("Type").Value);
                            valueParameter.Append(keyParameter.Attribute("Value").Value);
                        }
                        objRealty.SetAreaType(typeParameter.ToString());
                        objRealty.SetArea(valueParameter.ToString());
                    }
                    if (elUncompleted.Element(elUncompleted.Name.Namespace + "EntitySpatial") != null)
                    {
                        objRealty.SetEntitySpatial(ReadEntitySpatial(elUncompleted));
                    }
                }
            }
            else if (e.Name.LocalName == "build_record" || e.Name.LocalName == "construction_record" 
                || e.Name.LocalName == "object_under_construction_record")
            {
                objRealty.SetKadNum(e.Element("object").Element("common_data").Element("cad_number").Value);
                objRealty.SetObjectType(e.Element("object").Element("common_data").Element("type").Element("value").Value);
                if (e.Element("params") != null)
                {
                    if (e.Element("params").Element("purpose") != null)
                    {
                        if (e.Element("params").Element("purpose").Element("value") != null)
                        {
                            objRealty.SetAssignationName(e.Element("params").Element("purpose").Element("value").Value);
                        }
                        else
                        {
                            objRealty.SetAssignationName(e.Element("params").Element("purpose").Value);
                        }
                    }
                    if (e.Element("params").Element("area") != null)
                    {
                        objRealty.SetArea(e.Element("params").Element("area").Value);
                    }
                    if (e.Element("params").Element("base_parameters") != null)
                    {
                        foreach (XElement baseParam in e.Element("params").Element("base_parameters").Elements("base_parameter"))
                        {
                            if (baseParam.Element("area") != null)
                            {
                                objRealty.SetAreaType("Площадь в кв. метрах");
                                objRealty.SetArea(baseParam.Element("area").Value);
                            }
                            if (baseParam.Element("built_up_area") != null)
                            {
                                objRealty.SetAreaType("Площадь застройки в квадратных метрах с округлением до 0,1 квадратного метра");
                                objRealty.SetArea(baseParam.Element("built_up_area").Value);
                            }
                            if (baseParam.Element("extension") != null)
                            {
                                objRealty.SetAreaType("Протяженность в метрах с округлением до 1 метра");
                                objRealty.SetArea(baseParam.Element("extension").Value);
                            }
                            if (baseParam.Element("depth") != null)
                            {
                                objRealty.SetAreaType("Глубина в метрах с округлением до 0,1 метра");
                                objRealty.SetArea(baseParam.Element("depth").Value);
                            }
                            if (baseParam.Element("occurence_depth") != null)
                            {
                                objRealty.SetAreaType("Глубина залегания в метрах с округлением до 0,1 метра");
                                objRealty.SetArea(baseParam.Element("occurence_depth").Value);
                            }
                            if (baseParam.Element("volume") != null)
                            {
                                objRealty.SetAreaType("Объем в кубических метрах с округлением до 1 кубического метра");
                                objRealty.SetArea(baseParam.Element("volume").Value);
                            }
                            if (baseParam.Element("height") != null)
                            {
                                objRealty.SetAreaType("Высота в метрах с округлением до 0,1 метра");
                                objRealty.SetArea(baseParam.Element("height").Value);
                            }
                        }
                    }
                }
                if (e.Element("contours") != null)
                {
                    if (e.Element("contours").Element("contour").Element("number_pp") != null)
                    {
                        foreach (XElement elContour in e.Element("contours").Elements("contour"))
                        {
                            Contour contour = new Contour();
                            contour.SetNumContour(elContour.Element("number_pp").Value);
                            contour.SetEntitySpatial(ReadEntitySpatial(elContour));
                            contours.Add(contour);
                        }
                        objRealty.SetContours(contours.ToArray());
                    }
                    else
                    {
                        objRealty.SetEntitySpatial(ReadEntitySpatial(e.Element("contours").Element("contour")));
                    }
                }
            }
            Console.Write(".");
            return objRealty;
        }

        public OMSPoint OMSPoint()
        {
            OMSPoint omsPoint = new OMSPoint();
            XDocument doc = XDocument.Parse(reader.ReadOuterXml());
            XElement e = doc.Root;

            if (e.Name.LocalName == "OMSPoint")
            {
                omsPoint.SetNumPunkt(e.Element(e.Name.Namespace + "PNmb").Value);
                omsPoint.SetName(e.Element(e.Name.Namespace + "PName").Value);
                omsPoint.SetKlass(e.Element(e.Name.Namespace + "PKlass").Value);
                omsPoint.SetOrdX(double.Parse(e.Element(e.Name.Namespace + "OrdY").Value, CultureInfo.InvariantCulture));
                omsPoint.SetOrdY(double.Parse(e.Element(e.Name.Namespace + "OrdX").Value, CultureInfo.InvariantCulture));
            }
            if (e.Name.LocalName == "oms_point")
            {
                omsPoint.SetNumPunkt(e.Element("p_nmb").Value);
                omsPoint.SetName(e.Element("p_name").Value);
                omsPoint.SetKlass(e.Element("p_klass").Value);
                omsPoint.SetOrdX(double.Parse(e.Element("ord_y").Value, CultureInfo.InvariantCulture));
                omsPoint.SetOrdY(double.Parse(e.Element("ord_x").Value, CultureInfo.InvariantCulture));
            }
            Console.Write(".");
            return omsPoint;
        }

        public Bound Bound()
        {
            Bound bound = new Bound();
            List<Contour> contours = new List<Contour>();
            XDocument doc = XDocument.Parse(reader.ReadOuterXml());
            XElement e = doc.Root;

            if (e.Name.LocalName == "Bound")
            {
                bound.SetDesc(e.Element(e.Name.Namespace + "Description").Value);
                bound.SetAccNum(e.Element(e.Name.Namespace + "AccountNumber").Value);
                if (e.Element(e.Name.Namespace + "Boundaries") != null)
                {
                    foreach (XElement elBoundary in e.Element(e.Name.Namespace + "Boundaries").Elements(e.Name.Namespace + "Boundary"))
                    {
                        bound.SetEntitySpatial(ReadEntitySpatial(elBoundary));
                    }
                }
            }
            if (e.Name.LocalName == "subject_boundary_record" || e.Name.LocalName == "municipal_boundary_record"
                || e.Name.LocalName == "inhabited_locality_boundary_record"
                || e.Name.LocalName == "coastline_record")
            {
                bound.SetDesc(e.Descendants("b_object").ElementAt(0).Element("type_boundary").Element("value").Value);
                bound.SetAccNum(e.Descendants("b_object").ElementAt(0).Element("reg_numb_border").Value);
                if (e.Element("b_contours_location") != null)
                {
                    if (e.Element("b_contours_location").Element("contours").Element("contour").Element("number_pp") != null)
                    {
                        foreach (XElement elContour in e.Element("b_contours_location").Element("contours").Elements("contour"))
                        {
                            Contour contour = new Contour();
                            if (elContour.Element("number_pp") != null)
                            {
                                contour.SetNumContour(elContour.Element("number_pp").Value);
                            }
                            contour.SetEntitySpatial(ReadEntitySpatial(elContour));
                            contours.Add(contour);
                        }
                        bound.SetContours(contours.ToArray());
                    }
                    else
                    {
                        bound.SetEntitySpatial(ReadEntitySpatial(e.Element("b_contours_location").Element("contours").Element("contour")));
                    }
                }
            }
            Console.Write(".");
            return bound;
        }

        public Zone Zone()
        {
            Zone zone = new Zone();
            List<Contour> contours = new List<Contour>();
            XDocument doc = XDocument.Parse(reader.ReadOuterXml());
            XElement e = doc.Root;
            if (e.Name.LocalName == "Zone")
            {
                if (e.Element(e.Name.Namespace + "Description") != null)
                {
                    zone.SetDesc(e.Element(e.Name.Namespace + "Description").Value);
                }                
                zone.SetAccNum(e.Element(e.Name.Namespace + "AccountNumber").Value);
                zone.SetEntitySpatial(ReadEntitySpatial(e));
            }
            if (e.Name.LocalName == "zones_and_territories_record")
            {
                if (e.Element("b_object_zones_and_territories").Element("description") != null)
                {
                    zone.SetDesc(e.Element("b_object_zones_and_territories").Element("description").Value);
                }
                zone.SetAccNum(e.Element("b_object_zones_and_territories").Element("b_object").Element("reg_numb_border").Value);
                if (e.Element("b_contours_location") != null)
                {
                    if (e.Element("b_contours_location").Element("contours").Element("contour").Element("number_pp") != null)
                    {
                        foreach (XElement elContour in e.Element("b_contours_location").Element("contours").Elements("contour"))
                        {
                            Contour contour = new Contour();
                            if (elContour.Element("number_pp") != null)
                            {
                                contour.SetNumContour(elContour.Element("number_pp").Value);
                            }
                            contour.SetEntitySpatial(ReadEntitySpatial(elContour));
                            contours.Add(contour);
                        }
                        zone.SetContours(contours.ToArray());
                    }
                    else
                    {
                        zone.SetEntitySpatial(ReadEntitySpatial(e.Element("b_contours_location").Element("contours").Element("contour")));
                    }
                }
            }
            Console.Write(".");
            return zone;
        }

        public EntitySpatial ReadEntitySpatial(XElement e)
        {
            EntitySpatial es = new EntitySpatial();
            if (e.Name.LocalName == "Parcel" || e.Name.LocalName == "Building" || e.Name.LocalName == "Construction" || e.Name.LocalName == "Uncompleted" ||
                e.Name.LocalName == "Contour" || e.Name.LocalName == "SpatialData" || e.Name.LocalName == "Zone" || e.Name.LocalName == "Boundary")
            {
                XElement entSpatial = e.Element(e.Name.Namespace + "EntitySpatial");
                if (entSpatial != null)
                {
                    foreach (XElement elSpatial in entSpatial.Elements(entSpatial.Elements().First().Name.Namespace + "SpatialElement"))
                    {
                        es.AddSpatialElement();
                        foreach (XElement elSpUnit in elSpatial.Elements(elSpatial.Name.Namespace + "SpelementUnit"))
                        {
                            var su = new EntitySpatial.SpelementUnit();
                            su.TypeUnit = elSpUnit.Attribute("TypeUnit").Value;
                            su.SuNmb = (elSpUnit.Attribute("SuNmb") != null) ? elSpUnit.Attribute("SuNmb").Value : null;
                            su.R = (elSpUnit.Element(elSpUnit.Name.Namespace + "R") != null) ? double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "R").Value, CultureInfo.InvariantCulture) : 0;

                            var ord = new EntitySpatial.Ordinate();
                            ord.X = double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Y").Value, CultureInfo.InvariantCulture);
                            ord.Y = double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("X").Value, CultureInfo.InvariantCulture);
                            ord.OrdNmb = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("OrdNmb") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("OrdNmb").Value : null;
                            ord.NumGeopoint = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("NumGeopoint") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("NumGeopoint").Value : null;
                            ord.DeltaGeopoint = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("DeltaGeopoint") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("DeltaGeopoint").Value : null;

                            su.ordinate = ord;

                            es.AddSpelementUnit(su);
                        }
                    }
                }
                entSpatial = e.Element(e.Name.Namespace + "Entity_Spatial");
                if (entSpatial != null)
                {
                    foreach (XElement elSpatial in entSpatial.Elements(entSpatial.Elements().First().Name.Namespace + "Spatial_Element"))
                    {
                        es.AddSpatialElement();
                        foreach (XElement elSpUnit in elSpatial.Elements(elSpatial.Name.Namespace + "Spelement_Unit"))
                        {
                            var su = new EntitySpatial.SpelementUnit();
                            su.TypeUnit = elSpUnit.Attribute("Type_Unit").Value;
                            su.SuNmb = (elSpUnit.Attribute("Su_Nmb") != null) ? elSpUnit.Attribute("Su_Nmb").Value : null;
                            su.R = (elSpUnit.Element(elSpUnit.Name.Namespace + "R") != null) ? double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "R").Value, CultureInfo.InvariantCulture) : 0;

                            var ord = new EntitySpatial.Ordinate();
                            ord.X = double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Y").Value, CultureInfo.InvariantCulture);
                            ord.Y = double.Parse(elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("X").Value, CultureInfo.InvariantCulture);
                            ord.OrdNmb = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Ord_Nmb") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Ord_Nmb").Value : null;
                            ord.NumGeopoint = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Num_Geopoint") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Num_Geopoint").Value : null;
                            ord.DeltaGeopoint = (elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Delta_Geopoint") != null) ? elSpUnit.Element(elSpUnit.Name.Namespace + "Ordinate").Attribute("Delta_Geopoint").Value : null;

                            su.ordinate = ord;

                            es.AddSpelementUnit(su);
                        }
                    }
                }
            }
            if (e.Name.LocalName == "contour" || e.Name.LocalName == "spatial_data")
            {
                if (e.Element("entity_spatial") != null)
                {
                    if (e.Element("entity_spatial").Element("spatials_elements") != null)
                    {
                        foreach (XElement spElement in e.Element("entity_spatial").Element("spatials_elements").Elements("spatial_element"))
                        {
                            es.AddSpatialElement();
                            foreach (XElement ordinate in spElement.Element("ordinates").Elements("ordinate"))
                            {
                                var su = new EntitySpatial.SpelementUnit();
                                su.R = (ordinate.Element("r") != null) ? double.Parse(ordinate.Element("r").Value, CultureInfo.InvariantCulture) : 0;

                                var ord = new EntitySpatial.Ordinate();
                                ord.X = double.Parse(ordinate.Element("y").Value, CultureInfo.InvariantCulture);
                                ord.Y = double.Parse(ordinate.Element("x").Value, CultureInfo.InvariantCulture);
                                ord.OrdNmb = (ordinate.Element("ord_nmb") != null) ? ordinate.Element("ord_nmb").Value : null;
                                ord.NumGeopoint = (ordinate.Element("num_geopoint") != null) ? ordinate.Element("num_geopoint").Value : null;
                                ord.DeltaGeopoint = (ordinate.Element("delta_geopoint") != null) ? ordinate.Element("delta_geopoint").Value : null;

                                su.ordinate = ord;
                                es.AddSpelementUnit(su);
                            }
                        }   
                    }                    
                }
            }
            if (e.Name.LocalName == "Entity_Spatial" || e.Name.LocalName == "EntitySpatial")
            {
                foreach (XElement spElement in e.Elements())
                {
                    es.AddSpatialElement();                    
                    foreach (XElement elUnit in spElement.Elements())
                    {
                        var su = new EntitySpatial.SpelementUnit();
                        foreach (XElement el in elUnit.Elements())
                        {
                            if (!el.HasAttributes)
                            {
                                su.R = (el != null) ? double.Parse(el.Value, CultureInfo.InvariantCulture) : 0;
                            }
                            else
                            {
                                var ord = new EntitySpatial.Ordinate();
                                ord.X = double.Parse(el.Attribute("Y").Value, CultureInfo.InvariantCulture);
                                ord.Y = double.Parse(el.Attribute("X").Value, CultureInfo.InvariantCulture);

                                su.ordinate = ord;
                            }
                        }                        
                        es.AddSpelementUnit(su);
                    }
                }
            }
            return es;
        }
    }
}