using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.PropertyGrid
{

    /// <summary>
    /// 
    /// </summary>
    internal class PGObject : ICustomTypeDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        private PropertyDescriptorCollection _propDescCol = null;
        /// <summary>
        /// 原始对象
        /// </summary>
        private object _originalObject = null;
        /// <summary>
        /// 
        /// </summary>
        private PGObjectBuilder.Settings _settings = null;

        public Component Component
        {
            get
            {
                return this._originalObject as Component;
            }
        }

        public PGObject(object originalObject, PGObjectBuilder.Settings settings)
        {
            this._originalObject = originalObject;
            this._settings = settings;
        }
        #region ICustomTypeDescriptor 成员

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        private static List<string> requiredPropList = null;

        private List<string> RequiredPropList
        {
            get
            {
                if (requiredPropList == null)
                {
                    String propList = "Text,Name,Top,Left,Width,Height,Dock,Anchor,Location,Margin,BackColor,Font,Enabled,Visible,ForeColor,TextAlign,BorderStyle,Size,MaxLength,Multilne,ReadOnly,Padding,Value,ImeMode,MaxDdate,MinDate";

                    requiredPropList = new List<string>();

                    foreach (String item in propList.Split(','))
                    {
                        requiredPropList.Add(item.ToLower());
                    }

                }

                return requiredPropList;
            }
        }


        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (_propDescCol == null)
            {
                // Get the collection of properties
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this._originalObject, attributes, true);
                _propDescCol = new PropertyDescriptorCollection(null);

                foreach (PropertyDescriptor oProp in baseProps)
                {

                    if (_settings.DisplayMode == PropertyGridCn.DisplayModeEnum.ForNormalUser &&
                        !RequiredPropList.Contains(oProp.DisplayName.ToLower()) &&
                        oProp.ToString() != "System.ComponentModel.ExtendedPropertyDescriptor" &&
                        oProp.Category != "Lookup 属性")
                    {
                        continue;
                    }

                    _propDescCol.Add(new PGPropertyDescriptor(oProp));
                    //}
                }
            }
            return _propDescCol;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            if (_propDescCol == null)
            {
                // Get the collection of properties
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this._originalObject, true);

                _propDescCol = new PropertyDescriptorCollection(null);

                foreach (PropertyDescriptor oProp in baseProps)
                {
                    if (_settings.DisplayMode == PropertyGridCn.DisplayModeEnum.ForNormalUser &&
                          !RequiredPropList.Contains(oProp.DisplayName.ToLower()) &&
                          oProp.ToString() != "System.ComponentModel.ExtendedPropertyDescriptor")
                    {
                        continue;
                    }

                    _propDescCol.Add(new PGPropertyDescriptor(oProp));
                }
            }
            return _propDescCol;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this._originalObject;
        }

        #endregion
    }
}
