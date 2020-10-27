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
    public partial class PropertyGridBaseForm : Form
    {
        public virtual object Dval { get; set; }
        public PropertyGridBaseForm()
        {
            InitializeComponent();
        }

    }
}
