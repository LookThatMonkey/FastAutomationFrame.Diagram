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
