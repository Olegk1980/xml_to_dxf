using System;
using System.Collections.Generic;
using System.IO;

namespace Import.RosReestrXML
{    
    public class RosReestrXML 
    {
        public static List<String> _args = new List<string>();
        public static string _pathToXML;
        public static void Main(string[] args)
        {
            _args.AddRange(args);
            if (_args.Contains("--help"))
            {
                getText();
                Exit();
            }
            getText();
            Console.Write("Укажите путь к каталогу с XML: ");
		    _pathToXML = Console.ReadLine();
            
            if (String.IsNullOrEmpty(_pathToXML))
            {
                getText();
                Console.WriteLine("Внимание!!! ПУТЬ К КАТАЛОГУ НЕ УКАЗАН!");
                Exit();
            }
            else
            {
                _pathToXML = _pathToXML[_pathToXML.Length - 1] == Path.DirectorySeparatorChar ? _pathToXML : _pathToXML + Path.DirectorySeparatorChar;
            }
            
            if (!Directory.Exists(_pathToXML))
            {
                getText();
                Console.WriteLine("Внимание!!! КАТАЛОГ НЕ ОБНАРУЖЕН");
                Exit();
            }
            new XmlImport(_pathToXML);
        }
        private static void getText()
        {
            System.Console.WriteLine("Использование: ImportXML [Опции]");
            System.Console.WriteLine("Опции:");
            System.Console.WriteLine("              --help   - эта справка");            
            System.Console.WriteLine("              --split  - объеденить обрабатываемые файлы.");
            System.Console.WriteLine("              --np     - без выгрузки ЗУ в файл");
            System.Console.WriteLine("              --nr     - без выгрузки ОКС в файл");
            System.Console.WriteLine("              --nz     - без выгрузки Зон в файл");
            System.Console.WriteLine("              --nb     - без выгрузки Границ в файл");
            System.Console.WriteLine("              --noms   - без выгрузки Точек ОМС");
            System.Console.WriteLine("\n Для доработки программы эл.почта: olegk1980@mail.ru");
        }
    
        private static void Exit()
        {
            Console.Write("нажмите ENTER для завершения программы");
		    Console.ReadLine();
            System.Environment.Exit(0);
        }
    }
    
}