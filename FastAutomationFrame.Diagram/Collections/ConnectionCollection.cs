using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.Collections
{
	public class ConnectionCollection : CollectionBase
	{
		public ConnectionCollection()
		{
		}

		public int Add(Connection con)
		{
			return this.InnerList.Add(con);
		}

		public Connection this[int index]
		{
			get
			{
				return this.InnerList[index] as Connection;
			}
		}

		public void Remove(Connection con)
		{
			this.InnerList.Remove(con);
		}
	}
}
