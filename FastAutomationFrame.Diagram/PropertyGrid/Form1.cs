using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    public partial class Form1 : PropertyGridBaseForm
    {
        public override object Dval { get; set; }
        public Form1(object dval)
        {
            InitializeComponent();
            if (dval != null)
                this.textBox1.Text = dval.ToString();
            this.Dval = dval;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Dval = this.textBox1.Text;
        }
    }
}
