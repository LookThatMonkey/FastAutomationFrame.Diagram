//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :DiagramToolBoxControl.cs
//        description :
//
//        created by 张恭亮 at  2020/9/22 10:55:28
//
//======================================================================

using FastAutomationFrame.Diagram.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastAutomationFrame.Diagram
{
    public class DiagramToolBoxControl : Panel
    {
        public DiagramToolBoxControl()
            :base()
        {
            this.AutoScroll = true;
            AppendToolShape(new OvalShape());
            AppendToolShape(new SimpleRectangle());
        }

        public void AppendToolShape(ShapeBase shape)
        {
            DiagramToolShapeItem diagramToolShapeItem = new DiagramToolShapeItem(shape);
            this.Controls.Add(diagramToolShapeItem);
            this.Controls.SetChildIndex(diagramToolShapeItem, 0);
        }
    }
}
