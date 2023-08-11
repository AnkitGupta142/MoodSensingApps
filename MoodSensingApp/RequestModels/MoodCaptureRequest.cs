using Microsoft.AspNetCore.Http;
using MoodSensingApp.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.RequestModels
{
    public class MoodCaptureRequest
    {
        public int UserId { get; set; }

        [Required]
        [AllowedImageExtensions(".jpg", ".jpeg", ".png")]
        public IFormFile Image { get; set; }
        
        public string Mood { get; set; }
    }
}
