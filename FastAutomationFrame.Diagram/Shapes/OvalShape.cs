using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.Shapes
{
	public class OvalShape : ShapeBase
	{
		#region Fields
		/// <summary>
		/// holds the bottom connector
		/// </summary>
		protected Connector cBottom, cLeft, cRight, cTop;
		private int _borderLength = 20;
		private int _marginBottom = 5;
		#endregion

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

		protected override void SiteChanged()
		{
			cBottom = new Connector(new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Bottom));
			cBottom.Site = this.site;
			cBottom.Name = "Bottom connector";
			cBottom.ConnectorDirection = ConnectorDirection.Down;
			connectors.Add(cBottom);

			cLeft = new Connector(new Point(rectangle.Left, (int)(rectangle.Top + rectangle.Height / 2)));
			cLeft.Site = this.site;
			cLeft.Name = "Left connector";
			cLeft.ConnectorDirection = ConnectorDirection.Left;
			connectors.Add(cLeft);

			cRight = new Connector(new Point(rectangle.Right, (int)(rectangle.Top + rectangle.Height / 2)));
			cRight.Site = this.site;
			cRight.Name = "Right connector";
			cRight.ConnectorDirection = ConnectorDirection.Right;
			connectors.Add(cRight);

			cTop = new Connector(new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Top));
			cTop.Site = this.site;
			cTop.Name = "Top connector";
			cTop.ConnectorDirection = ConnectorDirection.Up;
			connectors.Add(cTop);
		}

		/// <summary>
		/// Tests whether the mouse hits this shape
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(System.Drawing.Point p)
		{
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			p.X = p.X - point.X;
			p.Y = p.Y - point.Y;

			Rectangle r = new Rectangle(p, new Size(5, 5));
			return rectangle.Contains(r);
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			Brush brush = new SolidBrush(BackGroundColor);
			g.FillEllipse(brush,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					rectangle.Width,
					rectangle.Height);

			if (hovered || isSelected)
			{
				Pen p = new Pen(BoderSelectedColor, 2F);
				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					rectangle.Width,
					rectangle.Height);
			}
			else if (ShowBorder)
			{
				Pen p = new Pen(BoderColor);

				g.DrawEllipse(p,
					rectangle.X + point.X,
					rectangle.Y + point.Y,
					rectangle.Width,
					rectangle.Height);
			}

			if (isSelected)
			{
				Point point1, point2, point3;
				Point[] pntArr;

				#region 上连接点

				point1 = new Point(
					(rectangle.Width - _borderLength) / 2 + rectangle.X + point.X,
					rectangle.Y + point.Y - _marginBottom);

				point2 = new Point(
					(rectangle.Width + _borderLength) / 2 + rectangle.X + point.X,
					rectangle.Y + point.Y - _marginBottom);

				point3 = new Point(
					rectangle.Width / 2 + rectangle.X + point.X,
					(int)((rectangle.Y + point.Y - _marginBottom) - _borderLength * Math.Sin(Math.PI / 3)));

				pntArr = new Point[] { point1, point2, point3 };
				g.FillPolygon(Brushes.Gold, pntArr);

				#endregion

				#region 下连接点

				point1 = new Point(
					(rectangle.Width - _borderLength) / 2 + rectangle.X + point.X,
					rectangle.Y + point.Y + rectangle.Height + _marginBottom);

				point2 = new Point(
					(rectangle.Width + _borderLength) / 2 + rectangle.X + point.X,
					rectangle.Y + point.Y + rectangle.Height + _marginBottom);

				point3 = new Point(
					rectangle.Width / 2 + rectangle.X + point.X,
					(int)((rectangle.Y + point.Y + rectangle.Height + _marginBottom) + _borderLength * Math.Sin(Math.PI / 3)));

				pntArr = new Point[] { point1, point2, point3 };
				g.FillPolygon(Brushes.Gold, pntArr);

				#endregion

				#region 右连接点

				point1 = new Point(
					rectangle.Width + rectangle.X + point.X + _marginBottom,
					(rectangle.Height - _borderLength) / 2 + rectangle.Y + point.Y);

				point2 = new Point(
					rectangle.Width + rectangle.X + point.X + _marginBottom,
					(rectangle.Height + _borderLength) / 2 + rectangle.Y + point.Y);

				point3 = new Point(
					(int)(rectangle.Width + rectangle.X + point.X + _marginBottom + _borderLength * Math.Sin(Math.PI / 3)),
					rectangle.Height / 2 + rectangle.Y + point.Y);

				pntArr = new Point[] { point1, point2, point3 };
				g.FillPolygon(Brushes.Gold, pntArr);

				#endregion

				#region 左连接点

				point1 = new Point(
					rectangle.X + point.X - _marginBottom,
					(rectangle.Height - _borderLength) / 2 + rectangle.Y + point.Y);

				point2 = new Point(
					rectangle.X + point.X - _marginBottom,
					(rectangle.Height + _borderLength) / 2 + rectangle.Y + point.Y);

				point3 = new Point(
					(int)(rectangle.X + point.X - _marginBottom - _borderLength * Math.Sin(Math.PI / 3)),
					rectangle.Height / 2 + rectangle.Y + point.Y);

				pntArr = new Point[] { point1, point2, point3 };
				g.FillPolygon(Brushes.Gold, pntArr);

				#endregion
			}

			for (int k = 0; k < connectors.Count; k++)
			{
				connectors[k].Paint(g);
			}

			if (text != string.Empty)
				g.DrawString(text, font, Brushes.Black, rectangle.X + point.X + 10, rectangle.Y + point.Y + 10);
		}

		public override Connector HitConnector(System.Drawing.Point p)
		{
			Point point0 = new Point(0, 0);
			if (this.site != null)
				point0 = this.site.ViewOriginPoint.GetPoint();

			Rectangle contentionRectangle;
			Rectangle r = new Rectangle(p, new Size(5, 5));

			#region 上

			contentionRectangle = new Rectangle(
				(rectangle.Width - _borderLength) / 2 + rectangle.X + point0.X,
				(int)((rectangle.Y + point0.Y - _marginBottom) - _borderLength * Math.Sin(Math.PI / 3)),
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				Point point = cTop.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				return base.HitConnector(point);
			}

			#endregion

			#region 下

			contentionRectangle = new Rectangle(
				(rectangle.Width - _borderLength) / 2 + rectangle.X + point0.X,
				rectangle.Y + point0.Y + rectangle.Height + _marginBottom,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				Point point = cBottom.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				return base.HitConnector(point);
			}

			#endregion

			#region 右

			contentionRectangle = new Rectangle(
				rectangle.Width + rectangle.X + point0.X + _marginBottom,
				(rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				Point point = cRight.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				return base.HitConnector(point);
			}

			#endregion

			#region 左

			contentionRectangle = new Rectangle(
				rectangle.X + point0.X - _marginBottom - _borderLength,
				(rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				Point point = cLeft.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				return base.HitConnector(point);
			}

			#endregion

			return base.HitConnector(p);
		}

		/// <summary>
		/// Invalidates the shape
		/// </summary>
		public override void Invalidate()
		{
			if (this.site == null) return;

			Point point = this.site.ViewOriginPoint.GetPoint();
			Rectangle r = rectangle;
			r.X += point.X;
			r.Y += point.Y;
			r.Offset(-5, -5);
			r.Inflate(20, 20);
			site.Invalidate(r);
		}
		public override void Resize(int width, int height)
		{
			base.Resize(width, height);

			if(cBottom != null)
				cBottom.Point = new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Bottom);
			if (cLeft != null)
				cLeft.Point = new Point(rectangle.Left, (int)(rectangle.Top + rectangle.Height / 2));
			if (cRight != null)
				cRight.Point = new Point(rectangle.Right, (int)(rectangle.Top + rectangle.Height / 2));
			if (cTop != null)
				cTop.Point = new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Top);
			Invalidate();
		}

		#endregion
	}
}
