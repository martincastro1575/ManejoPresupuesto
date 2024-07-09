namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {

    }
    public class RepositorioTiposCuentas: IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            
        }
    }
}
