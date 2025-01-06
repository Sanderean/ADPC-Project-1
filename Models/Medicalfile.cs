using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models
{
    public partial class Medicalfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Checkup ID")]
        public int Checkupid { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters.")]
        [Display(Name = "File Name")]
        public string Filename { get; set; } = null!;

        [Required]
        [StringLength(500, ErrorMessage = "File path cannot exceed 500 characters.")]
        [Display(Name = "File Path")]
        public string Filepath { get; set; } = null!;

        [Display(Name = "Upload Date")]
        [DataType(DataType.Date)]
        public DateTime? Uploaddate { get; set; }

        public virtual Checkup? Checkup { get; set; }
    }
}
