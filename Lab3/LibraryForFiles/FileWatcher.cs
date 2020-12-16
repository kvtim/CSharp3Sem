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
        private AllData data = new AllData();
        public FileWatcher(string path)
        {
            string jsonFile = path + "appSettings.json";
            string xmlFile = path + "xmlConfig.xml";

            try
            {
                if (File.Exists(jsonFile))
                {
                    var manager = new ConfigurationManager();

                    data.PathsOptions = manager.GetOption<PathsOptions>(jsonFile);
                    data.EncryptingOptions = manager.GetOption<EncryptingOptions>(jsonFile);
                    data.CompressOptions = manager.GetOption<CompressOptions>(jsonFile);

                    WatcherCreate();

                }
                else if (File.Exists(xmlFile))
                {
                    var manager = new ConfigurationManager();

                    data.PathsOptions = manager.GetOption<PathsOptions>(xmlFile);
                    data.EncryptingOptions = manager.GetOption<EncryptingOptions>(xmlFile);
                    data.CompressOptions = manager.GetOption<CompressOptions>(xmlFile);

                    WatcherCreate();
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void WatcherCreate()
        {
            watcher = new FileSystemWatcher(data.PathsOptions.SourceDirectory);

            watcher.Filter = "*.txt";

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
                try
                {
                    Console.WriteLine("You have added the file to your directory!\n");

                    RenameAndMoveToCorrectFolder(file);

                    EncryptionAndCompression(file.FullName, FoldersCreator(file, data.PathsOptions.TargetDirectory) + @"\" + Path.GetFileNameWithoutExtension(file.FullName) + data.CompressOptions.CompressFormat);

                    DecryptionAndDecompression(FoldersCreator(file, data.PathsOptions.TargetDirectory) + @"\" + Path.GetFileNameWithoutExtension(file.FullName) + data.CompressOptions.CompressFormat, FoldersCreator(file, data.PathsOptions.TargetDirectory) + @"\" + file.Name);

                    Console.WriteLine("Press q to exit.\n\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        private void EncryptionAndCompression(string sourceFile, string compressedFile)
        {
            if (ushort.TryParse(data.EncryptingOptions.Key, out ushort key))
            {
                EncodeDecrypt(sourceFile, key);
                Console.WriteLine("File encrypted!\n");
            }
            else
            {
                throw new ArgumentException();
            }


            Compress(sourceFile, compressedFile);
            Console.WriteLine("File compressed and moved!\n");
        }
        private void DecryptionAndDecompression(string sourceFile, string compressedFile)
        {
            Decompress(sourceFile, compressedFile);
            Console.WriteLine("File decompressed!\n");

            if (ushort.TryParse(data.EncryptingOptions.Key, out ushort key))
            {
                EncodeDecrypt(compressedFile, key);
                Console.WriteLine("File decrypted!\n");
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private void RenameAndMoveToCorrectFolder(FileInfo file)
        {
            file.MoveTo($@"{FoldersCreator(file, data.PathsOptions.SourceDirectory)}\Sales_{file.CreationTime.Year}_{file.CreationTime.Month}_{file.CreationTime.Day}_{file.CreationTime.Hour}_{file.CreationTime.Minute}_{file.CreationTime.Second}{file.Extension}");
        }
        private string FoldersCreator(FileInfo file, string directory)
        {
            DirectoryInfo newDirectory = new DirectoryInfo(directory);
            newDirectory = newDirectory.CreateSubdirectory(file.CreationTime.Year + @"\" + file.CreationTime.Month + @"\" + file.CreationTime.Day);

            return newDirectory.FullName;
        }
    }
}
