using System;

class JobDispatcher
{
    private int[,] board;
    private Random random;

    public JobDispatcher(int[,] initialBoard)
    {
        board = (int[,])initialBoard.Clone();
        random = new Random();
    }

    public int[,] GetBoard()
    {
        return board;
    }

    public bool RandomJobAssigment()
    {
        int row, col;

        if (!FindEmptyLocation(out row, out col))
            return true; // Sudoku çözüldü.

        int[] numbers = { 1, 2, 3, 4, 5, 6 };
        Shuffle(numbers);

        foreach (int num in numbers)
        {
            if (IsSafe(row, col, num))
            {
                board[row, col] = num;

                if (RandomJobAssigment())
                    return true;

                board[row, col] = 0; // Geri al
            }
        }
        return false; // Bu durumda geri gidilir.
    }

    private bool FindEmptyLocation(out int row, out int col)
    {
        row = -1;
        col = -1;

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (board[i, j] == 0)
                {
                    row = i;
                    col = j;
                    return true;
                }
            }
        }
        return false;
    }

    private bool UsedInRow(int row, int num)
    {
        for (int col = 0; col < 6; col++)
        {
            if (board[row, col] == num)
                return true;
        }
        return false;
    }

    private bool UsedInCol(int col, int num)
    {
        for (int row = 0; row < 6; row++)
        {
            if (board[row, col] == num)
                return true;
        }
        return false;
    }

    private bool IsSafe(int row, int col, int num)
    {
        // Aynı satırda aynı sayı olmamalı
        if (UsedInRow(row, num))
            return false;

        // Aynı sütunda aynı sayı olmamalı
        if (UsedInCol(col, num))
            return false;

        // Ardışık sayı kontrolü
        if (col > 0 && board[row, col - 1] == num - 1)
            return false;
        if (col < 5 && board[row, col + 1] == num + 1)
            return false;

        return true;
    }

    private void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        int[,] board = {
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0}
        };

        JobDispatcher jobDispatcher = new JobDispatcher(board);

        int currentRow = 0;

        while (true)
        {
            Console.WriteLine("Enter'e basın devam etmek için, çıkmak için 'q' tuşuna basın.");
            var key = Console.ReadKey(true);
            if (key.KeyChar == 'q')
                break;

            if (jobDispatcher.RandomJobAssigment())
            {
                Console.WriteLine("Çözüm:");
                PrintRow(jobDispatcher.GetBoard(), currentRow);
                currentRow++;
                if (currentRow >= 6)
                {
                    Console.WriteLine("--------------------------------");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Çözüm yok.");
            }
        }
    }

    public static void PrintRow(int[,] solvedBoard, int row)
    {
        for (int j = 0; j < 6; j++)
        {
            Console.Write(solvedBoard[row, j] + " ");
        }
        Console.WriteLine();
    }
}