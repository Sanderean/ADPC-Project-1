using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models
{
    public partial class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Checkup ID")]
        public int Checkupid { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Medication name cannot exceed 255 characters.")]
        [Display(Name = "Medication Name")]
        public string Medicationname { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "Dosage cannot exceed 100 characters.")]
        [RegularExpression(@"^\d+(\.\d+)?\s?(mg|ml|tablet|capsule)$", ErrorMessage = "Invalid dosage format. Use the format 'number unit' (e.g., '500 mg').")]
        [Display(Name = "Dosage")]
        public string Dosage { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "Duration cannot exceed 100 characters.")]
        [RegularExpression(@"^\d+\s*(days?|weeks?|months?)$", ErrorMessage = "Invalid duration format. Use 'X days', 'Y weeks', or 'Z months'.")]
        [Display(Name = "Duration")]
        public string Duration { get; set; } = null!;

        public virtual Checkup? Checkup { get; set; }

    }
}
