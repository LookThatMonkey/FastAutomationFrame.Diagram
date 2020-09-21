using FastAutomationFrame.Diagram.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram
{
	public class LocationEventArgs : EventArgs
	{
		public int OffsetX = 0;
		public int OffsetY = 0;
	}
	public class AttachedToChangedEventArgs : EventArgs
	{
		public Connector New { get; set; }
	}
	
	public class Connector : Entity
	{

		#region Events

		public event EventHandler<LocationEventArgs> LocationChanged;
		private void OnLocationChanged(LocationEventArgs e)
		{
            LocationChanged?.Invoke(this, e);
        }
		public event EventHandler<AttachedToChangedEventArgs> AttachedToChanged;
		private void OnAttachedToChanged(AttachedToChangedEventArgs e)
		{
            AttachedToChanged?.Invoke(this, e);
        }

		#endregion

		#region Fields
		/// <summary>
		/// the location of this connector
		/// </summary>
		protected DiagramPoint point;
		/// <summary>
		/// the connectors attached to this connector
		/// </summary>
		protected ConnectorCollection attachedConnectors;
		/// <summary>
		/// the connector, if any, to which this connector is attached to
		/// </summary>
		protected Connector attachedTo;
		/// <summary>
		/// the name of this connector
		/// </summary>
		protected string name;
		#endregion

		#region Properties

		/// <summary>
		/// The name of this connector
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private ConnectorDirection? _connectorDirection = null;
		public ConnectorDirection ConnectorDirection
		{
			get
			{
				if (_connectorDirection != null && point != null)
					point.ConnectorDirection = (ConnectorDirection)_connectorDirection;

				if (point == null)
					return _connectorDirection == null ? ConnectorDirection.None : (ConnectorDirection)_connectorDirection;

				return point.ConnectorDirection;
			}
			set
			{
				if (point != null)
					point.ConnectorDirection = value;
				_connectorDirection = value;
			}
		}

		/// <summary>
		/// If the connector is attached to another connector
		/// </summary>
		public Connector AttachedTo
		{
			get { return attachedTo; }
			set { 
				attachedTo = value;
				point = attachedTo.point.Copy();
				_connectorDirection = attachedTo.ConnectorDirection;
				OnAttachedToChanged(new AttachedToChangedEventArgs() { New = value });
			}
		}

		/// <summary>
		/// The location of this connector
		/// </summary>
		public DiagramPoint Point
		{
			get { return point; }
			set { point = value; if (_connectorDirection != null) point.ConnectorDirection = (ConnectorDirection)_connectorDirection; }
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Default connector
		/// </summary>
		public Connector()
		{
			attachedConnectors = new ConnectorCollection();
		}

		/// <summary>
		/// Constructs a connector, passing its location
		/// </summary>
		/// <param name="p"></param>
		public Connector(DiagramPoint p)
		{
			attachedConnectors = new ConnectorCollection();
			Point = p;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Paints the connector on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{
			if (hovered && this.site != null)
				g.FillRectangle(Brushes.Blue, point.X + this.site.ViewOriginPoint.GetPoint().X - 5, point.Y + this.site.ViewOriginPoint.GetPoint().Y - 5, 10, 10);
			//else
			//	g.FillRectangle(Brushes.Green, point.X + this.site.ViewOriginPoint.X - 2, point.Y + this.site.ViewOriginPoint.Y - 2, 4, 4);
		}

		/// <summary>
		/// Tests if the mouse hits this connector
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(Point p)
		{
			p.X = p.X - this.site.ViewOriginPoint.GetPoint().X;
			p.Y = p.Y - this.site.ViewOriginPoint.GetPoint().Y;

			Point a = p;
			Point b = point.GetPoint();
			b.Offset(-7, -7);
			//a.Offset(-1,-1);
			Rectangle r = new Rectangle(a, new Size(0, 0));
			Rectangle d = new Rectangle(b, new Size(15, 15));
			return d.Contains(r);
		}

		/// <summary>
		/// Invalidates the connector
		/// </summary>
		public override void Invalidate()
		{
			Point p = point.GetPoint();
			p.X += this.site.ViewOriginPoint.GetPoint().X;
			p.Y += this.site.ViewOriginPoint.GetPoint().Y;
			p.Offset(-5, -5);
			site.Invalidate(new Rectangle(p, new Size(10, 10)));
		}

		/// <summary>
		/// Moves the connector with the given shift-vector
		/// </summary>
		/// <param name="p"></param>
		public override void Move(Point p)
		{
			OnLocationChanged(new LocationEventArgs() { OffsetX = p.X, OffsetY = p.Y });
			this.point.X = this.point.X + p.X;
			this.point.Y = this.point.Y + p.Y;
			for (int k = 0; k < attachedConnectors.Count; k++)
				attachedConnectors[k].Move(p);
		}

		/// <summary>
		/// Attaches the given connector to this connector
		/// </summary>
		/// <param name="c"></param>
		public void AttachConnector(Connector c)
		{
			//remove from the previous, if any
			if (c.attachedTo != null)
			{
				c.attachedTo.attachedConnectors.Remove(c);
			}
			attachedConnectors.Add(c);
			c.AttachedTo = this;

		}

		/// <summary>
		/// Detaches the given connector from this connector
		/// </summary>
		/// <param name="c"></param>
		public void DetachConnector(Connector c)
		{
			attachedConnectors.Remove(c);
		}

		/// <summary>
		/// Releases this connector from any other
		/// </summary>
		public void Release()
		{
			if (this.attachedTo != null)
			{
				this.attachedTo.attachedConnectors.Remove(this);
				this.AttachedTo = null;
			}

		}

		#endregion
	}
}
