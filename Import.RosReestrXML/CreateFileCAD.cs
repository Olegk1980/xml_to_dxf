using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using DxfBlock = netDxf.Blocks.Block;

namespace Import.RosReestrXML{
    class CreateFileCAD
    {
        private AttributeDefinition attributeDefinition = null;
        private DxfDocument dxfDocument = new DxfDocument();
        private DxfBlock dxfBlockBloks = null;
        public List<Block> blocks;
        private Block block;
        private EntityObject entity;
        private string nameLayer;

        public CreateFileCAD(List<Block> blocks)
        {
            this.blocks = blocks;
            this.Dxf();
        }
        public CreateFileCAD(List<List<Block>> listBlocks)
        {
            foreach (List<Block> blocks in listBlocks)
            {
                this.blocks = blocks;
                this.Dxf();
            }
            if (listBlocks.Count > 0)
            {
                SaveDocument("split_Cadastral_Block");
            }            
        }
        private void Dxf()
        {                                      
            System.Console.Write("\nСоздание DXF:");
            IEnumerator<Block> enumerator = blocks.GetEnumerator();
            System.Console.Write("Квартал:");
            while (enumerator.MoveNext())
            {
                System.Console.Write(".");
                block = enumerator.Current;
                dxfBlockBloks = new DxfBlock(block.GetKadNum().Replace(':', '_'));
                
                if (RosReestrXML.ARGS != null){                    
                    if (!RosReestrXML.ARGS.Contains("--np"))
                    {
                        this.ParcelToDxfDocument();
                    }
                    if (!RosReestrXML.ARGS.Contains("--nr"))
                    {
                        this.ObjRealtyToDxfDocument();
                    }
                    if (!RosReestrXML.ARGS.Contains("--nb"))
                    {
                        this.BoundToDxfDocument();
                    }
                    if (!RosReestrXML.ARGS.Contains("--nz"))
                    {
                        this.ZoneToDxfDocument();
                    }
                    if (!RosReestrXML.ARGS.Contains("--noms"))
                    {
                        this.OMSPointToDxfDocument();
                    }
                }
                else
                {
                    this.ParcelToDxfDocument();
                    this.ObjRealtyToDxfDocument();
                    this.BoundToDxfDocument();
                    this.ZoneToDxfDocument();
                    this.OMSPointToDxfDocument();
                }

                if (block.GetEntitySpatial() == null)
                {
                    continue;
                }

                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("Кадастровый_номер", block.GetKadNum()));
                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("Площадь", block.GetArea()));
                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("ЗУ", block.GetParcels().Length + " шт."));
                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("ОКС", block.GetObjectsRealty().Length + " шт."));
                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("Границы", block.GetBounds().Length + " шт."));
                dxfBlockBloks.AttributeDefinitions.Add(this.AttributeObject("Зоны", block.GetZones().Length + " шт."));

                try
                {
                    nameLayer = "CadastralBlocks";
                    EntitySpatial es = block.GetEntitySpatial();                    
                    foreach (var spelements in es.GetSpatialElement())
                    {
                        this.PointsToEntity(es.GetEntityPoints(spelements));
                        dxfBlockBloks.Entities.Add(entity);
                    }
                    dxfBlockBloks.Layer = new Layer(nameLayer);
                    dxfDocument.Entities.Add(new Insert(dxfBlockBloks));
                }
                catch (System.Exception e)
                {                    
                    System.Console.Write("\nКвартал: " + block.GetKadNum() +  "\n" + e.Message);
                }                
            }
            // save to file
            string fileName = blocks.Count > 0 ? blocks[0].GetKadNum().Replace(':', '_') : DateTime.UnixEpoch.ToString();
            if (!RosReestrXML.ARGS.Contains("--split") )
            {
                SaveDocument(fileName);
            }
        }

        private void OMSPointToDxfDocument()
        {
            System.Console.Write("\nТочки ОМС:");
            foreach (OMSPoint omsPoint in block.GetOMSPoints())
            {
                System.Console.Write(".");
            }
        }

        private void ZoneToDxfDocument()
        {
            System.Console.Write("\nЗоны:");
            foreach (Zone zone in block.GetZones())
            {
                System.Console.Write(".");
                DxfBlock dxfBlockZones = new DxfBlock(zone.GetAccNum().Replace(':', '_'));

                dxfBlockZones.AttributeDefinitions.Add(this.AttributeObject("Реестровый_номер", zone.GetAccNum()));
                dxfBlockZones.AttributeDefinitions.Add(this.AttributeObject("Описание", zone.GetDesc()));

                if (zone.GetContours() == null)
                {
                    if (zone.GetEntitySpatial() == null)
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            nameLayer = "CadastralZones";
                            EntitySpatial es = zone.GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockZones.Entities.Add(entity);
                            }
                            dxfBlockZones.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockZones));
                        }
                        catch (System.Exception e)
                        {
                            System.Console.Write("\nЗона: " + zone.GetAccNum() +  "\n" + e.Message);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < zone.GetContours().Length; i++)
                    {
                        try
                        {
                            nameLayer = "CadastralZones";
                            EntitySpatial es = zone.GetContours()[i].GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockZones.Entities.Add(entity);
                            }
                            dxfBlockZones.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockZones));
                        }
                        catch (System.Exception e)
                        {                            
                            System.Console.Write("\nЗона: " + zone.GetAccNum() + " контур " + i + "\n" + e.Message);
                        }
                    }
                }
            }
        }

        private void BoundToDxfDocument()
        {
            System.Console.Write("\nГраницы:");
            foreach (Bound bound in block.GetBounds())
            {
                System.Console.Write(".");
                DxfBlock dxfBlockBounds = new DxfBlock(bound.GetAccNum().Replace(':', '_'));

                dxfBlockBounds.AttributeDefinitions.Add(this.AttributeObject("Реестровый_номер", bound.GetAccNum()));
                dxfBlockBounds.AttributeDefinitions.Add(this.AttributeObject("Описание", bound.GetDesc()));

                if (bound.GetContours() == null)
                {
                    if (bound.GetEntitySpatial() == null)
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            nameLayer = "CadastralBounds";
                            EntitySpatial es = bound.GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockBounds.Entities.Add(entity);
                            }
                            dxfBlockBounds.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockBounds));
                        }
                        catch (System.Exception e)
                        {
                            System.Console.Write("\nГраница: " + bound.GetAccNum() + "\n" + e.Message);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < bound.GetContours().Length; i++)
                    {
                        try
                        {
                            nameLayer = "CadastralBounds";
                            EntitySpatial es = bound.GetContours()[i].GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockBounds.Entities.Add(entity);
                            }   
                            dxfBlockBounds.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockBounds));

                        }
                        catch (System.Exception e)
                        {
                            System.Console.Write("\nГраница: " + bound.GetAccNum() + " контур " + i + "\n" + e.Message);
                        }
                    }
                }
            }
        }

        private void ObjRealtyToDxfDocument()
        {
            System.Console.Write("\nОКС:");
            foreach(ObjectRealty realty in block.GetObjectsRealty())
            {
                System.Console.Write(".");
                DxfBlock dxfBlockRealty = new DxfBlock(realty.GetKadNum().Replace(':', '_'));

                dxfBlockRealty.AttributeDefinitions.Add(this.AttributeObject("Кадастровый_номер", realty.GetKadNum()));
                dxfBlockRealty.AttributeDefinitions.Add(this.AttributeObject("Площадь", realty.GetArea()));
                dxfBlockRealty.AttributeDefinitions.Add(this.AttributeObject("Вид_ОКС", realty.GetObjectType()));
                dxfBlockRealty.AttributeDefinitions.Add(this.AttributeObject("Описание", realty.GetAssignationName()));

                if (realty.GetContours() == null)
                {
                    if (realty.GetEntitySpatial() == null)
                    {
                        continue;
                    }
                    else
                    {
                        try
                        {
                            nameLayer = "CadastralRealty";
                            EntitySpatial es = realty.GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockRealty.Entities.Add(entity);
                            }
                            dxfBlockRealty.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockRealty));
                        }
                        catch (System.Exception e)
                        {
                            System.Console.Write("\nОКС: " + realty.GetKadNum() + "\n" + e.Message);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < realty.GetContours().Length; i++)
                    {
                        try
                        {
                            nameLayer = "CadastralRealty";
                            EntitySpatial es = realty.GetContours()[i].GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockRealty.Entities.Add(entity);
                            }
                            dxfBlockRealty.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockRealty));

                        }
                        catch (System.Exception e)
                        {
                            System.Console.Write("\nОКС: " + realty.GetKadNum() + " контур " + i + "\n" + e.Message);
                        }
                    }
                }
            }
        }

        private void ParcelToDxfDocument()
        {
            System.Console.Write("\nЗУ:");
            foreach (Parcel parcel in block.GetParcels())
            {
                System.Console.Write(".");
                if (parcel.GetParcelEZ() != null)
                {
                    foreach (Parcel parcelEZ in parcel.GetParcelEZ())
                    {
                        prepareParcel(parcelEZ);
                    }
                }
                else
                {
                    prepareParcel(parcel);
                }
            }
        }

        private void prepareParcel(Parcel parcel)
        {
            nameLayer = "CadastralParcels";
            DxfBlock dxfBlockParcels = new DxfBlock(parcel.GetKadNum().Replace(':', '_'));
            
            dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Кадастровый_номер", parcel.GetKadNum()));
            if (!String.IsNullOrEmpty(parcel.GetKadNumEZ()))
            {
                dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Кадастровый_номер_ЕЗ", parcel.GetKadNumEZ()));
            }
            dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Площадь", parcel.GetArea()));
            dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Статус", parcel.GetStatus()));
            dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Категория", parcel.GetCategory()));
            dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Наименование", parcel.GetName()));
            if (parcel.GetUtilization() != null)
            {
                dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Использование", String.Join(", ", parcel.GetUtilization())));    
            }
            if (parcel.GetParentKN() != null)
            {
                dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Родительские_ЗУ", String.Join(", ", parcel.GetParentKN().ToArray())));
            }

            if (parcel.GetSubParcels() == null)
            {
                if (parcel.GetContours() == null)
                {
                    try
                    {
                        if (parcel.GetEntitySpatial() == null)
                        {
                            return;
                        }
                        else
                        {
                            EntitySpatial es = parcel.GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockParcels.Entities.Add(entity);
                            }                            
                            dxfBlockParcels.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockParcels));
                        }
                    }
                    catch (System.Exception e)
                    {
                        System.Console.Write("\nУчасток: " + parcel.GetKadNum() + "\n" + e.Message);
                    }                    
                }
                else
                {
                    for (int i = 0; i < parcel.GetContours().Length; i++)
                    {
                        try
                        {
                            if (parcel.GetContours()[i].GetEntitySpatial() == null)
                            {
                                continue;
                            }
                            else
                            {                                
                                //dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Номер_контура", (i + 1).ToString()));
                                //indexContour = i + 1;
                                EntitySpatial es = parcel.GetContours()[i].GetEntitySpatial();
                                foreach (var spelements in es.GetSpatialElement())
                                {
                                    this.PointsToEntity(es.GetEntityPoints(spelements));
                                    dxfBlockParcels.Entities.Add(entity);
                                }  
                                dxfBlockParcels.Layer = new Layer(nameLayer);
                                dxfDocument.Entities.Add(new Insert(dxfBlockParcels));
                            }
                        }
                        catch (System.Exception e)
                        {                            
                            System.Console.Write("\nУчасток: " + parcel.GetKadNum() + "(" + i + ")\n" + e.Message);
                        }                       
                    }
                }
            }
            else
            {
                for (int i = 0; i < parcel.GetSubParcels().Length; i++)
                {
                    try
                    {
                        if (parcel.GetSubParcels()[i].GetEntitySpatial() == null)
                        {
                            continue;
                        }
                        else
                        {
                            //dxfBlockParcels.AttributeDefinitions.Add(this.AttributeObject("Номер_части", (i + 1).ToString()));
                            //indexSupParcel = i;
                            EntitySpatial es = parcel.GetSubParcels()[i].GetEntitySpatial();
                            foreach (var spelements in es.GetSpatialElement())
                            {
                                this.PointsToEntity(es.GetEntityPoints(spelements));
                                dxfBlockParcels.Entities.Add(entity);
                            }
                            dxfBlockParcels.Layer = new Layer(nameLayer);
                            dxfDocument.Entities.Add(new Insert(dxfBlockParcels));
                        }
                    }
                    catch (System.Exception e)
                    {                        
                        System.Console.Write("\nУчасток: " + parcel.GetKadNum() + "/ЧЗУ:" + i + "\n" + e.Message);
                    }                                      
                }
            }
        }

        private void PointsToEntity(List<EntityPoint> points)
        {
            try
            {
                if (points[0].radius == 0)
                {
                    if (points.Count > 1)
                    {
                        if ((points[0].x == points[points.Count - 1].x) && (points[0].y == points[points.Count - 1].y))
                        {
                            List<Polyline2DVertex> polyline2DVertices = new List<Polyline2DVertex>();
                            foreach (EntityPoint point in points)
                            {
                                polyline2DVertices.Add(new Polyline2DVertex(point.x, point.y));
                            }
                            entity = new Polyline2D(polyline2DVertices, true);
                            entity.Layer = new Layer(nameLayer);
                        }
                        else
                        {
                            List<Polyline2DVertex> polyline2DVertices = new List<Polyline2DVertex>();
                            foreach (EntityPoint point in points)
                            {
                                polyline2DVertices.Add(new Polyline2DVertex(point.x, point.y));
                            }
                            entity = new Polyline2D(polyline2DVertices, false);
                            entity.Layer = new Layer(nameLayer);
                        }
                    }
                    else
                    {
                        throw new Exception("Не достаточно координат, для отрисовки контура объекта!");
                    }
                }
                else
                {
                    entity = new Circle(new Vector2(points[0].x, points[0].y), points[0].radius);
                }
            } catch (System.ArgumentOutOfRangeException)
            {
                throw new Exception("Не достаточно координат, для отрисовки контура объекта!");
            }
        }
    
        private AttributeDefinition AttributeObject(string name, string value)
        {
            attributeDefinition = new AttributeDefinition(name);
            attributeDefinition.Value = value;
            attributeDefinition.Flags = AttributeFlags.Hidden;
            return attributeDefinition;
        }
    
        private void SaveDocument(string name)
        {
            dxfDocument.Save(name + ".dxf");
            System.Console.WriteLine("\nФайл создан: " + name + ".dxf");
        }

    }
}