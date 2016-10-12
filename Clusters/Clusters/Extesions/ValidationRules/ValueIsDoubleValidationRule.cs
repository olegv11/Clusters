using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Clusters.Extesions.ValidationRules
{
    public class ValueIsDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "value must exist");
            }
            double result;
            if (Double.TryParse((string)value, NumberStyles.Any, cultureInfo, out result) == false)
            {
                return new ValidationResult(false, "value must be double");
            }
            return new ValidationResult(true, "validated");
        }
    }
}
