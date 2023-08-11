using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace MoodSensingApp.CustomValidation
{

    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedImageExtensionsAttribute(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                if (Array.Exists(_allowedExtensions, extension => extension.ToLowerInvariant() == fileExtension))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(GetErrorMessage());
        }

        private string GetErrorMessage()
        {
            return $"Only files with allowed extensions: {string.Join(", ", _allowedExtensions)} are allowed.";
        }
    }

}
