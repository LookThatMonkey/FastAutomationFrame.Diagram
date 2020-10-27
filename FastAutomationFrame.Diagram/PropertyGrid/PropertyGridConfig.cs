using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    public static class PropertyGridConfig
    {
        public static UITypeEditorEditStyle UITypeEditorEditStyle { get; private set; } = UITypeEditorEditStyle.Modal;
        public static Type DataType { get; private set; } = typeof(string);
        public static Type FormType { get; private set; } = typeof(Form1);
        public static void SetFormType<T>() where T : PropertyGridBaseForm
        {
            FormType = typeof(T);
        }
        public static void SetDataType<T>()
        {
            DataType = typeof(T);
        }
        public static void SetUITypeEditorEditStyle(UITypeEditorEditStyle uITypeEditorEditStyle)
        {
            UITypeEditorEditStyle = uITypeEditorEditStyle;
        }
    }
}
