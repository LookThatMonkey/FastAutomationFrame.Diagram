using FastAutomationFrame.Diagram;
using FastAutomationFrame.Diagram.FAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicsShape
{
	[ToolShape(DisPlayText = "椭圆形")]
	public class OvalShape : ShapeBase
	{
		#region Proterties

		[Browsable(false)]
		public override string ShapeName => "OvalShape";

		#endregion

		#region Constructor

		public OvalShape() : base()
		{
			rectangle.Width = 100;
			rectangle.Height = 33;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public OvalShape(DiagramControl s) : base(s)
		{
			rectangle.Width = 100;
			rectangle.Height = 33;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			Brush brush = new SolidBrush(BackGroundColor);
			g.FillEllipse(brush,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					this.Width,
					this.Height);

			if (hovered || isSelected)
			{
				Pen p = new Pen(BoderSelectedColor, 2F);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					this.Width,
					this.Height);
			}
			else if (ShowBorder)
			{
				Pen p = new Pen(BoderColor);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					this.Width,
					this.Height);
			}

			base.Paint(g);
		}

		#endregion
	}
}
