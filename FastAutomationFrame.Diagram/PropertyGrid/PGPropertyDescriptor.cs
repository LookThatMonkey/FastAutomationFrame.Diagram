using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastAutomationFrame.Diagram.PropertyGrid
{
    /// <summary>
    /// 类属性描述
    /// </summary>
    internal class PGPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        private PropertyDescriptor basePropertyDescriptor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePropertyDescriptor"></param>
        public PGPropertyDescriptor(PropertyDescriptor basePropertyDescriptor)
            : base(basePropertyDescriptor)
        {
            this.basePropertyDescriptor = basePropertyDescriptor;
        }

        public override bool CanResetValue(object component)
        {
            return basePropertyDescriptor.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get { return basePropertyDescriptor.ComponentType; }
        }

        public override object GetValue(object component)
        {
            return basePropertyDescriptor.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return this.basePropertyDescriptor.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return this.basePropertyDescriptor.PropertyType; }
        }

        public override void ResetValue(object component)
        {
            this.basePropertyDescriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            this.basePropertyDescriptor.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.basePropertyDescriptor.ShouldSerializeValue(component);
        }

        public override string DisplayName
        {
            get
            {
                return PropCnNameDict.Instance[this.basePropertyDescriptor.DisplayName];
            }
        }

        public override string Category
        {
            get
            {
                return base.Category;
            }
        }

        public override string Description
        {
            get
            {
                return this.basePropertyDescriptor.Description;
            }
        }
    }
}