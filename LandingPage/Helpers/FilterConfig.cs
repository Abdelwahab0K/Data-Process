using LandingPage.Models;

namespace LandingPage.Helpers
{
    public static class FilterConfig
    {
        public static Filtre_Multiple GetFiltersForPage(string page)
        {
            var model = new Filtre_Multiple
            {
                Page = page,
                Controleur = "Home",
                TitrePage = $"Filtres - {page}"
            };

            switch (page.ToLower())
            {
                case "testfilter":
                    model.TypeFiltre = "pce_id";

                    // ✅ Activer uniquement les filtres nécessaires
                    model.PceId = "";
                    model.Semaine = "";
                    
                    // ✅ Activer les boutons
                    model.PageExporter = null; // Pas de bouton d’export pour ce test
                    model.Objctifs = "1";

                    // ❌ Désactiver les autres filtres
                    
                    model.Jour = null;
                    model.Mois = null;
                    model.MoisAnnee = null;
                    model.SelectMoisAnnee = null;
                    model.Annee = null;
                    model.SelectAnnee = null;
                    model.PeriodeDu = null;
                    model.PeriodeAu = null;
                
                    break;

                case "analyseecarts":
                    model.TypeFiltre = "mois";
                    model.PceId = "";
                   
                    model.Jour = "";
                    model.Semaine = ""; 
                    model.Mois = "";
                    model.MoisAnnee = "";
                    model.SelectMoisAnnee = "<option value='2024'>2024</option><option value='2025'>2025</option>";
                    model.Annee = "";
                    model.SelectAnnee = "<option value='2024'>2024</option><option value='2025'>2025</option>";
                    model.PeriodeDu = "";
                    model.PeriodeAu = "";

                    model.PageExporter = "ExportAnalyseEcarts";
                    model.Objctifs = "1";
                    break;
            }

            return model;
        }
    }
}
