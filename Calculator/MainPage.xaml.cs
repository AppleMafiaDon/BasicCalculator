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

            this.equation.Text += pressed + " ";
		}

		void OnClear(object sender, EventArgs e)
		{
            this.equation.Text = "";
            this.resultText.Text = "0";
		}

        void OnCalculate(object sender, EventArgs e)
        {
            string eq = this.equation.Text;
            if (Regex.Matches(eq, @"[a-zA-Z]").Count > 1)
            { 
                this.resultText.Text = "Error: Invalid Equation; Letters are present";
                return;
            }
            else if(Regex.Matches(eq, @"(").Count != Regex.Matches(eq, @")").Count)
            {
                this.resultText.Text = "Error: Invalid Equation; Wrong number of parenthesis";
                return;
            }

            var result = SimpleCalculator.Calculate(this.equation.Text);
            if (result == -1)
            {
                this.resultText.Text = "Error: Invalid Equation; Wrong number of operators or arguments";
                return;
            }
            this.resultText.Text = result.ToString();
		}
	}
}
