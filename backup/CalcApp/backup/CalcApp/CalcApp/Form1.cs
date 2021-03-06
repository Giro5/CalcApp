﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace CalcApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //double[] nums = new double[100];
        //public string[] znaks = new string[100];
        //public int moves { get; set; } = 0;
        //List<int> nums = new List<int> { };
        //char Znak { get; set; }
        //int Count { get; set; } = 0;

        public const string Version = "0.1.4.4", LastUpdate = "LastUpdate:\n· Improvement system of errors";

        double Result { get; set; } = 0;
        double Digit { get; set; } = 0;
        double Number { get; set; } = 0;
        double PossibleAnswer { get; set; }
        double PiDivide180 { get; } = 0.01745329251994329576923690768489d;
        double PiDivede200 { get; } = 0.0157079632679489661923132169164d;
        double[] MNumbers = new double[0];

        string Operation { get; set; } = "";
        string DRG { get; set; } = "DEG";

        string Mul { get; } = "×";
        string Plus { get; } = "+";
        string Minus { get; } = "-";
        string Dev { get; } = "÷";

        char mul { get; } = '×';
        char plus { get; } = '+';
        char minus { get; } = '-';
        char dev { get; } = '÷';

        bool Entering { get; set; } = false;
        bool Cleaner { get; set; } = false;
        bool Stoper { get; set; } = false;
        bool Adding { get; set; } = false;

        int CountOperations { get; set; } = 0;
        int CountBrackets { get; set; } = 0;
        int Wid { get; set; }

        float MinWidText { get; } = 12f;
        float MaxWidText { get; } = 32f;

        private void Form1_Load(object sender, EventArgs e)
        {
            Wid = ClientSize.Width;
            label1.Text += " " + Version;
            ChangeSizeForm();
            textBox2.Text = "";
            if(Screen.PrimaryScreen.Bounds.Size.Height < 1080 && Screen.PrimaryScreen.Bounds.Size.Width < 1920)
            {
                Width = MinimumSize.Width;
                Height = MinimumSize.Height;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ChangeSizeForm();
        }

        private void buttonPlusAndMinus_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("e+") || textBox1.Text.Contains("e-") || textBox1.Text.Contains("E+") || textBox1.Text.Contains("E-"))
            {
                int ie = 0;
                for (int i = 0; i < textBox1.Text.Length; i++)
                    if (textBox1.Text[i] == 'e')
                        ie = i;
                ie++;
                if (textBox1.Text[ie] == '+')
                    textBox1.Text = textBox1.Text.Replace('+', '-');
                else
                    textBox1.Text = textBox1.Text.Replace('-', '+');
            }
            else
            {
                try
                {
                    textBox1.Text = (Convert.ToDouble(textBox1.Text) * (-1)).ToString();
                    Digit = Convert.ToDouble(textBox1.Text);
                }
                catch
                {
                    string tmp = textBox1.Text;
                    for (int i = 0; i < textBox1.Text.Length; i++)
                    {
                        if ((!Char.IsDigit(textBox1.Text[i]) && textBox1.Text[i] != ',') && (i != 0 || textBox1.Text[i] != '-'))
                        {
                            textBox1.Text = textBox1.Text.Remove(i, 1);
                            i--;
                        }
                    }
                    try
                    {
                        textBox1.Text = (Convert.ToDouble(textBox1.Text) * (-1)).ToString();
                        Digit = Convert.ToDouble(textBox1.Text);
                        MessageBox.Show("Code #1:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nSuccessful corrected.\nFor continued press OK.", "Error");
                    }
                    catch
                    {
                        MessageBox.Show("Code #1:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nFailed corrected.\nFor continued press OK.", "Error");
                        buttonCancel.PerformClick();
                    }
                }
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);

            if (textBox1.Size.Width > textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size < 24f)
                textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 0.5f);

            if (textBox1.Text.Length == 0)
                buttonCE.PerformClick();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Wid < ClientSize.Width && textBox1.Size.Width > textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size < MaxWidText)
                textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 0.5f);
            if (Wid > ClientSize.Width && textBox1.Size.Width < textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size > MinWidText)
                textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 0.5f);
            Wid = ClientSize.Width;
            textBox1.TextAlign = HorizontalAlignment.Right;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void NumbersAndDot(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (Cleaner)
                buttonCancel.PerformClick();
            Cleaner = false;
            if (!Entering)
                textBox1.Text = "";
            if (Adding && textBox1.Text.Split('e')[1].Count() < 5)
            {
                if (textBox1.Text.Last() == '0' && (textBox1.Text[textBox1.Text.Length - 2] == '+' || textBox1.Text[textBox1.Text.Length - 2] == '-'))
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                if (b.Text != ",")
                    textBox1.Text += b.Text;
            }
            else
            {
                if (b.Text == ",")
                {
                    if (!textBox1.Text.Contains(",") && (textBox1.Text.Length != 0))
                        textBox1.Text += ",";
                    else if (textBox1.Text.Length == 0)
                        textBox1.Text = "0,";
                    Entering = true;
                }
                else if (b.Text == "0")
                {
                    if (textBox1.Text != "0")
                        textBox1.Text += "0";
                }
                else
                {
                    textBox1.Text += b.Text;
                    Entering = true;
                }
            }
            ChangeSizeForm();
        }

        private void OperatingSymbols_Click(object sender, EventArgs e)
        {
            if (Cleaner)
            {
                buttonCancel.PerformClick();
                return;
            }
            Cleaner = false;
            Adding = false;
            CountOperations++;
            Button b = (Button)sender;
            if (b.Text == "xʸ")
                Operation = "^";
            else if (b.Text == "ʸ√x")
                Operation = "yroot";
            else
                Operation = b.Text;
            try
            {
                PossibleAnswer = Convert.ToDouble(textBox1.Text);
            }
            catch
            {
                string tmp = textBox1.Text;
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    if ((!Char.IsDigit(textBox1.Text[i]) && textBox1.Text[i] != ',') && (i != 0 || textBox1.Text[i] != '-'))
                    {
                        textBox1.Text = textBox1.Text.Remove(i, 1);
                        i--;
                    }
                }
                try
                {
                    PossibleAnswer = Convert.ToDouble(textBox1.Text);
                    MessageBox.Show("Code #0:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nSuccessful corrected.\nFor continued press OK.", "Error");
                }
                catch
                {
                    MessageBox.Show("Code #0:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nFailed corrected.\nFor continued press OK.", "Error");
                    buttonCancel.PerformClick();
                    return;
                }
            }
            if (!Entering && CountOperations > 1 && (textBox2.Text.Last() == '*' || textBox2.Text.Last() == '+' || textBox2.Text.Last() == '-' || textBox2.Text.Last() == dev
               || textBox2.Text.Last() == 'd' || textBox2.Text.Last() == '^' || textBox2.Text.Last() == 't'))
            {
                if (textBox2.Text.Last() == 'd')
                {
                    if (textBox2.Text[textBox2.Text.Length - 5] != ')')
                    {
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 4, 4) + ") " + Operation;
                        textBox2.Text = textBox2.Text.Insert(1, "(");
                    }
                    else
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 4, 4) + " " + Operation;
                }
                else if (textBox2.Text.Last() == 't')
                {
                    if (textBox2.Text[textBox2.Text.Length - 7] != ')')
                    {
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 6, 6) + ") " + Operation;
                        textBox2.Text = textBox2.Text.Insert(1, "(");
                    }
                    else
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 6, 6) + " " + Operation;
                }
                else
                {
                    if(Operation == "*" || Operation == "/" || Operation == "^")
                    {
                        if(textBox2.Text[textBox2.Text.Length - 3] != ')' && CountOperations > 2)
                        {
                            textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 2, 2) + ") " + Operation;
                            textBox2.Text = textBox2.Text.Insert(1, "(");
                        }
                        else
                            textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 2, 2) + " " + Operation;
                    }
                    else
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 2, 2) + " " + Operation;
                }
                CountOperations--;
            }
            else if (textBox2.Text.Contains("(") && textBox2.Text[textBox2.Text.Length - 2] == ')')
                textBox2.Text += Operation;
            else if (textBox2.Text.Contains("(") && textBox2.Text[textBox2.Text.Length - 1] == '(')
                textBox2.Text += PossibleAnswer.ToString() + " " + Operation;
            else
                textBox2.Text += " " + PossibleAnswer.ToString() + " " + Operation;
            Entering = false;
            if (CountOperations > 1)
            {
                Solution(textBox2.Text);
                if (Operation == "+" || Operation == "-" || Operation == "Mod")
                    textBox1.Text = Result.ToString();
            }
        }

        private void Solution(string text)
        {

            if (text.Length > 2 && (text[text.Length - 1] == '*' || text[text.Length - 1] == '+' || text[text.Length - 1] == '-' || text[text.Length - 1] == dev || text[text.Length - 1] == '^'))
                text = text.Remove(text.Length - 2);
            else if (text.Length > 2 && text[text.Length - 1] == 'd')
                text = text.Remove(text.Length - 4);
            else if (text.Length > 2 && text[text.Length - 1] == 't')
                text = text.Remove(text.Length - 7);
            if (text.Length > 0 && text[0] == ' ')
                text = text.Remove(0, 1);
            try
            {
                Result = Convert.ToDouble(text);
            }
            catch
            {
                if (text.Contains("(")) //handler brackets
                {
                    int ibegin = 0, iend = 0, CountLeftBrackets = 0, CountRightBrackets = 0, oldiend = 0;
                    for (int i = 0; i < text.Length; i++)
                        if (text[i] == '(')
                            CountLeftBrackets++;
                    for (int i = 0; i < text.Length; i++)
                        if (text[i] == ')')
                            CountRightBrackets++;
                    if (CountLeftBrackets > CountRightBrackets)
                        text = text + new String(')', CountLeftBrackets - CountRightBrackets);
                    for (int loop = 0; loop < CountLeftBrackets; loop++)
                    {
                        for (int i = 0; i < text.Length; i++)
                            if (text[i] == '(')
                                ibegin = i;
                        for (int j = ibegin + 1; j < text.Length; j++)
                            if (text[j] == ')')
                            {
                                iend = j;
                                break;
                            }
                        oldiend = iend;
                        string tmp = text;
                        if (ibegin > 0 && text[ibegin - 1] != ' ')
                        {
                            for (int i = ibegin - 1; i >= 0 && text[i] != ' '; i--)
                                ibegin--;
                            for (int j = ibegin + 1; j < text.Length; j++)
                                if (text[j] == ')')
                                    iend = j;
                            if (iend != text.Length - 1)
                                tmp = text.Remove(iend + 1);
                            tmp = tmp.Remove(0, ibegin);
                            SolutionFunc(tmp);
                        }
                        else
                        {
                            if (oldiend <= text.Length - 1)
                                tmp = text.Remove(oldiend);
                            if(tmp[0] == '(')
                                tmp = tmp.Remove(0, ibegin + 1);
                            Solution(tmp);
                        }
                        CountBrackets--;
                        text = text.Remove(ibegin, oldiend - ibegin + 1);
                        text = text.Insert(ibegin, Result.ToString());
                    }
                }
                try
                {
                    for (int i = 0; i < text.Length; i++) //degree and(or) root
                    {
                        if ((text[i] == '^' && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == 'y' && text[i + 1] == 'r' && text[i + 2] == 'o' && text[i + 3] == 'o' && text[i + 4] == 't'))
                        {
                            int ibegin = i - 2, iend = i + 2;
                            if (text[i] == 'y' && text[i + 1] == 'r' && text[i + 2] == 'o' && text[i + 3] == 'o' && text[i + 4] == 't')
                                iend += 4;
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == '-'); --ibegin)
                                tmp = text[ibegin].ToString() + tmp;
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '^':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, Math.Pow(Digit, Number).ToString());
                                    break;
                                case 'y':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, Math.Pow(Digit, 1.0 / Number).ToString());
                                    break;
                            }
                            i = 0;
                        }
                    }
                    for (int i = 0; i < text.Length; i++) //multiplication and(or) dividing and(or) Mod
                    {
                        if (text[i] == '*' || (text[i] == dev && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == 'M' && text[i + 1] == 'o' && text[i + 2] == 'd'))
                        {
                            int ibegin = i - 2, iend = i + 2;
                            if (text[i] == 'M' && text[i + 1] == 'o' && text[i + 2] == 'd')
                                iend += 2;
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == '-'); --ibegin)
                                tmp = text[ibegin].ToString() + tmp;
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '*':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit * Number).ToString());
                                    break;
                                case '÷':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit / Number).ToString());
                                    break;
                                case 'M':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit % Number).ToString());
                                    break;
                            }
                            i = 0;
                        }
                    }
                    for (int i = 0; i < text.Length; i++) //summation and(or) subtracting
                    {
                        if ((text[i] == '+' && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == '-' && text[i + 1] == ' '))
                        {
                            int ibegin = i - 2, iend = i + 2;
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == '-'); --ibegin)
                                tmp = text[ibegin].ToString() + tmp;
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '+':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit + Number).ToString());
                                    break;
                                case '-':
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit - Number).ToString());
                                    break;
                            }
                            i = 0;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Code #3:\n" + $@"""{text}""" + "\n...\nUnknown error\nFor continued press OK.", "Error");
                    buttonCancel.PerformClick();
                }
                try
                {
                    text = text.Replace('.', ',');
                    Result = Convert.ToDouble(text);
                }
                catch
                {
                    try
                    {
                        Solution(text);
                        MessageBox.Show("Code #2:\n" + $@"""{text}""" + "\n...\nSuccessful corrected.\nFor continued press OK.", "Error");
                    }
                    catch
                    {
                        MessageBox.Show("Code #2:\n" + $@"""{text}""" + "\n...\nFailed corrected.\nFor continued press OK.", "Error");
                        buttonCancel.PerformClick();
                    }
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            buttonCE.PerformClick();
            textBox2.Text = "";
            Result = 0;
            CountOperations = 0;
            Operation = "";
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            Entering = false;
            Adding = false;
            ChangeSizeForm();
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            Cleaner = false;
            Adding = false;
            try
            {
                if (textBox2.Text.Last() != 'd')
                    Operation = textBox2.Text.Last().ToString();
                PossibleAnswer = Convert.ToDouble(textBox1.Text);
            }
            catch { }
            if(textBox1.Text.Contains("e"))
            {
                try
                {
                    PossibleAnswer = Convert.ToDouble(textBox1.Text);
                }
                catch { }
            }

            if (CountOperations == 0 && Operation != "" && !(textBox2.Text.Contains(")") || textBox2.Text.Contains("(")))
                Solution(textBox1.Text + " " + Operation + " " + PossibleAnswer.ToString());
            else if (textBox2.Text.Length > 2 && textBox2.Text[textBox2.Text.Length - 2] != ')')
                Solution(textBox2.Text + " " + PossibleAnswer.ToString());
            else if (CountOperations == 0 && (textBox2.Text.Length == 0 || textBox2.Text.Contains("(")))
                Result = Convert.ToDouble(textBox1.Text);
            else
                Solution(textBox2.Text);

            if (Double.IsInfinity(Result))
                textBox1.Text = "Деление на ноль невозможно";
            else
                textBox1.Text = Result.ToString();
            textBox2.Text = "";
            Entering = false;
            CountOperations = 0;
            ChangeSizeForm();
        }

        private void buttonEscape_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Point movestart;
        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movestart = new Point(e.X, e.Y);
            }
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                Point deltaPos = new Point(e.X - movestart.X, e.Y - movestart.Y);
                Location = new Point(Location.X + deltaPos.X, Location.Y + deltaPos.Y);
            }
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void buttonMaximaze_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                buttonMaximaze.Image = CalcApp.Properties.Resources.icons8_Virtual_Machine_20px_1;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                buttonMaximaze.Image = CalcApp.Properties.Resources.рамка1;
            }
            ChangeSizeForm();
        }

        private void ChangeSizeForm()
        {
            for(int i = 0; i < 20; i++)
            {
                if (textBox1.Size.Width > textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size < MaxWidText)
                    textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 0.5f);
                if (textBox1.Size.Width < textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size > MinWidText)
                    textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 0.5f);
            }
        }

        private void buttonPi_Click(object sender, EventArgs e)
        {
            textBox1.Text = Math.PI.ToString();
            Entering = true;
        }

        private void buttonLeftBracket_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (textBox2.Text.Last() == '*' || textBox2.Text.Last() == '+' || textBox2.Text.Last() == '-' || textBox2.Text.Last() == dev
               || textBox2.Text.Last() == 'd' || textBox2.Text.Last() == '^' || textBox2.Text.Last() == 't')
                {
                    if (CountBrackets <= 25)
                    {
                        textBox2.Text += " (";
                        CountBrackets++;
                    }
                }
                else
                {
                    textBox2.Text = "";
                    buttonLeftBracket.PerformClick();
                }
            }
            else
            {
                if (CountBrackets <= 25)
                {
                    textBox2.Text += " (";
                    CountBrackets++;
                }
            }

        }

        private void buttonRightBracket_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains("("))
            {
                int clb = 0, crb = 0;
                for(int i = 0; i < textBox2.Text.Length; i++)
                {
                    if (textBox2.Text[i] == '(')
                        clb++;
                    else if (textBox2.Text[i] == ')')
                        crb++;
                }
                if (clb > crb)
                {
                    if (textBox2.Text.Contains("(") && textBox2.Text[textBox2.Text.Length - 1] == '(')
                        textBox2.Text += textBox1.Text + ") ";
                    else
                        textBox2.Text += " " + textBox1.Text + ") ";
                    Solution(textBox2.Text);
                    textBox1.Text = Result.ToString();
                    Cleaner = true;
                    Entering = true;
                }
            }
        }

        private void buttonDRG_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (DRG == "DEG")
            {
                DRG = "RAD";
                b.Font = new Font(b.Font.FontFamily, 12f, FontStyle.Bold);
            }
            else if (DRG == "RAD")
            {
                DRG = "GRAD";
                b.Font = new Font(b.Font.FontFamily, 10.5f, FontStyle.Bold);
            }
            else
            { 
                DRG = "DEG";
                b.Font = new Font(b.Font.FontFamily, 12f, FontStyle.Bold);
            }
        b.Text = DRG;
        }

        private void buttonHYP_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.FlatAppearance.BorderColor = Color.White;
            if (b.FlatAppearance.BorderSize == 0)
                b.FlatAppearance.BorderSize = 2;
            else
                b.FlatAppearance.BorderSize = 0;
            RefreshButtonTrigonometry();
        }

        private void RefreshButtonTrigonometry()
        {
            if (buttonHYP.FlatAppearance.BorderSize == 2)
            {
                if (buttonInverse.FlatAppearance.BorderSize == 3)
                {
                    buttonSin_And_Arcsin.Text = "sinhˉ¹";
                    buttonCos_And_Arccos.Text = "coshˉ¹";
                    buttonTan_And_Arctan.Text = "tanhˉ¹";
                }
                else
                {
                    buttonSin_And_Arcsin.Text = "sinh";
                    buttonCos_And_Arccos.Text = "cosh";
                    buttonTan_And_Arctan.Text = "tanh";
                }
            }
            else
            {
                if (buttonInverse.FlatAppearance.BorderSize == 3)
                {
                    buttonSin_And_Arcsin.Text = "sinˉ¹";
                    buttonCos_And_Arccos.Text = "cosˉ¹";
                    buttonTan_And_Arctan.Text = "tanˉ¹";
                }
                else
                {
                    buttonSin_And_Arcsin.Text = "sin";
                    buttonCos_And_Arccos.Text = "cos";
                    buttonTan_And_Arctan.Text = "tan";
                }
            }
        }

        private void buttonInverse_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.FlatAppearance.BorderColor = Color.White;
            if (b.FlatAppearance.BorderSize == 0)
            {
                b.FlatAppearance.BorderSize = 3;
                buttonFact_And_deg.Text = "deg";
                button10degreeX_And_EdegreeX.Text = "eˣ";
                buttonExp_And_dms.Text = "dms";
                buttonXdegreeY_And_YrootX.Text = "ʸ√x";
                buttonSqrt_And_1divideX.Text = "⅟ₓ";
                buttonSqr_And_Cube.Text = "x³";
            }
            else
            {
                b.FlatAppearance.BorderSize = 0;
                buttonFact_And_deg.Text = "n!";
                button10degreeX_And_EdegreeX.Text = "10ˣ";
                buttonExp_And_dms.Text = "Exp";
                buttonXdegreeY_And_YrootX.Text = "xʸ";
                buttonSqrt_And_1divideX.Text = "√";
                buttonSqr_And_Cube.Text = "x²";
            }
            RefreshButtonTrigonometry();
        }

        /* Table FuncOperations
         * x - already entered
         * y - enter after pressing
         * ╔═══════╦════════════╦═══════════════════════╗
         * ║ n!    ║ fact(x)    ║                       ║
         * ║ 10ˣ   ║ 10^(x)     ║                       ║
         * ║ Exp   ║ x,e+0      ║ if x э d              ║
         * ║ sin   ║ sin(x)     ║    (x int),(x fp)e+0  ║
         * ║ cos   ║ cos(x)     ║                       ║
         * ║ tan   ║ tan(x)     ║                       ║
         * ║ xʸ    ║ x ^ y     *║                       ║
         * ║ √     ║ √(x)       ║ x^(1/2)               ║
         * ║ x²    ║ sqr(x)     ║ x^(2)                 ║
         * ╠═══════╬════════════╬═══════════════════════╣
         * ║ log   ║ log(x)     ║ log₁₀(x)  or  lg(x)   ║
         * ║ ln    ║ ln(x)      ║ logₑ(x)               ║
         * ╠═══════╬════════════╬═══════════════════════╣
         * ║ deg   ║ degrees(x) ║                       ║
         * ║ eˣ    ║ e^(x)      ║                       ║
         * ║ dms   ║ dms(x)     ║                       ║
         * ║ sinˉ¹ ║ sinˉ¹(x)   ║ arcsin                ║
         * ║ cosˉ¹ ║ cosˉ¹(x)   ║ arccos                ║
         * ║ tanˉ¹ ║ tanˉ¹(x)   ║ arctan                ║
         * ║ ꙷ√x   ║ x yroot y *║ x^(1/y)               ║
         * ║ ⅟ₓ    ║ 1/(x)      ║                       ║
         * ║ x³    ║ cube(x)    ║ x^(3)                 ║
         * ║       ║            ║                       ║
         * ║       ║            ║                       ║
         * ║       ║            ║                       ║
         * ║       ║            ║                       ║
         * ╚═══════╩════════════╩═══════════════════════╝
         */

        private void FuncOperation_Click(object sender, EventArgs e)
        {
            Cleaner = false;
            Button b = (Button)sender;
            string str = null;
            //list of funcs
            switch (b.Text)
            {
                case "n!":
                    str = "fact";
                    break;
                case "10ˣ":
                    str = "10^";
                    break;
                case "Exp":
                    str = "e+0";
                    break;
                case "sin":
                    if (DRG == "DEG")
                        str = "sinₒ";
                    else if (DRG == "RAD")
                        str = "sinᵣ";
                    else if(DRG == "GRAD")
                        str = "sin₉";
                        break;
                case "cos":
                    if (DRG == "DEG")
                        str = "cosₒ";
                    else if (DRG == "RAD")
                        str = "cosᵣ";
                    else if (DRG == "GRAD")
                        str = "cos₉";
                    break;
                case "tan":
                    if (DRG == "DEG")
                        str = "tanₒ";
                    else if (DRG == "RAD")
                        str = "tanᵣ";
                    else if (DRG == "GRAD")
                        str = "tan₉";
                    break;
                //case "xʸ": break;
                case "√":
                    str = "√";
                    break;
                case "x²":
                    str = "sqr";
                    break;
                case "log":
                    str = "log";
                    break;
                case "ln":
                    str = "ln";
                    break;
                case "deg":
                    str = "degrees";
                    break;
                case "eˣ":
                    str = "e^";
                    break;
                case "dms":
                    str = "dms";
                    break;
                case "sinˉ¹":
                    if (DRG == "DEG")
                        str = "sinₒˉ¹";
                    else if (DRG == "RAD")
                        str = "sinᵣˉ¹";
                    else if (DRG == "GRAD")
                        str = "sin₉ˉ¹";
                    break;
                case "cosˉ¹":
                    if (DRG == "DEG")
                        str = "cosₒˉ¹";
                    else if (DRG == "RAD")
                        str = "cosᵣˉ¹";
                    else if (DRG == "GRAD")
                        str = "cos₉ˉ¹";
                    break;
                case "tanˉ¹":
                    if (DRG == "DEG")
                        str = "tanₒˉ¹";
                    else if (DRG == "RAD")
                        str = "tanᵣˉ¹";
                    else if (DRG == "GRAD")
                        str = "tan₉ˉ¹";
                    break;
                //case "ꙷ√x": break;
                case "⅟ₓ":
                    str = "1/";
                    break;
                case "x³":
                    str = "cube";
                    break;
                case "sinh":
                    str = b.Text;
                    break;
                case "cosh":
                    str = b.Text;
                    break;
                case "tanh":
                    str = b.Text;
                    break;
                case "sinhˉ¹":
                    str = b.Text;
                    break;
                case "coshˉ¹":
                    str = b.Text;
                    break;
                case "tanhˉ¹":
                    str = b.Text;
                    break;
            }
            if (str != "e+0")
            {
                if (textBox2.Text.Length > 3 && textBox2.Text[textBox2.Text.Length - 2] == ')')
                {
                    string text = textBox2.Text;
                    int CountLeftBrackets = 0, CountRightBrackets = 0, ibegin = 0, iend = 0;// ((( )))   ( )( )  (( ) ( ))
                    for (int i = 0; i < text.Length; i++)
                        if (text[i] == ')')
                        {
                            iend = i;
                            CountRightBrackets++;
                        }
                    CountLeftBrackets = CountRightBrackets;
                    for (int i = text.Length - 3; i > 0; i--)
                    {
                        if (text[i] == '(')
                        {
                            CountLeftBrackets--;
                            if (CountLeftBrackets == 0)
                            {
                                ibegin = i;
                                break;
                            }
                        }
                    }
                    string tmp = text.Remove(iend + 1);
                    if (ibegin > 0 && text[ibegin - 1] != ' ')
                        for (int i = ibegin - 1; text[i] != ' '; i--)
                            ibegin--;
                    tmp = tmp.Remove(0, ibegin);
                    if (text[ibegin] != '(')
                        str += "(" + tmp + ")";
                    else
                        str += tmp;
                    textBox2.Text = textBox2.Text.Remove(ibegin, iend - ibegin + 1);
                    textBox2.Text = textBox2.Text.Insert(ibegin, str);
                }
                else
                {
                    str += "(" + textBox1.Text + ")";
                    textBox2.Text += " " + str + " ";
                }
                SolutionFunc(str);
                if (Stoper)
                {
                    buttonCE.PerformClick();
                    textBox1.Text = "Введены неверные данные";
                    Stoper = false;
                    Cleaner = true;
                }
                else
                    textBox1.Text = Result.ToString();
                ChangeSizeForm();
                Entering = false;
                Cleaner = true;
            }
            else //func exp/exf, if(str == "e+0")
            {
                if(textBox2.Text.Length == 0 || (textBox2.Text.Length > 2 && !textBox2.Text.Contains("(")))
                {
                    if (textBox1.Text != "0")
                    {
                        string text = textBox1.Text;
                        text += str;
                        textBox1.Text = text;
                        Adding = true;
                    }
                }

            }
        }

        private void SolutionFunc(string text) //text is briefly func. Example: "fact(5)" will "120"
        {
            if (text[0] == ' ')
                text = text.Remove(0, 1);
            int ibegin = 0, iend = 0;
            for (int i = 0; i < text.Length; i++)
                if (text[i] == '(')
                {
                    ibegin = i;
                    break;
                }
            for (int j = ibegin + 1; j < text.Length; j++)
                if (text[j] == ')')
                    iend = j;
            string tmp = text.Remove(iend);
            tmp = tmp.Remove(0, ibegin + 1);
            Solution(tmp);
            string func = text.Remove(ibegin);
            switch (func)
            {
                case "fact":
                    for (double i = Result - 1.0; i > 1.0; i--)
                        Result *= i;
                    break;
                case "10^":
                    Result = Math.Pow(10.0, Result);
                    if (Double.IsInfinity(Result))
                        Stoper = true;
                    break;
                    //beginning trigonometry func
                case "sinᵣ":
                    Result *= 1.0 / PiDivide180;
                    Result = Math.Round(Result);
                    goto case "sinₒ";
                case "sinₒ":
                    if ((Result % 90.0 != 0.0) || (Result / 90.0 % 2.0 != 0.0))
                        Result = Math.Sin(PiDivide180 * Result);
                    else
                        Result = 0.0;
                    break;
                case "sin₉":
                    if ((Result % 100.0 != 0.0) || (Result / 100.0 % 2.0 != 0.0))
                        Result = Math.Sin(PiDivede200 * Result);
                    else
                        Result = 0.0;
                    break;
                case "cosᵣ":
                    Result *= 1.0 / PiDivide180;
                    Result = Math.Round(Result);
                    goto case "cosₒ";
                case "cosₒ":
                    if ((Result % 90.0 != 0.0) || (Result / 90.0 % 2.0 == 0.0))
                        Result = Math.Cos(PiDivide180 * Result);
                    else
                        Result = 0.0;
                    break;
                case "cos₉":
                    if ((Result % 100.0 != 0.0) || (Result / 100.0 % 2.0 == 0.0))
                        Result = Math.Cos(PiDivede200 * Result);
                    else
                        Result = 0.0;
                    break;
                case "tanᵣ":
                    Result *= 1.0 / PiDivide180;
                    Result = Math.Round(Result);
                    goto case "tanₒ";
                case "tanₒ":
                    if (Result % 90.0 != 0.0)
                        Result = Math.Tan(PiDivide180 * Result);
                    else if (Result / 90.0 % 2.0 == 0.0)
                        Result = 0.0;
                    else
                        Stoper = true;
                    break;
                case "tan₉":
                    if (Result % 100.0 != 0.0)
                        Result = Math.Tan(PiDivede200 * Result);
                    else if (Result / 100.0 % 2.0 == 0.0)
                        Result = 0.0;
                    else
                        Stoper = true;
                    break;
                    //ending trigonometry func
                case "√":
                    if (Result >= 0)
                        Result = Math.Sqrt(Result);
                    else
                        Stoper = true;
                    break;
                case "sqr":
                    Result = Math.Pow(Result, 2.0);
                    break;
                case "log":
                    if (Result > 0)
                        Result = Math.Log10(Result);
                    else
                        Stoper = true;
                    break;
                case "ln":
                    if (Result > 0)
                        Result = Math.Log(Result);
                    else
                        Stoper = true;
                    break;
                case "degrees": //conversion to decimal numbers
                    Digit = Math.Truncate(Result);
                    Result -= Digit;
                    Result = Digit + Result * 100 / 60;
                    break;
                case "e^":
                    Result = Math.Exp(Result);
                    break;
                case "dms": //conversion to degrees
                    Digit = Math.Truncate(Result);
                    Result -= Digit;
                    string temp = Result.ToString();
                    for (int i = 2; i < temp.Length; i += 2)
                    {
                        if (temp.Length - i > 1)
                            Number = Math.Round(Convert.ToDouble(Result.ToString()[i] + Result.ToString()[i + 1]) * 60 / 100);
                        else
                            Number = Math.Round(Convert.ToDouble(Result.ToString()[i] + "0") * 60 / 100);
                        Digit += Number / Math.Pow(10, i);
                    }
                    Result = Digit;
                    break;
                    //beginning arc trigonometry func
                case "sinᵣˉ¹":
                    Result = Math.Asin(Result);
                    break;
                case "sinₒˉ¹":
                    Result = 1.0 / PiDivide180 * Math.Asin(Result);
                    break;
                case "sin₉ˉ¹":
                    Result = 1.0 / PiDivede200 * Math.Asin(Result);
                    break;
                case "cosᵣˉ¹":
                    Result = Math.Acos(Result);
                    break;
                case "cosₒˉ¹":
                    Result = 1.0 / PiDivide180 * Math.Acos(Result);
                    break;
                case "cos₉ˉ¹":
                    Result = 1.0 / PiDivede200 * Math.Acos(Result);
                    break;
                case "tanᵣˉ¹":
                    Result = Math.Atan(Result);
                    break;
                case "tanₒˉ¹":
                    Result = 1.0 / PiDivide180 * Math.Atan(Result);
                    break;
                case "tan₉ˉ¹":
                    Result = 1.0 / PiDivede200 * Math.Atan(Result);
                    break;
                    //ending arc trigonometry func
                case "1/":
                    Result = 1.0 / Result;
                    if (Double.IsInfinity(Result))
                        Stoper = true;
                    break;
                case "cube":
                    Result = Math.Pow(Result, 3.0);
                    break;
                    //hyperbolic trigonometry func
                case "sinh":
                    Result = Math.Sinh(Result);
                    break;
                case "cosh":
                    Result = Math.Cosh(Result);
                    break;
                case "tanh":
                    Result = Math.Tanh(Result);
                    break;
                    //arc hyperbolic trigonometry func
                case "sinhˉ¹":
                    Result = Math.Log(Result + Math.Sqrt(Math.Pow(Result, 2.0) + 1.0));
                    break;
                case "coshˉ¹":
                    if (Result >= 1.0)
                        Result = Math.Log(Result + Math.Sqrt(Math.Pow(Result, 2.0) - 1.0));
                    else
                        Stoper = true;
                    break;
                case "tanhˉ¹":
                    if (Result < 1.0 && Result > -1.0)
                        Result = 0.5 * Math.Log((1.0 + Result) / (1.0 - Result));
                    else
                        Stoper = true;
                    break;
            }
        }

        private void buttonSin_And_Arcsin_TextChanged(object sender, EventArgs e)
        {
            if (buttonSin_And_Arcsin.Text.Contains("hˉ¹"))
            {
                buttonSin_And_Arcsin.Font = new Font(buttonSin_And_Arcsin.Font.FontFamily, 12.5f);
                buttonCos_And_Arccos.Font = new Font(buttonCos_And_Arccos.Font.FontFamily, 12.5f);
                buttonTan_And_Arctan.Font = new Font(buttonTan_And_Arctan.Font.FontFamily, 12.5f);
            }
            else if (buttonSin_And_Arcsin.Text.Contains("ˉ¹"))
            {
                buttonSin_And_Arcsin.Font = new Font(buttonSin_And_Arcsin.Font.FontFamily, 14);
                buttonCos_And_Arccos.Font = new Font(buttonCos_And_Arccos.Font.FontFamily, 14);
                buttonTan_And_Arctan.Font = new Font(buttonTan_And_Arctan.Font.FontFamily, 14);
            }
            else if (buttonSin_And_Arcsin.Text.Contains("h"))
            {
                buttonSin_And_Arcsin.Font = new Font(buttonSin_And_Arcsin.Font.FontFamily, 15);
                buttonCos_And_Arccos.Font = new Font(buttonCos_And_Arccos.Font.FontFamily, 15);
                buttonTan_And_Arctan.Font = new Font(buttonTan_And_Arctan.Font.FontFamily, 15);
            }
            else
            {
                buttonSin_And_Arcsin.Font = new Font(buttonSin_And_Arcsin.Font.FontFamily, 16);
                buttonCos_And_Arccos.Font = new Font(buttonCos_And_Arccos.Font.FontFamily, 16);
                buttonTan_And_Arctan.Font = new Font(buttonTan_And_Arctan.Font.FontFamily, 16);
            }
        }

        Form2 f2 = new Form2();
        private void buttonM_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Text == "M")
            {
                f2.Show();
                f2.Location = Location;
                f2.Width = Width;
            }
            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Paste();
            Entering = true;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(LastUpdate, label1);
        }
    }
}
/* Здеся будут мысли и справочные мат-лы, т.к. чет калькулятор сложный какой-то
 * hyp - гиперболические тригонометрические функции(если шо, гуглить формулы(я их даже не проходил))
 * EXP(или EXF) ну тип представление числа в экс-ом формате, а если буквы P и F имеют разный смысл, то плакать
 * я боюсь скобок
 * градусы, радианы и ... что? Грады?
 * dms перевод десятичных чисел в градусы, минуты, секунды; degree - наоборот (я вот хз)
 * ща бы историю расчетов не забыть...
 * реинкарнация или копирайт? ну да
 * ммм кнопочки, ммм шрифт, пасеба за то что не надо бояться за выступы(нет)
 * дааа.. иконочка, ща изи вставлю......................................................да ну нафиг
 * делаю калькулятор, но больше работаю со строками, ничего не обычного 
 * 
 */
/* А тута будут номера ошибок
 * code 0 - некорректность введенных значений в textBox1 при обработке новой операции (OperatingSymbols_Click)
 * code 1 - некорректность введенных значений в textBox1 при изменении знака числа (buttonPlusAndMinus_Click)
 * code 2 - не возможность преобразовать решение в число (Solution)
 * code 3 - ошибка где-то в решении (Solution)
 * code 4
 * code 5
 */
