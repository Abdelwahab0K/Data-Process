using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LandingPage.Models;
using System;
using System.Collections.Generic;

namespace LandingPage.Controllers
{
    /// <summary>
    /// Test controller for debugging and testing filter functionality.
    /// Provides a sandbox environment to test various filter combinations
    /// before implementing them in production controllers.
    /// </summary>
    [Authorize]
    public class TestFilterController : Controller
    {
        #region Fields

        private readonly ILogger<TestFilterController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TestFilterController.
        /// </summary>
        /// <param name="logger">Logger instance for tracking test operations</param>
        public TestFilterController(ILogger<TestFilterController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Action Methods

        /// <summary>
        /// Displays the test filter page with applied filters and sample results.
        /// This method serves as both the initial page load and the result display
        /// after filter parameters are applied.
        /// </summary>
        /// <param name="txtTypeFiltre">Type of filter to apply (jour, semaine, mois, pce_id, periode)</param>
        /// <param name="txtPceId"
        /// <param name="txtJour"
        /// <param name="txtSemaine"
        /// <param name="txtMois"
        /// <param name="txtPeriodeDu"
        /// <param name="txtPeriodeAu"
        /// <returns>TestFilter view with filter model and test data</returns>
        public IActionResult Index(
            string txtTypeFiltre = "jour",
            string txtPceId = "",
            string txtJour = "",
            string txtSemaine = "",
            string txtMois = "",
            string txtPeriodeDu = "",
            string txtPeriodeAu = "")
        {
            try
            {
                // Create and configure the filter model for the test page
                var filterModel = CreateTestFilterModel(
                    txtTypeFiltre, txtPceId, txtJour, txtSemaine,
                    txtMois, txtPeriodeDu, txtPeriodeAu);

                // Populate ViewBag with current filter values for the view
                PopulateViewBagWithFilterData(filterModel);

                // Generate test data based on current filter settings
                ViewBag.TestData = GenerateTestData(filterModel);

                _logger.LogInformation("Test filter page loaded with filter type: {FilterType}", txtTypeFiltre);

                // Return the test filter view with the configured model
                return View("~/Views/Home/TestFilter.cshtml", filterModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading test filter page");

                // Return a basic model in case of error to prevent page crash
                var errorModel = CreateTestFilterModel();
                return View("~/Views/Home/TestFilter.cshtml", errorModel);
            }
        }

        /// <summary>
        /// Processes filter application from the test form.
        /// Handles POST requests from the filter form and redirects to the Index action
        /// with the new filter parameters to maintain RESTful URL structure.
        /// </summary>
        /// <param name="model">Filter model containing all filter parameters from the form</param>
        /// <returns>Redirects to Index action with filter parameters as query string</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyFilter(Filtre_Multiple model)
        {
            if (model == null)
            {
                _logger.LogWarning("Received null model in ApplyFilter POST request");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _logger.LogInformation("Applying test filter with type: {FilterType}", model.TypeFiltre);

                // Redirect to Index with all filter parameters as query string
                // This maintains clean URLs and allows for bookmarking/sharing of filter states
                return RedirectToAction(nameof(Index), CreateFilterRouteValues(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying test filter");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Private Helper Methods - Model Creation

        /// <summary>
        /// Creates and initializes a test filter model with specified parameters.
        /// Sets up default values and configuration specific to the test environment.
        /// </summary>
        /// <param name="typeFiltre"
        /// <param name="pceId"
        /// <param name="jour"
        /// <param name="semaine"
        /// <param name="mois"
        /// <param name="periodeDu"
        /// <param name="periodeAu"
        /// <returns>Configured Filtre_Multiple model for testing</returns>
        private Filtre_Multiple CreateTestFilterModel(
            string typeFiltre = "jour",
            string pceId = "",
            string jour = "",
            string semaine = "",
            string mois = "",
            string periodeDu = "",
            string periodeAu = "")
        {
            var now = DateTime.Now;

            return new Filtre_Multiple
            {
                // Page configuration for test environment
                Controleur = "TestFilter",
                Page = "ApplyFilter",
                TitrePage = "Test du Filtre",
                LibelleFiltre = "Test des filtres personnalisés",
                PageExporter = null, // No export functionality for test page
                Largeur_Modal = "30%",

                // Filter configuration
                TypeFiltre = typeFiltre,
                PceId = string.IsNullOrWhiteSpace(pceId) ? "" : pceId,
                Jour = string.IsNullOrWhiteSpace(jour) ? now.ToString("yyyy-MM-dd") : jour,
                Semaine = string.IsNullOrWhiteSpace(semaine) ? GenerateCurrentWeekString() : semaine,
                Mois = string.IsNullOrWhiteSpace(mois) ? now.Month.ToString() : mois,
                PeriodeDu = string.IsNullOrWhiteSpace(periodeDu) ? now.AddDays(-7).ToString("yyyy-MM-dd") : periodeDu,
                PeriodeAu = string.IsNullOrWhiteSpace(periodeAu) ? now.ToString("yyyy-MM-dd") : periodeAu,

                // Specify which filters are visible in the test interface
                VisibleFilters = new List<string> { "pce_id", "mois", "jour" },

                // Additional test-specific configuration
                Objctifs = "1"
            };
        }

        /// <summary>
        /// Creates route values object for redirecting with filter parameters.
        /// Converts the filter model into an anonymous object suitable for route generation.
        /// </summary>
        /// <param name="model">Filter model to convert</param>
        /// <returns>Anonymous object containing route parameters</returns>
        private object CreateFilterRouteValues(Filtre_Multiple model)
        {
            return new
            {
                txtTypeFiltre = model.TypeFiltre,
                txtPceId = model.PceId,
                txtJour = model.Jour,
                txtSemaine = model.Semaine,
                txtMois = model.Mois,
                txtPeriodeDu = model.PeriodeDu,
                txtPeriodeAu = model.PeriodeAu
            };
        }

        #endregion

        #region Private Helper Methods - ViewBag Population

        /// <summary>
        /// Populates ViewBag with filter data for use in the test view.
        /// This data is used by the view to display current filter values and
        /// populate form fields with existing selections.
        /// </summary>
        /// <param name="model">Filter model containing current filter values</param>
        private void PopulateViewBagWithFilterData(Filtre_Multiple model)
        {
            ViewBag.TypeFiltre = model.TypeFiltre;
            ViewBag.PceId = model.PceId;
            ViewBag.Jour = model.Jour;
            ViewBag.Semaine = model.Semaine;
            ViewBag.Mois = model.Mois;
            ViewBag.PeriodeDu = model.PeriodeDu;
            ViewBag.PeriodeAu = model.PeriodeAu;
        }

        #endregion

        #region Private Helper Methods - Data Generation

        /// <summary>
        /// Generates current week string in ISO format (YYYY-WXX).
        /// Uses the current culture's calendar to determine week numbers.
        /// </summary>
        /// <returns>Formatted week string (e.g., "2024-W15")</returns>
        private static string GenerateCurrentWeekString()
        {
            var currentDate = DateTime.Now;
            var weekOfYear = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                currentDate,
                System.Globalization.CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);

            return $"{currentDate.Year}-W{weekOfYear:D2}";
        }

        /// <summary>
        /// Generates sample test data based on the applied filter parameters.
        /// This method creates mock data to demonstrate how the filtering system works
        /// without requiring actual database connections or real data.
        /// 
        /// In a production environment, this would be replaced with actual data service calls.
        /// </summary>
        /// <param name="filter">Filter model containing current filter settings</param>
        /// <returns>Dynamic object containing test results and metadata</returns>
        private object GenerateTestData(Filtre_Multiple filter)
        {
            // Create base test data structure
            var baseTestData = new
            {
                FilterType = filter.TypeFiltre,
                Timestamp = DateTime.Now,

                // Sample test results that would typically come from a database
                SampleResults = new[]
                {
                    new {
                        Parameter = "Test Parameter 1",
                        Value = "Result 1",
                        Status = "OK",
                        Description = "Paramètre de test basique - fonctionnel"
                    },
                    new {
                        Parameter = "Test Parameter 2",
                        Value = "Result 2",
                        Status = "Warning",
                        Description = "Paramètre avec avertissement - attention requise"
                    },
                    new {
                        Parameter = "Test Parameter 3",
                        Value = "Result 3",
                        Status = "Error",
                        Description = "Paramètre en erreur - intervention nécessaire"
                    }
                }
            };

            // Generate filter-specific display value based on the current filter type
            var filterValue = filter.TypeFiltre?.ToLower() switch
            {
                "pce_id" => $"PCE ID: {filter.PceId ?? "Non spécifié"}",
                "jour" => $"Jour: {filter.Jour ?? "Non spécifié"}",
                "semaine" => $"Semaine: {filter.Semaine ?? "Non spécifié"}",
                "mois" => $"Mois: {filter.Mois ?? "Non spécifié"}",
                "periode" => $"Période: {filter.PeriodeDu ?? "Non spécifié"} - {filter.PeriodeAu ?? "Non spécifié"}",
                _ => "Filtre par défaut - aucun filtre spécifique appliqué"
            };

            // Return combined test data with filter information
            return new
            {
                FilterType = baseTestData.FilterType,
                FilterValue = filterValue,
                SampleResults = baseTestData.SampleResults,
                Timestamp = baseTestData.Timestamp,

                // Additional metadata for testing purposes
                TestMetadata = new
                {
                    TotalResults = baseTestData.SampleResults.Length,
                    FilterApplied = !string.IsNullOrWhiteSpace(filter.TypeFiltre),
                    TestEnvironment = "Development/Testing"
                }
            };
        }

        #endregion
    }
}