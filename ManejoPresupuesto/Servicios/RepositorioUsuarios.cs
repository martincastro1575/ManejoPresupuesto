﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;


namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            
        }



        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);

            var usuarioId = await connection.QuerySingleAsync<int>("Usuario_Insertar",
                new
                {

                    usuario.Email,
                    usuario.EmailNormalizado,
                    usuario.PasswordHash

                },
                commandType: System.Data.CommandType.StoredProcedure);

            await connection.ExecuteAsync("CrearDatosUsuarioNuevo", new { usuarioId },
                                commandType: System.Data.CommandType.StoredProcedure);
            return usuarioId;
            
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>("Usuario_Buscar_PorEmail", 
                new { emailNormalizado },commandType: System.Data.CommandType.StoredProcedure);
        }



    }
}
