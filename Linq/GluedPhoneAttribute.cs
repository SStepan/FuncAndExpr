using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Linq {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class GluedPhoneAttribute : ValidationAttribute {
		private const string Pattern = @"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$";

		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			bool isValid = false;

			string stringValue = null;
			if (value != null)
				stringValue = value.ToString();

			if (string.IsNullOrEmpty(stringValue) || Regex.IsMatch(stringValue, Pattern))
				isValid = true;

			return isValid
				? ValidationResult.Success
				: new ValidationResult("Incorrect number");
		}
	}
}
