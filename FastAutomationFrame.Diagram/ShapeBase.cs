//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :ShapeBase.cs
//        description :
//
//        created by 张恭亮 at  2020/9/22 10:55:28
//
//======================================================================

using FastAutomationFrame.Diagram.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram
{
	public class ShapeBase : Entity
	{
		#region Fields

		private int _borderLength = 20;
		private int _marginBottom = 5;

		/// <summary>
		/// holds the bottom connector
		/// </summary>
		protected Connector cBottom, cLeft, cRight, cTop;
		/// <summary>
		/// the rectangle on which any shape lives
		/// </summary>
		protected Rectangle rectangle;
		/// <summary>
		/// the collection of connectors onto which you can attach a connection
		/// </summary>
		protected ConnectorCollection connectors;
		/// <summary>
		/// the text on the shape
		/// </summary>
		protected string text = string.Empty;
		#endregion

		#region Properties

		protected bool enableBottomSourceConnector = true;

		[Browsable(true), Description("显示底部源点"), Category("Layout")]
		public bool EnableBottomSourceConnector
		{
			get { return enableBottomSourceConnector; }
			set { enableBottomSourceConnector = value; }
		}

		protected bool enableLeftSourceConnector = true;

		[Browsable(true), Description("显示左侧源点"), Category("Layout")]
		public bool EnableLeftSourceConnector
		{
			get { return enableLeftSourceConnector; }
			set { enableLeftSourceConnector = value; }
		}

		protected bool enableRightSourceConnector = true;

		[Browsable(true), Description("显示右侧源点"), Category("Layout")]
		public bool EnableRightSourceConnector
		{
			get { return enableRightSourceConnector; }
			set { enableRightSourceConnector = value; }
		}

		protected bool enableTopSourceConnector = true;

		[Browsable(true), Description("显示顶部源点"), Category("Layout")]
		public bool EnableTopSourceConnector
		{
			get { return enableTopSourceConnector; }
			set { enableTopSourceConnector = value; }
		}

		protected bool enableBottomTargetConnector = true;

		[Browsable(true), Description("显示底部目标点"), Category("Layout")]
		public bool EnableBottomTargetConnector
		{
			get { return enableBottomTargetConnector; }
			set { enableBottomTargetConnector = value; }
		}

		protected bool enableLeftTargetConnector = true;

		[Browsable(true), Description("显示左侧目标点"), Category("Layout")]
		public bool EnableLeftTargetConnector
		{
			get { return enableLeftTargetConnector; }
			set { enableLeftTargetConnector = value; }
		}

		protected bool enableRightTargetConnector = true;

		[Browsable(true), Description("显示右侧目标点"), Category("Layout")]
		public bool EnableRightTargetConnector
		{
			get { return enableRightTargetConnector; }
			set { enableRightTargetConnector = value; }
		}

		protected bool enableTopTargetConnector = true;

		[Browsable(true), Description("显示顶部目标点"), Category("Layout")]
		public bool EnableTopTargetConnector
		{
			get { return enableTopTargetConnector; }
			set { enableTopTargetConnector = value; }
		}

		[Browsable(false)]
		public virtual string ShapeName => "ShapeBase";
		/// <summary>
		/// Gets or sets the connectors of this shape
		/// </summary>
		[Browsable(false)]
		public ConnectorCollection Connectors
		{
			get { return connectors; }
			set { connectors = value; }
		}

		/// <summary>
		/// Gets or sets the width of the shape
		/// </summary>
		[Browsable(true), Description("宽度"), Category("Layout")]
		public int Width
		{
			get { return this.rectangle.Width; }
			set { Resize(value, this.Height); }
		}

		/// <summary>
		/// Gets or sets the height of the shape
		/// </summary>		
		[Browsable(true), Description("高度"), Category("Layout")]
		public int Height
		{
			get { return this.rectangle.Height; }
			set { Resize(this.Width, value); }
		}

		/// <summary>
		/// Gets or sets the text of the shape
		/// </summary>
		[Browsable(true), Description("显示的文本"), Category("Layout")]
		public string Text
		{
			get { return this.text; }
			set { this.text = value; this.Invalidate(); }
		}

		/// <summary>
		/// the x-coordinate of the upper-left corner
		/// </summary>
		[Browsable(true), Description("X坐标"), Category("Layout")]
		public int X
		{
			get { return rectangle.X; }
			set
			{
				Point p = new Point(value - rectangle.X, rectangle.Y);
				this.Move(p);

				if(Site != null)
					Site.Invalidate(); //note that 'this.Invalidate()' will not be enough
			}
		}

		/// <summary>
		/// the y-coordinate of the upper-left corner
		/// </summary>
		[Browsable(true), Description("Y坐标"), Category("Layout")]
		public int Y
		{
			get { return rectangle.Y; }
			set
			{
				Point p = new Point(rectangle.X, value - rectangle.Y);
				this.Move(p);

				if (Site != null)
					Site.Invalidate();
			}
		}

		private bool showBorder = true;
		[Browsable(true), Description("是否显示边框"), Category("Layout")]
		public bool ShowBorder
		{
			get { return showBorder; }
			set { showBorder = value; Invalidate(); }
		}

		/// <summary>
		/// the backcolor of the shapes
		/// </summary>
		protected Color backGroundColor = Color.White;
		/// <summary>
		/// The backcolor of the shape
		/// </summary>
		[Browsable(true), Description("背景颜色"), Category("Layout")]
		public Color BackGroundColor
		{
			get { return backGroundColor; }
			set { backGroundColor = value; Invalidate(); }
		}

		protected Color borderColor = Color.Black;
		[Browsable(true), Description("边框颜色"), Category("Layout")]
		public Color BoderColor
		{
			get { return borderColor; }
			set { borderColor = value; Invalidate(); }
		}

		protected Color borderSelectedColor = Color.GreenYellow;
		[Browsable(true), Description("边框选中后颜色"), Category("Layout")]
		public Color BoderSelectedColor
		{
			get { return borderSelectedColor; }
			set { borderSelectedColor = value; Invalidate(); }
		}

		/// <summary>
		/// Gets or sets the location of the shape;
		/// </summary>
		[Browsable(false)]
		public Point Location
		{
			get { return new Point(this.rectangle.X, this.rectangle.Y); }
			set
			{
				//we use the move method but it requires the delta value, not an absolute position!
				Point p = new Point(value.X - rectangle.X, value.Y - rectangle.Y);
				//if you'd use this it would indeed move the shape but not the connector s of the shape
				//this.rectangle.X = value.X; this.rectangle.Y = value.Y; Invalidate();
				this.Move(p);
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Default ctor
		/// </summary>
		public ShapeBase()
		{
			Init();
		}
		/// <summary>
		/// Constructor with the site of the shape
		/// </summary>
		/// <param name="site">the DiagramControl instance to which the shape is attached</param>
		public ShapeBase(DiagramControl site) : base(site)
		{
			Init();
		}

		#endregion

		#region Methods

		protected override void SiteChanged()
		{
			connectors.Remove(cBottom);
			cBottom = new Connector(new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Bottom));
			cBottom.Site = this.site;
			cBottom.Name = "Bottom connector";
			cBottom.ConnectorDirection = ConnectorDirection.Down;
			connectors.Add(cBottom);

			connectors.Remove(cLeft);
			cLeft = new Connector(new Point(rectangle.Left, (int)(rectangle.Top + rectangle.Height / 2)));
			cLeft.Site = this.site;
			cLeft.Name = "Left connector";
			cLeft.ConnectorDirection = ConnectorDirection.Left;
			connectors.Add(cLeft);

			connectors.Remove(cRight);
			cRight = new Connector(new Point(rectangle.Right, (int)(rectangle.Top + rectangle.Height / 2)));
			cRight.Site = this.site;
			cRight.Name = "Right connector";
			cRight.ConnectorDirection = ConnectorDirection.Right;
			connectors.Add(cRight);

			connectors.Remove(cTop);
			cTop = new Connector(new Point((int)(rectangle.Left + rectangle.Width / 2), rectangle.Top));
			cTop.Site = this.site;
			cTop.Name = "Top connector";
			cTop.ConnectorDirection = ConnectorDirection.Up;
			connectors.Add(cTop);
		}

		/// <summary>
		/// Summarizes the initialization used by the constructors
		/// </summary>
		private void Init()
		{
			rectangle = new Rectangle(0, 0, 100, 70);
			connectors = new ConnectorCollection();
		}

		/// <summary>
		/// Returns the connector hit by the mouse, if any
		/// </summary>
		/// <param name="p">the mouse coordinates</param>
		/// <param name="targetNode">是否为目标节点</param>
		/// <returns>the connector hit by the mouse</returns>
		public virtual Connector HitConnector(Point p, bool targetNode = false)
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
				if (!EnableTopSourceConnector)
				{
					return null;
				}

				Point point = cTop.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				//return base.HitConnector(point);
				return cTop;
			}

			#endregion

			#region 下

			contentionRectangle = new Rectangle(
				(rectangle.Width - _borderLength) / 2 + rectangle.X + point0.X,
				rectangle.Y + point0.Y + rectangle.Height + _marginBottom,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				if (!EnableBottomSourceConnector)
				{
					return null;
				}

				Point point = cBottom.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				//return base.HitConnector(point);
				return cBottom;
			}

			#endregion

			#region 右

			contentionRectangle = new Rectangle(
				rectangle.Width + rectangle.X + point0.X + _marginBottom,
				(rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				if (!EnableRightSourceConnector)
				{
					return null;
				}

				Point point = cRight.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				//return base.HitConnector(point);
				return cRight;
			}

			#endregion

			#region 左

			contentionRectangle = new Rectangle(
				rectangle.X + point0.X - _marginBottom - _borderLength,
				(rectangle.Height - _borderLength) / 2 + rectangle.Y + point0.Y,
				_borderLength, _borderLength);

			if (contentionRectangle.Contains(r))
			{
				if (!EnableLeftSourceConnector)
				{
					return null;
				}

				Point point = cLeft.Point.GetPoint();
				point.Offset(point0.X - 7, point0.Y - 7);
				//return base.HitConnector(point);
				return cLeft;
			}

			#endregion

			for (int k = 0; k < connectors.Count; k++)
			{
				if (connectors[k].Hit(p))
				{
					if (!targetNode)
					{
						if (!EnableBottomSourceConnector && connectors[k].ConnectorDirection == ConnectorDirection.Down)
						{
							return null;
						}

						if (!EnableTopSourceConnector && connectors[k].ConnectorDirection == ConnectorDirection.Up)
						{
							return null;
						}

						if (!EnableRightSourceConnector && connectors[k].ConnectorDirection == ConnectorDirection.Right)
						{
							return null;
						}

						if (!EnableLeftSourceConnector && connectors[k].ConnectorDirection == ConnectorDirection.Left)
						{
							return null;
						}
					}
					else
					{
						if (!EnableBottomTargetConnector && connectors[k].ConnectorDirection == ConnectorDirection.Down)
						{
							return null;
						}

						if (!EnableTopTargetConnector && connectors[k].ConnectorDirection == ConnectorDirection.Up)
						{
							return null;
						}

						if (!EnableRightTargetConnector && connectors[k].ConnectorDirection == ConnectorDirection.Right)
						{
							return null;
						}

						if (!EnableLeftTargetConnector && connectors[k].ConnectorDirection == ConnectorDirection.Left)
						{
							return null;
						}
					}

					connectors[k].hovered = true;
					connectors[k].Invalidate();
					return connectors[k];
				}
				else
				{
					connectors[k].hovered = false;
					connectors[k].Invalidate();
				}
			}
			return null;

		}

		/// <summary>
		/// Overrides the abstract paint method
		/// </summary>
		/// <param name="g">a graphics object onto which to paint</param>
		public override void Paint(System.Drawing.Graphics g)
		{
			Point point = new Point(0, 0);
			if (this.site != null)
				point = this.site.ViewOriginPoint.GetPoint();

			if (isSelected)
			{
				Point point1, point2, point3;
				Point[] pntArr;

				if (EnableTopSourceConnector)
				{
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
				}

				if (EnableBottomSourceConnector)
				{
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
				}

				if (EnableRightSourceConnector)
				{
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
				}

				if (EnableLeftSourceConnector)
				{
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
			}

			for (int k = 0; k < connectors.Count; k++)
			{
				connectors[k].Paint(g);
			}

			if (text != string.Empty)
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.LineAlignment = StringAlignment.Center;
				stringFormat.Alignment = StringAlignment.Center;
				Rectangle rectangle = new Rectangle(this.rectangle.X + point.X, this.rectangle.Y + point.Y, this.rectangle.Width, this.rectangle.Height);
				g.DrawString(text, font, Brushes.Black, rectangle, stringFormat);
			}
		}

		/// <summary>
		/// Override the abstract Hit method
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
		/// Overrides the abstract Invalidate method
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



		/// <summary>
		/// Moves the shape with the given shift
		/// </summary>
		/// <param name="p">represent a shift-vector, not the absolute position!</param>
		public override void Move(Point p)
		{
			this.rectangle.X += p.X;
			this.rectangle.Y += p.Y;
			for (int k = 0; k < this.connectors.Count; k++)
			{
				connectors[k].Move(p);
			}
			this.Invalidate();
		}

		/// <summary>
		/// Resizes the shape and moves the connectors
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public virtual void Resize(int width, int height)
		{
			this.rectangle.Height = height;
			this.rectangle.Width = width;

			if (cBottom != null)
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
