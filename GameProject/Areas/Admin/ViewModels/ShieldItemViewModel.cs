using GameProject.Enums;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class ShieldItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole Poziom jakości jest wymagane.")]
        [Display(Name = "Poziom jakości")]
        public int QualityLevel { get; set; }

        [Required(ErrorMessage = "Pole Wytrzymałość jest wymagane.")]
        [Display(Name = "Wytrzymałość")]
        public int Durability { get; set; }

        [Required(ErrorMessage = "Pole Cena jest wymagane.")]
        [Display(Name = "Cena")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Pole Pancerz jest wymagane.")]
        [Display(Name = "Pancerz")]
        public int Armor { get; set; }

        [Required(ErrorMessage = "Pole Wymagana siła obrażenia jest wymagane.")]
        [Display(Name = "Wymagana siła")]
        public int RequireStrength { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
