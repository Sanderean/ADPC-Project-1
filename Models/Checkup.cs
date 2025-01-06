using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ADPC_Project_1.Models
{
    public partial class Checkup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Patient ID")]
        public int Patientid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Checkup Date")]
        public DateTime Checkupdate { get; set; }

        [Required]
        [Display(Name = "Procedure Code")]
        public string Procedurecode { get; set; } = null!;

        public virtual ICollection<Medicalfile> Medicalfiles { get; set; } = new List<Medicalfile>();

        public virtual Patient? Patient { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
