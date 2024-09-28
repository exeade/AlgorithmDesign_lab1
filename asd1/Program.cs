using System.Diagnostics;

namespace asd1;

public static class ArrayGenerator
{
    public static void Generate(int size, string pathFile)
    {
        int[] numbers = new int[size];

        Random rand = new Random();

        for (int i = 0; i < size; i++)
            numbers[i] = rand.Next(int.MinValue, int.MaxValue);

        using (StreamWriter writer = new StreamWriter(pathFile))
        {
            foreach (int number in numbers)
                writer.WriteLine(number);
        }
        
        Console.WriteLine($"Array from {size} numbers successfully generated and written in {pathFile}");
    }
}

public static class NaturalMergeSort
{
    private static readonly List<int> SeriesNum = new List<int>();
    
    private static int? TryParseNullable(string? input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        return null;
    }
    
    public static void ExternalSort(string unsortedFile)
    {
        do
        {
            SeriesNum.Clear();
            SeriesSplit(unsortedFile);
            MergeSeries(unsortedFile);
            
        } while (SeriesNum.Count > 1);
    }

    private static void SeriesSplit(string fileToSplit)
    {
        using StreamReader readerA = new StreamReader(fileToSplit);
        using StreamWriter writerB = new StreamWriter("file_B.txt");
        using StreamWriter writerC = new StreamWriter("file_C.txt");

        int counter = 0;
        bool fileTurn = true;
        
        int? prevNumber = TryParseNullable(readerA.ReadLine());
        int? currNumber = TryParseNullable(readerA.ReadLine());

        while (prevNumber != null)
        {
            bool tempTurn = fileTurn;
            if (currNumber != null)
            {
                if (prevNumber <= currNumber)
                {
                    counter++;
                }
                else
                {
                    SeriesNum.Add(counter + 1);
                    counter = 0;
                    tempTurn = !tempTurn;
                }
            }

            if (fileTurn)
            {
                writerB.WriteLine(prevNumber);
            }
            else
            {
                writerC.WriteLine(prevNumber);
            }

            prevNumber = currNumber;
            currNumber = TryParseNullable(readerA.ReadLine());
            fileTurn = tempTurn;
        }
        
        SeriesNum.Add(counter + 1);
    }

    private static void MergeSeries(string mergeOutputFile)
    {
        using StreamReader readerB = new StreamReader("file_B.txt");
        using StreamReader readerC = new StreamReader("file_C.txt");
        using StreamWriter writerA = new StreamWriter(mergeOutputFile);

        int counterB = 0;
        int counterC = 0;
        
        int indexB = 0;
        int indexC = 1;

        int? numberB = TryParseNullable(readerB.ReadLine());
        int? numberC = TryParseNullable(readerC.ReadLine());

        while (numberB != null || numberC != null)
        {
            if (SeriesNum[indexB] == counterB && SeriesNum[indexC] == counterC)
            {
                indexB += 2;
                indexC += 2;
                counterC = 0;
                counterB = 0;
                
                continue;
            }

            if (SeriesNum.Count == indexB || SeriesNum[indexB] == counterB)
            {
                writerA.WriteLine(numberC);
                numberC = TryParseNullable(readerC.ReadLine());
                counterC++;
                
                continue;
            }

            if (SeriesNum.Count == indexC || SeriesNum[indexC] == counterC)
            {
                writerA.WriteLine(numberB);
                numberB = TryParseNullable(readerB.ReadLine());
                counterB++;
                
                continue;
            }

            if (numberB <= numberC)
            {
                writerA.WriteLine(numberB);
                numberB = TryParseNullable(readerB.ReadLine());
                counterB++;
            }
            else
            {
                writerA.WriteLine(numberC);
                numberC = TryParseNullable(readerC.ReadLine());
                counterC++;
            }
        }
    }
}

static class Program
{
    static void Main()
    {

        string unsortedFile = "source_file.txt";
        int size = 1000000;
        
        ArrayGenerator.Generate(size, unsortedFile);
        
        Stopwatch time = new Stopwatch();
        time.Start();
        NaturalMergeSort.ExternalSort(unsortedFile);
        time.Stop();
        Console.WriteLine($"Sorting took {time.ElapsedMilliseconds} ms.");
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}