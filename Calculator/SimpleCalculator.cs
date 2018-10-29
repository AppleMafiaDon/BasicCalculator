using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class SimpleCalculator
    {
        public double Calculate(string input)
        {
            string value = ""; //empty storage to help build operands
            double result = 0; //stores the value once parsed from built value
            var precedence = new Dictionary<char, int> { { '(', 0 }, { '*', 3 }, { '/', 3 }, { '+', 1 }, { '-', 2 }, { ')', 3 } }; //sets precedence
            Stack<char> operators = new Stack<char>(); //stack for operators
            Stack<double> operands = new Stack<double>(); //stack for operands
            char[] eq = input.ToCharArray(); //get a character array of the input equation
            foreach (char term in eq)
            {
                if (term == '.' || Char.IsDigit(term)) //if the char is part of one of the operands
                    value += term; //add it to the operand being built
                else if (term == ' ') //else if it is a space
                {
                    if (value != "") //if we have an operand that was being built it has ended
                    {
                        if (double.TryParse(value, out result)) //try to parse the operand to a double
                        {
                            operands.Push(result); //push the operand onto the operand stack
                        }
                        value = ""; //reset the operand being built to empty
                    }
                }
                else if (precedence.ContainsKey(term)) //else if it is one of the characters in the precedence dictionary
                {
                    if (value != "") //if there  was an operand being built
                    {
                        if (double.TryParse(value, out result)) //try to parse it to result
                        {
                            operands.Push(result); //push the value onto the operand stack
                        }
                        value = ""; //reset value
                    }

                    switch (term)
                    {
                        case ')':
                            bool parenClosed = false;
                            while (!parenClosed)
                            {
                                char oper = operators.Pop();
                                if (operators.Count != 0 && oper != '(')
                                {
                                    while (precedence[operators.Peek()] > precedence[oper])
                                    {
                                        double skippedOperand = operands.Pop();
                                        char higherOp = operators.Pop();
                                        switch (higherOp)
                                        {
                                            case '*':
                                                operands.Push(operands.Pop() * operands.Pop());
                                                break;
                                            case '/':
                                                double divisor = operands.Pop();
                                                operands.Push(operands.Pop() / divisor);
                                                break;
                                            case '-':
                                                double subtractor = operands.Pop();
                                                operands.Push(operands.Pop() - subtractor);
                                                break;
                                        }
                                        operands.Push(skippedOperand);
                                        if (operators.Count == 0)
                                            break;
                                    }
                                }
                                switch (oper)
                                {
                                    case '*':
                                        operands.Push(operands.Pop() * operands.Pop());
                                        break;
                                    case '/':
                                        double divisor = operands.Pop();
                                        operands.Push(operands.Pop() / divisor);
                                        break;
                                    case '+':
                                        operands.Push(operands.Pop() + operands.Pop());
                                        break;
                                    case '-':
                                        double subtractor = operands.Pop();
                                        operands.Push(operands.Pop() - subtractor);
                                        break;
                                    case '(':
                                        parenClosed = true;
                                        break;
                                }
                                }
                            
                            break;
                        case '*':
                            operators.Push(term);
                            break;
                        case '/':
                            operators.Push(term);
                            break;
                        case '+':
                            operators.Push(term);
                            break;
                        case '-':
                            operators.Push(term);
                            break;
                        case '(':
                            operators.Push(term);
                            break;
                    }
                }
                else //Otherwise it is a character that isn't a number, space, operator, or period
                {
                    return -1;
                }
            }
            if (value != "")
            {
                if (double.TryParse(value, out result))
                {
                    operands.Push(result);
                }
                value = "";
            }
            while (operators.Count > 0)
            {
                char oper = operators.Pop();
                if (operators.Count != 0)
                {
                    while (precedence[operators.Peek()] > precedence[oper])
                    {
                        double skippedOperand = operands.Pop();
                        char higherOp = operators.Pop();
                        switch (higherOp)
                        {
                            case '*':
                                operands.Push(operands.Pop() * operands.Pop());
                                break;
                            case '/':
                                double divisor = operands.Pop();
                                operands.Push(operands.Pop() / divisor);
                                break;
                            case '-':
                                double subtractor = operands.Pop();
                                operands.Push(operands.Pop() - subtractor);
                                break;
                        }
                        operands.Push(skippedOperand);
                        if (operators.Count == 0)
                            break;
                    }
                }
                switch (oper)
                {
                    case '*':
                        operands.Push(operands.Pop() * operands.Pop());
                        break;
                    case '/':
                        double divisor = operands.Pop();
                        operands.Push(operands.Pop() / divisor);
                        break;
                    case '+':
                        operands.Push(operands.Pop() + operands.Pop());
                        break;
                    case '-':
                        double subtractor = operands.Pop();
                        operands.Push(operands.Pop() - subtractor);
                        break;
                }
            }
            if (operands.Count != 1)
                return -1;

            return operands.Pop();
        }
    }
}
