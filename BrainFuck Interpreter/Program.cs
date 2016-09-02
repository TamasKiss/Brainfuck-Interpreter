using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFuck_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine(); //read in everything
            int[] memory = new int[3000];
            int ip = 0; // instruction pointer
            int dp = 0; //data pointer

            input = optimalize(input);

            while (input.Length < ip)
            {
                int nesting = 0;
                char c = input[ip]; //current operation

                switch (c)
                {
                    case '>':
                        dp++;
                        break;
                    case '<':
                        dp--;
                        break;
                    case '+':
                        memory[dp]++;
                        break;
                    case '-':
                        memory[dp]--;
                        break;
                    case '.':
                        Console.Write(Convert.ToChar(memory[dp]));
                        break;
                    case ',':
                        memory[dp] = Console.Read();
                        break;
                    case '[':
                        if (memory[dp] == 0)
                            for (int j = ip + 1; j < input.Length; j++) //look forward and find corresponding ']'
                            {
                                if (input[j] == '[') //We've found a nested for
                                    nesting++;
                                else if (input[j] == ']')
                                    //Resolve one layer, further checking if this is the nest we are looking for
                                {
                                    if (nesting == 0)
                                    {
                                        ip = j; //If it is, jump there
                                        break;
                                    }
                                    nesting--; //If not, reduce nesting
                                }
                            }
                        break;
                    case ']':
                        if (memory[dp] != 0)
                            for (int j = ip - 1; j >= 0; j--)
                            {
                                if (input[j] == ']')
                                    nesting++;
                                else if (input[j] == '[')
                                {
                                    if (nesting == 0)
                                    {
                                        ip = j - 1; // -1 beacuse ip is increased at the end of the loop
                                        break;
                                    }
                                    nesting--;
                                }
                            }
                        break;
                }
                ip++;
            }


            Console.ReadKey();
        }

        private static string optimalize(string input)
        {
            string optimalized = "";

            for (int i = 0; i < input.Length; i++)
            {
                int lastInSequence;

                switch (input[i])
                {
                    case '>':
                    case '<':
                        optimalized += Simplify(input, '>', '<', i, out lastInSequence);
                        i += lastInSequence - i - 1;
                        break;
                    case '+':
                    case '-':
                        optimalized += Simplify(input, '+', '-', i, out lastInSequence);
                        i += lastInSequence - i - 1;
                        break;
                    case '.':
                    case ',':
                    case '[':
                    case ']':
                        optimalized += input[i];
                        break;
                }
            }

            return optimalized;
        }

        private static string Simplify(string input, char left, char right, int where, out int j)
        {
            int leftNumber = 0;

            for (j = where; j < input.Length && (input[j] == left || input[j] == right); j++)
            {
                if (input[j] == left)
                    leftNumber++;
                else
                    leftNumber--;
            }

            if (j == where + 1)
                return input[where].ToString();

            return AddItem(left, right, leftNumber);
        }

        private static string AddItem(char charLeft, char charRight, int leftNumber)
        {
            string result = "";

            if (leftNumber > 0)
                for (int i = 0; i < leftNumber; i++)
                    result += charLeft;
            else
                for (int i = 0; i < -1*leftNumber; i++)
                    result += charRight;

            return result;
        }
    }
}

