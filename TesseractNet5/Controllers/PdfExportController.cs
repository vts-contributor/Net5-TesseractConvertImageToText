using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Tesseract;

namespace TesseractNet5
{
    [ApiController]
    [Route("api/")]
    public class PdfExportController : ControllerBase
    {
        private readonly ILogger<PdfExportController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PdfExportController(ILogger<PdfExportController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("/getNowDate")]
        public string GetOrders()
        {
            return DateTime.Now.ToString();
        }

        [HttpPost]
        [Route("get-text-from-image")]
        public string GetTextFromPdf(IFormFile file)
        {
            //location to testdata for eng.traineddata
            var path = _hostingEnvironment.WebRootPath + "/AppFile/TrainedData";
            //your sample image location
            var text = string.Empty;

            byte[] fileBytes = null;
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            using (var engine = new TesseractEngine(path, "vie"))
            {
                using (var img = Pix.LoadFromMemory(fileBytes))
                {
                    using (var page = engine.Process(img))
                    {
                        text = page.GetText();
                    }
                }
            }
            return text;
        }
    }
}
