using System;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if(dt >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}