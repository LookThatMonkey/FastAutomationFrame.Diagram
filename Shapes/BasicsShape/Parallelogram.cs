//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :Parallelogram.cs
//        description :
//
//        created by 张恭亮 at  2020/10/21 17:03:05
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
	[ToolShape(DisPlayText = "平行四边形")]
	public class Parallelogram : ShapeBase
	{
		#region Proterties

		private int _offset = 10;

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
		public override string ShapeName => "Parallelogram";

		#endregion

		#region Constructor

		public Parallelogram() : base()
		{
			rectangle.Width = 100;
			rectangle.Height = 33;
		}

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public Parallelogram(DiagramControl s) : base(s)
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
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			Brush brush = new SolidBrush(BackGroundColor);
			Rectangle rect = new Rectangle(rectangle.X + point.X, rectangle.Y + point.Y, this.Width, this.Height);
			List<PointF> points = new List<PointF>();
			points.Add(new PointF(rectangle.X + point.X + (int)(_offset * Scale), rectangle.Y + point.Y));
			points.Add(new PointF(rectangle.X + point.X + this.Width, rectangle.Y + point.Y));
			points.Add(new PointF(rectangle.X + point.X + this.Width - (int)(_offset * Scale), rectangle.Y + point.Y + this.Height));
			points.Add(new PointF(rectangle.X + point.X, rectangle.Y + point.Y + this.Height));
			using (GraphicsPath graphicsPath = CreateRoundedRectanglePath(points))
			{
				g.FillPath(brush, graphicsPath);

				if (hovered || isSelected)
				{
					Pen p = new Pen(BoderSelectedColor, 2F);
					p.StartCap = LineCap.Round;
					p.EndCap = LineCap.Round;
					p.LineJoin = LineJoin.Round;
					g.DrawPath(p, graphicsPath);
				}
				else if (ShowBorder)
				{
					Pen p = new Pen(BoderColor);
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
