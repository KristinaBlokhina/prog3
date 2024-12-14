using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string[] files = new string[20];
        for (int i = 10; i <= 29; i++)
        {
            files[i - 10] = $"{i}.txt";
        }

        List<string> missingFiles = new List<string>();
        List<string> badDataFiles = new List<string>();
        List<string> overflowFiles = new List<string>();

        long sumOfProducts = 0;
        int countOfValidProducts = 0;

        foreach (var file in files)
        {
            try
            {
                if (!File.Exists(file))
                    throw new FileNotFoundException($"File {file} does not exist");

                string[] lines = File.ReadAllLines(file);
                if (lines.Length < 2)
                    throw new FormatException($"File {file} does not contain enough lines");

                if (!int.TryParse(lines[0], out int firstNumber) || !int.TryParse(lines[1], out int secondNumber))
                    throw new FormatException($"File {file} contains invalid data");

                long product = (long)firstNumber * secondNumber;

                if (product < int.MinValue || product > int.MaxValue)
                    throw new OverflowException($"Overflow in file {file}");

                sumOfProducts += product;
                countOfValidProducts++;
            }
            catch (FileNotFoundException)
            {
                missingFiles.Add(file);
            }
            catch (FormatException)
            {
                badDataFiles.Add(file);
            }
            catch (OverflowException)
            {
                overflowFiles.Add(file);
            }
        }

        if (missingFiles.Count > 0 || badDataFiles.Count > 0 || overflowFiles.Count > 0)
        {
            Console.WriteLine("Error: Some files could not be processed.");

            File.WriteAllLines("no_file.txt", missingFiles);
            File.WriteAllLines("bad_data.txt", badDataFiles);
            File.WriteAllLines("overflow.txt", overflowFiles);

            Console.ReadLine(); 
            return;
        }

        if (countOfValidProducts > 0)
        {
            double average = sumOfProducts / (double)countOfValidProducts;
            Console.WriteLine($"Average: {average}");
        }
        else
        {
            Console.WriteLine("No valid products were calculated.");
        }

        Console.ReadLine();
    }
}
