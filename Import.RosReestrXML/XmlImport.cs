using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using netDxf;

namespace Import.RosReestrXML
{
    public class XmlImport
    {
        private List<List<Block>> listBlocks = new List<List<Block>>();
        private string pathToXML;
        public XmlImport()
        {
            this.pathToXML = Directory.GetCurrentDirectory();
            ReadXMLFile();
        }
        
        public XmlImport(string pathToXML)
        {
            this.pathToXML = pathToXML;
            ReadXMLFile();
        }

        private void ReadXMLFile()
        {            
            string[] f1 = Directory.GetFiles(pathToXML, "*.xml");
            string[] f2 = Directory.GetFiles(pathToXML, "*.XML");
            string[] files = new string[f1.Length + f2.Length];
            f1.CopyTo(files, 0);
            f2.CopyTo(files, f1.Length);

            Block block = null;

            foreach(string file in files)
            {
                System.Console.Write("\nЧитаем файл: " + new FileInfo(file).Name);
                List<Block> blocks = new List<Block>();

                using (XmlReader reader = XmlReader.Create(file))
                {
                    List<Parcel> parcels = new List<Parcel>();
                    List<ObjectRealty> objRealty = new List<ObjectRealty>();
                    List<OMSPoint> OMSPoints = new List<OMSPoint>();
                    List<Bound> bounds = new List<Bound>();
                    List<Zone> zones = new List<Zone>();

                    XmlImportElement xmlImportElement = new XmlImportElement(reader);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (reader.Name == "Cadastral_Block" 
                                || reader.Name == "cadastral_block")
                            {
                                block.AddParcels(parcels);
                                block.AddObjectsRealty(objRealty);
                                block.AddOMSPoints(OMSPoints);
                                block.AddBounds(bounds);
                                block.AddZones(zones);
                            }
                        }

                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "Cadastral_Block" || reader.Name == "cadastral_block")
                            {
                                block = new Block();
                                blocks.Add(block);

                                if (reader.GetAttribute("CadastralNumber") != null)
                                {
                                    block.SetKadNum(reader.GetAttribute("CadastralNumber"));
                                } 
                                else
                                {            
                                    block.SetKadNum(getValue(reader));                                                                        
                                }                                
                            }

                            if (reader.Name == "Total" || (reader.Name).ToLower() == "area_quarter")
                            {
                                block.SetArea(getValue(reader));
                            }

                            if (reader.Name == "SpatialData" || (reader.Name).ToLower() == "spatial_data")
                            {
                                XDocument doc = XDocument.Parse(reader.ReadOuterXml());
                                block.SetEntitySpatial(xmlImportElement.ReadEntitySpatial(doc.Root));
                            }

                            if (reader.Name == "Parcel" || (reader.Name).ToLower() == "land_record")
                            {
                                parcels.Add(xmlImportElement.Parcel());
                                continue;
                            }

                            if (reader.Name == "ObjectRealty" || (reader.Name).ToLower() == "build_record" 
                                || (reader.Name).ToLower() == "construction_record"
                                || (reader.Name).ToLower() == "object_under_construction_record")
                            {
                                objRealty.Add(xmlImportElement.ObjectRealty());
                                continue;
                            }

                            if (reader.Name == "OMSPoint" || (reader.Name).ToLower() == "oms_point")
                            {
                                OMSPoints.Add(xmlImportElement.OMSPoint());
                                continue;
                            }

                            if (reader.Name == "Bound" || (reader.Name).ToLower() == "subject_boundary_record"
                                || (reader.Name).ToLower() == "municipal_boundary_record"
                                || (reader.Name).ToLower() == "inhabited_locality_boundary_record"
                                || (reader.Name).ToLower() == "coastline_record")
                            {
                                bounds.Add(xmlImportElement.Bound());
                                continue;
                            }

                            if (reader.Name == "Zone" || (reader.Name).ToLower() == "zones_and_territories_record")
                            {
                                zones.Add(xmlImportElement.Zone());
                                continue;
                            }
                            if ((reader.Name).ToLower() == "tp") {
                                objRealty.Add(xmlImportElement.ObjectRealty());
                                break;
                            }
                        }   
                        reader.Read();                     
                    }
                    if (block == null)
                    {
                        block = new Block();
                    }
                    block.AddParcels(parcels);
                    block.AddObjectsRealty(objRealty);
                    block.AddOMSPoints(OMSPoints);
                    block.AddBounds(bounds);
                    block.AddZones(zones);
                    blocks.Add(block);
                }
                if (RosReestrXML._args.Contains("--split"))
                {
                    listBlocks.Add(blocks);
                }
                else
                {
                    new CreateFileCAD(blocks);
                }                
                blocks = null;                
            }
            if (listBlocks.Count > 0)
            {
                new CreateFileCAD(listBlocks);
            }            
        }

        private string getValue(XmlReader reader)
        {
            do
            {
                reader.Read();
            } while (reader.NodeType != XmlNodeType.Text);
            return reader.Value;
        }
    }
}