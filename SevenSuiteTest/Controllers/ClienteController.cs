using DbService;
using DbService.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;

namespace SevenSuiteTest.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ISeveClieInterface _seveClieInterface;
        private readonly IEstadoCivilInterface _seveEstadoCivilService;
        public ClienteController(ISeveClieInterface seveClieInterface, IEstadoCivilInterface seveEstadoCivilService) {
            _seveClieInterface = seveClieInterface;
            _seveEstadoCivilService = seveEstadoCivilService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _seveClieInterface.GetAllSeveClie();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new SeveClie();
            var estadoCiviles = await _seveEstadoCivilService.GetAllSeveClie();
            ViewBag.EstadoCivilList = estadoCiviles.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Descripcion
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cliente = await _seveClieInterface.GetSeveClieById(id);
            if (cliente == null)
            {
                return NotFound();
            }

            var estadoCiviles = await _seveEstadoCivilService.GetAllSeveClie();
            ViewBag.EstadoCivilList = estadoCiviles.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Descripcion
            }).ToList();

            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SeveClie model)
        {
           
            
                await _seveClieInterface.UpdateSeveClie(model);
                return Json(new { success = true });
            

            var estadoCiviles = await _seveEstadoCivilService.GetAllSeveClie();
            ViewBag.EstadoCivilList = estadoCiviles.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Descripcion
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _seveClieInterface.HardDelete(id);
            if (model != null)
            {
                return Json(new { success = true, message = "Cliente eliminado exitosamente." });
            }
            return Json(new { success = false, message = "Error al eliminar el cliente." });

        }
        public async Task<IActionResult> GenerateReport()
        {
          
            var reportData = await _seveClieInterface.GetAllSeveClie();
            LocalReport localReport = new LocalReport();
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "SeveClieReport.rdlc");
            localReport.ReportPath = reportPath;

            ReportDataSource reportDataSource = new ReportDataSource("SeveClieDataSet", reportData);
            localReport.DataSources.Add(reportDataSource);

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType, "SeveClieReport.pdf");
        }
    }
}
    