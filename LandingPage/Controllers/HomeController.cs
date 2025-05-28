using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LandingPage.Models;
using System.Security.Claims;

namespace LandingPage.Controllers
{
    /// <summary>
    /// Main controller handling home page actions, authentication, and analysis views.
    /// Manages user authentication, landing pages, and analysis of deviations with filtering capabilities.
    /// </summary>
    public class HomeController : Controller
    {
        #region Fields and Constants

        private readonly ILogger<HomeController> _logger;

        // Authentication constants - Consider moving to configuration
        private const string AdminEmail = "admin@example.com";
        private const string AdminPassword = "admin123";
        private const string LoginErrorMessage = "Email ou mot de passe incorrect.";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HomeController.
        /// </summary>
        /// <param name="logger">Logger instance for logging controller activities</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Action Methods - Views

        /// <summary>
        /// Displays the main index page.
        /// </summary>
        /// <returns>Index view</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Displays the clean landing page template.
        /// </summary>
        /// <returns>LandingClean view</returns>
        public IActionResult LandingClean() => View();

        /// <summary>
        /// Displays the KPI (Key Performance Indicators) dashboard.
        /// </summary>
        /// <returns>KPI view</returns>
        public IActionResult KPI() => View();

        #endregion

        #region Authentication Methods

        /// <summary>
        /// Displays the login form.
        /// </summary>
        /// <returns>Login view</returns>
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// Processes user login authentication.
        /// Validates credentials and creates authentication cookie on success.
        /// </summary>
        /// <param name="email">User email address</param>
        /// <param name="password">User password</param>
        /// <returns>Redirects to Dashboard on success, returns to Login view with error on failure</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (IsValidCredentials(email, password))
                {
                    await SignInUserAsync(email);
                    _logger.LogInformation("User {Email} successfully logged in", email);
                    return RedirectToAction("Dashboard", "Dashboard");
                }

                _logger.LogWarning("Failed login attempt for email: {Email}", email);
                ViewBag.Error = LoginErrorMessage;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process for email: {Email}", email);
                ViewBag.Error = "Une erreur s'est produite lors de la connexion.";
                return View();
            }
        }

        /// <summary>
        /// Logs out the current user by clearing authentication cookie.
        /// </summary>
        /// <returns>Redirects to Login page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _logger.LogInformation("User logged out successfully");
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout process");
                return RedirectToAction(nameof(Login));
            }
        }

        #endregion

        #region Analysis Methods

        /// <summary>
        /// Displays the deviation analysis page with filtering capabilities.
        /// Supports multiple filter types: day, week, month, year, period, and PCE ID.
        /// </summary>
        /// <param name="txtTypeFiltre">Type of filter to apply (jour, semaine, mois, annee, periode, pce_id)</param>
        /// <param name="txtSousFiltre"
        /// <param name="txtPceId"
        /// <param name="txtJour"
        /// <param name="txtSemaine"
        /// <param name="txtMois"
        /// <param name="txtMoisAnnee"
        /// <param name="txtSelectMoisAnnee"
        /// <param name="txtSelectAnnee"
        /// <param name="txtPeriodeDu"
        /// <param name="txtPeriodeAu"
        /// <returns>AnalyseEcarts view with filtered data</returns>
        [Authorize]
        public IActionResult AnalyseEcarts(
            string txtTypeFiltre = "jour",
            string txtSousFiltre = "",
            string txtPceId = "",
            string txtJour = "",
            string txtSemaine = "",
            string txtMois = "",
            string txtMoisAnnee = "",
            string txtSelectMoisAnnee = "",
            string txtSelectAnnee = "",
            string txtPeriodeDu = "",
            string txtPeriodeAu = "")
        {
            try
            {
                var model = CreateAnalysisFilterModel();
                ApplyFilterParameters(model, txtTypeFiltre, txtSousFiltre, txtPceId,
                    txtJour, txtSemaine, txtMois, txtMoisAnnee, txtSelectMoisAnnee,
                    txtSelectAnnee, txtPeriodeDu, txtPeriodeAu);

                ViewBag.AnalysisData = GenerateAnalysisData(model);
                ViewBag.txtTypeFiltre = model.TypeFiltre;

                _logger.LogInformation("Analysis page loaded with filter type: {FilterType}", model.TypeFiltre);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading analysis page");
                return View(CreateAnalysisFilterModel());
            }
        }


        /// <summary>
        /// Processes POST request for deviation analysis with model binding.
        /// Redirects to GET action with query parameters to maintain RESTful pattern.
        /// </summary>
        /// <param name="model">Filter model containing all filter parameters</param>
        /// <returns>Redirects to GET AnalyseEcarts action with filter parameters</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnalyseEcarts(Filtre_Multiple model)
        {
            if (model == null)
            {
                _logger.LogWarning("Received null model in POST AnalyseEcarts");
                return RedirectToAction(nameof(AnalyseEcarts));
            }

            try
            {
                return RedirectToAction(nameof(AnalyseEcarts), CreateFilterRouteValues(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing analysis filter POST");
                return RedirectToAction(nameof(AnalyseEcarts));
            }
        }

        /// <summary>
        /// Exports analysis data based on applied filters.
        /// Currently redirects to analysis page - implement actual export logic as needed.
        /// </summary>
        /// <param name="txtTypeFiltre">Filter type</param>
        /// <param name="txtSousFiltre">Sub-filter</param>
        /// <param name="txtPceId">PCE ID</param>
        /// <param name="txtJour">Day filter</param>
        /// <param name="txtSemaine">Week filter</param>
        /// <param name="txtMois">Month filter</param>
        /// <param name="txtMoisAnnee">Month year</param>
        /// <param name="txtSelectMoisAnnee">Selected month-year</param>
        /// <param name="txtSelectAnnee">Selected year</param>
        /// <param name="txtPeriodeDu">Period start</param>
        /// <param name="txtPeriodeAu">Period end</param>
        /// <returns>File download or redirect to analysis page</returns>
        public IActionResult ExportAnalyseEcarts(
            string txtTypeFiltre = "jour",
            string txtSousFiltre = "",
            string txtPceId = "",
            string txtJour = "",
            string txtSemaine = "",
            string txtMois = "",
            string txtMoisAnnee = "",
            string txtSelectMoisAnnee = "",
            string txtSelectAnnee = "",
            string txtPeriodeDu = "",
            string txtPeriodeAu = "")
        {
            try
            {
                var model = CreateAnalysisFilterModel();
                ApplyFilterParameters(model, txtTypeFiltre, txtSousFiltre, txtPceId,
                    txtJour, txtSemaine, txtMois, txtMoisAnnee, txtSelectMoisAnnee,
                    txtSelectAnnee, txtPeriodeDu, txtPeriodeAu);

                var data = GenerateAnalysisData(model);

                // TODO: Implement actual export logic (Excel, CSV, PDF, etc.)
                // For now, redirecting back to analysis page
                _logger.LogInformation("Export requested for filter type: {FilterType}", model.TypeFiltre);

                return RedirectToAction(nameof(AnalyseEcarts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during export process");
                return RedirectToAction(nameof(AnalyseEcarts));
            }
        }

        #endregion

        #region Private Helper Methods - Authentication

        /// <summary>
        /// Validates user credentials against stored values.
        /// TODO: Replace with proper authentication service and hashed passwords.
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <param name="password">Password to validate</param>
        /// <returns>True if credentials are valid</returns>
        private static bool IsValidCredentials(string email, string password)
        {
            return !string.IsNullOrWhiteSpace(email) &&
                   !string.IsNullOrWhiteSpace(password) &&
                   email.Equals(AdminEmail, StringComparison.OrdinalIgnoreCase) &&
                   password == AdminPassword;
        }

        /// <summary>
        /// Creates authentication claims and signs in the user.
        /// </summary>
        /// <param name="email">User email for claims</param>
        private async Task SignInUserAsync(string email)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, email),
                new(ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        #endregion

        #region Private Helper Methods - Filter Model Management

        /// <summary>
        /// Creates and initializes a new analysis filter model with default values.
        /// </summary>
        /// <returns>Initialized Filtre_Multiple model</returns>
        private Filtre_Multiple CreateAnalysisFilterModel()
        {
            var now = DateTime.Now;

            return new Filtre_Multiple
            {
                Page = "AnalyseEcarts",
                Controleur = "Home",
                TitrePage = "ANALYSE DES ÉCARTS",
                LibelleFiltre = "Filtres d'analyse",
                PageExporter = "ExportAnalyseEcarts",
                TypeFiltre = "jour",
                SelectMoisAnnee = GenerateYearOptionsHtml(now.Year),
                SelectAnnee = GenerateYearOptionsHtml(now.Year),
                Jour = now.ToString("yyyy-MM-dd"),
                Mois = now.Month.ToString(),
                MoisAnnee = now.Year.ToString(),
                PeriodeDu = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                PeriodeAu = DateTime.Now.ToString("yyyy-MM-dd"),
                Semaine = GenerateCurrentWeekString(),
                VisibleFilters = new List<string> { "pce_id", "jour", "semaine", "mois", "annee", "periode" },
                Objctifs = "1"
            };
        }


        /// <summary>
        /// Applies filter parameters to the model, using provided values or defaults.
        /// </summary>
        /// <param name="model"
        /// <param name="txtTypeFiltre"
        /// <param name="txtSousFiltre"
        /// <param name="txtPceId"
        /// <param name="txtJour"
        /// <param name="txtSemaine">
        /// <param name="txtMois"
        /// <param name="txtMoisAnnee"
        /// <param name="txtSelectMoisAnnee"
        /// <param name="txtSelectAnnee"
        /// <param name="txtPeriodeDu"
        /// <param name="txtPeriodeAu"
        private void ApplyFilterParameters(
            Filtre_Multiple model,
            string? txtTypeFiltre = null,
            string? txtSousFiltre = null,
            string? txtPceId = null,
            string? txtJour = null,
            string? txtSemaine = null,
            string? txtMois = null,
            string? txtMoisAnnee = null,
            string? txtSelectMoisAnnee = null,
            string? txtSelectAnnee = null,
            string? txtPeriodeDu = null,
            string? txtPeriodeAu = null)
        {
            var now = DateTime.Now;

            model.TypeFiltre = txtTypeFiltre ?? "jour";
            model.PceId = txtPceId;
            model.Jour = !string.IsNullOrEmpty(txtJour) ? txtJour : model.Jour ?? now.ToString("yyyy-MM-dd");
            model.Semaine = !string.IsNullOrEmpty(txtSemaine) ? txtSemaine : model.Semaine ?? GenerateCurrentWeekString();
            model.Mois = !string.IsNullOrEmpty(txtMois) ? txtMois : model.Mois ?? now.Month.ToString();
            model.MoisAnnee = !string.IsNullOrEmpty(txtMoisAnnee) ? txtMoisAnnee : model.MoisAnnee ?? now.Year.ToString();
            model.SelectAnnee = !string.IsNullOrEmpty(txtSelectAnnee) ? txtSelectAnnee : model.SelectAnnee ?? now.Year.ToString();
            model.SelectMoisAnnee = txtSelectMoisAnnee ?? model.SelectMoisAnnee;
            model.PeriodeDu = !string.IsNullOrEmpty(txtPeriodeDu) ? txtPeriodeDu : model.PeriodeDu ?? now.AddDays(-7).ToString("yyyy-MM-dd");
            model.PeriodeAu = !string.IsNullOrEmpty(txtPeriodeAu) ? txtPeriodeAu : model.PeriodeAu ?? now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Creates route values object for redirecting with filter parameters.
        /// </summary>
        /// <param name="model">Filter model</param>
        /// <returns>Anonymous object with route values</returns>
        private object CreateFilterRouteValues(Filtre_Multiple model)
        {
            return new
            {
                txtTypeFiltre = model.TypeFiltre,
                txtPceId = model.PceId,
                txtJour = model.Jour,
                txtSemaine = model.Semaine,
                txtMois = model.Mois,
                txtMoisAnnee = model.MoisAnnee,
                txtSelectMoisAnnee = model.SelectMoisAnnee,
                txtSelectAnnee = model.SelectAnnee,
                txtPeriodeDu = model.PeriodeDu,
                txtPeriodeAu = model.PeriodeAu
            };
        }

        #endregion

        #region Private Helper Methods - Data Generation

        /// <summary>
        /// Generates current week string in ISO format (YYYY-WXX).
        /// </summary>
        /// <returns>Current week string</returns>
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
        /// Generates HTML option elements for year selection.
        /// Creates options from 5 years ago to 1 year in the future.
        /// </summary>
        /// <param name="currentYear">Current year to mark as selected</param>
        /// <returns>HTML option elements string</returns>
        private static string GenerateYearOptionsHtml(int currentYear)
        {
            var options = string.Empty;
            for (int year = currentYear - 5; year <= currentYear + 1; year++)
            {
                var selected = year == currentYear ? "selected" : "";
                options += $"<option value=\"{year}\" {selected}>{year}</option>";
            }
            return options.Trim();
        }

        /// <summary>
        /// Generates sample analysis data based on filter parameters.
        /// TODO: Replace with actual data service integration.
        /// </summary>
        /// <param name="filter">Filter parameters</param>
        /// <returns>Analysis data object</returns>
        private object GenerateAnalysisData(Filtre_Multiple filter)
        {
            var baseData = new
            {
                FilterType = filter.TypeFiltre,
                Timestamp = DateTime.Now,
                SampleData = new object[]
                {
                    new { Parameter = "Température", Expected = "1500°C", Actual = "1485°C", Deviation = -15 },
                    new { Parameter = "Pression", Expected = "2.5 bar", Actual = "2.3 bar", Deviation = -0.2 },
                    new { Parameter = "Débit", Expected = "120 L/min", Actual = "118 L/min", Deviation = -2 }
                }
            };

            return filter.TypeFiltre switch
            {
                "pce_id" => new { baseData.FilterType, FilterValue = $"PCE ID: {filter.PceId}", baseData.SampleData, baseData.Timestamp },
                "jour" => new { baseData.FilterType, FilterValue = $"Jour: {filter.Jour}", baseData.SampleData, baseData.Timestamp },
                "semaine" => new { baseData.FilterType, FilterValue = $"Semaine: {filter.Semaine}", baseData.SampleData, baseData.Timestamp },
                "mois" => new { baseData.FilterType, FilterValue = $"Mois: {filter.Mois}/{filter.MoisAnnee}", baseData.SampleData, baseData.Timestamp },
                "annee" => new { baseData.FilterType, FilterValue = $"Année: {filter.SelectAnnee}", baseData.SampleData, baseData.Timestamp },
                "periode" => new { baseData.FilterType, FilterValue = $"Période: {filter.PeriodeDu} - {filter.PeriodeAu}", baseData.SampleData, baseData.Timestamp },
                _ => new { baseData.FilterType, FilterValue = "Filtre par défaut", baseData.SampleData, baseData.Timestamp }
            };
        }

        #endregion
    }
}