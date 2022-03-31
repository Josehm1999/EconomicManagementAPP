using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Validations
{
    public class NumberValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object input, ValidationContext validationContext)
        {
            decimal n;
            bool result = Decimal.TryParse(input.ToString(), out n);
            if (!result)
            {
                return new ValidationResult("The field is numeric type.");
            }
            return Convert.ToDecimal(input) > 0
                ? ValidationResult.Success :
                new ValidationResult("The field is positive.");
        }
    }
}
