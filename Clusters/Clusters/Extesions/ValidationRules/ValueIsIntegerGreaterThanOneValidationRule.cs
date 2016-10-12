using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Clusters.Extesions.ValidationRules
{
    public class ValueIsIntegerGreaterThanOneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "value must exist");
            }
            int result;
            if (Int32.TryParse((string)value, NumberStyles.Any, cultureInfo, out result) == false)
            {
                return new ValidationResult(false, "value must be integer");
            }
            if (result < 2)
            {
                return new ValidationResult(false, "value must be greater than one");
            }
            return new ValidationResult(true, "validated");
        }
    }
}
