//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :Step.cs
//        description :
//
//        created by 张恭亮 at  2020/10/21 15:58:53
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
	[ToolShape(DisPlayText = "步骤")]
	public class Step : ShapeBase
	{
		#region Proterties

		private int _offset = 20;

		[Browsable(true), Description("偏移"), Category("Layout")]
		public int Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				_offset = value;
			}
		}

		[Browsable(false)]
		public override string ShapeName => "Step";

		#endregion

		#region Constructor

		public Step() : base()
		{
			this.Width = 100;
			this.Height = 33;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public Step(DiagramControl s) : base(s)
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
			List<PointF> points = new List<PointF>();
			points.Add(new PointF(rectangle.X + point.X + (int)(_offset * Scale), rectangle.Y + point.Y + rectangle.Height / 2));
			points.Add(new PointF(rectangle.X + point.X, rectangle.Y + point.Y));
			points.Add(new PointF(rectangle.X + point.X + rectangle.Width - (int)(_offset * Scale), rectangle.Y + point.Y));
			points.Add(new PointF(rectangle.X + point.X + rectangle.Width, rectangle.Y + point.Y + rectangle.Height / 2));
			points.Add(new PointF(rectangle.X + point.X + rectangle.Width - (int)(_offset * Scale), rectangle.Y + point.Y + rectangle.Height));
			points.Add(new PointF(rectangle.X + point.X, rectangle.Y + point.Y + rectangle.Height));
			using (GraphicsPath graphicsPath = CreateRoundedRectanglePath(points))
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

		private GraphicsPath CreateRoundedRectanglePath(List<PointF> points)
		{
			GraphicsPath roundedRect = new GraphicsPath();
			for (int i = 0; i < points.Count - 1; i++)
			{
				roundedRect.AddLine(points[i], points[i + 1]);
			}

			roundedRect.AddLine(points[points.Count - 1], points[0]);
			roundedRect.CloseFigure();
			return roundedRect;
		}
		#endregion
	}
}
