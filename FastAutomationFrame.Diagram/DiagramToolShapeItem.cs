using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastAutomationFrame.Diagram
{
    internal class DiagramToolShapeItem : Panel
    {
        #region Fields

        protected Font font = new Font("Verdana", 10F);

        #endregion

        #region Properties

        private int _height = 38;
        public new int Height
        {
            get
            {
                return _height;
            }
        }

        #endregion

        public ShapeBase Shape { get; set; }
        public string DisplayText { get; set; }

        public DiagramToolShapeItem(ShapeBase shape, string displayText)
            : base()
        {
            shape.Text = "";
            shape.Scale = shape.ToolBoxScale;
            shape.X = 5;
            shape.Y = (_height - shape.Height) / 2 + 10;
            base.Height = _height;
            Shape = shape;
            DisplayText = displayText;
            this.Dock = DockStyle.Top;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DragDropEffects dropEffect = this.DoDragDrop(new DragItem()
            {
                Shape = Shape
            },
                DragDropEffects.Copy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Shape != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Shape.Paint(e.Graphics);
                e.Graphics.DrawString(DisplayText, font, Brushes.Black, _height + 10, 10);
            }
        }
    }
    internal class DragItem
    {
        public ShapeBase Shape { get; set; }
    }
}
