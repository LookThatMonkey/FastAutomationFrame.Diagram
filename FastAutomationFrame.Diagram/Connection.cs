using FastAutomationFrame.Diagram.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram
{
	/// <summary>
	/// 
	/// </summary>
	public class Connection : Entity
	{
		#region Fields

		#endregion

		#region Properties

		protected Connector from;
		[Browsable(true), Description("始节点"), Category("Layout")]
		public Connector From
		{
			get 
			{ 
				return from;
			}
			private set
			{ 
				from = value;
				if (site != null)
					from.Site = site;

				from.ContainEntity = this;
			}
		}

		protected Connector to;
		[Browsable(true), Description("终节点"), Category("Layout")]
		public Connector To
		{
			get
			{
				return to;
			}
			private set
			{
				to = value;
				if (site != null)
					to.Site = site;

				to.ContainEntity = this;
			}
		}

		private bool _useBackColor = false;
		[Browsable(true), Description("始节点"), Category("Layout")]
		public bool UseBackColor
		{
			get
			{
				return _useBackColor;
			}
			set
			{
				_useBackColor = value;
			}
		}

		protected Color color = Color.Silver;
		[Browsable(true), Description("默认颜色"), Category("Layout")]
		public Color Color
		{
			get 
			{ 
				return color;
			}
			set 
			{ 
				color = value;
				_useBackColor = true;
			}
		}

		protected Color lineSelectedColor = Color.Green;
		[Browsable(true), Description("选中颜色"), Category("Layout")]
		public Color LineSelectedColor
		{
			get
			{ 
				return lineSelectedColor;
			}
			set 
			{
				lineSelectedColor = value;
				_useBackColor = true;
			}
		}

		protected Color lineHoveredColor = Color.Blue;
		[Browsable(true), Description("悬停颜色"), Category("Layout")]
		public Color LineHoveredColor
		{
			get 
			{ 
				return lineHoveredColor;
			}
			set 
			{ 
				lineHoveredColor = value;
				_useBackColor = true;
			}
		}

		[Browsable(false)]
		public ConnectorCollection Connectors { get; set; }

		#endregion

		#region Constructor
		/// <summary>
		/// Default ctor
		/// </summary>
		public Connection()
		{

		}

		/// <summary>
		/// Constructs a connection between the two given points
		/// </summary>
		/// <param name="from">the starting point of the connection</param>
		/// <param name="to">the end-point of the connection</param>
		public Connection(DiagramPoint from, DiagramPoint to)
		{
			Connector connector1, connector2;
			Connectors = new ConnectorCollection();

			this.from = new Connector(from.Copy());
			this.from.Name = "From";
			this.from.ContainEntity = this;
			this.from.LocationChanged += Connector_LocationChanged;
			connector1 = this.from;
			Connectors.Add(connector1);

			this.to = new Connector(to.Copy());
			this.To.Name = "To";
			this.to.ContainEntity = this;
			connector2 = this.to;
			this.to.LocationChanged += Connector_LocationChanged;
			this.To.AttachedToChanged += To_AttachedToChanged;
			Connectors.Add(connector2);
		}

        private void Connector_LocationChanged(object sender, LocationEventArgs e)
		{
			InitialPath();
		}

        private void To_AttachedToChanged(object sender, AttachedToChangedEventArgs e)
		{
			InitialPath();
		}
		internal void InitialPath()
		{
			Connector connector1 = this.from, connector2 = this.to;
			int x = 0, y = 0;
			ConnectorCollection connectors = new ConnectorCollection();
			connectors.Add(this.from);

			#region 下对上

			if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Y < connector2.Y && connector1.X != connector2.X)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if(connector1.Y >= connector2.Y)
				{
					x = connector1.X;
					y = connector1.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = (connector1.X + connector2.X) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y -= 30 + connector1.Y - connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 下对左

			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.Y < connector2.Y && connector1.X >= connector2.X)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y)
				{
					x = connector1.X;
					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.X != connector2.X)
				{
					x = connector1.X;
					y = connector1.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 下对右

			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.Y < connector2.Y && connector1.X <= connector2.X)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y)
				{
					x = connector1.X;
					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.X != connector2.X)
				{
					x = connector1.X;
					y = connector1.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 下对下

			else if (connector1.ConnectorDirection == ConnectorDirection.Down && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Y < connector2.Y && connector1.X != connector2.X)
				{
					x = connector1.X;
					y = connector2.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.X != connector2.X)
				{
					x = connector1.X;
					y = connector1.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.X;
					y = connector1.Y + 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector1.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y -= 10 + (connector1.Y - connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 右对上

			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Y < connector2.Y && connector1.X + 10 >= connector2.X)
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y)
				{
					x = connector2.X;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 右对右

			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				x = Math.Max(connector1.X + 10, connector2.X + 20);
				y = connector1.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));

				y = connector2.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}

			#endregion

			#region 右对下

			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Y < connector2.Y)
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y == connector2.Y)
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector1.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.X + 10 < connector2.X)
				{
					x = connector2.X;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 右对左

			else if (connector1.ConnectorDirection == ConnectorDirection.Right && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.X + 10 >= connector2.X)
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.X + 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 上对上

			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				x = connector1.X;
				y = Math.Min(connector1.Y - 10, connector2.Y - 20);
				connectors.Add(new Connector(new DiagramPoint(x, y)));

				x = connector2.X;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}

			#endregion

			#region 上对右

			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.Y > connector2.Y && connector1.X > connector2.X)
				{
					x = connector1.X;
					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y > connector2.Y)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y <= connector2.Y)
				{
					x = connector1.X;
					y = connector1.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 上对下

			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Y > connector2.Y && connector1.X != connector2.X)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y && connector1.X != connector2.X)
				{
					x = connector1.X;
					y = connector1.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = (connector1.X + connector2.X) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 上对左

			else if (connector1.ConnectorDirection == ConnectorDirection.Up && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				if (connector1.Y > connector2.Y && connector1.X < connector2.X)
				{
					x = connector1.X;
					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y > connector2.Y)
				{
					x = connector1.X;
					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y <= connector2.Y)
				{
					x = connector1.X;
					y = connector1.Y - 10;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 左对上

			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Up)
			{
				if (connector1.Y < connector2.Y && connector1.X - 10 >= connector2.X)
				{
					x = connector2.X;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if (connector1.Y < connector2.Y)
				{
					x = connector1.X - 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if(connector1.Y > connector2.Y)
				{
					x = connector1.X - 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y - 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 左对右

			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Right)
			{
				if (connector1.X - 10 >= connector2.X)
				{
					x = (connector1.X + connector2.X) / 2;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else
				{
					x = connector1.X - 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = (connector1.Y + connector2.Y) / 2;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 左对下

			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Down)
			{
				if (connector1.Y > connector2.Y && connector1.X > connector2.X)
				{
					x = connector2.X;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
				else if(connector1.X != connector2.X)
				{
					x = connector1.X - 10;
					y = connector1.Y;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					y = connector2.Y + 20;
					connectors.Add(new Connector(new DiagramPoint(x, y)));

					x = connector2.X;
					connectors.Add(new Connector(new DiagramPoint(x, y)));
				}
			}

			#endregion

			#region 左对左

			else if (connector1.ConnectorDirection == ConnectorDirection.Left && connector2.ConnectorDirection == ConnectorDirection.Left)
			{
				x = Math.Min(connector1.X - 10, connector2.X - 20);
				y = connector1.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));

				y = connector2.Y;
				connectors.Add(new Connector(new DiagramPoint(x, y)));
			}

			#endregion

			connectors.Add(this.to);
			Connectors = connectors;

		}
		#endregion

		#region Methods

		protected override void SiteChanged()
		{
			if (from != null)
				this.from.Site = site;
			if (to != null)
				this.to.Site = site;
		}

		/// <summary>
		/// Paints the connection on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			if (this.HideItem) return;
			Pen p = new Pen(UseBackColor ? this.Color : this.site.LineColor);
			p.Width = this.site.LineWidth;
			if (isSelected)
			{
				p.Color = UseBackColor ? this.LineSelectedColor : this.site.LineSelectedColor;
				p.Width = this.site.LineMouseWidth;
			}
			else if (hovered)
			{
				p.Color = UseBackColor ? this.LineHoveredColor : this.site.LineHoveredColor;
				p.Width = this.site.LineMouseWidth;
			}

			Connectors[0].Paint(g);
			for (int i = 1; i < Connectors.Count - 1; i++)
			{
				g.DrawLine(p,
					Connectors[i - 1].X + this.site.ViewOriginPoint.GetPoint().X,
					Connectors[i - 1].Y + this.site.ViewOriginPoint.GetPoint().Y,
					Connectors[i].X + this.site.ViewOriginPoint.GetPoint().X,
					Connectors[i].Y + this.site.ViewOriginPoint.GetPoint().Y);
				Connectors[i].Paint(g);
			}

			Connectors[Connectors.Count - 1].Paint(g);
			p.CustomEndCap = new AdjustableArrowCap(5, 5, true);
			g.DrawLine(p,
				Connectors[Connectors.Count - 2].X + this.site.ViewOriginPoint.GetPoint().X,
				Connectors[Connectors.Count - 2].Y + this.site.ViewOriginPoint.GetPoint().Y,
				Connectors[Connectors.Count - 1].X + this.site.ViewOriginPoint.GetPoint().X,
				Connectors[Connectors.Count - 1].Y + this.site.ViewOriginPoint.GetPoint().Y);
		}
		/// <summary>
		/// Invalidates the connection
		/// </summary>
		public override void Invalidate()
		{
			Rectangle f = new Rectangle(from.Point.GetPoint(), new Size(10, 10));
			f.X += this.site.ViewOriginPoint.GetPoint().X;
			f.Y += this.site.ViewOriginPoint.GetPoint().Y;
			Rectangle t = new Rectangle(to.Point.GetPoint(), new Size(10, 10));
			t.X += this.site.ViewOriginPoint.GetPoint().X;
			t.Y += this.site.ViewOriginPoint.GetPoint().Y;
			site.Invalidate(Rectangle.Union(f, t));
		}

		/// <summary>
		/// Tests if the mouse hits this connection
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(Point p)
		{
			p.Offset(-this.site.ViewOriginPoint.GetPoint().X, -this.site.ViewOriginPoint.GetPoint().Y);
			for (int i = 1; i < Connectors.Count; i++)
			{
				if (GetPointIsInLine(p, Connectors[i - 1].Point.GetPoint(), Connectors[i].Point.GetPoint(), 3))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断点是否在直线上
		/// </summary>
		/// <param name="pf"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="range">判断的的误差，不需要误差则赋值0</param>
		/// <returns></returns>
		private bool GetPointIsInLine(Point pf, Point p1, Point p2, double range)
		{
			double cross = (p2.X - p1.X) * (pf.X - p1.X) + (p2.Y - p1.Y) * (pf.Y - p1.Y);
			if (cross <= 0) return false;
			double d2 = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
			if (cross >= d2) return false;

			double r = cross / d2;
			double px = p1.X + (p2.X - p1.X) * r;
			double py = p1.Y + (p2.Y - p1.Y) * r;

			//判断距离是否小于误差
			return Math.Sqrt((pf.X - px) * (pf.X - px) + (py - pf.Y) * (py - pf.Y)) <= range;
		}

		/// <summary>
		/// Moves the connection with the given shift
		/// </summary>
		/// <param name="p"></param>
		public override void Move(Point p)
		{
		}


		#endregion

	}
}
