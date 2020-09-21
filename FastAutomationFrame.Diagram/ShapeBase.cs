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
		protected Color backGroundColor = Color.SteelBlue;
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
		/// <returns>the connector hit by the mouse</returns>
		public virtual Connector HitConnector(Point p)
		{
			for (int k = 0; k < connectors.Count; k++)
			{
				if (connectors[k].Hit(p))
				{
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
			return;
		}

		/// <summary>
		/// Override the abstract Hit method
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(System.Drawing.Point p)
		{
			return false;
		}

		/// <summary>
		/// Overrides the abstract Invalidate method
		/// </summary>
		public override void Invalidate()
		{
			site.Invalidate(rectangle);
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
		}


		#endregion
	}
}
