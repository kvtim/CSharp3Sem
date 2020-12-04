using Microsoft.VisualBasic;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(" Enter the path to the folder where you want to find or create the file: ");
            string catalog = Console.ReadLine();

            Console.Write(" Enter file name: ");
            string fileName = Console.ReadLine();

            _ = new FileExplorer(catalog, fileName, catalog + "\\" + fileName);
        }
    }
}

