using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryForFiles
{
    public class FileWatcher : OperationsWithFile
    {
        private FileSystemWatcher watcher;
        private string sourceDirectory;
        private string targetDirectory;
        public FileWatcher()
        {
            sourceDirectory = @"D:\Projects\C# 3 сем\\lab2\SourceDirectory";
            watcher = new FileSystemWatcher(sourceDirectory);
            watcher.Filter = "*.txt";
            targetDirectory = @"D:\Projects\C# 3 сем\lab2\TargetDirectory";
            Start();
        }
        private void Start()
        {
            watcher.Created += new FileSystemEventHandler(Created);
            watcher.Renamed += new RenamedEventHandler(Created); //Если пользователь при создании файла указывает новое имя

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("The service is active, you can add files.\nPress q to exit.\n");
            while (Console.Read() != 'q') ;
        }

        private void Created(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);

            Thread.Sleep(3000);

            if (file.Exists)
            {

                Console.WriteLine("You have added the file to your directory!\n");

                RenameAndMoveToCorrectFolder(file);

                EncryptionAndCompression(file.FullName, targetDirectory + @"\" + Path.GetFileNameWithoutExtension(file.FullName) + ".gz");

                DecryptionAndDecompression(targetDirectory + @"\" + Path.GetFileNameWithoutExtension(file.FullName) + ".gz", targetDirectory + @"\" + file.Name);

                Console.WriteLine("Press q to exit.\n\n");
            }

        }
        private void EncryptionAndCompression(string sourceFile, string compressedFile)
        {
            EncodeDecrypt(sourceFile, 0x0088);
            Console.WriteLine("File encrypted!\n");

            Compress(sourceFile, compressedFile);
            Console.WriteLine("File compressed and moved!\n");
        }
        private void DecryptionAndDecompression(string sourceFile, string compressedFile)
        {
            Decompress(sourceFile, compressedFile);
            Console.WriteLine("File decompressed!\n");

            EncodeDecrypt(compressedFile, 0x0088);
            Console.WriteLine("File decrypted!\n");
        }
        private void RenameAndMoveToCorrectFolder(FileInfo file)
        {
            file.MoveTo($@"{FoldersCreator(file)}\Sales_{file.CreationTime.Year}_{file.CreationTime.Month}_{file.CreationTime.Day}_{file.CreationTime.Hour}_{file.CreationTime.Minute}_{file.CreationTime.Second}{file.Extension}");
        }
        private string FoldersCreator(FileInfo file)
        {
            DirectoryInfo newDirectory = new DirectoryInfo(sourceDirectory);
            newDirectory = newDirectory.CreateSubdirectory(file.CreationTime.Year + @"\" + file.CreationTime.Month + @"\" + file.CreationTime.Day);

            return newDirectory.FullName;
        }
    }
}
