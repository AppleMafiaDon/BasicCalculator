using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text.RegularExpressions;


namespace Calculator
{    
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();
            OnClear(this, null);
        }

		void OnSelectButton(object sender, EventArgs e)
		{
            Button button = (Button)sender;
			string pressed = button.Text;
            if (pressed == "Space")
                pressed = " ";
            this.equation.Text += pressed;
		}

        //The action command when hitting C on the Calculator
		void OnClear(object sender, EventArgs e)
		{
            this.equation.Text = "";
            this.resultText.Text = "0";
		}

        void OnCalculate(object sender, EventArgs e)
        {
            string eq = this.equation.Text;
            if (Regex.Matches(eq, @"[a-zA-Z]").Count > 1) //If there are any letters in the string it is not a math equation
            { 
                this.resultText.Text = "Error: Invalid Equation; Letters are present";
                return;
            }
            int counter = 0;
            for (int i = 0; i < eq.Length; i ++)//Check to make sure the same number of opening as closing parentheses
            {
                if (eq[i] == '(')
                    counter++;
                else if (eq[i] == ')')
                    counter--;
            }
            if(counter != 0) //If the counter != 0 then we know the number of closing or opening parantheses is off
            {
                this.resultText.Text = "Error: Invalid Equation; Wrong number of parenthesis";
                return;
            }

            double result = SimpleCalculator.Calculate(eq);
            if (result == -1)
            {
                this.resultText.Text = "Error: Invalid Equation; Wrong number of operators or arguments";
                return;
            }
            this.resultText.Text = result.ToString();
		}
	}
}
