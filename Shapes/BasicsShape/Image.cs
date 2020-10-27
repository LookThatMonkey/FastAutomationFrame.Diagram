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
	[ToolShape(DisPlayText = "图片")]
	public class Image : ShapeBase
	{
		#region Proterties

		[Browsable(false)]
		public override string ShapeName => "Image";

		#endregion

		#region Constructor

		public Image() : base()
		{
			this.Width = 50;
			this.Height = 50;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public Image(DiagramControl s) : base(s)
		{
			this.Width = 50;
			this.Height = 50;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			if (this.HideItem) return;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			if (Image != null)
			{
				g.DrawImage(Image,
						rectangle.X + point.X,
						rectangle.Y + point.Y,
						rectangle.Width,
						rectangle.Height); //在窗口的画布中绘画出内存中的图像
			}
			else
			{
				g.DrawImage(global::BasicsShape.Properties.Resources.城市,
						rectangle.X + point.X,
						rectangle.Y + point.Y,
						rectangle.Width,
						rectangle.Height); //在窗口的画布中绘画出内存中的图像
			}

			if (hovered || isSelected)
			{
				Pen p = new Pen(BorderSelectedColor, 2F);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					rectangle.Width,
					rectangle.Height);
			}
			else if (ShowBorder)
			{
				Pen p = new Pen(BorderColor);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					rectangle.Width,
					rectangle.Height);
			}


			base.Paint(g);
		}

		#endregion
	}
}
