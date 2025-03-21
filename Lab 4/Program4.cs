using System;
using System.Collections.Generic;

class Program4
{
    public static void StringGeneration(string RE)
    {
        char[] REArr = RE.ToArray();
        for(int i = 0; i < REArr.Length;i++)
        {
            // ? 
            if ((REArr[i] == '?') && (char.IsLetter(REArr[i-1]) || char.IsDigit(REArr[i-1])) && (REArr[i-1]!=')'))
            {
                Random rand = new Random();
                int randomNum = rand.Next(2);
                if (randomNum == 1) Console.Write(REArr[i-1]);
            }
            // *
            if ((REArr[i] == '*') && (char.IsLetter(REArr[i-1]) || char.IsDigit(REArr[i-1])) && (REArr[i-1]!=')'))
            {
                Random rand = new Random();
                int randomNum = rand.Next(6);
                switch (randomNum)
                {
                    case 0: Console.Write(""); break;
                    case 1: Console.Write(REArr[i-1]); break;
                    case 2: Console.Write(new string(REArr[i - 1], 2)); break;
                    case 3: Console.Write(new string(REArr[i - 1], 3)); break;
                    case 4: Console.Write(new string(REArr[i - 1], 4)); break;
                    case 5: Console.Write(new string(REArr[i - 1], 5)); break;
                    default: Console.Write(""); break;
                }
            }
            // +
            if ((REArr[i] == '+') && (char.IsLetter(REArr[i-1]) || char.IsDigit(REArr[i-1])) && (REArr[i-1]!=')'))
            {
                Random rand = new Random();
                int randomNum = 1 + rand.Next(5);
                switch (randomNum)
                {
                    case 1: Console.Write(REArr[i-1]); break;
                    case 2: Console.Write(new string(REArr[i - 1], 2)); break;
                    case 3: Console.Write(new string(REArr[i - 1], 3)); break;
                    case 4: Console.Write(new string(REArr[i - 1], 4)); break;
                    case 5: Console.Write(new string(REArr[i - 1], 5)); break;
                    default: Console.Write(REArr[i-1]); break;
                }
            }
            // ^
            if ((REArr[i] == '^') && (char.IsLetter(REArr[i-1]) || char.IsDigit(REArr[i-1])) && (REArr[i-1]!=')'))
            {
                int k = int.Parse(REArr[i+1].ToString());
                Console.Write(new string(REArr[i-1], k)); 
            }
            // ()
            if ((REArr[i] == '(') && (REArr[i+4] == ')') && ((i+4 == REArr.Length-1) || (REArr[i+5] != '^')))
            {
                Random rand = new Random();
                int randomNum = rand.Next(2);
                if (randomNum == 0) Console.Write(REArr[i+1]);
                else Console.Write(REArr[i+3]);
            }
            else if ((REArr[i] == '(') && (REArr[i+4] == ')') && (REArr[i+5] == '^'))
            {
                Random rand = new Random();
                int randomNum = rand.Next(2);
                int k = int.Parse(REArr[i+6].ToString());
                if (randomNum == 0) Console.Write(new string(REArr[i+1], k));
                else Console.Write(new string(REArr[i+3], k));
            }
            else if ((REArr[i] == '(') && (REArr[i+6] == ')')  && ((i+6 == REArr.Length-1) || (REArr[i+7] != '^')))
            {
                Random rand = new Random();
                int randomNum = rand.Next(3);
                if (randomNum == 0) Console.Write(REArr[i+1]);
                else if (randomNum == 1) Console.Write(REArr[i+3]);
                else Console.Write(REArr[i+5]);
            }
            else if ((REArr[i] == '(') && (REArr[i+6] == ')') && (REArr[i+7] == '^'))
            {
                Random rand = new Random();
                int randomNum = rand.Next(3);
                int k = int.Parse(REArr[i+8].ToString());
                if (randomNum == 0) Console.Write(new string(REArr[i+1], k));
                else if (randomNum == 1) Console.Write(new string(REArr[i+3], k));
                else Console.Write(new string(REArr[i+5], k));
            }

        }
        Console.WriteLine();
    }
    public static void Main()
    {
        string RE1 = "M?N^2(O|P)^3Q*R+";
        string RE2 = "(X|Y|Z)^38+(9|0)";
        string RE3 = "(H|i)(J|K)L*N?";
        for (int l1 = 0; l1 < 5; l1++) { StringGeneration(RE1); }
        Console.WriteLine();
        for (int l2 = 0; l2 < 5; l2++) { StringGeneration(RE2); }
        Console.WriteLine();
        for (int l3 = 0; l3 < 5; l3++) { StringGeneration(RE3); }
        Console.WriteLine();

    }
}