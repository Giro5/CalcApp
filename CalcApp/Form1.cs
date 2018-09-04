using System;
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

        public const string Version = "1.0.2", //версия
            LastUpdate = "LastUpdate:\n· A little change in a SolutionFunc case dms";//комментарии к версии

        double Result { get; set; } = 0;//для результата решения
        double Digit { get; set; } = 0;//первое число для нахождения ответа
        double Number { get; set; } = 0;//второе число для нахождения ответа
        double PossibleAnswer { get; set; }//может временно содержать некоторый ответ
        double PiDivide180 { get; } = 0.01745329251994329576923690768489d;//константа пи/180
        double PiDivede200 { get; } = 0.0157079632679489661923132169164d;//константа пи/200
        //double[] MNumbers = new double[0];

        string Operation { get; set; } = "";//при обработки операции, будет ее содержать
        string DRG { get; set; } = "DEG";//режим решения тригоном. фун

        string Mul { get; } = "×";//константа строки умножения
        string Plus { get; } = "+";//константа строки плюса
        string Minus { get; } = "-";//константа строки минуса
        string Dev { get; } = "÷";//константа строки деления

        char mul { get; } = '×';//константа символа умножения
        char plus { get; } = '+';//константа символа плюса
        char minus { get; } = '-';//константа символа минуса
        char dev { get; } = '÷';//константа символа деления

        bool Entering { get; set; } = false;//вводится ли в данный момент число
        bool Cleaner { get; set; } = false;//очищать ли при след. действии поле
        bool Stoper { get; set; } = false;//введены некорректные данные
        bool Adding { get; set; } = false;//добавлять к числу в экс-ом виде

        int CountOperations { get; set; } = 0;//кол-во операций
        int CountBrackets { get; set; } = 0;//кол-во скобок
        int Wid { get; set; }//ширина формы

        float MinWidText { get; } = 12f;//мин. размер поля для числа
        float MaxWidText { get; } = 32f;//макс. размер поля для числа

        private void Form1_Load(object sender, EventArgs e)
        {
            Wid = ClientSize.Width; //сохранение ширины окна
            label1.Text += " " + Version; //приписка версии к названию программы
            ChangeSizeForm(); //нормирование ширины главного textbox'а
            textBox2.Text = ""; //опустошение textbox'а с уравнением
            if(Screen.PrimaryScreen.Bounds.Size.Height < 1080 &&
                Screen.PrimaryScreen.Bounds.Size.Width < 1920) //проверка на разрешение монитора 
            {
                //если да, то возвращает минимальный размер формы
                Width = MinimumSize.Width;
                Height = MinimumSize.Height;
            }
            else
            {
                Width = MinimumSize.Width;
                Height = MinimumSize.Height;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ChangeSizeForm();//нормирование ширины главного textbox'а
            if (textBox1.Text == "Введены неверные данные" || textBox1.Text == "Деление на ноль невозможно")
            {//выключение некоторых кнопок при неверных исчислений
                buttonMplus.Enabled = false;
                buttonMR.Enabled = false;
                buttonDivide.Enabled = false;
                buttonMultiply.Enabled = false;
                buttonMinus.Enabled = false;
                buttonPlus.Enabled = false;
                buttonDot.Enabled = false;
                buttonSqr_And_Cube.Enabled = false;
                buttonSqrt_And_1divideX.Enabled = false;
                buttonPlusAndMinus.Enabled = false;
                buttonXdegreeY_And_YrootX.Enabled = false;
                buttonLn.Enabled = false;
                buttonTan_And_Arctan.Enabled = false;
                buttonCos_And_Arccos.Enabled = false;
                buttonSin_And_Arcsin.Enabled = false;
                buttonExp_And_dms.Enabled = false;
                button10degreeX_And_EdegreeX.Enabled = false;
                buttonFact_And_deg.Enabled = false;
                buttonCE.Enabled = false;
                buttonPi.Enabled = false;
                buttonMminus.Enabled = false;
                buttonInverse.Enabled = false;
                buttonEquals.Enabled = false;
                buttonLog.Enabled = false;
                buttonBackspace.Enabled = false;
                buttonLeftBracket.Enabled = false;
                buttonRightBracket.Enabled = false;
                buttonMS.Enabled = false;
                buttonMod.Enabled = false;
                buttonDRG.Enabled = false;
                buttonHYP.Enabled = false;
            }
            else
            {//включение - иначе
                buttonDivide.Enabled = true;
                buttonMultiply.Enabled = true;
                buttonMinus.Enabled = true;
                buttonPlus.Enabled = true;
                buttonDot.Enabled = true;
                buttonSqr_And_Cube.Enabled = true;
                buttonSqrt_And_1divideX.Enabled = true;
                buttonPlusAndMinus.Enabled = true;
                buttonXdegreeY_And_YrootX.Enabled = true;
                buttonLn.Enabled = true;
                buttonTan_And_Arctan.Enabled = true;
                buttonCos_And_Arccos.Enabled = true;
                buttonSin_And_Arcsin.Enabled = true;
                buttonExp_And_dms.Enabled = true;
                button10degreeX_And_EdegreeX.Enabled = true;
                buttonFact_And_deg.Enabled = true;
                buttonCE.Enabled = true;
                buttonPi.Enabled = true;
                buttonInverse.Enabled = true;
                buttonEquals.Enabled = true;
                buttonLog.Enabled = true;
                buttonBackspace.Enabled = true;
                buttonLeftBracket.Enabled = true;
                buttonRightBracket.Enabled = true;
                buttonMod.Enabled = true;
                buttonDRG.Enabled = true;
                buttonHYP.Enabled = true;
            }
        }

        private void buttonPlusAndMinus_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("e+") || textBox1.Text.Contains("e-") || textBox1.Text.Contains("E+") || textBox1.Text.Contains("E-"))
            {//изменение знака с экс-ым числом 
                int ie = 0;
                for (int i = 0; i < textBox1.Text.Length; i++)
                    if (textBox1.Text[i] == 'e')
                        ie = i;
                ie++;
                if (textBox1.Text[ie] == plus)
                    textBox1.Text = textBox1.Text.Replace(plus, minus);
                else
                    textBox1.Text = textBox1.Text.Replace(minus, plus);
            }
            else //изменение знака обычного числа
            {
                try
                {
                    textBox1.Text = (Convert.ToDouble(textBox1.Text) * (-1)).ToString();
                    Digit = Convert.ToDouble(textBox1.Text);
                }
                catch //ошибка при попытки изменить знака
                {
                    string tmp = textBox1.Text;
                    for (int i = 0; i < textBox1.Text.Length; i++) //попытка исправления ошибки
                    {
                        if ((!Char.IsDigit(textBox1.Text[i]) && textBox1.Text[i] != ',') && (i != 0 || textBox1.Text[i] != minus))
                        {
                            textBox1.Text = textBox1.Text.Remove(i, 1);
                            i--;
                        }
                    }
                    try //проверка исправлено ли
                    {
                        textBox1.Text = (Convert.ToDouble(textBox1.Text) * (-1)).ToString();
                        Digit = Convert.ToDouble(textBox1.Text);
                        MessageBox.Show("Code #1:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nSuccessful corrected.\nFor continued press OK.", "Error");
                    }
                    catch //если не исправленно
                    {
                        MessageBox.Show("Code #1:\n" + $"\"{tmp}\" - to - \"{textBox1.Text}\"" + "\n...\nFailed corrected.\nFor continued press OK.", "Error");
                        buttonCancel.PerformClick();
                    }
                }
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) //если кол-во символов больше нуля
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1); //удаляем последний символ
                if (textBox1.Text.Length > 0)//снова проверка на кол-во символов
                {
                    if (textBox1.Text.Last() == plus || (textBox1.Text.Last() == minus && textBox1.Text.Length > 1))//удаление при экс-ым числом
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 2);
                    else if (textBox1.Text.Last() == minus && textBox1.Text.Length == 1)//если последний символ минус - удалить его
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                }
            }
            if (textBox1.Text.Length == 0) //если в строке не осталось символов - онулирование
                buttonCE.PerformClick();
            ChangeSizeForm(); //нормирование ширины главного textbox'а
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //условия для нормирования главного textbox'а
            if (Wid < ClientSize.Width && textBox1.Size.Width > textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size < MaxWidText)
                textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 0.5f);
            if (Wid > ClientSize.Width && textBox1.Size.Width < textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size > MinWidText)
                textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 0.5f);
            Wid = ClientSize.Width; //сохранение ширины окна
            textBox1.TextAlign = HorizontalAlignment.Right; //обновление св-ва textbox'а
        }

        private void NumbersAndDot(object sender, EventArgs e)
        {
            Button b = (Button)sender; //экземпляр кнопки, которая была нажата
            if (Cleaner) //проверка на очистку
                buttonCancel.PerformClick();
            Cleaner = false;
            if (!Entering)//проверка вводится ли число, если нет - очистка
                textBox1.Text = "";
            if (Adding && textBox1.Text.Split('e')[1].Count() < 5) //добавление чисел при экс-ом числе
            {
                if (textBox1.Text.Last() == '0' && (textBox1.Text[textBox1.Text.Length - 2] == plus || textBox1.Text[textBox1.Text.Length - 2] == minus))
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                if (b.Text != ",")
                    textBox1.Text += b.Text;
            }
            else //обычные условия
            {
                if (b.Text == ",") //десятичная точка
                {
                    if (!textBox1.Text.Contains(",") && (textBox1.Text.Length != 0))
                        textBox1.Text += ",";
                    else if (textBox1.Text.Length == 0)
                        textBox1.Text = "0,";
                    Entering = true;
                }
                else if (b.Text == "0") //нуль
                {
                    if (textBox1.Text != "0")
                        textBox1.Text += "0";
                }
                else //обычное число
                {
                    textBox1.Text += b.Text;
                    Entering = true;
                }
            }
            ChangeSizeForm();//нормирование ширины главного textbox'а
        }

        private void OperatingSymbols_Click(object sender, EventArgs e)
        {
            Cleaner = false; //выключение уже не нужных свойств
            Adding = false;
            CountOperations++; //счетчик
            Button b = (Button)sender;
            if (b.Text == "xʸ") //записывание нажатой операции (всего их 7 (но это не точно))
                Operation = "^";
            else if (b.Text == "ʸ√x")
                Operation = "yroot";
            else
                Operation = b.Text;
            try //проверка введенного числа 
            {
                PossibleAnswer = Convert.ToDouble(textBox1.Text);
            }
            catch//если не число, то попытка исправить его
            {
                string tmp = textBox1.Text;
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    if ((!Char.IsDigit(textBox1.Text[i]) && textBox1.Text[i] != ',') && (i != 0 || textBox1.Text[i] != minus))
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
            if (!Entering && CountOperations > 1 && (textBox2.Text.Last() == mul || textBox2.Text.Last() == plus || textBox2.Text.Last() == minus || textBox2.Text.Last() == dev
               || textBox2.Text.Last() == 'd' || textBox2.Text.Last() == '^' || textBox2.Text.Last() == 't'))
            {//изменение операции -слеш- "заскобирование"
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
                    if(Operation == Mul || Operation == Dev || Operation == "^")
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
                textBox2.Text += Operation; //операция для закрытой скобки
            else if (textBox2.Text.Contains("(") && textBox2.Text[textBox2.Text.Length - 1] == '(')
                textBox2.Text += PossibleAnswer.ToString() + " " + Operation; //операция для начала открытой скобки
            else
                textBox2.Text += " " + PossibleAnswer.ToString() + " " + Operation; //обычная операция
            Entering = false;//обновление ввода числа
            if (CountOperations > 1) //отправка на решение уже введенного уравнения
            {
                Solution(textBox2.Text);
                if (Operation == Plus || Operation == Minus || Operation == "Mod")
                    textBox1.Text = Result.ToString();
            }
        }

        private void Solution(string text)
        {

            if (text.Length > 2 && (text[text.Length - 1] == mul || text[text.Length - 1] == plus || text[text.Length - 1] == minus || text[text.Length - 1] == dev || text[text.Length - 1] == '^'))
                text = text.Remove(text.Length - 2);
            else if (text.Length > 2 && text[text.Length - 1] == 'd')
                text = text.Remove(text.Length - 4);
            else if (text.Length > 2 && text[text.Length - 1] == 't')
                text = text.Remove(text.Length - 7);
            if (text.Length > 0 && text[0] == ' ')
                text = text.Remove(0, 1);
            //все что было сверху это форматирование/подготовка строки для решения
            try //если решать ничего не нужно
            {
                Result = Convert.ToDouble(text);
            }
            catch//решаем...
            {
                if (text.Contains("(")) //handler brackets
                {
                    int ibegin = 0, iend = 0, CountLeftBrackets = 0, CountRightBrackets = 0, oldiend = 0;//временные файлы
                    for (int i = 0; i < text.Length; i++)
                        if (text[i] == '(')
                            CountLeftBrackets++;//счетчик левых скобок
                    for (int i = 0; i < text.Length; i++)
                        if (text[i] == ')')
                            CountRightBrackets++;//счетчик правых скобок
                    if (CountLeftBrackets > CountRightBrackets)
                        text = text + new String(')', CountLeftBrackets - CountRightBrackets);//выравнивание скобок
                    MessageBox.Show(text);
                    for (int loop = 0; loop < CountLeftBrackets; loop++)//решение всех собок
                    //while(text.Contains("(m"))
                    {
                        for (int i = 0; i < text.Length; i++) //поиск последней левой скобки
                            if (text[i] == '(')
                                ibegin = i;
                        for (int j = ibegin + 1; j < text.Length; j++)//поиск первой правой скобки
                            if (text[j] == ')')
                            {
                                iend = j;
                                break;
                            }
                        //while (iend < ibegin)
                        //    for (int j = iend + 1; j < text.Length; j++)
                        //        if (text[j] == ')')
                        //        {
                        //            iend = j;
                        //            break;
                        //        }
                        oldiend = iend;//сохранение индекса для выхода из рекурсии
                        string tmp = text;
                        //
                        //code of shit
                        //
                        if (ibegin > 0 && text[ibegin - 1] != ' ')//получаем функцию
                        {
                            for (int i = ibegin - 1; i >= 0 && text[i] != ' '; i--)
                                ibegin--;
                            for (int j = ibegin + 1; j < text.Length; j++)
                                if (text[j] == ')')
                                    iend = j;
                            if (iend != text.Length - 1)
                                tmp = text.Remove(iend + 1);
                            tmp = tmp.Remove(0, ibegin);
                            MessageBox.Show(tmp + "| tmp");
                            SolutionFunc(tmp);//и отправляем ее на решение
                        }
                        else//получаем строку внитри скобок
                        {
                            if (oldiend <= text.Length - 1)
                                tmp = text.Remove(oldiend);
                            if (tmp[0] == '(')
                                tmp = tmp.Remove(0, ibegin + 1);
                            Solution(tmp);//и отправляем ее на решение(рекурсия)
                        }
                        CountBrackets--;//отсчитываем счетчик, т.к. решили скобку
                        text = text.Remove(ibegin, oldiend - ibegin + 1);//убираем решенную скобку 
                        text = text.Insert(ibegin, Result.ToString());//вставляем решение скобки
                    }
                }
                try
                {
                    for (int i = 0; i < text.Length; i++) //degree and(or) root
                    {
                        if ((text[i] == '^' && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == 'y' && text[i + 1] == 'r' && text[i + 2] == 'o' && text[i + 3] == 'o' && text[i + 4] == 't'))
                        { //решение степеней
                            int ibegin = i - 2, iend = i + 2;//создание начальных индексов
                            if (text[i] == 'y' && text[i + 1] == 'r' && text[i + 2] == 'o' && text[i + 3] == 'o' && text[i + 4] == 't')
                                iend += 4; 
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == minus); --ibegin)
                                tmp = text[ibegin].ToString() + tmp; //поиск левого числа, т.е. стоящего перед операцией
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();//поиск правого числа, т.е. стоящего после операции
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '^': //возведение в степень
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, Math.Pow(Digit, Number).ToString());
                                    break;
                                case 'y': //нахождение корня
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, Math.Pow(Digit, 1.0 / Number).ToString());
                                    break;
                            }
                            i = 0;//после решения оперции, поиск оперций происходит сначала
                        }
                    }
                    for (int i = 0; i < text.Length; i++) //multiplication and(or) dividing and(or) Mod
                    {
                        if (text[i] == mul || (text[i] == dev && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == 'M' && text[i + 1] == 'o' && text[i + 2] == 'd'))
                        {
                            int ibegin = i - 2, iend = i + 2;
                            if (text[i] == 'M' && text[i + 1] == 'o' && text[i + 2] == 'd')
                                iend += 2;
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == minus); --ibegin)
                                tmp = text[ibegin].ToString() + tmp;
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '×': //умножение
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit * Number).ToString());
                                    break;
                                case '÷': //деление
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit / Number).ToString());
                                    break;
                                case 'M': //остаток от деления
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit % Number).ToString());
                                    break;
                            }
                            i = 0;
                        }
                    }
                    for (int i = 0; i < text.Length; i++) //summation and(or) subtracting
                    {
                        if ((text[i] == plus && text[i - 1] == ' ' && text[i + 1] == ' ') || (text[i] == minus && text[i + 1] == ' '))
                        {
                            int ibegin = i - 2, iend = i + 2;
                            string tmp = null;
                            for (; ibegin >= 0 && (Char.IsDigit(text[ibegin]) || text[ibegin] == ',' || text[ibegin] == minus); --ibegin)
                                tmp = text[ibegin].ToString() + tmp;
                            ibegin++;
                            Digit = Convert.ToDouble(tmp);
                            tmp = null;
                            for (; iend <= text.Length - 1 && (Char.IsDigit(text[iend]) || text[iend] == ','); iend++)
                                tmp += text[iend].ToString();
                            Number = Convert.ToDouble(tmp);
                            switch (text[i])
                            {
                                case '+': //складывание
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit + Number).ToString());
                                    break;
                                case '-': //вычитание
                                    text = text.Remove(ibegin, iend - ibegin);
                                    text = text.Insert(ibegin, (Digit - Number).ToString());
                                    break;
                            }
                            i = 0;
                        }
                    }
                }
                catch //если же при решение случилась ошибка 
                {//она обрабатывается как нерешаемая
                    MessageBox.Show("Code #3:\n" + $@"""{text}""" + "\n...\nUnknown error\nFor continued press OK.", "Error");
                    buttonCancel.PerformClick();
                }
                try//попытка принять решение, если удалось, то оно записывается в Result
                {
                    text = text.Replace('.', ',');
                    Result = Convert.ToDouble(text);
                }
                catch//что-то пошло не так
                {
                    try//попытка исправить
                    {
                        Solution(text);
                        MessageBox.Show("Code #2:\n" + $@"""{text}""" + "\n...\nSuccessful corrected.\nFor continued press OK.", "Error");
                    }
                    catch//обработка неудачи
                    {
                        MessageBox.Show("Code #2:\n" + $@"""{text}""" + "\n...\nFailed corrected.\nFor continued press OK.", "Error");
                        buttonCancel.PerformClick();
                    }
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {//кнопочка удаления всего
            buttonCE.PerformClick();
            textBox1.Text = "0";
            textBox2.Text = "";
            Result = 0;
            CountOperations = 0;
            Operation = "";
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {//остановка действий и очиска поля
            textBox1.Text = "0";
            Entering = false;
            Adding = false;
            ChangeSizeForm();
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            Cleaner = false;
            Adding = false;
            try//попытка получить последнюю операцию и число
            {//для повторного вычисления
                if (textBox2.Text.Last() != 'd')
                    Operation = textBox2.Text.Last().ToString();
                PossibleAnswer = Convert.ToDouble(textBox1.Text);
            }
            catch { }
            if(textBox1.Text.Contains("e"))//представления экс-ого числа в обычном виде
            {
                try
                {
                    PossibleAnswer = Convert.ToDouble(textBox1.Text);
                }
                catch { }
            }
            //решения/вычисления
            if (CountOperations == 0 && Operation != "" && !(textBox2.Text.Contains(")") || textBox2.Text.Contains("(")))
                Solution(textBox1.Text + " " + Operation + " " + PossibleAnswer.ToString()); //повторное вычисление
            else if (textBox2.Text.Length > 2 && textBox2.Text[textBox2.Text.Length - 2] != ')')
                Solution(textBox2.Text + " " + PossibleAnswer.ToString());//обычное вычисление
            else if (CountOperations == 0 && (textBox2.Text.Length == 0 || textBox2.Text.Contains("(")))
                Result = Convert.ToDouble(textBox1.Text);//уже решенно
            else
                Solution(textBox2.Text);//решение строки с уравнением

            if (Double.IsInfinity(Result))//проверка на бесконечность
            {
                textBox1.Text = "Деление на ноль невозможно";
                Cleaner = true;
            }
            else//вставка решения
            {
                textBox1.Text = Result.ToString();
                textBox2.Text = "";
            }
            Entering = false;//завершение действий
            CountOperations = 0;
            ChangeSizeForm();
        }

        private void buttonEscape_Click(object sender, EventArgs e)
        {
            Application.Exit(); //выход
        }

        private Point movestart;//точка для сохранения коорд. мыши
        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//при зажатой ЛКМ на верхней панели 
            {
                movestart = new Point(e.X, e.Y);//получаем ее координаты
            }
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {//перемещаем окно за мышью
                Point deltaPos = new Point(e.X - movestart.X, e.Y - movestart.Y);
                Location = new Point(Location.X + deltaPos.X, Location.Y + deltaPos.Y);
            }
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;//сворачивание окна
        }

        private void buttonMaximaze_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)//разворачивание на весь экран
            {
                WindowState = FormWindowState.Maximized;
                buttonMaximaze.Image = Properties.Resources.icons8_Virtual_Machine_20px_1;
            }
            else//нормализация окна
            {
                WindowState = FormWindowState.Normal;
                buttonMaximaze.Image = Properties.Resources.рамка1;
            }
            ChangeSizeForm();
        }

        private void ChangeSizeForm()
        {//регулирование размера строки в поле 
            for(int i = 0; i < 20; i++)
            {
                if (textBox1.Size.Width > textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size < MaxWidText)
                    textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size + 0.5f);//увеличение
                if (textBox1.Size.Width < textBox1.Font.Size * textBox1.Text.Length && textBox1.Font.Size > MinWidText)
                    textBox1.Font = new Font(textBox1.Font.FontFamily, textBox1.Font.Size - 0.5f);//уменьшение
            }
        }

        private void buttonPi_Click(object sender, EventArgs e)
        {//вставка числа пи
            textBox1.Text = Math.PI.ToString();
            Entering = true;
        }

        private void buttonLeftBracket_Click(object sender, EventArgs e)
        {//открытие скобок
            if (CountBrackets <= 25)
            {
                if (textBox2.Text != "")//если поле с уравнениями не пустое
                {
                    if (textBox2.Text.Last() == mul || textBox2.Text.Last() == plus || textBox2.Text.Last() == minus || textBox2.Text.Last() == dev
                   || textBox2.Text.Last() == 'd' || textBox2.Text.Last() == '^' || textBox2.Text.Last() == 't')
                    {
                        //если есть операция - вставки скобки
                        textBox2.Text += " (";
                        CountBrackets++;
                    }
                    else if (textBox2.Text.Last() == '(')
                    {
                        {//если есть bracket - вставки скобки
                            textBox2.Text += " (";
                            CountBrackets++;
                        }
                    }
                    else//значит было решение без операций
                    {//очистка поля и вставка скобки
                        textBox2.Text = "";
                        buttonLeftBracket.PerformClick();
                        CountBrackets = 1;
                    }
                }
                else //ставим скобку
                {
                    textBox2.Text += " (";
                    CountBrackets++;
                }
            }
        }

        private void buttonRightBracket_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains("("))//если левые скобки
            {
                int clb = 0, crb = 0;
                for(int i = 0; i < textBox2.Text.Length; i++)
                {
                    if (textBox2.Text[i] == '(')
                        clb++;
                    else if (textBox2.Text[i] == ')')
                        crb++;
                }
                if (clb > crb)//закрывает скобку только когда есть, что закрыть
                {
                    if (textBox2.Text.Contains("(") && textBox2.Text[textBox2.Text.Length - 1] == '(')
                        textBox2.Text += textBox1.Text + ") ";
                    else if (textBox2.Text.Last() == ' ')
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1) + ") ";
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
        {//переключатель режима решения тригоном. фун.
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
        {//вкл./выкл. режим гиперболических фун.
            Button b = (Button)sender; 
            b.FlatAppearance.BorderColor = Color.White;
            if (b.FlatAppearance.BorderSize == 0)
                b.FlatAppearance.BorderSize = 2;
            else
                b.FlatAppearance.BorderSize = 0;
            RefreshButtonTrigonometry();
        }

        private void RefreshButtonTrigonometry()
        {//обновляет названия кнопок с тригонометрией
            if (buttonHYP.FlatAppearance.BorderSize != 0)
            {
                if (buttonInverse.FlatAppearance.BorderSize != 0)
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
                if (buttonInverse.FlatAppearance.BorderSize != 0)
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
        {//меняет раскладки клавиатуры функций
            Button b = (Button)sender;
            b.FlatAppearance.BorderColor = Color.White;
            if (b.FlatAppearance.BorderSize == 0)
            {
                b.FlatAppearance.BorderSize = 2;
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
        {//при клике на фун.
            Cleaner = false;
            Button b = (Button)sender;
            string str = null;
            //list of funcs
            switch (b.Text)//списочек который определяет функцию
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
                //case "xʸ": break; - их тут нет потому, что по-моей логики они операции
                //case "ꙷ√x": break; - оставил тут, как знак того, что я их помню
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
            {//временное решения фун., а точнее работа со скобками 2
                if (textBox2.Text.Length > 3 && textBox2.Text[textBox2.Text.Length - 2] == ')')
                {//т.е. вот это все происходит по вине того, что скобка есть в поле уравнения
                    string text = textBox2.Text;
                    int CountLeftBrackets = 0, CountRightBrackets = 0, ibegin = 0, iend = 0;
                    // ((( )))   ( )( )  (( ) ( )) - виды возможных скобок 
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
                else//если скобок небыло
                {
                    str += "(" + textBox1.Text + ")";
                    textBox2.Text += " " + str + " ";
                }
                SolutionFunc(str);//отправка на решение
                if (Stoper)
                {//ну там видно что это handler of exceptions
                    buttonCE.PerformClick();
                    textBox1.Text = "Введены неверные данные";
                    Stoper = false;
                    Cleaner = true;
                }
                else//если все хорошо с решением
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
        {//решение функций(хорошо что я решил это делать в отдельном методе, а то в Solution'е итак много всякого) 
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
            Solution(tmp);//рекурсия ^^
            string func = text.Remove(ibegin);
            //все что вверху было, да-да это обработка функции для решения
            switch (func)
            {//решение огромного кол-во фун.
                case "fact"://факториал
                    for (double i = Result - 1.0; i > 1.0; i--)
                        Result *= i;
                    break;
                case "10^"://10 в вашей степени (много не ставить, а то даже кальк винды ругается)
                    Result = Math.Pow(10.0, Result);
                    if (Double.IsInfinity(Result))
                        Stoper = true;
                    break;
                    //beginning trigonometry func
                case "sinᵣ":
                    Result *= 1.0 / PiDivide180;
                    Result = Math.Round(Result);
                    goto case "sinₒ";//мое гениальное решение проблемы табличных значений
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
                case "√"://квадратный корень
                    if (Result >= 0)
                        Result = Math.Sqrt(Result);
                    else
                        Stoper = true;
                    break;
                case "sqr"://квадрат
                    Result = Math.Pow(Result, 2.0);
                    break;
                case "log"://десятичный логарифм
                    if (Result > 0)
                        Result = Math.Log10(Result);
                    else
                        Stoper = true;
                    break;
                case "ln"://натуральный логарифм
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
                case "dms": //conversion to degrees
                    Digit = Math.Truncate(Result);
                    Result -= Digit;
                    string temp = Result.ToString(), temp1 = temp.Remove(3);
                    double tmp1 = Convert.ToDouble(temp), tmp2 = Convert.ToDouble(temp1), tmp3 = tmp1 - tmp2;
                    tmp2 *= 0.6;
                    tmp3 *= 0.36;
                    Result = Digit + tmp2 + tmp3;
                    break;
                case "e^"://возведение в экспаненту
                    Result = Math.Exp(Result);
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
                case "1/"://один делите на, что хотите(кроме нуля)
                    Result = 1.0 / Result;
                    if (Double.IsInfinity(Result))
                        Stoper = true;
                    break;
                case "cube"://куб
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
        {//штука изменяет размеры шрифта(это даже не смешно(это больно))
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

        //Form2 f2 = new Form2();
        //private void buttonM_Click(object sender, EventArgs e)
        //{
        //    Button b = (Button)sender;
        //    if (b.Text == "M")
        //    {
        //        f2.Show();
        //        f2.Location = Location;
        //        f2.Width = Width;
        //    }

        //}

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {//копирует в поле числа
            textBox1.SelectAll();
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {//вставляет в поле числа
            string tmp = textBox1.Text;
            textBox1.Text = "";
            textBox1.Paste();
            for (int i = 0; i < textBox1.Text.Length; i++)
            {//если вы не правильно откуда-то скопировали, тут это попробуют утрести
                if ((!Char.IsDigit(textBox1.Text[i]) && textBox1.Text[i] != ',') && (i != 0 || textBox1.Text[i] != minus))
                {
                    textBox1.Text = textBox1.Text.Remove(i, 1);
                    i--;
                }
            }
            try
            {
                PossibleAnswer = Convert.ToDouble(textBox1.Text);
                textBox1.Text = PossibleAnswer.ToString();
                Entering = true;
            }
            catch
            {//ну или не попробуют
                textBox1.Text = tmp;
            }
            ChangeSizeForm();
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            if (menuStrip1.Visible == false)
                menuStrip1.Visible = true;
            else
                menuStrip1.Visible = false;
        }

        private void buttonMenuChange_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem b = (ToolStripMenuItem)sender;
            labelMode.Text = b.Text;
            menuStrip1.Visible = false;
        }

        private void menuStrip1_MouseLeave(object sender, EventArgs e)
        {
            menuStrip1.Visible = false;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {//показывает апдейтики при наведении на версию
            toolTip1.Show(LastUpdate, label1);
        }
    }
}
/* Здесь будут мысли и справочные мат-лы, т.к. что-то калькулятор сложный какой-то
 * hyp - гиперболические тригонометрические функции(если что, гуглить формулы(я их даже не проходил))
 * EXP(или EXF) ну тип представление числа в экс-ом формате, а если буквы P и F имеют разный смысл, то плакать
 * я боюсь скобок
 * градусы, радианы и ... что? Грады?
 * dms перевод десятичных чисел в градусы, минуты, секунды; degree - наоборот (я вот хз)
 * сейчас бы историю расчетов не забыть...
 * реинкарнация или копирайт? ну да
 * ммм кнопочки, ммм шрифт, спасибо за то что не надо бояться за выступы(нет)
 * дааа.. иконочка, ща легко вставлю......................................................да ну на...
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
