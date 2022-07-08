using PetroGastStation.Web.Models;
using System.Collections.Generic;

namespace PetroGastStation.Web.Helpers
{
    public class ListERP : IListERP
    {
        public List<PipeERPViewModel> GetAllAsync()
        {
            List<PipeERPViewModel> list = new List<PipeERPViewModel> 
            {
                new PipeERPViewModel { ERP = "Mompani-Autozone", RazonSocial = "SERVICIO PETRO MOMPANI S.A DE C.V", NumeroPermiso= "PL/22421/EXP/ES/2019" },
                new PipeERPViewModel { ERP = "Petro Mompani Nexus", RazonSocial = "PETRO FIGUES S.A DE C.V", NumeroPermiso = "PL/20126/EXP/ES/2017" },
                new PipeERPViewModel { ERP = "El Rancho", RazonSocial = "PETRO EL RANCHO S.A DE C.V", NumeroPermiso = "PL/22065/EXP/ES/2019" },
                new PipeERPViewModel { ERP = "Petro Junipero", RazonSocial = "SERVICIO PETRO JUNIPERO S.A DE C.V	", NumeroPermiso = "PL/22827/EXP/ES/2019" },
                new PipeERPViewModel { ERP = "Apaseo Nexus", RazonSocial = "SERVICIO PETRO APASEO S.A DE C.V", NumeroPermiso = "PL/20812/EXP/ES/2018" },
                new PipeERPViewModel { ERP = "Brujas I", RazonSocial = "GASOLINERA OPERADORA GONZER S.A DE C.V ", NumeroPermiso = "PL/21118/EXP/ES/2018" },
                new PipeERPViewModel { ERP = "Brujas II", RazonSocial = "GASOLINERA OPERADORA GONZER S.A DE C.V ", NumeroPermiso = "PL/21172/EXP/ES/2018" },
                new PipeERPViewModel { ERP = "Petro Monsalt", RazonSocial = "Petro Monsalt s.a. de c.v.", NumeroPermiso = "PL/22413/EXP/ES/2019" },
                new PipeERPViewModel { ERP = "Villas de San Francisco", RazonSocial = "SERVICIO PETRO LAS BRUJAS  S.A DE C.V", NumeroPermiso = "PL/21616/EXP/ES/2018" },
                new PipeERPViewModel { ERP = "PETRO EL VALLE (MINAS)", RazonSocial= "SERVICIO PETRO VALLE S.A DE C.V 	", NumeroPermiso= "PL/23699/EXP/ES/2020" },
                new PipeERPViewModel { ERP = "FIGUE PESQUERIA (KIA)", RazonSocial = "SERVICIO FIGUE PESQUERIA  S.A DE C.V", NumeroPermiso = "PL/23667/EXP/ES/2020" },
                new PipeERPViewModel { ERP = "PETRO SAN ROQUE", RazonSocial = "PETRO SAN ROQUE S.A DE C.V", NumeroPermiso = "PL/23916/EXP/ES/2021" },
                new PipeERPViewModel { ERP = "CARBURANTES BEAR PLUS", RazonSocial = "CARBURANTES BEAR PLUS S.A DE C.V", NumeroPermiso = "PL/24065/EXP/ES/2022" }
            };


           return list;
        }
    }
}
