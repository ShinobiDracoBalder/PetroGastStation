using Dapper;
using Microsoft.AspNetCore.Mvc;
using PetroGastStation.Common.Responses;
using PetroGastStation.Web.Helpers;
using PetroGastStation.Web.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PetroGastStation.Web.Controllers
{
    public class GasStationController : Controller
    {
        private readonly IDapperHelper _dapperHelper;
        private List<PriceSimpleReportViewModel> _lstModel;
        public GasStationController(IDapperHelper dapperHelper)
        {
            _dapperHelper = dapperHelper;
            _lstModel = new List<PriceSimpleReportViewModel>();
        }
        public async Task<IActionResult> Index()
        {
            string sp = string.Empty;
            sp = "SelectSPPetroFigues";
            DynamicParameters parameter = new DynamicParameters();
            return View(await Task.FromResult(_dapperHelper.GetAll<StationGasViewModel>(sp, parameter, commandType: CommandType.StoredProcedure)));
        }

        public async Task<IActionResult> PetroFiguesDetails(int? id)
        {
            string SP = string.Empty;
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            //SP = $"{"Select g.GasEstacion_Id,g.permiso , g.nombre, g.rfc as'RFC',g.direccion as 'Direccion', g.precioregular, g.precioPremium, g.precioDiesel,[dbo].[f_SugeridoPrecioRegula]() 'SugeridoPrecioRegular',dbo.f_SugeridoPrecioPremium()  'SugeridoPrecioPremium',dbo.f_SugeridoPrecioDiesel() 'SugeridoPrecioDiesel' ,g.estado, g.municipio,g.MainPetro "}{" from[dbo].[TblGasEstacion] g Where g.GasEstacion_Id = @id order by g.MainPetro"}";
            SP = $"Select g.GasEstacion_Id,g.permiso , g.nombre, g.rfc as'RFC',g.direccion as 'Direccion', g.precioregular, g.precioPremium, g.precioDiesel,[dbo].[f_SugeridoPrecioRegular]({id}) 'SugeridoPrecioRegular',dbo.f_SugeridoPrecioPremium({id})  'SugeridoPrecioPremium',dbo.f_SugeridoPrecioDiesel({id}) 'SugeridoPrecioDiesel',[dbo].[f_SDirectCompetitionRegularPrice]({id}) 'DirectCompetitionRegularPrice',[dbo].[f_SDirectCompetitionPremiumPrice]({id}) 'DirectCompetitionPremiumPrice',[dbo].[f_SDirectCompetitionDieselPrice]({id}) 'DirectCompetitionDieselPrice' ,g.estado, g.municipio,g.MainPetro {" from[dbo].[TblGasEstacion] g Where g.GasEstacion_Id = @id order by g.MainPetro"}";
            DynamicParameters parameter = new DynamicParameters();

            parameter.Add("@id", id.Value, DbType.Int32, ParameterDirection.Input);

            var model = await Task.FromResult(_dapperHelper.GetOnlyAvatar<StationGasViewModel>(SP, parameter, commandType: CommandType.Text));
            SP = string.Empty;
            DynamicParameters parameter1 = new DynamicParameters();
            if (!model.IsSuccess)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            parameter1.Add("@id", id.Value, DbType.Int32, ParameterDirection.Input);
            SP = $"Select g.GasEstacion_Id,g.permiso , g.nombre, g.rfc as'RFC',g.direccion as 'Direccion', g.precioregular, g.precioPremium, g.precioDiesel, g.estado, g.municipio,g.MainPetro " +
                 $"from[dbo].[TblGasEstacion] g Where g.permiso in({"Select e.permiso from dbo.TblGasEstacion e Inner Join dbo.TblSectorStation s on e.GasEstacion_Id = s.DestinyEstacion_Id Where s.OriginEstacion_Id = @Id);"}";
            //switch (model.Result.MainPetro)
            //{
            //    case 1:
            //        SP = SP + $" 'PL/23763/EXP/ES/2021','PL/20617/EXP/ES/2017','PL/6578/EXP/ES/2015','PL/20772/EXP/ES/2017','PL/21340/EXP/ES/2018','PL/21990/EXP/ES2018','PL/1115/EXP/ES/2015','PL/3376/EXP/ES/2015','PL/19310/EXP/ES/2016') order by g.MainPetro";
            //        break;
            //    case 2:
            //        SP = SP + $" 'PL/23763/EXP/ES/2021','PL/20617/EXP/ES/2017','PL/21340/EXP/ES/2018',	'PL/20769/EXP/ES/2017',	'PL/23759/EXP/ES/2021',	'PL/21990/EXP/ES2018') order by g.MainPetro";
            //        break;
            //    case 3:
            //        SP = SP + $" 'PL/2542/EXP/ES/2015','PL/10787/EXP/ES/2015',	'PL/8150/EXP/ES/2015','PL/4424/EXP/ES/2015','PL/19826/EXP/ES/2016') order by g.MainPetro";
            //        break;
            //    case 4:
            //        SP = SP + $" 'PL/20062/EXP/ES/2017','PL/23510/EXP/ES/2020','PL/3301/EXP/ES/2015', 'PL/9977/EXP/ES/2015',PL/2397/EXP/ES/2015','PL/3389/EXP/ES/2015') order by g.MainPetro";
            //        break;
            //    case 5:
            //        SP = SP + $"'PL/8348/EXP/ES/2015','PL/22389/EXP/ES/2019','PL/1248/EXP/ES/2015','PL/11326/EXP/ES/2015') order by g.MainPetro";
            //        break;
            //    case 6:
            //        SP = SP + $"'PL/23387/EXP/ES/2020','PL/8252/EXP/ES/2015') order by g.MainPetro";
            //        break;
            //    case 7:
            //        SP = SP + $"'PL/21179/EXP/ES/2018') order by g.MainPetro";
            //        break;
            //}
            var modelDetail = await Task.FromResult(_dapperHelper.GetAllAsync<StationGasViewModel>(SP, parameter1, commandType: CommandType.Text));

            if (!modelDetail.Result.IsSuccess)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            StationGasVMDetails stationGas = new StationGasVMDetails {

                IdPrecio = model.Result.IdPrecio,
                Permiso = model.Result.Permiso,
                Nombre = model.Result.Nombre,
                RFC = model.Result.RFC,
                Direccion = model.Result.Direccion,
                Municipio = model.Result.Municipio,
                Estado = model.Result.Estado,
                PrecioRegular = model.Result.PrecioRegular,
                ActualizadoRegular = model.Result.ActualizadoRegular,
                AtipicoRegular = model.Result.AtipicoRegular,
                PrecioPremium = model.Result.PrecioPremium,
                ActualizadoPremium = model.Result.ActualizadoPremium,
                AtipicoPremium = model.Result.AtipicoPremium,
                PrecioDiesel = model.Result.PrecioDiesel,
                ActualizadoDiesel = model.Result.ActualizadoPremium,
                AtipicoDiesel = model.Result.AtipicoDiesel,
                FechaReporte = model.Result.FechaReporte,
                MainPetro = model.Result.MainPetro,
                GasEstacion_Id = model.Result.GasEstacion_Id,
                SugeridoPrecioRegular = model.Result.SugeridoPrecioRegular,
                SugeridoPrecioPremium = model.Result.SugeridoPrecioPremium,
                SugeridoPrecioDiesel = model.Result.SugeridoPrecioDiesel,
                DirectCompetitionRegularPrice = model.Result.DirectCompetitionRegularPrice,
                DirectCompetitionPremiumPrice = model.Result.DirectCompetitionPremiumPrice,
                DirectCompetitionDieselPrice = model.Result.DirectCompetitionDieselPrice,
            };
            stationGas.CompetitionDetails = modelDetail.Result.ListResults;
            return View(stationGas);
        }
        
        [HttpGet]
        public async Task<IActionResult> StationPricePie(int? id) {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            string sp = string.Empty;
            sp = "uspFindPriceStacionSectors";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@id", id.Value, DbType.Int32, ParameterDirection.Input);
            _lstModel = await Task.FromResult(_dapperHelper.GetAll<PriceSimpleReportViewModel>(sp, parameter, commandType: CommandType.StoredProcedure));
            return View(_lstModel);
        }

        [HttpGet]
        public async Task<IActionResult> StationPriceRadarChart(int? id) {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            string sp = string.Empty;
            sp = "uspFindPriceStacionSectors";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@id", id.Value, DbType.Int32, ParameterDirection.Input);
            _lstModel = await Task.FromResult(_dapperHelper.GetAll<PriceSimpleReportViewModel>(sp, parameter, commandType: CommandType.StoredProcedure));

            if (_lstModel == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }


            return View(_lstModel);
        }

        [HttpGet]
        public async Task<IActionResult> StationPriceRadarGroupChart(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            string sp = string.Empty;
            sp = "uspFindPriceStacionSectors";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@id", id.Value, DbType.Int32, ParameterDirection.Input);
            _lstModel = await Task.FromResult(_dapperHelper.GetAll<PriceSimpleReportViewModel>(sp, parameter, commandType: CommandType.StoredProcedure));

            if (_lstModel == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }


            return View(_lstModel);
        }
    }
}
