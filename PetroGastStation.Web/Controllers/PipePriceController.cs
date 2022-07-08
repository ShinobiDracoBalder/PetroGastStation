using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PetroGastStation.Web.DataAccess.IDBInterface;
using PetroGastStation.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetroGastStation.Web.Controllers
{
    public class PipePriceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment Environment;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PipePriceController(IUnitOfWork unitOfWork,
            IConfiguration configuration, IHostingEnvironment _environment, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            Environment = _environment;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(PipeViewModel formCollection)
        {
            try
            {
                if (formCollection.ImageFile != null)
                {
                    string rpta = "";
                    byte[] buffer;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formCollection.ImageFile.CopyTo(ms);
                        buffer = ms.ToArray();
                    }
                    List<string> listaanchos = new List<string>();
                    //using (MemoryStream ms = new MemoryStream(buffer))
                    //{
                    //    using (ExcelPackage ep = new ExcelPackage(ms))
                    //    {
                    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    //        var ew1 = ep.Workbook.Worksheets["Sheet1"];
                    //        int ncolumnas = ew1.Dimension.End.Column;
                    //        int nfilas = ew1.Dimension.End.Row;
                    //        for (int i = 1; i <= ncolumnas; i++)
                    //        {
                    //            listaanchos.Add((ew1.Column(i).Width * 7).ToString());
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(formCollection);
        }

        //public List<PipeViewModel> readExcel(string FileName)
        //{
        //    try
        //    {
        //        string FilePath = $"{_configuration["FilePath:SecretNormalFilePath"]}{FileName}";
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        List<PipeViewModel> excelData = new List<PipeViewModel>();
        //        // FileInfo existingFile = new FileInfo(FilePath);

        //        using (var pck = new ExcelPackage())
        //        {
        //            using (var stream = System.IO.File.OpenRead(FilePath))
        //            {
        //                var ws = pck.Workbook.Worksheets["Sheet1"];
        //                var tbl = new DataTable();
        //                var cells = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
        //                for (var i = cells.Start.Column; i <= cells.End.Column; i++)
        //                {
        //                    tbl.Columns.Add(cells[1, i].Value.ToString());
        //                }
        //            }
        //        }


        //        //using (ExcelPackage package = new ExcelPackage(existingFile))
        //        //{
        //        //    ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
        //        //    int colCount = worksheet.Dimension.End.Column;  //get Column Count
        //        //    int rowCount = worksheet.Dimension.End.Row;     //get row count
        //        //    foreach (ExcelWorksheet worksheet1 in package.Workbook.Worksheets)
        //        //    {

        //        //    }
        //        //    //int rowCount = worksheet.Dimension.End.Row;
        //        //    for (int row = 1; row <= rowCount; row++)
        //        //    {
        //        //        //string[] getMonthYear = worksheet.Cells[row, 4].Value.ToString().Trim().Split(' ');
        //        //        //excelData.Add(new PipeViewModel()
        //        //        //{
        //        //        //    EnrollmentNo = worksheet.Cells[row, 2].Value.ToString().Trim(),
        //        //        //    Semester = worksheet.Cells[row, 3].Value.ToString().Trim(),
        //        //        //    Month = getMonthYear[0],
        //        //        //    Year = getMonthYear[1],
        //        //        //    StatementNo = worksheet.Cells[row, 5].Value.ToString().Trim()
        //        //        //});
        //        //    }
        //        //}
        //        return excelData;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async Task<IActionResult> OnPostExporttoExcel()
        //{
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string fileName = @"Testingdummy.xlsx";
        //    string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);
        //    FileInfo file = new FileInfo(Path.Combine(webRootPath, fileName));
        //    var memoryStream = new MemoryStream();
        //    // --- Below code would create excel file with dummy data----  
        //    using (var fs = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Create, FileAccess.Write))
        //    {
        //        IWorkbook workbook = new XSSFWorkbook();
        //        ISheet excelSheet = workbook.CreateSheet("Testingdummy");

        //        IRow row = excelSheet.CreateRow(0);
        //        row.CreateCell(0).SetCellValue("CustomerId");
        //        row.CreateCell(1).SetCellValue("FirstName");
        //        row.CreateCell(2).SetCellValue("LastName");
        //        row.CreateCell(3).SetCellValue("Email");

        //        cust = from s in _context.Customers select s;
        //        int counter = 1;
        //        foreach (var customer in cust)
        //        {
        //            string FirstName = string.Empty;
        //            if (customer.FirstName.Length > 100)
        //                FirstName = customer.FirstName.Substring(0, 100);
        //            else
        //                FirstName = customer.FirstName;
        //            row = excelSheet.CreateRow(counter);
        //            row.CreateCell(0).SetCellValue(customer.CustomerId);
        //            row.CreateCell(1).SetCellValue(FirstName);
        //            row.CreateCell(2).SetCellValue(customer.LastName);
        //            row.CreateCell(3).SetCellValue(customer.Email);
        //            counter++;
        //        }
        //        workbook.Write(fs);
        //    }
        //    using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Open))
        //    {
        //        await fileStream.CopyToAsync(memoryStream);
        //    }
        //    memoryStream.Position = 0;
        //    return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //}

        [HttpPost]
        public IActionResult OnPostImportFromExcel()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    // Start creating the html which would be displayed in tabular format on the screen  
                    sb.Append("<table class='table'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return this.Content(sb.ToString());
        }
    }
}
