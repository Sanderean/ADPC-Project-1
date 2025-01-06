using ADPC_Project_1.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADPC_Project_1.Models;

public partial class Patient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "Personal ID cannot exceed 20 characters.")]
    [Display(Name = "Personal ID")]
    public string Personalidentificationnumber { get; set; } = null!;

    [Required]
    [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
    [Display(Name = "First Name")]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(20, ErrorMessage = "Surname cannot exceed 20 characters.")]
    [Display(Name = "Last Name")]
    public string Surname { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    [DateOfBirth(ErrorMessage = "Date of Birth cannot be in the future.")]
    public DateOnly Dateofbirth { get; set; }

    [Required]
    [Display(Name = "Gender")]
    public char Sex { get; set; }

    public virtual ICollection<Checkup> Checkups { get; set; } = new List<Checkup>();

    public virtual ICollection<Medicalrecord> Medicalrecords { get; set; } = new List<Medicalrecord>();

    public override string ToString()
=> $"Patient id: {Id}, Full name: {Name} {Surname}";
}
