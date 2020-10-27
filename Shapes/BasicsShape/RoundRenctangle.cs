//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :RoundRenctangle.cs
//        description :
//
//        created by 张恭亮 at  2020/10/21 15:30:52
//
//======================================================================

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
	[ToolShape(DisPlayText = "圆角矩形")]
	public class RoundRenctangle : ShapeBase
	{
		#region Proterties

		private int _cornerRadius = 10;

		[Browsable(true), Description("圆角率"), Category("Layout")]
		public int CornerRadius
		{
			get
			{
				return _cornerRadius;
			}
			set
			{
				_cornerRadius = value;
			}
		}

		[Browsable(false)]
		public override string ShapeName => "RoundRenctangle";

		#endregion

		#region Constructor

		public RoundRenctangle() : base()
		{
			this.Width = 100;
			this.Height = 33;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public RoundRenctangle(DiagramControl s) : base(s)
		{
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

			Brush brush = new SolidBrush(BackGroundColor);
			Rectangle rect = new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, rectangle.Width, rectangle.Height);
			using (GraphicsPath graphicsPath = CreateRoundedRectanglePath(rect, (int)(_cornerRadius * Scale)))
			{
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
					g.FillPath(brush, graphicsPath);
				}

				if (hovered || isSelected)
				{
					Pen p = new Pen(BorderSelectedColor, 2F);
					p.StartCap = LineCap.Round;
					p.EndCap = LineCap.Round;
					p.LineJoin = LineJoin.Round;
					g.DrawPath(p, graphicsPath);
				}
				else if (ShowBorder)
				{
					Pen p = new Pen(BorderColor);
					p.StartCap = LineCap.Round;
					p.EndCap = LineCap.Round;
					p.LineJoin = LineJoin.Round;
					g.DrawPath(p, graphicsPath);
				}
			}

			base.Paint(g);
		}

		private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
		{
			GraphicsPath roundedRect = new GraphicsPath();
			roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
			roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
			roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
			roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
			roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
			roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
			roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
			roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
			roundedRect.CloseFigure();
			return roundedRect;
		}
		#endregion
	}
}
