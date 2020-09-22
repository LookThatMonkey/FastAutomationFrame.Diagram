//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :DiagramControl.cs
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
using System.Windows.Forms;

namespace FastAutomationFrame.Diagram
{
    public class DiagramControl : Panel
    {
        #region Events and delegates

        public event EventHandler<DeletingEventArgs> OnElementDeleting;
        public event EventHandler<EventArgs> OnElementDeleted;
        public event EventHandler<SelectElementChangedEventArgs> OnSelectElementChanged;

        #endregion

        #region Fields

        private bool _leftMouse = false;
        /// <summary>
        /// 用于计算鼠标移动距离
        /// </summary>
        private DiagramPoint _stratPoint = new DiagramPoint();
        private bool tracking = false;
        private GlobalHook hook;
        protected Size gridSize = new Size(10, 10);
        protected Proxy proxy;
        protected Entity selectedEntity;
        protected Entity hoveredEntity;
        protected ShapeCollection shapes;
        protected ConnectionCollection connections;
        protected Random rnd;

        #endregion

        #region Fields && Properties

        protected bool showGrid = true;
        [Description("是否显示点网格"), Category("Layout")]
        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; Invalidate(true); }
        }

        protected Color lineColor = Color.Silver;
        [Browsable(true), Description("连线默认颜色"), Category("Layout")]
        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        protected Color lineSelectedColor = Color.Green;
        [Browsable(true), Description("连线选中颜色"), Category("Layout")]
        public Color LineSelectedColor
        {
            get { return lineSelectedColor; }
            set { lineSelectedColor = value; }
        }

        protected Color lineHoveredColor = Color.Blue;
        [Browsable(true), Description("连线悬停颜色"), Category("Layout")]
        public Color LineHoveredColor
        {
            get { return lineHoveredColor; }
            set { lineHoveredColor = value; }
        }

        private DiagramPoint _viewOriginPoint = new DiagramPoint(0, 0);
        [Browsable(true), Description("视觉原点，左上角坐标"), Category("Layout")]
        public DiagramPoint ViewOriginPoint
        {
            get { return _viewOriginPoint; }
            set { _viewOriginPoint = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default ctor
        /// </summary>
        public DiagramControl()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            this.AllowDrop = true;

            shapes = new ShapeCollection();
            connections = new ConnectionCollection();
            rnd = new Random();
            proxy = new Proxy(this);

            hook = new GlobalHook();
            hook.KeyDown += new KeyEventHandler(hook_KeyDown);
            bool b = hook.Start();
        }

        #endregion

        #region Methods

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            object objData = drgevent.Data.GetData(typeof(DragItem));
            if (objData != null)
            {
                DragItem dragItem = objData as DragItem;
                ShapeBase shapeBase = (ShapeBase)Activator.CreateInstance(dragItem.Shape.GetType());
                this.AddShape(shapeBase);
                Point p1 = Control.MousePosition;
                Point p2 = this.PointToScreen(new Point(0, 0));
                shapeBase.X = (p1.X - p2.X - ViewOriginPoint.GetPoint().X - 1) / 2;
                shapeBase.Y = p1.Y - p2.Y - ViewOriginPoint.GetPoint().Y;
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            if (drgevent.Data.GetDataPresent(typeof(DragItem)))
            {
                drgevent.Effect = DragDropEffects.Copy;
            }
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectElement();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _leftMouse = e.Button == MouseButtons.Left;
            _stratPoint = e.Location;

            int entityTYpe = -1;//0:ShapeBase;1:ShapeBase.connectors;2:connections;3:connections.From;4:connections.To.
            int index = -1;
            Entity hoveredentity = shapes.Cast<ShapeBase>().FirstOrDefault(f =>
            {
                if (f.Hit(e.Location))
                {
                    return true;
                }
                return false;
            });

            if (hoveredentity == null && selectedEntity != null && selectedEntity is ShapeBase)
            {
                Connector connector = (selectedEntity as ShapeBase).HitConnector(e.Location);
                if (connector != null)
                {
                    Point point = e.Location;
                    point.Offset(-this.ViewOriginPoint.GetPoint().X, -this.ViewOriginPoint.GetPoint().Y);

                    Connection connection = this.AddConnection(connector.Point, point);
                    UpdateSelected(connection.To);
                    connector.AttachConnector(connection.From);
                    tracking = true;
                    Invalidate(true);
                    return;
                }
            }

            if (hoveredentity != null)
            {
                tracking = true;
                OnSelectChanged(hoveredentity, new SelectElementChangedEventArgs() { CurrentEntity = hoveredentity, PreviousEntity = selectedEntity });
            }
            else
            {
                hoveredentity = connections.Cast<Connection>().FirstOrDefault(f =>
                {
                    if (f.Hit(e.Location))
                    {
                        entityTYpe = 2;
                        return true;
                    }
                    if (f.From.Hit(e.Location))
                    {
                        entityTYpe = 3;
                        return true;
                    }
                    if (f.To.Hit(e.Location))
                    {
                        entityTYpe = 4;
                        return true;
                    }
                    return false;
                });

                if (entityTYpe == 3)
                {
                    hoveredentity = ((Connection)hoveredentity).From;
                    tracking = true;
                }
                else if (entityTYpe == 4)
                {
                    hoveredentity = ((Connection)hoveredentity).To;
                    tracking = true;
                }
                else if (entityTYpe == 2)
                {
                    OnSelectChanged(hoveredentity, new SelectElementChangedEventArgs() { CurrentEntity = hoveredentity, PreviousEntity = selectedEntity });
                }
            }

            if (hoveredentity == null)
            {
                OnSelectChanged(this.proxy, new SelectElementChangedEventArgs() { CurrentEntity = hoveredentity, PreviousEntity = selectedEntity });
            }
            UpdateSelected(hoveredentity);
            Invalidate(true);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _leftMouse = false;

            if (tracking)
            {
                Point p = new Point(e.X, e.Y);

                if (typeof(Connector).IsInstanceOfType(selectedEntity))
                {
                    Connector con;
                    for (int k = 0; k < shapes.Count; k++)
                    {
                        if ((con = shapes[k].HitConnector(p)) != null)
                        {
                            con.AttachConnector((selectedEntity as Connector));
                            con.hovered = false;
                            tracking = false;
                            return;
                        }
                    }
                  (selectedEntity as Connector).Release();

                }
                tracking = false;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (tracking)
            {
                selectedEntity.Move(new Point(e.X - _stratPoint.GetPoint().X, e.Y - _stratPoint.GetPoint().Y));
                if (typeof(Connector).IsInstanceOfType(selectedEntity))
                {
                    for (int k = 0; k < shapes.Count; k++)
                    {
                        shapes[k].HitConnector(e.Location);
                    }
                }
            }
            else if (_leftMouse)
            {
                _viewOriginPoint = new DiagramPoint(_viewOriginPoint.GetPoint().X + e.X - _stratPoint.GetPoint().X, _viewOriginPoint.GetPoint().Y + e.Y - _stratPoint.GetPoint().Y);
            }

            int entityTYpe = -1;//0:ShapeBase;1:ShapeBase.connectors;2:connections;3:connections.From;4:connections.To.
            Entity hoveredentity = shapes.Cast<Entity>().FirstOrDefault(f=>f.Hit(e.Location));
            if (hoveredentity == null)
            {
                hoveredentity = connections.Cast<Connection>().FirstOrDefault(f =>
                {
                    if (f.Hit(e.Location))
                    {
                        entityTYpe = 2;
                        return true;
                    }
                    if (f.From.Hit(e.Location))
                    {
                        entityTYpe = 3;
                        return true;
                    }
                    if (f.To.Hit(e.Location))
                    {
                        entityTYpe = 4;
                        return true;
                    }
                    return false; 
                });
            }

            if (entityTYpe == 3)
                hoveredentity = ((Connection)hoveredentity).From;
            if (entityTYpe == 4)
                hoveredentity = ((Connection)hoveredentity).To;

            UpdateHovered(hoveredentity);

            _stratPoint = e.Location;
            Invalidate(true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;

            if (showGrid)
                ControlPaint.DrawGrid(g, this.ClientRectangle, gridSize, this.BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);//解决闪烁

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int k = 0; k < connections.Count; k++)
            {
                connections[k].Paint(g);
                connections[k].From.Paint(g);
                connections[k].To.Paint(g);
            }

            for (int k = 0; k < shapes.Count; k++)
            {
                shapes[k].Paint(g);
            }

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);//解决闪烁 
        }

        public void DeleteElement(Entity entity)
        {
            if (entity == null) return;

            if (entity is ShapeBase)
            {
                DeletingEventArgs deletingEventArgs = new DeletingEventArgs();
                OnDeleting(entity, deletingEventArgs);

                if (!deletingEventArgs.Cancel)
                {
                    this.shapes.Remove(entity as ShapeBase);
                    this.Invalidate(true);
                    OnDeleted(entity, EventArgs.Empty);
                }
            }
            else if (entity is Connection)
            {
                DeletingEventArgs deletingEventArgs = new DeletingEventArgs();
                OnDeleting(entity, deletingEventArgs);

                if (!deletingEventArgs.Cancel)
                {
                    this.connections.Remove(entity as Connection);
                    this.Invalidate(true);
                    OnDeleted(entity, EventArgs.Empty);
                }
            }
        }
        public ShapeBase AddShape(ShapeBase shape)
        {
            shapes.Add(shape);
            shape.Site = this;
            this.Invalidate(true);
            return shape;
        }

        public Connection AddConnection(Connector from, Connector to)
        {
            Connection con = this.AddConnection(from.Point, to.Point);
            con.Site = this;
            from.AttachConnector(con.From);
            to.AttachConnector(con.To);

            return con;
        }

        public Connection AddConnection(DiagramPoint from, DiagramPoint to)
        {
            Connection con = new Connection(from, to);
            con.Site = this;
            this.AddConnection(con);
            return con;
        }

        public Connection AddConnection(Connection con)
        {
            connections.Add(con);
            con.Site = this;
            con.From.Site = this;
            con.To.Site = this;
            this.Invalidate(true);
            return con;
        }

        public Connection AddConnection(Point startPoint)
        {
            //let's take a random point and assume this control is not infinitesimal (bigger than 20x20).
            Point rndPoint = new Point(rnd.Next(20, this.Width - 20), rnd.Next(20, this.Height - 20));
            Connection con = new Connection(startPoint, rndPoint);
            con.Site = this;
            this.AddConnection(con);
            //use the end-point and simulate a dragging operation, see the OnMouseDown handler
            selectedEntity = con.To;
            tracking = true;
            this.Invalidate(true);
            return con;
        }

        public void DeleteSelectElement()
        {
            DeleteElement(selectedEntity);
        }

        private void OnDeleting(object sender, DeletingEventArgs e)
        {
            OnElementDeleting?.Invoke(sender, e);
        }

        private void OnDeleted(object sender, EventArgs e)
        {
            OnElementDeleted?.Invoke(sender, e);
        }

        private void OnSelectChanged(object sender, SelectElementChangedEventArgs e)
        {
            OnSelectElementChanged?.Invoke(sender, e);
        }

        private void UpdateSelected(Entity oEnt)
        {
            if (selectedEntity != null)
            {
                selectedEntity.IsSelected = false;
                selectedEntity.Invalidate();
                selectedEntity = null;
            }

            if (oEnt != null)
            {
                selectedEntity = oEnt;
                oEnt.IsSelected = true;
                oEnt.Invalidate();
            }
        }

        protected void UpdateHovered(Entity oEnt = null)
        {
            if (hoveredEntity != null)
            {
                hoveredEntity.hovered = false;
                hoveredEntity.Invalidate();
                hoveredEntity = null;
            }

            if (oEnt != null)
            {
                oEnt.hovered = true;
                hoveredEntity = oEnt;
                hoveredEntity.Invalidate();
            }
        }

        #endregion

    }
    public class DiagramPoint : Component
    {
        private Point _point = new Point();
        [Description("X偏移量"), Category("Layout")]
        public int X
        {
            get
            {
                return _point.X;
            }
            set
            {
                _point.X = value;
            }
        }

        [Description("Y偏移量"), Category("Layout")]
        public int Y
        {
            get
            {
                return _point.Y;
            }
            set
            {
                _point.Y = value;
            }
        }

        public ConnectorDirection ConnectorDirection = ConnectorDirection.None;

        public DiagramPoint()
        {

        }

        public DiagramPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point GetPoint()
        {
            return _point;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public static implicit operator DiagramPoint(Point point)
        {
            return new DiagramPoint(point.X, point.Y);
        }

        public DiagramPoint Copy()
        {
            DiagramPoint diagramPoint = new DiagramPoint(this.X, this.Y);
            diagramPoint.ConnectorDirection = this.ConnectorDirection;
            return diagramPoint;
        }
    }

    public enum ConnectorDirection
    {
        None,
        Up,
        Right,
        Left,
        Down
    }

    public class DeletingEventArgs : EventArgs
    {
        public bool Cancel { get; set; } = false;
    }

    public class SelectElementChangedEventArgs : EventArgs
    {
        public Entity CurrentEntity { get; set; }
        public Entity PreviousEntity { get; set; }
    }

    public class Proxy
    {
        #region Fields

        private DiagramControl site;

        #endregion

        #region Constructor

        public Proxy(DiagramControl site)
        { this.site = site; }

        #endregion

        #region Fields && Properties

        [Browsable(false)]
        public DiagramControl Site
        {
            get { return site; }
            set { site = value; }
        }
        [Browsable(true), Description("背景颜色"), Category("Layout")]
        public Color BackColor
        {
            get { return this.site.BackColor; }
            set { this.site.BackColor = value; }
        }

        [Browsable(true), Description("获取/设置网格显示状态"), Category("Layout")]
        public bool ShowGrid
        {
            get { return this.site.ShowGrid; }
            set { this.site.ShowGrid = value; }
        }

        [Browsable(true), Description("视觉原点，左上角坐标"), Category("Layout")]
        public DiagramPoint ViewOriginPoint
        {
            get { return this.site.ViewOriginPoint; }
            set { this.site.ViewOriginPoint = value; }
        }

        [Browsable(true), Description("连线默认颜色"), Category("Layout")]
        public Color LineColor
        {
            get { return this.site.LineColor; }
            set { this.site.LineColor = value; }
        }

        [Browsable(true), Description("连线选中颜色"), Category("Layout")]
        public Color LineSelectedColor
        {
            get { return this.site.LineSelectedColor; }
            set { this.site.LineSelectedColor = value; }
        }

        [Browsable(true), Description("连线悬停颜色"), Category("Layout")]
        public Color LineHoveredColor
        {
            get { return this.site.LineHoveredColor; }
            set { this.site.LineHoveredColor = value; }
        }

        #endregion
    }
}
