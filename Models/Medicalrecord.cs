using ADPC_Project_1.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models
{
    public partial class Medicalrecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Patient ID")]
        public int Patientid { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Disease name cannot exceed 255 characters.")]
        [Display(Name = "Disease Name")]
        public string Diseasename { get; set; } = null!;

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly Startdate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [EndDateGreaterThanStartDate]
        public DateOnly? Enddate { get; set; }


        public virtual Patient? Patient { get; set; }
    }
}
