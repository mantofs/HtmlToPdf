using System;
using System.Collections.Generic;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;

namespace HtmlToPdf
{

    public class PdfConverter : IPdfConverter
    {

        readonly IConverter _converter;
        public PdfConverter(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] Create(string content, object model, string[] pageBreak = null)
        {
            var pages = new List<ObjectSettings>();

            var listPages = (pageBreak == null ? new string[] { content } : content.Split(pageBreak, StringSplitOptions.RemoveEmptyEntries));

            foreach (var page in listPages)
            {
                pages.Add(new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = page,
                    WebSettings = { DefaultEncoding = "utf-8"
                              , LoadImages=true
                              , EnableJavascript = false}
                });
            }

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                }
            };

            doc.Objects.AddRange(pages);

            return _converter.Convert(doc);

        }

        public byte[] CreateWithSecurity(string content, object model, string[] pageBreak = null, string pwdUser = "", string pwdOwner = "")
        {

            var pdf = Create(content, model, pageBreak);

            PdfDocument document = PdfReader.Open(new MemoryStream(pdf));
            PdfSecuritySettings securitySettings = document.SecuritySettings;

            securitySettings.UserPassword = pwdUser;
            securitySettings.OwnerPassword = pwdOwner;

            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            using (MemoryStream pdfSecurity = new MemoryStream())
            {
                document.Save(pdfSecurity);
                return pdfSecurity.ToArray();
            }
        }
    }
}