using ClosedXML.Excel;
using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LandingPage.Controllers
{
    public class DashboardController : Controller
    {
        private readonly STMprocessDB _context;
        private const int PageSize = 20;

        public DashboardController(STMprocessDB context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            var data = _context.ProcessDatabase
                               .Take(PageSize)
                               .ToList();

            ViewBag.TotalItems = _context.ProcessDatabase.Count();
            return View("~/Views/Home/Dashboard.cshtml", data);
        }

        [HttpGet]
        public IActionResult GetPagedData(string? search, int page = 1)
        {
            var query = _context.ProcessDatabase.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(d =>
                    (d.Journee ?? "").ToLower().Contains(search) ||
                    (d.PceId ?? "").ToLower().Contains(search) ||
                    (d.CoilId ?? "").ToLower().Contains(search) ||
                    (d.Nmic ?? "").ToLower().Contains(search) ||
                    (d.Restart ?? "").ToLower().Contains(search) ||
                    (d.Nature ?? "").ToLower().Contains(search) ||
                    (d.Grade ?? "").ToLower().Contains(search) ||
                    (d.Fournisseur_brame ?? "").ToLower().Contains(search) ||
                    (d.Client ?? "").ToLower().Contains(search)
                );
            }

            var totalItems = query.Count();
            var pagedData = query.OrderBy(d => d.Journee)
                                 .Skip((page - 1) * PageSize)
                                 .Take(PageSize)
                                 .ToList();

            return Json(new
            {
                data = pagedData,
                totalItems = totalItems,
                currentPage = page,
                pageSize = PageSize
            });
        }

        private IEnumerable<ProcessDatabase> GetOptimizedData(string search)
        {
            var query = _context.ProcessDatabase.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(d =>
                    (d.Journee ?? "").ToLower().Contains(search) ||
                    (d.PceId ?? "").ToLower().Contains(search) ||
                    (d.CoilId ?? "").ToLower().Contains(search) ||
                    (d.Nmic ?? "").ToLower().Contains(search) ||
                    (d.Restart ?? "").ToLower().Contains(search) ||
                    (d.Nature ?? "").ToLower().Contains(search) ||
                    (d.Grade ?? "").ToLower().Contains(search) ||
                    (d.Fournisseur_brame ?? "").ToLower().Contains(search) ||
                    (d.Client ?? "").ToLower().Contains(search)
                );
            }

            return query.AsNoTracking()
                        .OrderBy(d => d.Journee)
                        .ToList();
        }


        [HttpGet]
        public IActionResult ExportToExcel(string search = "")
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();

                var data = GetOptimizedData(search).ToList();
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "ParametresProcess.xlsx");

                if (!System.IO.File.Exists(templatePath))
                {
                    return BadRequest(new { success = false, message = $"Template not found at path: {templatePath}" });
                }

                using var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
                using var workbook = new XLWorkbook(templateStream);
                var worksheet = workbook.Worksheet(1);

                // Désactive le recalcul auto pour éviter erreurs de formule
                workbook.CalculateMode = XLCalculateMode.Manual;

                int row = 8;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 2).Value = item.Journee;
                    worksheet.Cell(row, 3).Value = item.PceId;
                    worksheet.Cell(row, 4).Value = item.CoilId;
                    worksheet.Cell(row, 5).Value = item.StartTime?.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(row, 6).Value = item.EndTime?.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(row, 7).Value = item.NombrePass;
                    worksheet.Cell(row, 8).Value = item.Restart;

                    string restart = item.Restart ?? "";
                    worksheet.Cell(row, 9).Value = restart.Contains("1") ? 1 : 0;

                    worksheet.Cell(row, 10).Value = item.Cobble;
                    worksheet.Cell(row, 11).Value = item.Grade;
                    worksheet.Cell(row, 12).Value = item.Fournisseur_brame;
                    worksheet.Cell(row, 13).Value = item.Client;
                    worksheet.Cell(row, 14).Value = item.Poids_slab_balance_Four;

                    var poids = item.Poids_slab_balance_Four ?? 0;
                    worksheet.Cell(row, 15).Value = (poids > 12000 && poids < 30000) ? 0 : 1;

                    worksheet.Cell(row, 16).Value = item.slab_len;
                    worksheet.Cell(row, 17).Value = item.Forme_brame;
                    worksheet.Cell(row, 18).Value = item.slab_wid_min;
                    worksheet.Cell(row, 19).Value = item.slab_wid_max;
                    worksheet.Cell(row, 20).Value = item.largeur_min;
                    worksheet.Cell(row, 22).Value = item.largeur_max;
                    worksheet.Cell(row, 23).Value = item.NC_largeur;
                    worksheet.Cell(row, 24).Value = item.Variation_largeur;
                    worksheet.Cell(row, 25).Value = item.larg_moy_filtré;
                    worksheet.Cell(row, 26).Value = item.larg_min_filtré;
                    worksheet.Cell(row, 27).Value = item.larg_max_filtré;
                    worksheet.Cell(row, 28).Value = item.Defaut_sous_Largeur;
                    worksheet.Cell(row, 29).Value = item.Defaut_sur_Largeur;
                    worksheet.Cell(row, 30).Value = item.edger_use;
                    worksheet.Cell(row, 31).Value = item.Nb_pass_edger;
                    worksheet.Cell(row, 32).Value = item.Width_bias_obj;
                    worksheet.Cell(row, 33).Value = item.Width_bias;
                    worksheet.Cell(row, 34).Value = item.looper_bias;

                    row++;
                }

                // ❌ Supprimée pour éviter les erreurs de formules
                // workbook.RecalculateAllFormulas();

                using var exportStream = new MemoryStream();
                workbook.SaveAs(exportStream);
                exportStream.Position = 0;

                sw.Stop();
                Console.WriteLine($"⏱️ Excel export took: {sw.ElapsedMilliseconds} ms");

                string fileName = $"Export_Paramètres_Process_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                return File(exportStream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Export failed: {ex.Message}" });
            }
        }

    }
}