using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    internal class PropCnNameDict
    {
        private static PropCnNameDict _dict = null;
        private static Dictionary<string, string> wordDict = new Dictionary<string, string>();

        private PropCnNameDict()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var vvv = asm.GetManifestResourceNames();
            Stream sm = asm.GetManifestResourceStream("FastAutomationFrame.Diagram.PropertyGrid.PropCnName.txt");

            StreamReader reader = new StreamReader(sm);

            while (reader.Peek() != -1)
            {
                String sb = reader.ReadLine();

                if (sb.IndexOf(":") == -1)
                    continue;

                string[] kv = sb.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                string val = kv[0].ToLower().Trim();

                if (!wordDict.ContainsKey(val))
                    wordDict.Add(val, kv[1].Trim());
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String this[String key]
        {
            get
            {
                string val = key.ToLower().Trim();

                if (wordDict.ContainsKey(val))
                    return wordDict[val];

                return key;
            }
        }

        public static PropCnNameDict Instance
        {
            get
            {
                if (_dict == null)
                    _dict = new PropCnNameDict();

                return _dict;
            }
        }
    }
}
