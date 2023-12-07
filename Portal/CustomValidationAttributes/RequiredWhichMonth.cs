using EPRN.Common.Enums;
using EPRN.Portal.Resources;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Portal.CustomValidationAttributes
{
    public class RequiredWhichMonth : ValidationAttribute
    {
        private readonly string _doneWaste;

        public RequiredWhichMonth(string doneWaste)
        {
            _doneWaste = doneWaste;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var enumPropertyInfo = validationContext.ObjectType.GetProperty(_doneWaste);

            if (enumPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_doneWaste}");
            }

            var enumValue = (DoneWaste)enumPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (enumValue == DoneWaste.ReprocessedIt && value == null)
            {
                return new ValidationResult(WhichQuarterResources.ErrorMessageReceived);
            }
            else if (enumValue == DoneWaste.SentItOn && value == null)
            {
                return new ValidationResult(WhichQuarterResources.ErrorMessageSent);
            }

            return ValidationResult.Success;

        }

    }
}
