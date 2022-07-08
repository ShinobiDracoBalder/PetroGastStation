using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetroGastStation.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace PetroGastStation.Web.Controllers
{
    public class ReadExcelController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        private DataTable ReadData()
        {
            var filePath = $"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{"image"}";//HttpContext.Current.Server.MapPath("~/ExcelFile/Test.xlsx");
            var dataTable = new DataTable();
            using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader;
                if (Path.GetExtension(filePath).ToUpper() == ".XLS")
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
            }
        //    IExcelDataReader excelReader = fileExtension.ToLower().Equals("xls") ? ExcelReaderFactory.CreateBinaryReader(inputStream) : ExcelReaderFactory.CreateOpenXmlReader(inputStream);
        //    DataSet result = excelReader.AsDataSet();   < -----Won't work
        //excelReader.IsFirstRowAsColumnNames = false;    < -----Won't work
        //DataTable table = result.Tables[0];
            return dataTable;
        }
        
        [HttpPost]
        public IActionResult OnPost(IFormFile file)
        {
            List<PipeViewModel> ListPipe = new List<PipeViewModel>();
            List<string> Lstring = new List<string>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    
                    while (reader.Read()) //Each row of the file
                    {
                        string item = string.Empty;
                        string itemStation = string.Empty;
                        string item0 = string.Empty;
                        string item1 = string.Empty;
                        string item2 = string.Empty;
                        string item3 = string.Empty;
                        string item4 = string.Empty;
                        string item5 = string.Empty;
                        string item6 = string.Empty;
                        string item7 = string.Empty;
                        string item8 = string.Empty;
                        string item9 = string.Empty;
                        string item10 = string.Empty;
                        string item11 = string.Empty;
                        string item12 = string.Empty;
                        string item13 = string.Empty;
                        string item14 = string.Empty;
                        string item15 = string.Empty;



                        //users.Add(new PipeViewModel { Name = reader.GetValue(0).ToString(), City = reader.GetValue(1).ToString() });
                        //item0 = String.IsNullOrEmpty(reader.GetValue(0).ToString()) ? string.Empty : reader.GetValue(0).ToString();
                        //item1 = String.IsNullOrEmpty(reader.GetValue(1).ToString()) ? string.Empty : reader.GetValue(1).ToString();
                        //item2 = String.IsNullOrEmpty(reader.GetValue(2).ToString()) ? string.Empty : reader.GetValue(2).ToString();
                        //item3 = String.IsNullOrEmpty(reader.GetValue(3).ToString()) ? string.Empty : reader.GetValue(3).ToString();
                        //item4 = String.IsNullOrEmpty(reader.GetValue(4).ToString()) ? string.Empty : reader.GetValue(4).ToString();
                        //item5 = String.IsNullOrEmpty(reader.GetValue(5).ToString()) ? string.Empty : reader.GetValue(5).ToString();
                        //item6 = String.IsNullOrEmpty(reader.GetValue(6).ToString()) ? string.Empty : reader.GetValue(6).ToString();
                        //item7 = String.IsNullOrEmpty(reader.GetValue(7).ToString()) ? string.Empty : reader.GetValue(7).ToString();
                        //item8 = String.IsNullOrEmpty(reader.GetValue(8).ToString()) ? string.Empty : reader.GetValue(8).ToString();
                        //item9 = String.IsNullOrEmpty(reader.GetValue(9).ToString()) ? string.Empty : reader.GetValue(9).ToString();
                        //item10 = String.IsNullOrEmpty(reader.GetValue(10).ToString()) ? string.Empty : reader.GetValue(10).ToString();
                        //item11 = String.IsNullOrEmpty(reader.GetValue(11).ToString()) ? string.Empty : reader.GetValue(11).ToString();
                        //item12 = String.IsNullOrEmpty(reader.GetValue(12).ToString()) ? string.Empty : reader.GetValue(12).ToString();
                        //item13 = String.IsNullOrEmpty(reader.GetValue(13).ToString()) ? string.Empty : reader.GetValue(13).ToString();
                        //item14 = String.IsNullOrEmpty(reader.GetValue(14).ToString()) ? string.Empty : reader.GetValue(14).ToString();

                        if (!reader.IsDBNull(0))
                        {
                            if (reader.GetValue(0).ToString() != "Fecha de Impresión")
                            {
                                if (reader.GetValue(0).ToString().Length > 0 && reader.IsDBNull(1))
                                {
                                    itemStation = reader.GetValue(0).ToString();
                                    item = $"{itemStation}";
                                    StationViewModel stationView = new StationViewModel
                                    {
                                        itemStation = itemStation,
                                    };
                                    PipeViewModel pipeViewModel = new PipeViewModel{
                                        Station = stationView,
                                    };
                                    ListPipe.Add(pipeViewModel);
                                }
                                else
                                {
                                    //if (reader.GetValue(0).ToString().Length > 0 && reader.GetValue(1).ToString().Length > 0)
                                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                                    {
                                        if (reader.GetValue(0).ToString()!= "TAR" && reader.GetValue(0).ToString() != "Remision")
                                        {
                                            item0 = reader.IsDBNull(0) ?   string.Empty : reader.GetValue(0).ToString();
                                            item1 = reader.IsDBNull(1) ?   string.Empty : reader.GetValue(1).ToString();
                                            item2 = reader.IsDBNull(2) ?   string.Empty : reader.GetValue(2).ToString();
                                            item3 = reader.IsDBNull(3) ?   string.Empty : reader.GetValue(3).ToString();
                                            item4 = reader.IsDBNull(4) ?   string.Empty : reader.GetValue(4).ToString();
                                            item5 = reader.IsDBNull(5) ?   string.Empty : reader.GetValue(5).ToString();
                                            item6 = reader.IsDBNull(6) ?   string.Empty : reader.GetValue(6).ToString();
                                            item7 = reader.IsDBNull(7) ?   "" : reader.GetValue(7).ToString();
                                            item8 = reader.IsDBNull(8) ?   string.Empty : reader.GetValue(8).ToString();
                                            item9 = reader.IsDBNull(9) ?   string.Empty : reader.GetValue(9).ToString();
                                            item10 = reader.IsDBNull(10) ? string.Empty : reader.GetValue(10).ToString();
                                            item11 = reader.IsDBNull(11) ? string.Empty : reader.GetValue(11).ToString();
                                            item12 = reader.IsDBNull(12) ? string.Empty : reader.GetValue(12).ToString();
                                            item13 = reader.IsDBNull(13) ? string.Empty : reader.GetValue(13).ToString();
                                            item14 = reader.IsDBNull(14) ? string.Empty : reader.GetValue(14).ToString();
                                            item15 = reader.IsDBNull(15) ? string.Empty : reader.GetValue(15).ToString();

                                            PipeViewModel pipeViewModel = new PipeViewModel {
                                                 TAR = reader.GetValue(0).ToString(),
                                                 Remision= reader.GetValue(1).ToString(), 
                                                 Factura = reader.GetValue(2).ToString(),
                                                 Fecha = reader.GetValue(3).ToString(),
                                                 Vencimiento = reader.GetValue(4).ToString(),
                                                 Combustible = reader.GetValue(5).ToString(),
                                                 LtsRecibidos = reader.GetValue(6).ToString(), 
                                                 Litros = reader.GetValue(7).ToString(),
                                                 Subtotal = reader.GetValue(8).ToString(),
                                                 IVA = reader.GetValue(9).ToString(),
                                                 IEPS = reader.GetValue(10).ToString(),
                                                 Flete = reader.GetValue(11).ToString(),
                                                 IVAFlete = reader.GetValue(12).ToString(),
                                                 Ret = reader.GetValue(13).ToString(),
                                                 Desc = reader.GetValue(14).ToString(),
                                                 Total = reader.GetValue(15).ToString(),
                                            };

                                            item = $"{item0}-{item1}-{item2}-{item3}-{item4}-{item5}-{item6.ToString()}-{item7.ToString()}-{item8.ToString()}-{item9.ToString()}-{item10.ToString()}-{item11.ToString()}-{item12.ToString()}-{item13.ToString()}-{item14.ToString()}-{item15.ToString()};";
                                            ListPipe.Add(pipeViewModel);
                                        }
                                        
                                    }
                                }
                                Lstring.Add(item);
                            }
                        }
                    }
                }
            }
            ListPipes(ListPipe);
            //users // you got the values here
            return View();
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
                     StationPipeViewModel pipe = new StationPipeViewModel {
                        TAR = item.TAR,
                        Remision = item.Remision,
                        Factura = item.Factura,
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
                    };
                    GasStation.Add(new StationPipeViewModel { GasStation = itemStation,
                    TAR = item.TAR, Remision=item.Remision,Factura = item.Factura,
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
                    Total = item.Total,});   
                }
            }
            return GasStation;
        }
    }
}
