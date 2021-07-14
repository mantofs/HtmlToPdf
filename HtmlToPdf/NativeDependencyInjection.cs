using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdf
{
    public static class NativeDependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IPdfConverter, PdfConverter>();
        }
    }
}