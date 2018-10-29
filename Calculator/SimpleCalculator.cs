using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public static class SimpleCalculator
    {
        public static double Calculate(string input)
        {
            double value = 0;
            var precedence = new Dictionary<string, int> { { "(", 0 }, { "*", 1 }, { "/", 1 }, { "+", 2 }, { "-", 2 }, { ")", 3 } };
            Stack<string> operators = new Stack<string>();
            Stack<double> operands = new Stack<double>();
            string[] eq = input.Split(' ');
            foreach (string term in eq)
            {
                if (double.TryParse(term, out value))
                {
                    operands.Push(value);
                }
                else if (precedence.ContainsKey(term))
                {
                    bool keepLooping = true;
                    while (keepLooping && operators.Count > 0 && precedence[term] > precedence[operators.Peek()])
                    {
                        switch (operators.Peek())
                        {
                            case "+":
                                operands.Push(operands.Pop() + operands.Pop());
                                break;
                            case "-":
                                operands.Push(-operands.Pop() + operands.Pop());
                                break;
                            case "*":
                                operands.Push(operands.Pop() * operands.Pop());
                                break;
                            case "/":
                                var divisor = operands.Pop();
                                operands.Push(operands.Pop() / divisor);
                                break;
                            case "(":
                                keepLooping = false;
                                break;
                        }
                        if (keepLooping)
                            operators.Pop();
                    }
                    if (term == ")")
                        operators.Pop();
                    else
                        operators.Push(term);
                    break;
                }
                else
                {
                    return -1;
                }
            }
            if (operators.Count > 0 || operands.Count > 1)
                return -1;

            return operands.Pop();
        }
    }
}
