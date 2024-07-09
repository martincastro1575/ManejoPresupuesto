using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController: Controller

    {
        private readonly string connectionString;
        public TiposCuentasController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Crear() 
        { 
            using (var connection = new SqlConnection(connectionString))
            {
                var query = connection.Query("Select 1").FirstOrDefault();
            }
            return View();
        
        }

        [HttpPost]
        public IActionResult Crear(TipoCuenta tipocuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipocuenta);
            }

            return View();
        }
    }


}
