//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :ToolShapeAttribute.cs
//        description :
//
//        created by 张恭亮 at  2020/10/14 19:55:50
//
//======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.FAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ToolShapeAttribute : Attribute
    {
        public string DisPlayText { get; set; }
    }
}
