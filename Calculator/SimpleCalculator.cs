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
                        case ')': //if its a closing parentheses than we need to process out the next opening paren
                            bool parenClosed = false;
                            while (!parenClosed) //While we haven't fouind the next paren, going back
                            {
                                char oper = operators.Pop();//get the current operator to be processed
                                if (operators.Count != 0 && oper != '(')
                                {
                                    while (precedence[operators.Peek()] > precedence[oper]) //While there are higher precedence operators in the stack
                                    {
                                        if (operands.Count < 3) //if we wont have enough operands to perform a calculation return -1
                                            return -1;
                                        handleHigherOp(operands, operators); //handle them
                                        if (operators.Count == 0) //if we have no more operators
                                            break;
                                    }
                                }
                                if (operands.Count < 2) //if we wont have enough operands to perform a calculation return -1
                                    return -1;
                                switch (oper) //Now we can process the current operator
                                {
                                    case '*':
                                        operands.Push(operands.Pop() * operands.Pop());//Multiply the next two operands popped and store it back on the stack
                                        break;
                                    case '/':
                                        double divisor = operands.Pop(); //Get the divisor for the equation
                                        operands.Push(operands.Pop() / divisor); //Divide the next operand by the divisor and stor it on the stack
                                        break;
                                    case '+':
                                        operands.Push(operands.Pop() + operands.Pop()); //Add the next two operands and store it on the stack
                                        break;
                                    case '-':
                                        double subtractor = operands.Pop(); //Get the subtractor
                                        operands.Push(operands.Pop() - subtractor); //Store the subtracted value in the stack
                                        break;
                                    case '(':
                                        parenClosed = true;
                                        break;
                                }
                            }
                            
                            break;
                        case '*':                   //If it isn't a closing paren then just add the operator to the stack
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
            if (value != "") //If there was a final double being built at the end oif the equation store it to operands
            {
                if (double.TryParse(value, out result))
                {
                    operands.Push(result);
                }
                value = "";
            }
            while (operators.Count > 0) //While there are still operators we need to process them
            {
                char oper = operators.Pop();
                if (operators.Count != 0)
                {
                    while (precedence[operators.Peek()] > precedence[oper]) //While there are higher precedence operators in the stack
                    {
                        if (operands.Count < 3) //if we wont have enough operands to perform a calculation return -1
                            return -1;
                        handleHigherOp(operands, operators); //handle them
                        if (operators.Count == 0) //if we have no more operators
                            break;
                    }
                }
                if (operands.Count < 2) //if we wont have enough operands to perform a calculation return -1
                    return -1;
                switch (oper)
                {
                    case '*':
                        operands.Push(operands.Pop() * operands.Pop());//Multiply the next two operands popped and store it back on the stack
                        break;
                    case '/':
                        double divisor = operands.Pop(); //Get the divisor for the equation
                        operands.Push(operands.Pop() / divisor); //Divide the next operand by the divisor and stor it on the stack
                        break;
                    case '+':
                        operands.Push(operands.Pop() + operands.Pop());
                        break;
                    case '-':
                        double subtractor = operands.Pop(); //Get the subtractor
                        operands.Push(operands.Pop() - subtractor); //Store the subtracted value in the stack
                        break;
                }
            }
            if (operands.Count != 1) //If we had too many operands, or too few operands
                return -1;

            return operands.Pop();
        }

        //Private method which handles the generic higher operation processing
        //Subtraction included because the ordering of the operands matters for subtraction versus addition
        private static void handleHigherOp(Stack<double> operands, Stack<char> operators)
        {
            double skippedOperand = operands.Pop(); //Store the operand to be skipped
            char higherOp = operators.Pop(); //clear the operator being used
            switch (higherOp)
            {
                case '*':
                    operands.Push(operands.Pop() * operands.Pop()); //Multiply the next two operands popped and store it back on the stack
                    break;
                case '/':
                    double divisor = operands.Pop(); //Get the divisor for the equation
                    operands.Push(operands.Pop() / divisor); //Divide the next operand by the divisor and stor it on the stack
                    break;
                case '-':
                    double subtractor = operands.Pop(); //Get the subtractor
                    operands.Push(operands.Pop() - subtractor); //Store the subtracted value in the stack
                    break;
            }
            operands.Push(skippedOperand); //put the skipped operand back on the stack
        }
    }
}
