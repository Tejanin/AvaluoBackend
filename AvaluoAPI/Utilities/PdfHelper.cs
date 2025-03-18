using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using IronPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using AvaluoAPI.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;

public class PdfHelper
{
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _basePath;

    public PdfHelper(
        IWebHostEnvironment webHostEnvironment,
        IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;
        _httpContextAccessor = httpContextAccessor;

        // Establece el path base en el escritorio
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string baseFolder = "AvaluoFiles";
        _basePath = Path.Combine(desktopPath, baseFolder);

        // Crea la carpeta base si no existe
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    // Renderiza una vista Razor a un string HTML.
    public async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());

        using var sw = new StringWriter();
        var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if (!viewResult.Success)
        {
            throw new InvalidOperationException($"No se encontró la vista {viewName}");
        }

        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, new TempDataDictionary(actionContext.HttpContext, _tempDataProvider), sw, new HtmlHelperOptions());
        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }

    // Genera y guarda un PDF a partir de una vista Razor.
    public async Task<string> GenerarYGuardarPdfAsync(string viewName, object model, IRutaBuilder rutaBuilder, string fileName)
    {
        // 1. Renderizar la vista Razor a HTML
        string htmlContent = await RenderViewToStringAsync(viewName, model);

        // 2. Generar PDF con IronPDF
        var renderer = new HtmlToPdf();
        var pdfDocument = renderer.RenderHtmlAsPdf(htmlContent);

        // 3. Construir la ruta final 
        string relativePath = rutaBuilder.Construir();

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw new InvalidOperationException("La ruta generada es inválida.");
        }

        string fullPath = Path.Combine(_basePath, relativePath);

        // 4. Crear el directorio si no existe
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        // 5. Guardar el PDF
        string filePath = Path.Combine(fullPath, $"{fileName}.pdf");

        pdfDocument.SaveAs(filePath);

        return filePath;
    }
}
