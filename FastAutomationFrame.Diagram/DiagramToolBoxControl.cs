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

using FastAutomationFrame.Diagram.FAttribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastAutomationFrame.Diagram
{
    public class DiagramToolBoxControl : Panel
    {
        static DiagramToolBoxControl()
        {
            shapeBases = new List<(ShapeBase shape, string name)>();
            if (Directory.Exists(Path.Combine(Application.StartupPath, "DiagromShapes")))
            {
                string[] fileNames = Directory.GetFiles(Path.Combine(Application.StartupPath, "DiagromShapes"), "*.dll", SearchOption.TopDirectoryOnly);
                foreach (string fileName in fileNames)
                {
                    Assembly assembly = Assembly.LoadFile(fileName);
                    ToolAssemblyAttribute toolAssemblyAttribute = assembly.GetCustomAttribute<ToolAssemblyAttribute>();
                    if (toolAssemblyAttribute != null)
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            ToolShapeAttribute ToolShapeAttribute = type.GetCustomAttribute<ToolShapeAttribute>();
                            if (ToolShapeAttribute != null && typeof(ShapeBase).IsAssignableFrom(type))
                            {
                                ShapeBase shape = (ShapeBase)Activator.CreateInstance(type, true);
                                string disPlayText = ToolShapeAttribute.DisPlayText;
                                if (string.IsNullOrEmpty(disPlayText))
                                {
                                    disPlayText = shape.ShapeName;
                                }

                                shapeBases.Add((shape, disPlayText));
                            }
                        }
                    }
                }
            }
        }

        private static List<(ShapeBase shape, string name)> shapeBases;

        public DiagramToolBoxControl()
            :base()
        {
            this.AutoScroll = true;
            foreach (var v in shapeBases)
            {
                AppendToolShape(v.shape, v.name);
            }
        }

        public void AppendToolShape(ShapeBase shape, string displayText)
        {
            DiagramToolShapeItem diagramToolShapeItem = new DiagramToolShapeItem(shape, displayText);
            this.Controls.Add(diagramToolShapeItem);
            this.Controls.SetChildIndex(diagramToolShapeItem, 0);
        }
    }
}
