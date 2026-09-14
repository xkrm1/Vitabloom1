
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VitaBloom.Models
{
    public class CommunityPost
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(30)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please write something.")]
        [StringLength(200)]
        public string Message { get; set; } = "";

        public int Likes { get; set; } = 0;

        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }
    }
}
