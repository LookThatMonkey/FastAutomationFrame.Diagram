using FastAutomationFrame.Diagram;
using FastAutomationFrame.Diagram.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiagramDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void diagramControl1_OnSelectElementChanged(object sender, FastAutomationFrame.Diagram.SelectElementChangedEventArgs e)
        {
            PropertyGridConfig.SetFormType<Form2>();
            this.propertyGrid1.SelectedObject = sender;
            if (sender is ShapeBase)
            {
                (sender as ShapeBase).Click -= Form1_Click;
                (sender as ShapeBase).Click += Form1_Click;
                (sender as ShapeBase).DoubleClick -= Form1_DoubleClick;
                (sender as ShapeBase).DoubleClick += Form1_DoubleClick;
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            this.Text = (sender as ShapeBase).Text + " 双击" + (sender as ShapeBase).Content;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.Text = (sender as ShapeBase).Text + " 单击";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "流程文件（*.dg）|*.dg";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.diagramControl1.Save(sfd.FileName);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "流程文件（*.dg）|*.dg";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string msg;
                if (this.diagramControl1.Import(openFileDialog.FileName, out msg))
                {
                    MessageBox.Show(msg);
                }
            }
        }

        private void 仅隐藏单元ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Hide(diagramControl1.SelectShape, false);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 隐藏单元及连接线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Hide(diagramControl1.SelectShape);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 仅隐藏单元ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Hide(diagramControl1.SelectShape.GetType(), false);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 隐藏单元及连接线ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Hide(diagramControl1.SelectShape.GetType());
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 仅显示单元ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Show(diagramControl1.SelectShape, false);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 显示单元及连接线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Show(diagramControl1.SelectShape);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 仅显示单元ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Show(diagramControl1.SelectShape.GetType(), false);
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void 显示单元及连接线ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape != null)
            {
                diagramControl1.Show(diagramControl1.SelectShape.GetType());
            }
            else
            {
                MessageBox.Show("请选中单元！");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            diagramControl1.Show(this.toolStripTextBox1.Text);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            diagramControl1.Hide(this.toolStripTextBox1.Text);
        }

        private void 流程1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diagramControl1.Clear();
            if (this.diagramControl1.Import("1.dg", out string msg))
            {
                MessageBox.Show(msg);
            }
        }

        private void 流程2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diagramControl1.Clear();
            if (this.diagramControl1.Import("2.dg", out string msg))
            {
                MessageBox.Show(msg);
            }
        }

        private void 流程3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diagramControl1.Clear();
            if (this.diagramControl1.Import("3.dg", out string msg))
            {
                MessageBox.Show(msg);
            }
        }

        private void 流程4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diagramControl1.Clear();
            if (this.diagramControl1.Import("4.dg", out string msg, true, true))
            {
                MessageBox.Show(msg);
            }
        }

        private void diagramControl1_OnEnableEditChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.Enabled = diagramControl1.EnableEdit;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            diagramControl1.EnableEdit = true;
        }

        /// <summary>
        /// 自动排列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (diagramControl1.SelectShape == null)
            {
                MessageBox.Show("请选择第一个节点！");
            }
            else
            {
                diagramControl1.Sort(diagramControl1.SelectShape);
            }
        }
    }
}
