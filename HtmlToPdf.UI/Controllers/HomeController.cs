using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToPdf.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {

        private readonly IPdfConverter _pdfConverter;
        public HomeController(IPdfConverter pdfConverter)
        {
            _pdfConverter = pdfConverter;
        }

        [HttpGet]
        [Produces("application/pdf")]
        public FileResult GetActionResult(string url = "https://www.google.com.br")
        {
            WebClient client = new WebClient();

            var bytes = client.DownloadData(new Uri(url));

            var content = Encoding.Default.GetString(bytes);

            var pdf = _pdfConverter.Create(content, null);

            return File(pdf, "application/pdf", $"{Guid.NewGuid()}.pdf");

        }
    }
}
