using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.Shapes
{
	public class TextLabel : ShapeBase
	{
		#region Fields


		#endregion

		#region Proterties

		[Browsable(false)]
		public override string ShapeName => "TextLabel";

		#endregion

		#region Constructor

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public TextLabel(DiagramControl s) : base(s)
		{
			this.backGroundColor = Color.Transparent;
		}

		public TextLabel() : base()
		{
			this.backGroundColor = Color.Transparent;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Tests whether the mouse hits this shape
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(System.Drawing.Point p)
		{
			p.X = p.X - this.site.ViewOriginPoint.GetPoint().X;
			p.Y = p.Y - this.site.ViewOriginPoint.GetPoint().Y;

			Rectangle r = new Rectangle(p, new Size(5, 5));
			return rectangle.Contains(r);
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			Brush brush = new SolidBrush(BackGroundColor);
			g.FillRectangle(brush,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
					rectangle.Width,
					rectangle.Height);

			if (hovered || isSelected)
			{
				Pen p = new Pen(BoderSelectedColor, 2F);
				g.DrawRectangle(p,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
					rectangle.Width,
					rectangle.Height);
			}
			else if (ShowBorder)
			{
				Pen p = new Pen(BoderColor);
				g.DrawRectangle(p,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
					rectangle.Width,
					rectangle.Height);
			}

			if (text != string.Empty)
				g.DrawString(text, font, Brushes.Black, rectangle.X + this.site.ViewOriginPoint.GetPoint().X + 10, rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y + 10);
		}

		/// <summary>
		/// Invalidates the shape
		/// </summary>
		public override void Invalidate()
		{
			Rectangle r = rectangle;
			r.X += this.site.ViewOriginPoint.GetPoint().X;
			r.Y += this.site.ViewOriginPoint.GetPoint().Y;
			r.Offset(-5, -5);
			r.Inflate(20, 20);
			site.Invalidate(r);
		}
		public override void Resize(int width, int height)
		{
			base.Resize(width, height);
			Invalidate();
		}

		#endregion
	}
}
