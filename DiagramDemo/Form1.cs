//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :Form1.cs
//        description :
//
//        created by 张恭亮 at  2020/9/22 10:55:28
//
//======================================================================

using FastAutomationFrame.Diagram.Shapes;
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
            this.propertyGrid1.SelectedObject = sender;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            #region Creation of a sample diagram

            #region The shapes

            SimpleRectangle ent = new SimpleRectangle();
            this.diagramControl1.AddShape(ent);
            ent.X = 82;
            ent.Y = 80;
            ent.Text = "Entity";
            ent.Height = 33;
            ent.BackGroundColor = Color.SteelBlue;

            SimpleRectangle conn = new SimpleRectangle();
            this.diagramControl1.AddShape(conn);
            conn.X = -73;
            conn.Y = 219;
            conn.Text = "Connection";
            conn.Height = 33;
            conn.BackGroundColor = Color.LightSteelBlue;

            SimpleRectangle shbase = new SimpleRectangle();
            this.diagramControl1.AddShape(shbase);
            shbase.X = 82;
            shbase.Y = 219;
            shbase.Text = "ShapeBase";
            shbase.Height = 33;
            shbase.BackGroundColor = Color.LightSteelBlue;

            SimpleRectangle con = new SimpleRectangle();
            this.diagramControl1.AddShape(con);
            con.X = 247;
            con.Y = 219;
            con.Text = "Connector";
            con.Height = 33;
            con.BackGroundColor = Color.LightSteelBlue;

            OvalShape oval = new OvalShape();
            this.diagramControl1.AddShape(oval);
            oval.X = -89;
            oval.Y = 376;
            oval.Text = "Oval";
            oval.Height = 33;
            oval.BackGroundColor = Color.AliceBlue;

            OvalShape rec = new OvalShape();
            this.diagramControl1.AddShape(rec);
            rec.X = 32;
            rec.Y = 376;
            rec.Text = "SimpleRectangle";
            rec.Height = 33;
            rec.Width = 150;
            rec.BackGroundColor = Color.AliceBlue;

            OvalShape tl = new OvalShape();
            this.diagramControl1.AddShape(tl);
            tl.X = 207;
            tl.Y = 376;
            tl.Text = "TextLabel";
            tl.Height = 33;
            tl.BackGroundColor = Color.AliceBlue;

            #endregion

            #region The connections

            this.diagramControl1.AddConnection(ent.Connectors[0], conn.Connectors[3]);
            this.diagramControl1.AddConnection(ent.Connectors[0], con.Connectors[3]);
            this.diagramControl1.AddConnection(ent.Connectors[0], shbase.Connectors[3]);
            this.diagramControl1.AddConnection(shbase.Connectors[0], oval.Connectors[3]);
            this.diagramControl1.AddConnection(shbase.Connectors[0], rec.Connectors[3]);
            this.diagramControl1.AddConnection(shbase.Connectors[0], tl.Connectors[3]);

            #endregion

            #endregion
        }
    }
}
