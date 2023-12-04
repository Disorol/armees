using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    public class AttributeValid: ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not have the default value";
        public override bool IsValid(object value)
        {
            //NotDefault doesn't necessarily mean required
            if (string.IsNullOrWhiteSpace((string?)value))
            {
                return true;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }
            // non-null ref type
            return true;
        }
    }
}
