using GameProject.Enums;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class WeaponItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Typ jest wymagane.")]
        [Display(Name = "Typ")]
        public WeaponType Type { get; set; }

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

        [Required(ErrorMessage = "Pole Minimalne obrażenia jest wymagane.")]
        [Display(Name = "Minimalne obrażenia")]
        public int MinDamage { get; set; }

        [Required(ErrorMessage = "Pole Maksymalne obrażenia jest wymagane.")]
        [Display(Name = "Maksymalne obrażenia")]
        public int MaxDamage { get; set; }

        [Required(ErrorMessage = "Pole Szansa na trafienie jest wymagane.")]
        [Display(Name = "Szansa na trafienie")]
        public int ChanceToHit { get; set; }

        [Required(ErrorMessage = "Pole Szybkość atakku jest wymagane.")]
        [Display(Name = "Szybkość ataku")]
        public decimal AttackSpeed { get; set; }

        [Required(ErrorMessage = "Pole Wymagana siła obrażenia jest wymagane.")]
        [Display(Name = "Wymagana siła")]
        public int RequireStrength { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
