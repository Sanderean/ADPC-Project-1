using System;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models.Validation
{
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly date)
            {
                return date <= DateOnly.FromDateTime(DateTime.Today);
            }
            return false;
        }
    }
}
