using System;
using System.IO;
using LFA_Lab3.Lexer;

namespace Lab3
{
    class Program3
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose the file you want to tokanise: ");
            Console.WriteLine("    1) character.txt");
            Console.WriteLine("    2) quest.txt");
            Console.WriteLine("    3) dialogue.txt");
            Console.WriteLine("    4) combined.txt");
            Console.WriteLine();
            string? choice = Console.ReadLine();
            string filepath;
            switch(choice)
            {
                case "1": filepath = "Samples\\character.txt"; break;
                case "2": filepath = "Samples\\quest.txt"; break;
                case "3": filepath = "Samples\\dialogue.txt"; break;
                case "4": filepath = "Samples\\combined.txt"; break;
                default: filepath = "Samples\\combined.txt"; break;
            }
            string input = File.ReadAllText(filepath);
            Lexer lexer = new Lexer(input);
            File.WriteAllText("Samples\\output.txt", string.Empty);
            while (lexer.Has_Next_Token())
            {
                Token token = lexer.Next_Token();
                File.AppendAllText("Samples\\output.txt", token.ToString() + '\n');
            }
            Console.WriteLine("The tokenisation is shown in output.txt");
        }
    }
}