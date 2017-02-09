using System;
using System.IO;

namespace BFCompilr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string contents = string.Empty;

            Console.Write("Enter the file path: ");
            string filePath = Console.ReadLine();
            bool success = BracketMatcher(filePath);
            if (success)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                        contents = reader.ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                InterpretBF(contents);
            }
            Console.ReadKey();
        }

        private static bool BracketMatcher(string filePath)
        {
            try
            {
                int bracketCount = 0;
                char currentChar;
                using(StreamReader reader = new StreamReader(filePath))
                    while(reader.Peek() >= 0)
                    {
                        currentChar = (char)reader.Read();
                        if (currentChar == '[')
                            bracketCount++;
                        else if (currentChar == ']')
                            bracketCount--;
                    }
                if (bracketCount != 0)
                {
                    Console.Write("Brackets do not match");
                    return false;
                }
                else
                    return true;
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        private static void InterpretBF(string program)
        {
            int loop = 0;

            const int maxCells = 300000;
            int[] cells = new int[maxCells];
            int currentIndex = 0;
            char currentCharacter;

            for(int i = 0; i < program.Length; i++)
            {
                currentCharacter = program[i];
                if (currentCharacter == '>')
                    currentIndex = currentIndex + 1 >= maxCells ? 0 : currentIndex + 1;
                else if (currentCharacter == '<')
                    currentIndex = currentIndex - 1 < 0 ? maxCells - 1 : currentIndex - 1;
                else if (currentCharacter == '+')
                    cells[currentIndex] = cells[currentIndex] + 1 > 255 ? 0 : cells[currentIndex] + 1;
                else if (currentCharacter == '-')
                    cells[currentIndex] = cells[currentIndex] - 1 < 0 ? 255 : cells[currentIndex] - 1;
                else if (currentCharacter == '.')
                    Console.Write((char)cells[currentIndex]);
                else if (currentCharacter == ',')
                    cells[currentIndex] = Console.ReadKey().KeyChar;
                else if (currentCharacter == '[')
                    continue;
                else if (currentCharacter == ']' && cells[currentIndex] != 0)
                {
                    loop = 1;
                    while(loop > 0)
                    {
                        i--;
                        currentCharacter = program[i];
                        if (currentCharacter == '[')
                            loop--;
                        else if (currentCharacter == ']')
                            loop++;
                    }
                }
            }
        }
    }
}