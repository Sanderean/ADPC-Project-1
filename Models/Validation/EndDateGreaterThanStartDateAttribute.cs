using System;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models.Validation
{
    public class EndDateGreaterThanStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var medicalRecord = (Medicalrecord)validationContext.ObjectInstance;

            if (medicalRecord.Enddate.HasValue && medicalRecord.Enddate < medicalRecord.Startdate)
            {
                return new ValidationResult("End Date cannot be earlier than Start Date.");
            }

            return ValidationResult.Success;
        }
    }
}
