using System;
using System.Numerics;

const int rows = 4;
const int cols = rows;

Random random = new Random(Seed: 1);

Complex[,] matrix = new Complex[rows, cols];

Console.WriteLine("Исходная матрица");
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        //, random.Next(-9, 10)
        matrix[i, j] = new Complex(random.Next(-9,10), random.Next(-9, 10));
        if (matrix[i, j].Imaginary < 0)
        {
            Console.Write("{0}\t", matrix[i, j].Real + "" + matrix[i, j].Imaginary + "i");
        }
        else
        Console.Write("{0}\t", matrix[i, j].Real+"+"+matrix[i, j].Imaginary+"i");
    }
    Console.WriteLine();
}
double[] vector = new double[rows];
for (int i = 0; i < rows;)
{
    vector[i] = ++i;
}
Console.WriteLine();
Console.WriteLine("Вектор: ");
foreach (int i in vector)
{
    Console.WriteLine("{0}\t", i);
}
Console.WriteLine();

Complex[] result = new Complex[rows];
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        result[i] += matrix[i, j] * vector[j];
        
    }
}
Console.WriteLine();
Console.WriteLine("Результат умножения: ");
PrintVector(result);

Console.WriteLine("Исходная матрица");
PrintMatrix(matrix);
var root = GetRoot(matrix, result);
Console.WriteLine("Ответ: ");
PrintVector(root);


//Complex[] GetRoot(Complex[,] A)

Complex[] GetRoot(Complex[,] matrix, Complex[] vectorX)
{

    int n = matrix.GetLength(0);
    int m = matrix.GetLength(1);

    for (int i = 0; i < n - 1; ++i)
    {
        (int maxRow, bool swapFlag) = FindMaximumInColumn(matrix, i);
        if (swapFlag) {
            SwapRows(matrix, maxRow, i);
            Console.WriteLine("Матрица после обмена");
            PrintMatrix(matrix);
            SwapElements(vectorX, maxRow, i);
            Console.WriteLine("Вектор ответов после обмена: ");
            PrintVector(vectorX);
        }
        else
        {
            Console.WriteLine("Обмен не понадобился, матрица: ");
            PrintMatrix(matrix);
            Console.WriteLine("Обмен не понадобился, вектор ответов: ");
            PrintVector(vectorX);
        }
        
        
        
    }


    for (int i = 0; i < n - 1; ++i)
    {
        for (int j = i + 1; j < n; ++j)
        {
            Complex multiplier = matrix[j, i] / matrix[i, i];
            for (int k = i; k < m; ++k)
            {
                matrix[j, k] -= multiplier * matrix[i, k];
            }
            vectorX[j] -= multiplier * vectorX[i];
        }
        Console.WriteLine($"Прямой ход метода Гаусса, итерация: {i+1}");
        Console.WriteLine($"Матрица после прямого хода:");
        PrintMatrix(matrix);
        Console.WriteLine($"Вектор после прямого хода:");
        PrintVector(vectorX);
    }


    for (int i = n - 1; i >= 0; --i)
    {
        Complex sum = vectorX[i];
        for (int j = i + 1; j < n; ++j)
        {
            sum -= matrix[i, j] * vectorX[j];
        }
        vectorX[i] = sum / matrix[i, i];
        Console.WriteLine($"Обратный ход метода Гаусса, итерация: {n - i}");
        Console.WriteLine($"Вектор после обратного хода:");
        PrintVector(vectorX);
    }

    return vectorX;
}

void SwapRows(Complex[,] matrix, int row1, int row2)
{
    for (int col = 0; col < matrix.GetLength(0); ++col)
    {
        Complex temp = matrix[row1, col];
        matrix[row1, col] = matrix[row2, col];
        matrix[row2, col] = temp;
    }

}

void SwapElements(Complex[] array, int index1, int index2)
{
    Complex temp = array[index1];
    array[index1] = array[index2];
    array[index2] = temp;
}

(int,bool) FindMaximumInColumn(Complex[,] matrix, int startRow)
{
    int maxRow = startRow;
    bool swapFlag = true;
    Complex maxValue = matrix[startRow, startRow];
    for (int i = startRow + 1; i < matrix.GetLength(0); ++i)
    {
        if (Complex.Abs(matrix[i, startRow]) > Complex.Abs(maxValue))
        {
            maxRow = i;
            maxValue = matrix[i, startRow];
        }
    }
    if(maxRow == startRow)
    {
        Console.WriteLine($"Максимальное число на нужном месте, обмен не нужен. Индекс:[{startRow + 1},{maxRow}], число: {maxValue}, абсолютное значение: {Complex.Abs(maxValue)}");
        swapFlag = false;
    }
    else
    {
        Console.WriteLine($"Максимальное число найдено, необходим обмен. Индекс:[{startRow + 1},{maxRow}], число: {maxValue}, абсолютное значение: {Complex.Abs(maxValue)}");
    }
    return (maxRow,swapFlag);
}

void PrintMatrix(Complex[,] matrix)
{
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[i, j].Imaginary < 0)
            {
                Console.Write("{0}\t", Math.Round(matrix[i, j].Real,2) + "" + Math.Round(matrix[i, j].Imaginary,2) + "i");
            }
            else
                Console.Write("{0}\t", Math.Round(matrix[i, j].Real, 2) + "+" + Math.Round(matrix[i, j].Imaginary, 2) + "i");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
void PrintVector(Complex[] result)
{
    Console.WriteLine();
    foreach (var i in result)
    {
        if (i.Imaginary < 0)
        {
            Console.WriteLine("{0}\t", Math.Round(i.Real, 2) + "" + Math.Round(i.Imaginary,2) + "i");
        }
        else
            Console.WriteLine("{0}\t", Math.Round(i.Real, 2) + "+" + Math.Round(i.Imaginary, 2) + "i");
    }
    Console.WriteLine();
}

//multipe = A[1, 0] / A[0, 0];
//A[1, 0] -= A[0, 0] * multipe;
//A[1, 1] -= A[0, 1] * multipe;
//A[1, 2] -= A[0, 2] * multipe;
//A[1, 3] -= A[0, 3] * multipe;
//A[1, 4] -= A[0, 4] * multipe;

//multipe = A[2, 0] / A[0, 0];
//A[2, 0] -= A[0, 0] * multipe;
//A[2, 1] -= A[0, 1] * multipe;
//A[2, 2] -= A[0, 2] * multipe;
//A[2, 3] -= A[0, 3] * multipe;
//A[2, 4] -= A[0, 4] * multipe;

//multipe = A[3, 0] / A[0, 0];
//A[3, 0] -= A[0, 0] * multipe;
//A[3, 1] -= A[0, 1] * multipe;
//A[3, 2] -= A[0, 2] * multipe;
//A[3, 3] -= A[0, 3] * multipe;
//A[3, 4] -= A[0, 4] * multipe;

//Complex multipeRevers;

//multipeRevers = A[0, 1] / A[1, 1];
//A[0, 1] -= A[1, 1] * multipeRevers;
//A[0, 2] -= A[1, 2] * multipeRevers;
//A[0, 3] -= A[1, 3] * multipeRevers;
//A[0, 4] -= A[1, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {1}");
//PrintMatrix(A);


//multipeRevers = A[1, 2] / A[2, 2];
//A[1, 2] -= A[2, 2] * multipeRevers;
//A[1, 3] -= A[2, 3] * multipeRevers;
//A[1, 4] -= A[2, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {2}");
//PrintMatrix(A);


//multipeRevers = A[0, 2] / A[2, 2];
//A[0, 2] -= A[2, 2] * multipeRevers;
//A[0, 3] -= A[2, 3] * multipeRevers;
//A[0, 4] -= A[2, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {3}");
//PrintMatrix(A);

//multipeRevers = A[2, 3] / A[3, 3];
//A[2, 3] -= A[3, 3] * multipeRevers;
//A[2, 4] -= A[3, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {4}");
//PrintMatrix(A);

//multipeRevers = A[1, 3] / A[3, 3];
//A[1, 3] -= A[3, 3] * multipeRevers;
//A[1, 4] -= A[3, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {5}");
//PrintMatrix(A);

//multipeRevers = A[0, 3] / A[3, 3];
//A[0, 3] -= A[3, 3] * multipeRevers;
//A[0, 4] -= A[3, 4] * multipeRevers;

//Console.WriteLine($"Матрица после обратного хода, итерация {6}");
//PrintMatrix(A);

//Console.WriteLine((A[0, 4]));
//Console.WriteLine((A[1, 4]));
//Console.WriteLine((A[2, 4]));
//Console.WriteLine((A[3, 4]));