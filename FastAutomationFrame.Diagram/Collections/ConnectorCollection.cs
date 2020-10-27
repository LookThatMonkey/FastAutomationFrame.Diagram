using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.Collections
{
	public class ConnectorCollection : CollectionBase
	{
		public ConnectorCollection()
		{
		}

		public int Add(Connector con)
		{
			return this.InnerList.Add(con);
		}

		public void Insert(int index, Connector con)
		{
			this.InnerList.Insert(index, con);
		}

		public Connector this[int index]
		{
			get
			{
				return this.InnerList[index] as Connector;
			}
		}

		public void Remove(Connector c)
		{
			if (this.InnerList.Contains(c))
			{
				System.Diagnostics.Trace.WriteLine("yep, removed.");
			}
			this.InnerList.Remove(c);
		}
	}
}
