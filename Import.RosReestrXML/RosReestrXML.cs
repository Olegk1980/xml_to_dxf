using System;
using System.Collections.Generic;
using System.IO;

namespace Import.RosReestrXML
{    
    public class RosReestrXML 
    {
        public static List<String> ARGS = new List<string>();
        public static void Main(string[] args)
        {
            string pathToXML = Directory.GetCurrentDirectory();
            if (args.Length > 0)
            {
                if (args[0].Equals("--help"))
                {
                    getText();
                } 
                else if (Directory.Exists(args[0]))
                {
                    pathToXML = args[0];
                }

                if (args.Length > 1)
                {
                    ARGS.AddRange(args);
                }                
            }
            else
            {
                getText();
            }
            new XmlImport(pathToXML);
        }
        private static void getText()
        {
            System.Console.WriteLine("Использование: ImportXML [путь к xml файлам росреестра] [Опции]");
            System.Console.WriteLine("                если путь не указан файлы ищутся в локальной директории.");
            System.Console.WriteLine("              --help  - эта справка");
            System.Console.WriteLine("Опции:");
            System.Console.WriteLine("              --split  - объеденить обрабатываемые файлы.");
            System.Console.WriteLine("              --np - без выгрузки ЗУ в файл");
            System.Console.WriteLine("              --nr - без выгрузки ОКС в файл");
            System.Console.WriteLine("              --nz - без выгрузки Зон в файл");
            System.Console.WriteLine("              --nb - без выгрузки Границ в файл");
            System.Console.WriteLine("              --noms - без выгрузки Точек ОМС");
            System.Console.WriteLine("\n Автор: Князев Олег Викторович. \n эл.почта: olegk1980@mail.ru");
        }
    }
    
}