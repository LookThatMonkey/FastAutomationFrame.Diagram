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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.流程1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.流程2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.流程3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.流程4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.仅隐藏单元ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏单元及连接线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.仅隐藏单元ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏单元及连接线ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.仅显示单元ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示单元及连接线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.仅显示单元ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.显示单元及连接线ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.diagramControl1 = new FastAutomationFrame.Diagram.DiagramControl();
            this.propertyGrid1 = new FastAutomationFrame.Diagram.PropertyGrid.PropertyGridCn();
            this.diagramToolBoxControl1 = new FastAutomationFrame.Diagram.DiagramToolBoxControl();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3,
            this.toolStripDropDownButton4,
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton7});
            this.toolStrip1.Location = new System.Drawing.Point(200, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(954, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.流程1ToolStripMenuItem,
            this.流程2ToolStripMenuItem,
            this.流程3ToolStripMenuItem,
            this.流程4ToolStripMenuItem});
            this.toolStripButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton1.Text = "数据流程";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // 流程1ToolStripMenuItem
            // 
            this.流程1ToolStripMenuItem.Name = "流程1ToolStripMenuItem";
            this.流程1ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.流程1ToolStripMenuItem.Text = "流程1";
            this.流程1ToolStripMenuItem.Click += new System.EventHandler(this.流程1ToolStripMenuItem_Click);
            // 
            // 流程2ToolStripMenuItem
            // 
            this.流程2ToolStripMenuItem.Name = "流程2ToolStripMenuItem";
            this.流程2ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.流程2ToolStripMenuItem.Text = "流程2";
            this.流程2ToolStripMenuItem.Click += new System.EventHandler(this.流程2ToolStripMenuItem_Click);
            // 
            // 流程3ToolStripMenuItem
            // 
            this.流程3ToolStripMenuItem.Name = "流程3ToolStripMenuItem";
            this.流程3ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.流程3ToolStripMenuItem.Text = "流程3";
            this.流程3ToolStripMenuItem.Click += new System.EventHandler(this.流程3ToolStripMenuItem_Click);
            // 
            // 流程4ToolStripMenuItem
            // 
            this.流程4ToolStripMenuItem.Name = "流程4ToolStripMenuItem";
            this.流程4ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.流程4ToolStripMenuItem.Text = "流程4";
            this.流程4ToolStripMenuItem.Click += new System.EventHandler(this.流程4ToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton2.Text = "导出";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton3.Text = "导入";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仅隐藏单元ToolStripMenuItem,
            this.隐藏单元及连接线ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(69, 22);
            this.toolStripDropDownButton1.Text = "隐藏单元";
            // 
            // 仅隐藏单元ToolStripMenuItem
            // 
            this.仅隐藏单元ToolStripMenuItem.Name = "仅隐藏单元ToolStripMenuItem";
            this.仅隐藏单元ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.仅隐藏单元ToolStripMenuItem.Text = "仅隐藏单元";
            this.仅隐藏单元ToolStripMenuItem.Click += new System.EventHandler(this.仅隐藏单元ToolStripMenuItem_Click);
            // 
            // 隐藏单元及连接线ToolStripMenuItem
            // 
            this.隐藏单元及连接线ToolStripMenuItem.Name = "隐藏单元及连接线ToolStripMenuItem";
            this.隐藏单元及连接线ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.隐藏单元及连接线ToolStripMenuItem.Text = "隐藏单元及连接线";
            this.隐藏单元及连接线ToolStripMenuItem.Click += new System.EventHandler(this.隐藏单元及连接线ToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仅隐藏单元ToolStripMenuItem1,
            this.隐藏单元及连接线ToolStripMenuItem1});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(117, 22);
            this.toolStripDropDownButton2.Text = "隐藏所选单元类别";
            // 
            // 仅隐藏单元ToolStripMenuItem1
            // 
            this.仅隐藏单元ToolStripMenuItem1.Name = "仅隐藏单元ToolStripMenuItem1";
            this.仅隐藏单元ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.仅隐藏单元ToolStripMenuItem1.Text = "仅隐藏单元";
            this.仅隐藏单元ToolStripMenuItem1.Click += new System.EventHandler(this.仅隐藏单元ToolStripMenuItem1_Click);
            // 
            // 隐藏单元及连接线ToolStripMenuItem1
            // 
            this.隐藏单元及连接线ToolStripMenuItem1.Name = "隐藏单元及连接线ToolStripMenuItem1";
            this.隐藏单元及连接线ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.隐藏单元及连接线ToolStripMenuItem1.Text = "隐藏单元及连接线";
            this.隐藏单元及连接线ToolStripMenuItem1.Click += new System.EventHandler(this.隐藏单元及连接线ToolStripMenuItem1_Click);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仅显示单元ToolStripMenuItem,
            this.显示单元及连接线ToolStripMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(69, 22);
            this.toolStripDropDownButton3.Text = "显示单元";
            // 
            // 仅显示单元ToolStripMenuItem
            // 
            this.仅显示单元ToolStripMenuItem.Name = "仅显示单元ToolStripMenuItem";
            this.仅显示单元ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.仅显示单元ToolStripMenuItem.Text = "仅显示单元";
            this.仅显示单元ToolStripMenuItem.Click += new System.EventHandler(this.仅显示单元ToolStripMenuItem_Click);
            // 
            // 显示单元及连接线ToolStripMenuItem
            // 
            this.显示单元及连接线ToolStripMenuItem.Name = "显示单元及连接线ToolStripMenuItem";
            this.显示单元及连接线ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.显示单元及连接线ToolStripMenuItem.Text = "显示单元及连接线";
            this.显示单元及连接线ToolStripMenuItem.Click += new System.EventHandler(this.显示单元及连接线ToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仅显示单元ToolStripMenuItem1,
            this.显示单元及连接线ToolStripMenuItem1});
            this.toolStripDropDownButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton4.Image")));
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(117, 22);
            this.toolStripDropDownButton4.Text = "显示所选单元类别";
            // 
            // 仅显示单元ToolStripMenuItem1
            // 
            this.仅显示单元ToolStripMenuItem1.Name = "仅显示单元ToolStripMenuItem1";
            this.仅显示单元ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.仅显示单元ToolStripMenuItem1.Text = "仅显示单元";
            this.仅显示单元ToolStripMenuItem1.Click += new System.EventHandler(this.仅显示单元ToolStripMenuItem1_Click);
            // 
            // 显示单元及连接线ToolStripMenuItem1
            // 
            this.显示单元及连接线ToolStripMenuItem1.Name = "显示单元及连接线ToolStripMenuItem1";
            this.显示单元及连接线ToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.显示单元及连接线ToolStripMenuItem1.Text = "显示单元及连接线";
            this.显示单元及连接线ToolStripMenuItem1.Click += new System.EventHandler(this.显示单元及连接线ToolStripMenuItem1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "组号";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton4.Text = "根据组号显示";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton5.Text = "根据组号隐藏";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton6.Text = "设计时";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BottomLineOffset = 0;
            this.diagramControl1.DiagramLocked = false;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.EnableEdit = false;
            this.diagramControl1.LineColor = System.Drawing.Color.Silver;
            this.diagramControl1.LineHoveredColor = System.Drawing.Color.Blue;
            this.diagramControl1.LineMouseWidth = 2F;
            this.diagramControl1.LineSelectedColor = System.Drawing.Color.Green;
            this.diagramControl1.LineWidth = 1F;
            this.diagramControl1.Location = new System.Drawing.Point(200, 25);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.ShowGrid = false;
            this.diagramControl1.Size = new System.Drawing.Size(954, 514);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.OnSelectElementChanged += new System.EventHandler<FastAutomationFrame.Diagram.SelectElementChangedEventArgs>(this.diagramControl1_OnSelectElementChanged);
            this.diagramControl1.OnEnableEditChanged += new System.EventHandler<System.EventArgs>(this.diagramControl1_OnEnableEditChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.BackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.CommandsBackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.CommandsForeColor = System.Drawing.Color.Gray;
            this.propertyGrid1.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGrid1.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(112)))), ((int)(((byte)(128)))), ((int)(((byte)(144)))));
            this.propertyGrid1.DisplayMode = FastAutomationFrame.Diagram.PropertyGrid.PropertyGridCn.DisplayModeEnum.ForAdvancedUser;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Enabled = false;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.HelpForeColor = System.Drawing.Color.SlateGray;
            this.propertyGrid1.LineColor = System.Drawing.Color.Gainsboro;
            this.propertyGrid1.Location = new System.Drawing.Point(1154, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(308, 539);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ViewForeColor = System.Drawing.Color.SlateGray;
            // 
            // diagramToolBoxControl1
            // 
            this.diagramToolBoxControl1.AutoScroll = true;
            this.diagramToolBoxControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.diagramToolBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.diagramToolBoxControl1.Name = "diagramToolBoxControl1";
            this.diagramToolBoxControl1.Size = new System.Drawing.Size(200, 539);
            this.diagramToolBoxControl1.TabIndex = 0;
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(60, 21);
            this.toolStripButton7.Text = "自动排列";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 539);
            this.Controls.Add(this.diagramControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.diagramToolBoxControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastAutomationFrame.Diagram.DiagramToolBoxControl diagramToolBoxControl1;
        private FastAutomationFrame.Diagram.DiagramControl diagramControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 仅隐藏单元ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏单元及连接线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem 仅隐藏单元ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 隐藏单元及连接线ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem 仅显示单元ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示单元及连接线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        private System.Windows.Forms.ToolStripMenuItem 仅显示单元ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 显示单元及连接线ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private FastAutomationFrame.Diagram.PropertyGrid.PropertyGridCn propertyGrid1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem 流程1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 流程2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 流程3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 流程4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
    }
}

