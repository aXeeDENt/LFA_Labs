using System;
using System.Collections.Generic;
using LFA_Lab1;

namespace LFA_Lab2
{
    class Program2
    {
        static void Main(string[] args)
        {
            var grammar = new Grammar();
            int chomskyType = grammar.GetChomskyType();
            string typeDescription = grammar.GetChomskyTypeDescription();
            Console.WriteLine($"Chomsky Type -> {typeDescription}");
            switch (chomskyType)
            {
                case 3:
                    Console.WriteLine("This is a Regular Grammar");
                    break;
                case 2:
                    Console.WriteLine("This is a Context-Free Grammar");
                    break;
                case 1:
                    Console.WriteLine("This is a Context-Sensitive Grammar");
                    break;
                case 0:
                    Console.WriteLine("This is an Unrestricted Grammar");
                    break;
            }
        }
    }
}