using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            //Form2 form2 = new Form2
            //{
            //    Location = new Point(form1.Location.X, form1.Location.Y - 6),
            //    Width = form1.Width

            //};
            //Location = new Point(form1.Location.X, form1.Location.Y - 200);
            Width = form1.Width;
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            Close();
        }
    }
}
