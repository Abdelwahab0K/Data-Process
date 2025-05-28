using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LandingPage.Models
{
    public class Filtre_Multiple
    {
        #region Page Configuration
        public string Page { get; set; } = string.Empty;
        public string Controleur { get; set; } = string.Empty;
        public string TitrePage { get; set; } = string.Empty;
        public string LibelleFiltre { get; set; } = string.Empty;
        public string? PageExporter { get; set; }
        public string Largeur_Modal { get; set; } = "100%";
        public List<string> VisibleFilters { get; set; } = new List<string>();
        #endregion

        #region Filter Configuration
        public string? TypeFiltre { get; set; }
     
        #endregion

        #region Filter Values

        // Identifiants spécifiques
        public string? PceId { get; set; }

        // Filtres temporels
        [DataType(DataType.Date)]
        public string? Jour { get; set; }

        public string? Semaine { get; set; }
        public string? Mois { get; set; }
        public string? MoisAnnee { get; set; }
        public string? SelectMoisAnnee { get; set; }

        public string? Annee { get; set; }
        public string? SelectAnnee { get; set; }

        [DataType(DataType.Date)]
        public string? PeriodeDu { get; set; }

        [DataType(DataType.Date)]
        public string? PeriodeAu { get; set; }
        #endregion

        #region Additional Properties
        public string? Objctifs { get; set; }
        #endregion

        #region Constructor
        public Filtre_Multiple()
        {
            TypeFiltre = "jour";
            var now = DateTime.Now;

            Jour = now.ToString("yyyy-MM-dd");
            Mois = now.Month.ToString();
            MoisAnnee = now.Year.ToString();
            PeriodeDu = now.AddDays(-7).ToString("yyyy-MM-dd");
            PeriodeAu = now.ToString("yyyy-MM-dd");

            // Semaine courante formatée
            var weekOfYear = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                now,
                System.Globalization.CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);
            Semaine = $"{now.Year}-W{weekOfYear:D2}";
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Vérifie si un type de filtre est actif.
        /// </summary>
        public bool IsFilterActive(string filterType)
        {
            return TypeFiltre?.Equals(filterType, StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Retourne une chaîne de texte représentant la valeur actuelle du filtre.
        /// </summary>
        public string GetFilterDisplayValue()
        {
            return TypeFiltre?.ToLower() switch
            {
                "pce_id" => $"PCE ID: {PceId}",
                "jour" => $"Jour: {Jour}",
                "semaine" => $"Semaine: {Semaine}",
                "mois" => $"Mois: {Mois}/{MoisAnnee}",
                "annee" => $"Année: {SelectAnnee}",
                "periode" => $"Période: {PeriodeDu} - {PeriodeAu}",
                _ => "Aucun filtre"
            };  
        }

        /// <summary>
        /// Vérifie que les champs requis pour le filtre sont valides.
        /// </summary>
        public bool IsValidFilter()     
        {
            return TypeFiltre?.ToLower() switch
            {
                "pce_id" => !string.IsNullOrWhiteSpace(PceId),
                "jour" => !string.IsNullOrWhiteSpace(Jour),
                "semaine" => !string.IsNullOrWhiteSpace(Semaine),
                "mois" => !string.IsNullOrWhiteSpace(Mois) && !string.IsNullOrWhiteSpace(MoisAnnee),
                "annee" => !string.IsNullOrWhiteSpace(SelectAnnee),
                "periode" => !string.IsNullOrWhiteSpace(PeriodeDu) && !string.IsNullOrWhiteSpace(PeriodeAu),
                _ => true // Par défaut, filtre toujours valide
            };
        }

        #endregion
    }
}
