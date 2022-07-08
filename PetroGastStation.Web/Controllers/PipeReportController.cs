using Dapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class PipeReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApiService _apiService;
        private readonly IDapperHelper _dapperHelper;

        public PipeReportController(IWebHostEnvironment webHostEnvironment, IApiService apiService, IDapperHelper dapperHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _apiService = apiService;
            _dapperHelper = dapperHelper;
        }

        public async Task<IActionResult> Index()
        {
            DynamicParameters parameter = new DynamicParameters();
            return View(await Task.FromResult(_dapperHelper.GetAll<PipeReportViewModel>("[dbo].[stpGetPipeReports]", parameter, commandType: CommandType.StoredProcedure)));
        }
        [HttpGet]
        public IActionResult Upload()
        {
            CreatePost createPost = new CreatePost();
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
               var ListResponse = await ExcelReaderFactories(post.File);
                if (!ListResponse.IsSuccess)
                {
                    post.IsResponse = false;
                    post.IsSuccess = false;
                    post.Message = ListResponse.Message;
                    ModelState.AddModelError(string.Empty, ListResponse.Message);
                    return View(post);
                }
                List<StationPipeViewModel> DatabasePipe = new List<StationPipeViewModel>();
                GenericResponse<StationPipeViewModel> DatabasePipes = new GenericResponse<StationPipeViewModel>();
                DynamicParameters parameter = new DynamicParameters();
                string SqlDb = $"Select pr.PipeReportId,pr.RazonSocial, pr.NumeroPermiso,Apelativo, pr.TAR,pr.Remision,pr.Factura, pr.Fecha, pr.Vencimiento,pr.Combustible,"+
                               $"pr.LtsRecibidos, pr.Litros, pr.Subtotal, pr.IVA, pr.IEPS, pr.Flete, pr.IVAFlete, pr.Ret, pr.Descuento, pr.Total, pr.Rows from dbo.TblPipeReports pr " +
                               $" Where fecha Between DATEADD(DAY,-40,GETDATE()) and GETDATE();";

                DatabasePipes = await _dapperHelper.GetAllAsync<StationPipeViewModel>(SqlDb, parameter, commandType: CommandType.Text);

                if (!DatabasePipes.IsSuccess)
                {
                    post.IsResponse = false;
                    post.IsSuccess = false;
                    post.Message = ListResponse.Message;
                    ModelState.AddModelError(string.Empty, ListResponse.Message);
                    return View(post);
                }

               DatabasePipe  = DatabasePipes.ListResults;

               var ListResult =  GetDatabasePiperepot(DatabasePipe, ListResponse.ListResults);

                Response<StationPipeViewModel> DataResponse = await _dapperHelper.GetPlacesTransactionAsync<StationPipeViewModel>(ListResult.AddDatabasePipe);

                Response<StationPipeViewModel> DataResponseII = await _dapperHelper.GetPlacesModifTransactionAsync<StationPipeViewModel>(ListResult.ModDatabasePipe);


                if (!DataResponse.IsSuccess || !DataResponseII.IsSuccess)
                {
                    post.IsResponse = false;
                    post.IsSuccess = false;
                    post.Message = DataResponse.Message;
                    ModelState.AddModelError(string.Empty, DataResponse.Message);
                    return View(post);
                }

            }
            return RedirectToAction("Index", "PipeReport");
        }
        //[HttpPost]
        //public async Task<IActionResult> Upload(CreatePost post)
        //{
        //    List<PipeViewModel> ListPipe = new List<PipeViewModel>();
        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    if (post.File != null) 
        //    {
        //        post.IsResponse = true;

        //       await ExcelReaderFactories(post.File);

        //        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
        //        //create folder if not exist
        //        if (!Directory.Exists(path))
        //            Directory.CreateDirectory(path);

        //        //get file extension
        //        FileInfo fileInfo = new FileInfo(post.File.FileName);
        //        string fileName = fileInfo.Name; //post.FileName + fileInfo.Extension;

        //        string fileNameWithPath = Path.Combine(path, fileName);
        //        using (var stream1 = new FileStream(fileNameWithPath, FileMode.Create))
        //        {
        //            post.File.CopyTo(stream1);
        //        }
        //        post.IsSuccess = true;
        //        post.Message = "File upload successfully";

        //        var dataTable = new DataTable();
        //        using (FileStream stream = System.IO.File.Open(fileNameWithPath, FileMode.Open, FileAccess.Read)) 
        //        {
        //            IExcelDataReader excelReader;
        //            if (Path.GetExtension(fileNameWithPath).ToUpper() == ".XLS")
        //            {
        //                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        //            }
        //            else
        //            {
        //                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //            }

        //            var result = excelReader.AsDataSet();
        //            excelReader = ExcelReaderFactory.CreateReader(stream);

        //            var conf = new ExcelDataSetConfiguration
        //            {
        //                ConfigureDataTable = _ => new ExcelDataTableConfiguration
        //                {
        //                    UseHeaderRow = true
        //                }
        //            };

        //            var dataSet = excelReader.AsDataSet(conf);
        //            dataTable = dataSet.Tables[0];
        //            dataTable.Rows.RemoveAt(0);
        //            var worksheet = excelReader.AsDataSet().Tables["Sheet1"];
        //            IList<DataRow> rows = (from DataRow r in worksheet.Rows select r).ToList();
        //            var location = rows[0].ItemArray[0].ToString();
        //            var rowsCount = rows.Count();
        //            int rowsCounts = 0;

        //            DynamicParameters parameter = new DynamicParameters();

        //            var _Result = await Task.FromResult(_dapperHelper.GetOnly<SelectPipeReportViewModel>("[dbo].[SP_SelectPipeReport]", parameter, commandType: CommandType.StoredProcedure));

        //            if (!_Result.IsSuccess)
        //            {
        //                return RedirectToAction("Index", "PipeReport");
        //            }
        //            else
        //            {
        //                if (_Result.Result.RTotal == 0)
        //                {
        //                    rowsCounts = 1;
        //                }
        //                else
        //                {
        //                    rowsCounts++;
        //                }
        //            }
        //            for (int i = 1; i < rowsCount - 1; i++)
        //            {
        //                if (!string.IsNullOrEmpty(rows[i].ItemArray[0].ToString()) && string.IsNullOrEmpty(rows[i].ItemArray[1].ToString()))
        //                {
        //                    var Station = rows[i].ItemArray[0].ToString();
        //                    StationViewModel stationView = new StationViewModel
        //                    {
        //                        itemStation = Station,
        //                    };
        //                    PipeViewModel pipeViewModel = new PipeViewModel
        //                    {
        //                        Station = stationView,
        //                    };
        //                    ListPipe.Add(pipeViewModel);
        //                }
        //                else
        //                {
        //                    if (!string.IsNullOrEmpty(rows[i].ItemArray[3].ToString()) && !string.IsNullOrEmpty(rows[i].ItemArray[4].ToString()) && !string.IsNullOrEmpty(rows[i].ItemArray[5].ToString()))
        //                    {
        //                        //if (rows[i].ItemArray[0].ToString() != "TAR" && rows[i].ItemArray[1].ToString() != "Remision" && rows[i].ItemArray[1].ToString() != "Factura")
        //                        //if (rows[i].ItemArray[1].ToString() != "Remision" && rows[i].ItemArray[1].ToString() != "Factura")
        //                        if (rows[i].ItemArray[1].ToString() != "Remision" && rows[i].ItemArray[2].ToString() != "Factura")
        //                        {
        //                            if (rows[i].ItemArray[2].ToString() == "FACTURA10019274")
        //                            {
        //                                string FACTURA10019274 = "Listo";
        //                            }
        //                            PipeViewModel pipeViewModel = new PipeViewModel
        //                            {
        //                                Rows = rowsCounts,
        //                                TAR = rows[i].ItemArray[0].ToString(),
        //                                Remision = rows[i].ItemArray[1].ToString(),
        //                                Factura = rows[i].ItemArray[2].ToString(),
        //                                Fecha = rows[i].ItemArray[3].ToString(),
        //                                Vencimiento = rows[i].ItemArray[4].ToString(),
        //                                Combustible = rows[i].ItemArray[5].ToString(),
        //                                LtsRecibidos = rows[i].ItemArray[6].ToString(),
        //                                Litros = rows[i].ItemArray[7].ToString(),
        //                                Subtotal = rows[i].ItemArray[8].ToString(),
        //                                IVA = rows[i].ItemArray[9].ToString(),
        //                                IEPS = rows[i].ItemArray[10].ToString(),
        //                                Flete = rows[i].ItemArray[11].ToString(),
        //                                IVAFlete = rows[i].ItemArray[12].ToString(),
        //                                Ret = rows[i].ItemArray[13].ToString(),
        //                                Desc = rows[i].ItemArray[14].ToString(),
        //                                Total = rows[i].ItemArray[15].ToString(),
        //                            };
        //                            rowsCounts++;
        //                            ListPipe.Add(pipeViewModel);
        //                        }
        //                        else
        //                        {
        //                            string item = $"{rows[i].ItemArray[0].ToString()}-{rows[i].ItemArray[1].ToString()}-{rows[i].ItemArray[2].ToString()}-{rows[i].ItemArray[3].ToString()}-{rows[i].ItemArray[4].ToString()}-{rows[i].ItemArray[5].ToString()}-{rows[i].ItemArray[6].ToString()}-{rows[i].ItemArray[7].ToString()}-{rows[i].ItemArray[8].ToString()}-{rows[i].ItemArray[9].ToString()}-{rows[i].ItemArray[10].ToString()}-{rows[i].ItemArray[11].ToString()}-{rows[i].ItemArray[12].ToString()}-{rows[i].ItemArray[13].ToString()}-{rows[i].ItemArray[14].ToString()}-{rows[i].ItemArray[15].ToString()};";
        //                        }
        //                    }
        //                }
        //                var productName = rows[i].ItemArray[0].ToString();
        //            }
        //            if (System.IO.Directory.Exists(fileNameWithPath))
        //            {
        //                Directory.Delete(fileNameWithPath);
        //            }
        //        }
        //    }
        //    var ListGasS = ListPipes(ListPipe);

        //    var Result = _dapperHelper.GetPlacesTransactionAsync<StationPipeViewModel>(ListGasS);
        //    return RedirectToAction("Index", "PipeReport");
        //}

        public async Task<IActionResult> PipeDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DynamicParameters parameter = new DynamicParameters();
            var SqlQuery = $"{"Select [PipeReportId],	[RazonSocial],	[NumeroPermiso],	[Apelativo],	[Rows],	[TAR],	[Remision],	[Factura],	[Fecha],[Vencimiento],[Combustible],[LtsRecibidos],[Litros],[Subtotal],[IVA],[IEPS],[Flete],[IVAFlete],[Ret],[Descuento],[Total],[Deleted],	[RegisterDate] "}{"from [dbo].[TblPipeReports] Where PipeReportId ="}{id} ";
            PipeReportViewModel PipeReport = await Task.FromResult(_dapperHelper.Get<PipeReportViewModel>(SqlQuery,parameter, commandType: CommandType.Text));
            if (PipeReport == null)
            {
                return NotFound();
            }

            return View(PipeReport);
        }

        private List<StationPipeViewModel> ListPipes(List<PipeViewModel> ListPipe)
        {

            string itemStation = string.Empty;
            string itemStation2 = string.Empty;
            List<StationPipeViewModel> GasStation = new List<StationPipeViewModel>();

            try
            {
               
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
                            TAR = string.IsNullOrEmpty(item.TAR) ? "000" : item.TAR,
                            Remision = item.Remision,
                            Factura = String.IsNullOrEmpty(item.Factura) ? item.Remision : item.Factura,
                            Fecha = Convert.ToDateTime(item.Fecha.ToString()),
                            Vencimiento = Convert.ToDateTime(item.Vencimiento.ToString()),
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
                            Deleted = 0,
                            RegisterDate = DateTime.Now.ToUniversalTime(),
                        };
                        GasStation.Add(pipe);
                    }
                }
            }
            catch (Exception excp)
            {

                throw;
            }
            
            return GasStation;
        }

        private async Task<GenericResponse<StationPipeViewModel>> ExcelReaderFactories(IFormFile File)
        {
            try
            {
                List<PipeViewModel> ListPipe = new List<PipeViewModel>();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\GasFigues");
                //get file extension
                FileInfo fileInfo = new FileInfo(File.FileName);
                string fileName = fileInfo.Name; //post.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream1 = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    File.CopyTo(stream1);
                }

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (Stream stream = System.IO.File.Open(fileNameWithPath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader;
                    string extension = Path.GetExtension(fileNameWithPath);

                    if (extension == ".xls")
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (extension == ".xlsx")
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        throw new NotSupportedException("Wrong file extension");
                    }

                    DataSet resultSet = excelReader.AsDataSet();
                    excelReader = ExcelReaderFactory.CreateReader(stream);

                   DataTable dataTable = resultSet.Tables[0];
                    int rowsCounts = 0;
                    DynamicParameters parameter = new DynamicParameters();

                    var _Result = await Task.FromResult(_dapperHelper.GetOnly<SelectPipeReportViewModel>("[dbo].[SP_SelectPipeReport]", parameter, commandType: CommandType.StoredProcedure));

                    if (!_Result.IsSuccess)
                    {
                        //return RedirectToAction("Index", "PipeReport");
                    }
                    else
                    {
                        if (_Result.Result.RTotal == 0)
                        {
                            rowsCounts = 1;
                        }
                        else
                        {
                            rowsCounts = _Result.Result.RTotal+1;
                        }
                    }
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row.ItemArray[0].ToString()!= "Fecha de Impresión" && row.ItemArray[1].ToString() != "COMPRAS DE COMBUSTIBLE")
                        {
                            if (!string.IsNullOrEmpty(row.ItemArray[0].ToString()) && string.IsNullOrEmpty(row.ItemArray[1].ToString()))
                            {
                                var Station = row.ItemArray[0].ToString();
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
                                if (row.ItemArray[0].ToString() != "TAR" && row.ItemArray[1].ToString() != "Remision" && row.ItemArray[2].ToString() != "Factura")
                                {
                                    
                                    if (!string.IsNullOrEmpty(row.ItemArray[3].ToString()) && !string.IsNullOrEmpty(row.ItemArray[4].ToString())&& !string.IsNullOrEmpty(row.ItemArray[5].ToString()))
                                    {

                                        if (row.ItemArray[1].ToString().ToUpper() == "MAGNA" || row.ItemArray[1].ToString().ToUpper() == "PREMIUM" || row.ItemArray[1].ToString().ToUpper() == "DIESEL")
                                        {
                                            return new GenericResponse<StationPipeViewModel>
                                            {
                                                IsSuccess = false,
                                                Message = $"en el campo Remision esta no cuenta con la informacion correcto !  {row.ItemArray[1].ToString().ToUpper()}",
                                            };
                                        }
                                        rowsCounts++;

                                        PipeViewModel pipeViewModel = new PipeViewModel
                                        {
                                            Rows = rowsCounts,
                                            TAR = row.ItemArray[0].ToString(),
                                            Remision = row.ItemArray[1].ToString(),
                                            Factura = row.ItemArray[2].ToString(),
                                            Fecha = row.ItemArray[3].ToString(),
                                            Vencimiento = row.ItemArray[4].ToString(),
                                            Combustible = row.ItemArray[5].ToString(),
                                            LtsRecibidos = row.ItemArray[6].ToString(),
                                            Litros = row.ItemArray[7].ToString(),
                                            Subtotal = row.ItemArray[8].ToString(),
                                            IVA = row.ItemArray[9].ToString(),
                                            IEPS = row.ItemArray[10].ToString(),
                                            Flete = row.ItemArray[11].ToString(),
                                            IVAFlete = row.ItemArray[12].ToString(),
                                            Ret = row.ItemArray[13].ToString(),
                                            Desc = row.ItemArray[14].ToString(),
                                            Total = row.ItemArray[15].ToString(),
                                            
                                        };
                                        
                                        ListPipe.Add(pipeViewModel);
                                    }
                                }
                            }
                        }
                    }
                }
                var ListGasS = ListPipes(ListPipe);
                //var Result = _dapperHelper.GetPlacesTransactionAsync<StationPipeViewModel>(ListGasS);
                return new GenericResponse<StationPipeViewModel>
                {
                    IsSuccess = true,
                    ListResults = ListGasS.ToList(),
                };
            }
            catch (Exception exception)
            {
                return new  GenericResponse<StationPipeViewModel>{
                    IsSuccess = false,
                    Message = exception.Message,
                };
            }
        }
        //private async Task<GenericResponse<StationPipeViewModel>> GetDatabasePiperepot(List<StationPipeViewModel> gasPiperepoorts)
        private StationPipeUpdateAddViewModel GetDatabasePiperepot(List<StationPipeViewModel> DbgasPiperepoorts, List<StationPipeViewModel> FilegasPiperepoorts)
        {
            List<StationPipeViewModel> AddDatabasePipe = new List<StationPipeViewModel>();
            List<StationPipeViewModel> ModDatabasePipe = new List<StationPipeViewModel>();

            int rowsCounts = 0;
            DynamicParameters parameter = new DynamicParameters();

            var _Result = _dapperHelper.GetOnly<SelectPipeReportViewModel>("[dbo].[SP_SelectPipeReport]", parameter, commandType: CommandType.StoredProcedure);

            if (!_Result.IsSuccess)
            {
                //return RedirectToAction("Index", "PipeReport");
            }
            else
            {
                if (_Result.Result.RTotal == 0)
                {
                    rowsCounts = 1;
                }
                else
                {
                    rowsCounts = _Result.Result.RTotal+1;
                }
            }

            foreach (var item in FilegasPiperepoorts.ToList().OrderBy(x=> x.Rows))
            {
                var Items = DbgasPiperepoorts
                    .FirstOrDefault(y=> y.Remision.Equals(item.Remision)|| y.Factura.Equals(item.Factura) && y.Litros.Equals(item));

                if (Items != null)
                {
                    StationPipeViewModel Modif = new StationPipeViewModel
                    {
                        PipeReportId = Items.PipeReportId,
                        RazonSocial = item.RazonSocial, 
                        NumeroPermiso = Items.RazonSocial,
                        Apelativo = Items.Apelativo, 
                        TAR = Items.TAR ?? item.TAR,
                        Remision = Items.Remision ?? item.Remision,
                        Factura = Items.Factura ?? item.Factura, 
                        Fecha = Items.Fecha ?? item.Fecha, 
                        Vencimiento = Items.Vencimiento ?? item.Vencimiento,
                        Combustible = Items.Combustible ?? item.Combustible,
                        LtsRecibidos = Items.LtsRecibidos ?? item.LtsRecibidos,
                        Litros = Items.Litros ?? item.Litros,
                        Subtotal = Items.Subtotal ?? item.Subtotal,
                        IVA = Items.IVA ?? item.IVA,
                        IEPS = Items.IEPS ?? item.IEPS,
                        Flete = Items.Flete ?? item.Flete,
                        IVAFlete = Items.IVAFlete ?? item.IVAFlete,
                        Ret = Items.Ret ?? item.Ret,
                        Descuento = Items.Descuento ?? item.Descuento,
                        Total = Items.Total ?? item.Total,
                        GasStation= Items.GasStation ?? item.GasStation,
                        Rows = Items.Rows,
                        RegisterDate = DateTime.Now.ToUniversalTime(),
                    };
                    ModDatabasePipe.Add(Modif);
                }
                else 
                {
                    

                    StationPipeViewModel AddModel = new StationPipeViewModel
                    {
                        TAR = item.TAR,
                        Remision = item.Remision,
                        Factura = item.Factura,
                        Fecha =  item.Fecha,
                        Vencimiento =  item.Vencimiento,
                        Combustible = item.Combustible,
                        LtsRecibidos =  item.LtsRecibidos,
                        Litros = item.Litros,
                        Subtotal = item.Subtotal,
                        IVA = item.IVA,
                        IEPS =  item.IEPS,
                        Flete =  item.Flete,
                        IVAFlete = item.IVAFlete,
                        Ret =  item.Ret,
                        Descuento =  item.Descuento,
                        Total =  item.Total,
                        Rows = rowsCounts,
                        GasStation = item.GasStation,
                        RegisterDate= DateTime.Now.ToUniversalTime(),
                    };
                    rowsCounts++;
                    AddDatabasePipe.Add(AddModel);
                }
            }

            if (ModDatabasePipe.Count==0 && AddDatabasePipe.Count==0)
            {
                return new StationPipeUpdateAddViewModel
                {
                    IsSucceeded = false,
                    MessageSuccess = "No data",
                };
            }
            return new StationPipeUpdateAddViewModel
            {
                IsSucceeded = true,
                ModDatabasePipe = ModDatabasePipe.ToList(),
                AddDatabasePipe = AddDatabasePipe.ToList(),
            };
            //ModDatabasePipe = gasPiperepoorts.Select(x=> new StationPipeViewModel {

            //}).ToList();
        }

    }
}
