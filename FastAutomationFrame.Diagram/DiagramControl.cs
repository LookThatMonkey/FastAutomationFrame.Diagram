using FastAutomationFrame.Diagram.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FastAutomationFrame.Diagram
{
    public class DiagramControl : Panel
    {
        #region Events and delegates

        public event EventHandler<DeletingEventArgs> OnElementDeleting;
        public event EventHandler<EventArgs> OnElementDeleted;
        public event EventHandler<SelectElementChangedEventArgs> OnSelectElementChanged;
        public event EventHandler<EventArgs> OnEnableEditChanged;

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

        public ShapeBase SelectShape => selectedEntity is ShapeBase ? (ShapeBase)selectedEntity : null;

        #endregion

        #region Fields && Properties

        protected bool showGrid = false;
        [Description("是否显示点网格"), Category("Layout")]
        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; Invalidate(true); }
        }

        protected float lineWidth = 1F;
        [Browsable(true), Description("线宽"), Category("Layout")]
        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        protected float lineMouseWidth = 2F;
        [Browsable(true), Description("鼠标操作线宽"), Category("Layout")]
        public float LineMouseWidth
        {
            get { return lineMouseWidth; }
            set { lineMouseWidth = value; }
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

        private bool _enableEdit = true;
        [Browsable(true), Description("是否允许编辑"), Category("Layout")]
        public bool EnableEdit
        {
            get { return _enableEdit; }
            set { _enableEdit = value; OnEnableEditChanged?.Invoke(this, EventArgs.Empty); }
        }

        private bool _diagramLocked = false;
        [Browsable(true), Description("画布是否允许移动"), Category("Layout")]
        public bool DiagramLocked
        {
            get { return _diagramLocked; }
            set { _diagramLocked = value; }
        }

        private int _bottomLineOffset = 0;
        [Browsable(true), Description("底部连接点偏移量"), Category("Layout")]
        public int BottomLineOffset
        {
            get { return _bottomLineOffset; }
            set
            { 
                _bottomLineOffset = value;
                foreach (Connection v in connections)
                {
                    var bottomConnectors = v.Connectors.Cast<Connector>().Where(w=> w.ConnectorDirection == ConnectorDirection.Down);
                    if (bottomConnectors != null)
                    {
                        foreach (var bottomConnector in bottomConnectors)
                        {
                            bottomConnector.Offset = value;
                        }
                        v.InitialPath();
                    }
                }
            }
        }

        [Browsable(false)]
        public ShapeCollection ShapeCollection => shapes;

        [Browsable(false)]
        public ConnectionCollection Connections => connections;

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

        public void Sort(ShapeBase shape, int widthInterval = 50, int heightInterval = 50)
        {
            List<(ShapeBase shape, int generation)> allSortShapes = Sort(shape, 0);
            var q = from p in allSortShapes group p.generation by p.generation into g select new { count = g.Count(), generation = g.Key };
            int row = q.Max(m => m.count);
            int column = allSortShapes.Max(m => m.generation) + 1;
            int allHeight = row * shape.Height + (row - 1) * heightInterval;
            int x = shape.X + shape.Width + widthInterval;
            int y = shape.Y - allHeight / 2;
            for (int i = 1; i < column; i++)
            {
                var shapes = allSortShapes.Where(w => w.generation == i).Select(s=>s.shape);
                foreach (ShapeBase shapeBase in shapes)
                {
                    shapeBase.Location = new Point(x, y);
                    y += allHeight / shapes.Count();
                    var items = connections.Cast<Connection>().Where(w => w.To.ContainEntity == shapeBase || w.From.ContainEntity == shapeBase);
                    foreach (Connection c in items)
                    {
                        Connector cr = c.From.ContainEntity == shapeBase ? c.From : c.To;
                        if (cr.ConnectorDirection == ConnectorDirection.Down)
                        {
                            cr.Move(new Point(shapeBase.BottomConnector.Point.X - cr.Point.X, shapeBase.BottomConnector.Point.Y - cr.Point.Y));
                        }
                        else if (cr.ConnectorDirection == ConnectorDirection.Up)
                        {
                            cr.Move(new Point(shapeBase.TopConnector.Point.X - cr.Point.X, shapeBase.TopConnector.Point.Y - cr.Point.Y));
                        }
                        else if (cr.ConnectorDirection == ConnectorDirection.Left)
                        {
                            cr.Move(new Point(shapeBase.LeftConnector.Point.X - cr.Point.X, shapeBase.LeftConnector.Point.Y - cr.Point.Y));
                        }
                        else if (cr.ConnectorDirection == ConnectorDirection.Right)
                        {
                            cr.Move(new Point(shapeBase.RightConnector.Point.X - cr.Point.X, shapeBase.RightConnector.Point.Y - cr.Point.Y));
                        }
                    }
                }

                x += shape.Width + widthInterval;
                y = shape.Y - allHeight / 2;
            }
        }

        private List<(ShapeBase shape, int generation)> Sort(ShapeBase shape, int generation)
        {
            List<(ShapeBase shape, int generation)> allSortShapes = new List<(ShapeBase shape, int generation)>() { (shape, generation) };
            foreach (Connection connection in connections)
            {
                if (connection.Connectors.Cast<Connector>().FirstOrDefault(w => w.Name == "From" && w.ContainEntity == shape) != null)
                {
                    Connector c = connection.Connectors.Cast<Connector>().FirstOrDefault(w => w.Name == "To" && w.ContainEntity != null);
                    if(!allSortShapes.Select(s=>s.shape).Contains(c.ContainEntity))
                    {
                        allSortShapes.AddRange(Sort(c.ContainEntity as ShapeBase, generation + 1));
                    }
                }
            }

            allSortShapes.Sort(((ShapeBase shape, int generation) a, (ShapeBase shape, int generation) b) =>
            {
                if (a.generation != b.generation)
                    return a.generation.CompareTo(b.generation);

                return a.shape.Y.CompareTo(b.shape.Y);
            }
            );
            return allSortShapes;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if (!_enableEdit) return;
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

        public void Hide(string group, bool hideConnectionLines = true)
        {
            var items = shapes.OfType<ShapeBase>().Where(w=>w.Group == group);
            foreach (ShapeBase t in items)
            {
                t.HideItem = true;
                if (hideConnectionLines)
                {
                    var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == t) || (w.To != null && w.To.ContainEntity == t));
                    foreach (Connection c in tempconnections)
                    {
                        c.HideItem = true;
                    }
                }
            }

            this.Invalidate(true);
        }

        private void hide<T>(bool hideConnectionLines) where T : ShapeBase
        {
            var items = shapes.OfType<T>();
            foreach (T t in items)
            {
                t.HideItem = true;
                if (hideConnectionLines)
                {
                    var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == t) || (w.To != null && w.To.ContainEntity == t));
                    foreach (Connection c in tempconnections)
                    {
                        c.HideItem = true;
                    }
                }
            }

            this.Invalidate(true);
        }

        public void Hide(Type type, bool hideConnectionLines = true)
        {
            MethodInfo mi = this.GetType().GetMethod("hide", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { type });
            mi.Invoke(this, new object[] { hideConnectionLines });
        }

        public void Hide<T>(bool hideConnectionLines = true) where T : ShapeBase
        {
            hide<T>(hideConnectionLines);
        }

        public void Hide(ShapeBase entity, bool hideConnectionLines = true)
        {
            entity.HideItem = true;
            if(hideConnectionLines)
            {
                var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == entity) || (w.To != null && w.To.ContainEntity == entity));
                foreach (Connection c in tempconnections)
                {
                    c.HideItem = true;
                }
            }

            this.Invalidate(true);
        }

        public void Show(string group, bool hideConnectionLines = true)
        {
            var items = shapes.OfType<ShapeBase>().Where(w => w.Group == group);
            foreach (ShapeBase t in items)
            {
                t.HideItem = false;
                if (hideConnectionLines)
                {
                    var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == t) || (w.To != null && w.To.ContainEntity == t));
                    foreach (Connection c in tempconnections)
                    {
                        c.HideItem = false;
                    }
                }
            }

            this.Invalidate(true);
        }

        private void show<T>(bool hideConnectionLines) where T : ShapeBase
        {
            var items = shapes.OfType<T>();
            foreach (T t in items)
            {
                t.HideItem = false;
                if (hideConnectionLines)
                {
                    var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == t) || (w.To != null && w.To.ContainEntity == t));
                    foreach (Connection c in tempconnections)
                    {
                        c.HideItem = false;
                    }
                }
            }

            this.Invalidate(true);
        }

        public void Show(Type type, bool hideConnectionLines = true)
        {
            MethodInfo mi = this.GetType().GetMethod("show", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { type });
            mi.Invoke(this, new object[] { hideConnectionLines });
        }

        public void Show<T>(bool hideConnectionLines = true) where T : ShapeBase
        {
            show<T>(hideConnectionLines);
        }

        public void Show(ShapeBase entity, bool hideConnectionLines = true)
        {
            entity.HideItem = false;
            if (hideConnectionLines)
            {
                var tempconnections = connections.Cast<Connection>().Where(w => (w.From != null && w.From.ContainEntity == entity) || (w.To != null && w.To.ContainEntity == entity));
                foreach (Connection c in tempconnections)
                {
                    c.HideItem = false;
                }
            }
            this.Invalidate(true);
        }

        public void Save(string savePath)
        {
            SaveDataInfo data = this;
            XmlSerializer xs = new XmlSerializer(data.GetType());
            using (Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                xs.Serialize(stream, data);
            }
            SignXmlHelper.SignXml(savePath);
        }

        public bool Import(string DGPath, out string msg, bool needShapeData = true, bool needControlData = false)
        {
            //if (!SignXmlHelper.VerifyXml(DGPath))
            //{
            //    msg = "文件已被更改！";
            //    return false;
            //}

            msg = "";
            XmlSerializer xs = new XmlSerializer(typeof(SaveDataInfo));
            using (Stream stream = new FileStream(DGPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SaveDataInfo dataInfo = (xs.Deserialize(stream) as SaveDataInfo);
                if (needShapeData)
                {
                    this.CopyShapes(dataInfo);
                }

                if (needControlData)
                {
                    this.CopyControlParams(dataInfo);
                }
            }
            return true;
        }

        public void CopyControlParams(SaveDataInfo dataInfo)
        {
            this.ViewOriginPoint = dataInfo.ViewOriginPoint;
            this.LineHoveredColor = dataInfo.LineHoveredColor;
            this.LineSelectedColor = dataInfo.LineSelectedColor;
            this.LineColor = dataInfo.LineColor;
            this.BackColor = dataInfo.BackColor;
            this.ShowGrid = dataInfo.ShowGrid;
            this.LineWidth = dataInfo.LineWidth;
            this.LineMouseWidth = dataInfo.LineMouseWidth;
            this.DiagramLocked = dataInfo.DiagramLocked;
            this.BottomLineOffset = dataInfo.BottomLineOffset;
        }

        public void CopyShapes(SaveDataInfo dataInfo)
        {
            dataInfo.Shapes.ForEach(shape =>
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly selectAssemblie = assemblies.FirstOrDefault((Func<Assembly, bool>)(assemblie => assemblie.FullName == shape.AssemblyInfo));
                Type shapeType = selectAssemblie.GetType(shape.Type);
                Object[] constructParms = new object[] { };  //构造器参数
                ShapeBase createShape = (ShapeBase)Activator.CreateInstance(shapeType, constructParms);
                this.AddShape(createShape);
                createShape.ObjectID = shape.ObjectID;
                createShape.X = shape.X;
                createShape.Y = shape.Y;
                createShape.Text = shape.Text;
                createShape.BackGroundColor = shape.BackGroundColor;
                createShape.BorderColor = shape.BorderColor;
                createShape.Width = shape.Width <= 0 ? createShape.Width : shape.Width;
                createShape.Height = shape.Height <= 0 ? createShape.Height : shape.Height;
                createShape.BorderSelectedColor = shape.BorderSelectedColor;
                createShape.EnableBottomSourceConnector = shape.EnableBottomSourceConnector;
                createShape.EnableLeftSourceConnector = shape.EnableLeftSourceConnector;
                createShape.EnableRightSourceConnector = shape.EnableRightSourceConnector;
                createShape.EnableTopSourceConnector = shape.EnableTopSourceConnector;
                createShape.EnableBottomTargetConnector = shape.EnableBottomTargetConnector;
                createShape.EnableLeftTargetConnector = shape.EnableLeftTargetConnector;
                createShape.EnableRightTargetConnector = shape.EnableRightTargetConnector;
                createShape.EnableTopTargetConnector = shape.EnableTopTargetConnector;
                createShape.ShowBorder = shape.ShowBorder;
                createShape.Group = shape.Group;
                createShape.Content = shape.Content;
                createShape.TextPosition = (TextPosition)shape.TextPosition;
                createShape.Image = shape.Image;
            });

            dataInfo.Connections.ForEach(connection =>
            {
                ShapeBase shapeFrom = null;
                ShapeBase shapeTo = null;
                foreach (ShapeBase shape in shapes)
                {
                    if (connection.FromContainEntityObjectID == shape.ObjectID)
                    {
                        shapeFrom = shape;
                    }

                    if (connection.ToContainEntityObjectID == shape.ObjectID)
                    {
                        shapeTo = shape;
                    }

                    if (shapeFrom != null && shapeTo != null)
                    {
                        break;
                    }
                }

                if (shapeFrom == null || shapeTo == null)
                {
                    return;
                }

                this.AddConnection(shapeFrom.Connectors[connection.FromContainEntityIndex], shapeTo.Connectors[connection.ToContainEntityIndex]);
            });
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
            if (e.KeyCode == Keys.Delete && _enableEdit)
            {
                DeleteSelectElement();
            }
        }

        private ManualResetEvent _clickManualResetEvent = new ManualResetEvent(false);
        private int _clickCount = 0;
        private bool _isMove = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _leftMouse = e.Button == MouseButtons.Left;
            _stratPoint = e.Location;

            int entityTYpe = -1;//0:ShapeBase;1:ShapeBase.connectors;2:connections;3:connections.From;4:connections.To.
            Entity hoveredentity = shapes.Cast<ShapeBase>().FirstOrDefault(f =>
            {
                if (f.Hit(e.Location))
                {
                    return true;
                }
                return false;
            });

            if (hoveredentity == null && selectedEntity != null && selectedEntity is ShapeBase && _enableEdit)
            {
                Connector connector = (selectedEntity as ShapeBase).HitConnector(e.Location);
                if (connector != null)
                {
                    Point point = e.Location;
                    point.Offset(-this.ViewOriginPoint.GetPoint().X, -this.ViewOriginPoint.GetPoint().Y);

                    Connection connection = this.AddConnection(connector.Point, point);
                    connection.From.ContainEntity = connector.ContainEntity;
                    connection.From.ConnectorsIndexOfContainEntity = connector.ConnectorsIndexOfContainEntity;
                    UpdateSelected(connection.To);
                    connector.AttachConnector(connection.From);
                    connection.From.Offset = connection.From.ConnectorDirection == ConnectorDirection.Down ? this.BottomLineOffset : 0;
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
                            (selectedEntity as Connector).Offset = con.ConnectorDirection == ConnectorDirection.Down ? this.BottomLineOffset : 0;
                            con.AttachConnector((selectedEntity as Connector));
                            (selectedEntity as Connector).ContainEntity = con.ContainEntity;
                            (selectedEntity as Connector).ConnectorsIndexOfContainEntity = con.ConnectorsIndexOfContainEntity;
                            con.hovered = false;
                            tracking = false;
                            return;
                        }
                    }

                  (selectedEntity as Connector).Release();
                    this.DeleteElement((selectedEntity as Connector).ContainEntity);
                }
                else
                {

                }
                tracking = false;
            }

            if (!_isMove && selectedEntity is ShapeBase && !_enableEdit)
            {
                Task.Factory.StartNew((obj) => {
                    int c = Interlocked.Increment(ref _clickCount);
                    if (c == 1 && !_clickManualResetEvent.WaitOne(300))
                    {
                        if (_clickCount == 1)
                        {
                            this.Invoke(new Action(() => {
                                (obj as ShapeBase).ClickMethod();
                            }));
                        }
                        else
                        {
                            this.Invoke(new Action(() => {
                                (obj as ShapeBase).DoubleClickMethod();
                            }));
                        }

                        _clickCount = 0;
                    }
                }, selectedEntity);
            }

            _isMove = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            base.OnMouseMove(e);
            if (tracking && _enableEdit)
            {
                selectedEntity.Move(new Point(e.X - _stratPoint.GetPoint().X, e.Y - _stratPoint.GetPoint().Y));
                if (typeof(Connector).IsInstanceOfType(selectedEntity))
                {
                    for (int k = 0; k < shapes.Count; k++)
                    {
                        shapes[k].HitConnector(e.Location);
                    }
                }

                _isMove |= e.X != _stratPoint.GetPoint().X || e.Y != _stratPoint.GetPoint().Y;
            }
            else if (_leftMouse && !DiagramLocked)
            {
                _viewOriginPoint = new DiagramPoint(_viewOriginPoint.GetPoint().X + e.X - _stratPoint.GetPoint().X, _viewOriginPoint.GetPoint().Y + e.Y - _stratPoint.GetPoint().Y);
            }

            int entityTYpe = -1;//0:ShapeBase;1:ShapeBase.connectors;2:connections;3:connections.From;4:connections.To.
            Entity hoveredentity = shapes.Cast<Entity>().FirstOrDefault(f => f.Hit(e.Location));
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

        public void Clear()
        {

            this.shapes.Clear();
            this.connections.Clear();
            this.Invalidate(true);
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
            shape.ViewSizeChanged += Shape_ViewSizeChanged;
            shape.Site = this;
            this.Invalidate(true);
            return shape;
        }

        private void Shape_ViewSizeChanged(object sender, EventArgs e)
        {
            ShapeBase shapeBase = (ShapeBase)sender;
            var items = connections.Cast<Connection>().Where(w => w.To.ContainEntity == shapeBase || w.From.ContainEntity == shapeBase);
            foreach (Connection c in items)
            {
                Connector cr = c.From.ContainEntity == shapeBase ? c.From : c.To;
                if (cr.ConnectorDirection == ConnectorDirection.Down)
                {
                    cr.Move(new Point(shapeBase.BottomConnector.Point.X - cr.Point.X, shapeBase.BottomConnector.Point.Y - cr.Point.Y));
                }
                else if (cr.ConnectorDirection == ConnectorDirection.Up)
                {
                    cr.Move(new Point(shapeBase.TopConnector.Point.X - cr.Point.X, shapeBase.TopConnector.Point.Y - cr.Point.Y));
                }
                else if (cr.ConnectorDirection == ConnectorDirection.Left)
                {
                    cr.Move(new Point(shapeBase.LeftConnector.Point.X - cr.Point.X, shapeBase.LeftConnector.Point.Y - cr.Point.Y));
                }
                else if (cr.ConnectorDirection == ConnectorDirection.Right)
                {
                    cr.Move(new Point(shapeBase.RightConnector.Point.X - cr.Point.X, shapeBase.RightConnector.Point.Y - cr.Point.Y));
                }
            }
        }

        public Connection AddConnection(Connector from, Connector to)
        {
            Connection con = this.AddConnection(from.Point, to.Point);
            con.From.ContainEntity = from.ContainEntity;
            con.From.ConnectorsIndexOfContainEntity = from.ConnectorsIndexOfContainEntity;
            con.To.ContainEntity = to.ContainEntity;
            con.To.ConnectorsIndexOfContainEntity = to.ConnectorsIndexOfContainEntity;
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

        [Browsable(true), Description("是否允许编辑"), Category("Layout")]
        public bool EnableEdit
        {
            get { return this.site.EnableEdit; }
            set { this.site.EnableEdit = value; }
        }

        [Browsable(true), Description("底部连接点偏移量"), Category("Layout")]
        public int BottomLineOffset
        {
            get { return this.site.BottomLineOffset; }
            set { this.site.BottomLineOffset = value; }
        }

        [Browsable(true), Description("连线默认颜色"), Category("Layout")]
        public Color LineColor
        {
            get { return this.site.LineColor; }
            set { this.site.LineColor = value; }
        }

        [Browsable(true), Description("线宽"), Category("Layout")]
        public float LineWidth
        {
            get { return this.site.LineWidth; }
            set { this.site.LineWidth = value; }
        }

        [Browsable(true), Description("画布是否允许移动"), Category("Layout")]
        public bool DiagramLocked
        {
            get { return this.site.DiagramLocked; }
            set { this.site.DiagramLocked = value; }
        }

        [Browsable(true), Description("鼠标操作线宽"), Category("Layout")]
        public float LineMouseWidth
        {
            get { return this.site.LineMouseWidth; }
            set { this.site.LineMouseWidth = value; }
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

    public class SaveDataInfo
    {
        public Point ViewOriginPoint { get; set; }

        public int LineHoveredColorARGB
        {
            get
            {
                return LineHoveredColor.ToArgb();
            }
            set
            {
                LineHoveredColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color LineHoveredColor { get; set; } = Color.Blue;

        public int LineSelectedColorARGB
        {
            get
            {
                return LineSelectedColor.ToArgb();
            }
            set
            {
                LineSelectedColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color LineSelectedColor { get; set; } = Color.Green;

        public int LineColorARGB
        {
            get
            {
                return LineColor.ToArgb();
            }
            set
            {
                LineColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color LineColor { get; set; } = Color.Silver;

        public int BackColorARGB
        {
            get
            {
                return BackColor.ToArgb();
            }
            set
            {
                BackColor = Color.FromArgb(value);
            }
        }

        public float LineWidth { get; set; } = 0F;

        public bool DiagramLocked { get; set; } = false;
        public int BottomLineOffset { get; set; } = 0;

        public float LineMouseWidth { get; set; } = 0F;

        [XmlIgnore()]
        public Color BackColor { get; set; } = SystemColors.Control;
        public bool ShowGrid { get; set; } = false;
        public List<SaveShape> Shapes { get; set; }

        public List<SaveConnection> Connections { get; set; }

        public static implicit operator SaveDataInfo(DiagramControl diagramControl)
        {
            SaveDataInfo saveDataInfo = new SaveDataInfo();
            saveDataInfo.ViewOriginPoint = diagramControl.ViewOriginPoint.GetPoint();
            saveDataInfo.LineHoveredColor = diagramControl.LineHoveredColor;
            saveDataInfo.LineSelectedColor = diagramControl.LineSelectedColor;
            saveDataInfo.LineColor = diagramControl.LineColor;
            saveDataInfo.BackColor = diagramControl.BackColor;
            saveDataInfo.ShowGrid = diagramControl.ShowGrid;
            saveDataInfo.LineWidth = diagramControl.LineWidth;
            saveDataInfo.DiagramLocked = diagramControl.DiagramLocked;
            saveDataInfo.BottomLineOffset = diagramControl.BottomLineOffset;
            saveDataInfo.LineMouseWidth = diagramControl.LineMouseWidth;
            saveDataInfo.Shapes = new List<SaveShape>();
            saveDataInfo.Connections = new List<SaveConnection>();
            foreach (ShapeBase ShapeBase in diagramControl.ShapeCollection)
            {
                saveDataInfo.Shapes.Add(ShapeBase);
            }

            foreach (Connection connection in diagramControl.Connections)
            {
                saveDataInfo.Connections.Add(connection);
            }

            return saveDataInfo;
        }
    }

    public class SaveConnection
    {
        public string ObjectID { get; set; }
        public string FromObjectID { get; set; }
        public string FromContainEntityObjectID { get; set; }
        public int FromContainEntityIndex { get; set; } = -1;
        public string ToObjectID { get; set; }
        public string ToContainEntityObjectID { get; set; }
        public int ToContainEntityIndex { get; set; } = -1;

        public static implicit operator SaveConnection(Connection connection)
        {
            SaveConnection saveConnection = new SaveConnection();
            saveConnection.ObjectID = connection.ObjectID;
            saveConnection.FromObjectID = connection.From.ObjectID;
            saveConnection.FromContainEntityObjectID = connection.From.ContainEntity.ObjectID;
            saveConnection.FromContainEntityIndex = connection.From.ConnectorsIndexOfContainEntity;
            saveConnection.ToObjectID = connection.To.ObjectID;
            saveConnection.ToContainEntityObjectID = connection.To.ContainEntity.ObjectID;
            saveConnection.ToContainEntityIndex = connection.To.ConnectorsIndexOfContainEntity;
            return saveConnection;
        }
    }

    public class SaveShape
    {
        public static implicit operator SaveShape(ShapeBase shape)
        {
            SaveShape saveShape = new SaveShape();
            saveShape.AssemblyInfo = shape.GetType().Assembly.FullName;
            saveShape.Type = shape.GetType().FullName;
            saveShape.ObjectID = shape.ObjectID;
            saveShape.X = shape.X;
            saveShape.Y = shape.Y;
            saveShape.Width = shape.Width;
            saveShape.Height = shape.Height;
            saveShape.Text = shape.Text;
            Bitmap bitmap = shape.Image;
            saveShape.ImageBase64 = saveShape.ToBase64(shape.Image);
            saveShape.BackGroundColor = shape.BackGroundColor;
            saveShape.BorderColor = shape.BorderColor;
            saveShape.BorderSelectedColor = shape.BorderSelectedColor;
            saveShape.EnableBottomSourceConnector = shape.EnableBottomSourceConnector;
            saveShape.EnableLeftSourceConnector = shape.EnableLeftSourceConnector;
            saveShape.EnableRightSourceConnector = shape.EnableRightSourceConnector;
            saveShape.EnableTopSourceConnector = shape.EnableTopSourceConnector;
            saveShape.EnableBottomTargetConnector = shape.EnableBottomTargetConnector;
            saveShape.EnableLeftTargetConnector = shape.EnableLeftTargetConnector;
            saveShape.EnableRightTargetConnector = shape.EnableRightTargetConnector;
            saveShape.EnableTopTargetConnector = shape.EnableTopTargetConnector;
            saveShape.ShowBorder = shape.ShowBorder;
            saveShape.Group = shape.Group;
            saveShape.Content = shape.Content;
            saveShape.TextPosition = (int)shape.TextPosition;
            return saveShape;
        }
        private string ToBase64(Bitmap bmp)
        {
            try
            {
                if (bmp == null) return "";
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, bmp.RawFormat);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ImgToBase64String 转换失败 Exception:" + ex.Message);
                return "";
            }
        }

        private Bitmap Base64StringToImage(string inputStr)
        {
            try
            {
                if (string.IsNullOrEmpty(inputStr))
                {
                    return null;
                }

                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int TextPosition { get; set; } = 0;
        public string Group { get; set; }
        public string Content { get; set; }
        public string AssemblyInfo { get; set; }
        public string Type { get; set; }
        public string ObjectID { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public string Text { get; set; }

        public string ImageBase64 { get; set; }

        [XmlIgnore()]
        public Bitmap Image => Base64StringToImage(ImageBase64);

        public int BackGroundColorARGB
        {
            get
            {
                return BackGroundColor.ToArgb();
            }
            set
            {
                BackGroundColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color BackGroundColor { get; set; } = Color.White;

        public int BorderColorARGB
        {
            get
            {
                return BorderColor.ToArgb();
            }
            set
            {
                BorderColor = Color.FromArgb(value);
            }
        }
        [XmlIgnore()]
        public Color BorderColor { get; set; } = Color.Black;

        public int BorderSelectedColorARGB
        {
            get
            {
                return BorderSelectedColor.ToArgb();
            }
            set
            {
                BorderSelectedColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color BorderSelectedColor { get; set; } = Color.GreenYellow;
        public bool EnableBottomSourceConnector { get; set; } = true;
        public bool EnableLeftSourceConnector { get; set; } = true;
        public bool EnableRightSourceConnector { get; set; } = true;
        public bool EnableTopSourceConnector { get; set; } = true;
        public bool EnableBottomTargetConnector { get; set; } = true;
        public bool EnableLeftTargetConnector { get; set; } = true;
        public bool EnableRightTargetConnector { get; set; } = true;
        public bool EnableTopTargetConnector { get; set; } = true;
        public bool ShowBorder { get; set; } = true;
    }
}
