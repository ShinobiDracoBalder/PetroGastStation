using Dapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.Helpers;
using PetroGastStation.Web.Models;
using PetroGastStation.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Controllers
{
    public class ExcelDataReaderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApiService _apiService;
        private readonly IDapperHelper _dapperHelper;

        public ExcelDataReaderController(IWebHostEnvironment webHostEnvironment, IApiService apiService, IDapperHelper dapperHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _apiService = apiService;
            _dapperHelper = dapperHelper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string[] QSector1 = { "PL/22421/EXP/ES/2019", "PL/23763/EXP/ES/2021", "PL/20617/EXP/ES/2017", "PL/6578/EXP/ES/2015", "PL/20772/EXP/ES/2017", "PL/21340/EXP/ES/2018", "PL/21990/EXP/ES/2018", "PL/1115/EXP/ES/2015", "PL/3376/EXP/ES/2015", "PL/19310/EXP/ES/2016", "PL/23916/EXP/ES/2021" };
            string[] QSector2 = {"PL/20126/EXP/ES/2017","PL/23763/EXP/ES/2021","PL/20617/EXP/ES/2017","PL/21340/EXP/ES/2018","PL/20769/EXP/ES/2017","PL/23759/EXP/ES/2021","PL/21990/EXP/ES2018" };
            string[] QSector3 = {"PL/22065/EXP/2019","PL/2542/EXP/ES/2015","PL/10787/EXP/ES/2015","PL/8150/EXP/ES/2015","PL/4424/EXP/ES/2015","PL/19826/EXP/ES/2016"};
            string[] QSector4 = {"PL/22827/EXP/ES/2019","PL/20062/EXP/ES/2017","PL/23510/EXP/ES/2020","PL/3301/EXP/ES/2015","PL/9977/EXP/ES/2015","PL/2397/EXP/ES/2015","PL/3389/EXP/ES/2015" };
            CreatePost createPost = new CreatePost();
            string url= "https://servicioswebintel.com";
            Response<StationGasViewModel> response2 = await _apiService.GetListAsync<StationGasViewModel>(url, "/comtecom", "/wsConsulta.php");
            createPost.FullGasStations = response2.ResultList.Where(g => QSector1.Contains(g.Permiso)).ToList();
            return View(createPost);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(CreatePost post)
        {
            List<PipeViewModel> ListPipe = new List<PipeViewModel>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (post.File != null)
            {
                post.IsResponse = true;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                //string path = Path.Combine(_webHostEnvironment.WebRootPath, "wwwroot\\Files");
                
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                //get file extension
                FileInfo fileInfo = new FileInfo(post.File.FileName);
                string fileName = fileInfo.Name; //post.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream1 = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    post.File.CopyTo(stream1);
                }
                post.IsSuccess = true;
                post.Message = "File upload successfully";

                var dataTable = new DataTable();
                using (FileStream stream = System.IO.File.Open(fileNameWithPath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader;
                    if (Path.GetExtension(fileNameWithPath).ToUpper() == ".XLS")
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    var result = excelReader.AsDataSet();
                    excelReader = ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    var dataSet = excelReader.AsDataSet(conf);
                    dataTable = dataSet.Tables[0];
                    dataTable.Rows.RemoveAt(0);
                    var worksheet = excelReader.AsDataSet().Tables["Sheet1"];
                    IList<DataRow> rows = (from DataRow r in worksheet.Rows select r).ToList();
                    var location = rows[0].ItemArray[0].ToString();
                    var rowsCount = rows.Count();
                    int rowsCounts = 0;

                    DynamicParameters parameter = new DynamicParameters();

                    var _Result = await Task.FromResult(_dapperHelper.GetOnly<SelectPipeReportViewModel>("[dbo].[SP_SelectPipeReport]", parameter,commandType: CommandType.StoredProcedure));

                    if (!_Result.IsSuccess)
                    {
                        return RedirectToAction("Index", "ExcelDataReader");
                    }
                    else 
                    {
                        if (_Result.Result.RTotal == 0)
                        {
                            rowsCounts = 1;
                        }
                        else {
                            rowsCounts++;
                        }
                    }
                    for (int i = 1; i < rowsCount - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(rows[i].ItemArray[0].ToString()) && string.IsNullOrEmpty(rows[i].ItemArray[1].ToString()))
                        {
                            var Station = rows[i].ItemArray[0].ToString();
                            StationViewModel stationView = new StationViewModel
                            {
                                itemStation = Station,
                            };
                            PipeViewModel pipeViewModel = new PipeViewModel
                            {
                                Station = stationView,
                            };
                            ListPipe.Add(pipeViewModel);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(rows[i].ItemArray[0].ToString()) && !string.IsNullOrEmpty(rows[i].ItemArray[1].ToString())&& !string.IsNullOrEmpty(rows[i].ItemArray[2].ToString()))
                            {
                                if (rows[i].ItemArray[0].ToString() != "TAR" && rows[i].ItemArray[1].ToString() != "Remision" && rows[i].ItemArray[1].ToString() != "Factura")
                                {
                                    PipeViewModel pipeViewModel = new PipeViewModel
                                    {
                                        Rows = rowsCounts,
                                        TAR = rows[i].ItemArray[0].ToString(),
                                        Remision = rows[i].ItemArray[1].ToString(),
                                        Factura = rows[i].ItemArray[2].ToString(),
                                        Fecha = rows[i].ItemArray[3].ToString(),
                                        Vencimiento = rows[i].ItemArray[4].ToString(),
                                        Combustible = rows[i].ItemArray[5].ToString(),
                                        LtsRecibidos = rows[i].ItemArray[6].ToString(),
                                        Litros = rows[i].ItemArray[7].ToString(),
                                        Subtotal = rows[i].ItemArray[8].ToString(),
                                        IVA = rows[i].ItemArray[9].ToString(),
                                        IEPS = rows[i].ItemArray[10].ToString(),
                                        Flete = rows[i].ItemArray[11].ToString(),
                                        IVAFlete = rows[i].ItemArray[12].ToString(),
                                        Ret = rows[i].ItemArray[13].ToString(),
                                        Desc = rows[i].ItemArray[14].ToString(),
                                        Total = rows[i].ItemArray[15].ToString(),
                                    };
                                    rowsCounts++;
                                    ListPipe.Add(pipeViewModel);
                                }
                                else 
                                {
                                   string item = $"{rows[i].ItemArray[0].ToString()}-{rows[i].ItemArray[1].ToString()}-{rows[i].ItemArray[2].ToString()}-{rows[i].ItemArray[3].ToString()}-{rows[i].ItemArray[4].ToString()}-{rows[i].ItemArray[5].ToString()}-{rows[i].ItemArray[6].ToString()}-{rows[i].ItemArray[7].ToString()}-{rows[i].ItemArray[8].ToString()}-{rows[i].ItemArray[9].ToString()}-{rows[i].ItemArray[10].ToString()}-{rows[i].ItemArray[11].ToString()}-{rows[i].ItemArray[12].ToString()}-{rows[i].ItemArray[13].ToString()}-{rows[i].ItemArray[14].ToString()}-{rows[i].ItemArray[15].ToString()};";
                                }
                            }
                        }
                        
                        var productName = rows[i].ItemArray[0].ToString();
                        //var quantity = Convert.ToInt32(rows[i].ItemArray[2]);
                        //var price = Convert.ToDecimal(rows[i][3]);
                        //var report = new SaleReportDto(productName, quantity, price, date, location);
                        //reports.Add(report);
                    }

                    
                    if (System.IO.Directory.Exists(fileNameWithPath))
                    {
                        Directory.Delete(fileNameWithPath);
                    }
                }
            }
            var ListGasS   = ListPipes(ListPipe);

            var Result = _dapperHelper.GetPlacesTransactionAsync<StationPipeViewModel>(ListGasS);

            return RedirectToAction("Index", "ExcelDataReader");
        }
        private List<StationPipeViewModel> ListPipes(List<PipeViewModel> ListPipe)
        {
            string itemStation = string.Empty;
            string itemStation2 = string.Empty;
            List<StationPipeViewModel> GasStation = new List<StationPipeViewModel>();
            foreach (var item in ListPipe)
            {
                if (item.Station != null)
                {
                    itemStation = string.Empty;
                    itemStation = item.Station.itemStation.ToString();
                }
                else
                {
                    itemStation2 = $"{itemStation}-{item.TAR}-{item.Remision}-{item.Factura}-{item.Fecha}-{item.Vencimiento}" +
                        $"-{item.Combustible}-{item.LtsRecibidos}-{item.Litros}-{item.Subtotal}-{item.IVA}-{item.IEPS}-{item.Flete}" +
                        $"-{item.IVAFlete}-{item.Ret}-{item.Desc}-{item.Total}";
                    StationPipeViewModel pipe = new StationPipeViewModel
                    {
                        Rows = item.Rows,
                        GasStation = itemStation,
                        TAR = string.IsNullOrEmpty(item.TAR)? "000" : item.TAR,
                        Remision = item.Remision,
                        Factura = String.IsNullOrEmpty(item.Factura)? item.Remision : item.Factura,
                        Fecha = Convert.ToDateTime(item.Fecha),
                        Vencimiento = Convert.ToDateTime(item.Vencimiento),
                        Combustible = item.Combustible,
                        LtsRecibidos = item.LtsRecibidos,
                        Litros = item.Litros,
                        Subtotal = item.Subtotal,
                        IVA = item.IVA,
                        IEPS = item.IEPS,
                        Flete = item.Flete,
                        IVAFlete = item.IVAFlete,
                        Ret = item.Ret,
                        Descuento = item.Desc,
                        Total = item.Total,
                        Deleted =0,
                        RegisterDate = DateTime.Now.ToUniversalTime(),
                    };
                    GasStation.Add(pipe);   
                    //GasStation.Add(new StationPipeViewModel
                    //{
                    //    GasStation = itemStation,
                    //    TAR = item.TAR,
                    //    Remision = item.Remision,
                    //    Factura = item.Factura,
                    //    Fecha = item.Fecha,
                    //    Vencimiento = item.Vencimiento,
                    //    Combustible = item.Combustible,
                    //    LtsRecibidos = item.LtsRecibidos,
                    //    Litros = item.Litros,
                    //    Subtotal = item.Subtotal,
                    //    IVA = item.IVA,
                    //    IEPS = item.IEPS,
                    //    Flete = item.Flete,
                    //    IVAFlete = item.IVAFlete,
                    //    Ret = item.Ret,
                    //    Desc = item.Desc,
                    //    Total = item.Total,
                    //});
                }
            }
            return GasStation;
        }
    }
}