using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class CustomEditor : System.Drawing.Design.UITypeEditor
    {
        public CustomEditor()
        {
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (value != null && value.GetType() != PropertyGridConfig.DataType)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                object[] args = new object[] { value };
                Form form = (Form)Activator.CreateInstance(PropertyGridConfig.FormType, args);
                if (edSvc.ShowDialog(form) == System.Windows.Forms.DialogResult.Yes && ((PropertyGridBaseForm)form).Dval != null && ((PropertyGridBaseForm)form).Dval.GetType() == PropertyGridConfig.DataType)
                    return ((PropertyGridBaseForm)form).Dval;
            }


            return value;
        }

        //下面两个函数是为了在PropertyGrid中显示一个辅助的效果
        //可以不用重写
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
        }

        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
