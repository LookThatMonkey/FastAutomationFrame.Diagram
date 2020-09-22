//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :Form1.Desinger.cs
//        description :
//
//        created by 张恭亮 at  2020/9/22 10:55:28
//
//======================================================================

namespace DiagramDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.diagramControl1 = new FastAutomationFrame.Diagram.DiagramControl();
            this.diagramToolBoxControl1 = new FastAutomationFrame.Diagram.DiagramToolBoxControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.BackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.CommandsBackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.CommandsForeColor = System.Drawing.Color.Gray;
            this.propertyGrid1.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(112)))), ((int)(((byte)(128)))), ((int)(((byte)(144)))));
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.HelpForeColor = System.Drawing.Color.SlateGray;
            this.propertyGrid1.LineColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.Location = new System.Drawing.Point(632, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(168, 450);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ViewForeColor = System.Drawing.Color.SlateGray;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.LineColor = System.Drawing.Color.Silver;
            this.diagramControl1.LineHoveredColor = System.Drawing.Color.Blue;
            this.diagramControl1.LineSelectedColor = System.Drawing.Color.Green;
            this.diagramControl1.Location = new System.Drawing.Point(200, 25);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.ShowGrid = true;
            this.diagramControl1.Size = new System.Drawing.Size(432, 425);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.OnSelectElementChanged += new System.EventHandler<FastAutomationFrame.Diagram.SelectElementChangedEventArgs>(this.diagramControl1_OnSelectElementChanged);
            // 
            // diagramToolBoxControl1
            // 
            this.diagramToolBoxControl1.AutoScroll = true;
            this.diagramToolBoxControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.diagramToolBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.diagramToolBoxControl1.Name = "diagramToolBoxControl1";
            this.diagramToolBoxControl1.Size = new System.Drawing.Size(200, 450);
            this.diagramToolBoxControl1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(200, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(432, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton1.Text = "数据加载";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.diagramControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.diagramToolBoxControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastAutomationFrame.Diagram.DiagramToolBoxControl diagramToolBoxControl1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private FastAutomationFrame.Diagram.DiagramControl diagramControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

