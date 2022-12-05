namespace External_Balanced_Merge.Sorters;

public class TextSorter : ISorter
{
    public void Sort(string fileName, out string sortedFileName, int helpFilesCount = 3)
    {
        if (helpFilesCount < 2)
            throw new ArgumentException(null, nameof(helpFilesCount));
        string[] bSortFiles = Enumerable.Range(1, helpFilesCount).Select(i => $"B{i}.txt").ToArray();
        string[] cSortFiles = Enumerable.Range(1, helpFilesCount).Select(i => $"C{i}.txt").ToArray();
        DivideFiles(fileName, bSortFiles);
        SortHelper(bSortFiles, cSortFiles, out sortedFileName);
    }

    private void SortHelper(string[] bSortFiles, string[] cSortFiles, out string fileName)
    {   //Видалення пустих файлів(потоків зчитування для них) та утворення потоків зчитування з файлів
        var readers = bSortFiles.Select(f => new StreamReader(f)).ToList();
        readers.Where(r => r.EndOfStream).ToList().ForEach(r =>
        {
            r.Dispose();
            readers.Remove(r);
        });

        //Вихід з рекурсії шляхом перевірки на один файл - відповідно утворена відсортована фінальна послідовність
        if (readers.Count == 1)
        {
            fileName = ((FileStream)readers.First().BaseStream).Name;
            return;
        }

        //Утворення потоків зчитування з файлів
        var writers = cSortFiles.Select(f => new StreamWriter(f, append: false)).ToList();
        var currentWriter = writers.First();
        var currentReader = readers.First();

        //Лісти для запису Серії та наступної серії 
        var series = new List<int>();
        var nextSeries = new List<int>();

        // словарь для текущего потока записи и предыдущего значения для дальнейшего сравнения для создания серий с последним значением текущего потока чтения
        var readerAndPrevNum = readers.ToDictionary(r => r, _ => int.MinValue);

        while (readers.Count != 0)
        {   //При закінченні даного файлу видалити його та знищити
            while (currentReader.EndOfStream)
            {
                var readerToRemove = currentReader;
                currentReader = readers.NextAfter(currentReader);
                readers.Remove(readerToRemove);
                readerAndPrevNum.Remove(readerToRemove);
                readerToRemove.Dispose();
                if (readers.Count == 0)
                {
                    break;
                }
            }

            if (readers.Count == 0)
            {
                series.Sort();
                foreach (int n in series)
                {
                    currentWriter.WriteLine(n);
                }

                currentWriter = writers.NextAfter(currentWriter);
                nextSeries.Sort();
                foreach (int n in nextSeries)
                {
                    currentWriter.WriteLine(n);
                }

                break;
            }

            //Поділ на серії
            int num = int.Parse(currentReader.ReadLine()!);
            if (num >= readerAndPrevNum[currentReader])
            {
                series.Add(num);
                readerAndPrevNum[currentReader] = num;
            }
            else
            {
                nextSeries.Add(num);
                readerAndPrevNum[currentReader] = num;
                currentReader = readers.NextAfter(currentReader);
            }

            //При закінченні серій у всіх файлах
            if (nextSeries.Count >= readers.Count)
            {
                series.Sort();
                foreach (int n in series)
                {
                    currentWriter.WriteLine(n);
                }

                currentWriter = writers.NextAfter(currentWriter);
                series.Clear();
                series.AddRange(nextSeries);
                nextSeries.Clear();
            }
        }
        //Знищення потоків для вивільнення памяті та закриття алгоритму
        readers.ForEach(r => r.Dispose());
        writers.ForEach(w => w.Dispose());
        SortHelper(cSortFiles, bSortFiles, out fileName);
    }

    //Поділ вхідного файлу на задану кількість файлів шляхом виділення серій у ньому
    private void DivideFiles(string fileName, string[] bSortFiles)
    {
        var writers = bSortFiles.Select(f => new StreamWriter(f, append: false)).ToList();
        var currentWriter = writers.First();
        using var reader = new StreamReader(fileName);
        int lastNum = int.MinValue;
        //Зчитування з вхідного файлу данних та запис їх у файли за допомогою виділення серій у ньому
        while (!reader.EndOfStream)
        {
            int num = int.Parse(reader.ReadLine()!);
            //Порівняння для віднайдення можливих частково відсортованих послідовностей - серій
            if (num >= lastNum)
            {
                currentWriter.WriteLine(num);
            }
            //У разі невідповідності умові - число менше за минуле, серія закінчується
            //Поток запису перемикається на наступний за допомогою механізму кільцевої черги
            //для того щоб записати у наступний файл дане число
            else
            {
                currentWriter = writers.NextAfter(currentWriter);
                currentWriter.WriteLine(num);
            }
            //Переставлення значення для наступного порівняння
            lastNum = num;
        }
        //Знищення або закриття потоків для запису у файли
        writers.ForEach(w => w.Dispose());
    }
}