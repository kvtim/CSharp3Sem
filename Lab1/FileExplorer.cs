using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Lab1
{
    class FileExplorer
    {
        public FileExplorer(string catalog, string fileName, string fullNameFile)
        {
            WorkWithFile(catalog, fileName, fullNameFile);
        }
        private void WorkWithFile(string catalog, string fileName, string fullNameFile)
        {
            FileInfo file;

            int numberPerson = 0;
            Person[] person = new Person[0];

            try
            {
                foreach (string findedFile in Directory.EnumerateFiles(catalog, fileName, SearchOption.AllDirectories))
                {
                    fullNameFile = findedFile;
                }
                DirectoryInfo dirInfo = new DirectoryInfo(catalog);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                file = new FileInfo(fullNameFile);

                if (!file.Exists)
                {
                    CreateFile(fullNameFile, ref person);
                    file = new FileInfo(fullNameFile);
                }


                Console.WriteLine($"\n Information about file:\n\n Name: {file.Name}\n The path to the file: {file.FullName}" +
                    $"\n Size: {file.Length} bytes\n Date of creation: {file.CreationTime}");

                Read(file.FullName, ref person, ref numberPerson);

                Console.Write("\n Do you want to add or overwrite a new person? Yes or No: ");
                string yesOrNo = Console.ReadLine();

                if (yesOrNo == "Yes")
                {
                    try
                    {
                        Write(file.FullName, ref person);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n " + ex.Message);
                    }
                }
                else if (yesOrNo != "No")
                    Console.WriteLine("\n You enter wrong value!");

                Console.Write("\n Do you want to move the file? Yes or No: ");
                yesOrNo = Console.ReadLine();

                if (yesOrNo == "Yes")
                {
                    Console.Write("\n Enter the name of the new directory: ");
                    try
                    {
                        MoveFile(file, Console.ReadLine() + @"\" + file.Name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n " + ex.Message);
                    }
                }
                else if (yesOrNo != "No")
                    Console.WriteLine("\n You enter wrong value!");

                Console.Write("\n Do you want to copy this file? Yes or No: ");
                yesOrNo = Console.ReadLine();

                if (yesOrNo == "Yes")
                {
                    Console.Write("\n Enter the name of the directory where you want to copy the file: ");
                    try
                    {
                        file.CopyTo(Console.ReadLine() + @"\" + file.Name, true);
                        Console.WriteLine("\n Completed!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n " + ex.Message);
                    }
                }
                else if (yesOrNo != "No")
                    Console.WriteLine("\n You enter wrong value!");

                Console.Write("\n Do you want to rename this file? Yes or No: ");
                yesOrNo = Console.ReadLine();

                if (yesOrNo == "Yes")
                {
                    Console.Write("\n Enter new name: ");
                    string newName = Console.ReadLine();
                    try
                    {
                        file.MoveTo(file.Directory.FullName + @"\" + newName + file.Extension);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n " + ex.Message);
                    }
                }
                else if (yesOrNo != "No")
                    Console.WriteLine("\n You enter wrong value!");

                Console.Write("\n Do you want to zip this file? Yes or No: ");
                yesOrNo = Console.ReadLine();

                if (yesOrNo == "Yes")
                {
                    Console.Write("\n Enter new name of zip file: ");
                    string zipFileName = Console.ReadLine();
                    try
                    {
                        Compress(file.FullName, file.Directory.FullName + @"\" + zipFileName + ".gz");
                        Console.Write("\n Do you want to unzip this file? Yes or No: ");
                        yesOrNo = Console.ReadLine();

                        if (yesOrNo == "Yes")
                        {
                            Console.Write("\n Enter new name of unpacked file: ");
                            Decompress(file.Directory.FullName + @"\" + zipFileName + ".gz", file.Directory.FullName + @"\" + Console.ReadLine());
                        }
                        else if (yesOrNo != "No")
                            Console.WriteLine("\n You enter wrong value!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n " + ex.Message);
                    }
                }
                else if (yesOrNo != "No")
                    Console.WriteLine("\n You enter wrong value!");

                Console.Write(" Press any key to exit.......");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n " + ex.Message);
            }
        }
        private static void Read(string readPath, ref Person[] person, ref int numberPerson)
        {
            using (StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default))
            {
                string[] line = new string[1];
                int i = 0;
                while ((line[i] = sr.ReadLine()) != null)
                {
                    Array.Resize(ref line, line.Length + 1);
                    i++;

                    if (i % 4 == 0)
                    {
                        Array.Resize(ref person, person.Length + 1);
                        person[numberPerson] = new Person(Convert.ToInt32(line[i - 4]), line[i - 3], line[i - 2],
                            Convert.ToInt32(line[i - 1]));
                        numberPerson++;
                    }
                }
            }

            Console.WriteLine("\n Information that we took from the file and assigned to class objects:");
            for (int i = 0; i < person.Length; i++)
            {
                Console.WriteLine($"\n Person {i + 1}:\n Age: {person[i].Age}\n Name: {person[i].Name}\n Surname: {person[i].Surname}\n" +
                    $" Height: {person[i].Height}");
            }
        }
        private static void Write(string writePath, ref Person[] person)
        {
            Console.WriteLine("\n Enter information about the new person:");
            Array.Resize(ref person, person.Length + 1);

            Console.Write(" Enter age: ");

            if (int.TryParse(Console.ReadLine(), out int age))
            {
                Console.Write(" Enter name: ");
                string name = Console.ReadLine();

                Console.Write(" Enter surname: ");
                string surname = Console.ReadLine();

                Console.Write(" Enter height: ");

                if (int.TryParse(Console.ReadLine(), out int height))
                {
                    person[person.Length - 1] = new Person(age, name, surname, height);
                }

                Console.Write("\n What do you want to do?\n 1. Overwrite\n 2. Add \n Your choice coice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(Convert.ToString(person[person.Length - 1].Age));
                                sw.WriteLine(person[person.Length - 1].Name);
                                sw.WriteLine(person[person.Length - 1].Surname);
                                sw.WriteLine(Convert.ToString(person[person.Length - 1].Height));
                            }
                            break;
                        case 2:
                            using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(Convert.ToString(person[person.Length - 1].Age));
                                sw.WriteLine(person[person.Length - 1].Name);
                                sw.WriteLine(person[person.Length - 1].Surname);
                                sw.WriteLine(Convert.ToString(person[person.Length - 1].Height));
                            }
                            break;
                        default:
                            Console.WriteLine("\n You enter wrong value!");
                            break;
                    }
                }
                else
                    Console.WriteLine("\n You enter wrong value!");
            }
            else
                Console.WriteLine("\n You enter wrong value!");
        }

        private static void CreateFile(string fullNameFile, ref Person[] person)
        {
            Console.Write("\n How many people do you want to add? ");

            if (int.TryParse(Console.ReadLine(), out int personAmount))
            {
                for (int i = 0; i < personAmount; i++)
                {
                    Console.WriteLine($"\n Enter information about {i + 1} person:");

                    Console.Write(" Enter age: ");

                    if (int.TryParse(Console.ReadLine(), out int age))
                    {
                        Console.Write(" Enter name: ");
                        string name = Console.ReadLine();

                        Console.Write(" Enter surname: ");
                        string surname = Console.ReadLine();

                        Console.Write(" Enter height: ");

                        if (int.TryParse(Console.ReadLine(), out int height))
                        {
                            using (StreamWriter sw = new StreamWriter(fullNameFile, true, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(Convert.ToString(age));
                                sw.WriteLine(name);
                                sw.WriteLine(surname);
                                sw.WriteLine(Convert.ToString(height));
                            }
                        }
                        else
                            Console.WriteLine("\n You enter wrong value!");
                    }
                    else
                        Console.WriteLine("\n You enter wrong value!");
                }
            }
            else
                Console.WriteLine("\n You enter wrong value!");
        }

        private static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine($"\n Сжатие файла {sourceFile} завершено. Исходный размер: {sourceStream.Length.ToString()}" +
                            $"  сжатый размер: {targetStream.Length.ToString()}.");
                    }
                }
            }
        }
        private static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения исходного файла
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine($"\n Восстановлен файл: {targetFile}");
                    }
                }
            }
        }
        private static void MoveFile(FileInfo file, string newDirectory)
        {
            file.MoveTo(newDirectory);
            Console.WriteLine("\n Completed!");
        }
    }
}
