using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    /// <summary>
    /// 
    /// </summary>
    internal class PGObjectBuilder
    {
        public static PGObject Build(object obj, Settings settings)
        {
            return new PGObject(obj, settings);
        }


        public static PGObject[] Build(object[] objs, Settings settings)
        {
            List<PGObject> retObjs = new List<PGObject>();

            foreach (object obj in objs)
            {
                retObjs.Add(Build(obj, settings));
            }

            return retObjs.ToArray();
        }

        public class Settings
        {
            public PropertyGridCn.DisplayModeEnum DisplayMode { get; set; }
        }
    }
}
