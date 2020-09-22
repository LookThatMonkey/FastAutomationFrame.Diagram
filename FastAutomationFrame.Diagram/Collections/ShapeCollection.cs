//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :ShapeCollection.cs
//        description :
//
//        created by 张恭亮 at  2020/9/22 10:55:28
//
//======================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.Collections
{
	public class ShapeCollection : CollectionBase
	{
		public ShapeCollection() { }

		public int Add(ShapeBase shape)
		{
			return this.InnerList.Add(shape);
		}

		public ShapeBase this[int index]
		{
			get
			{
				return this.InnerList[index] as ShapeBase;
			}
		}

		public void Remove(ShapeBase shape)
		{
			this.InnerList.Remove(shape);
		}
	}
}
