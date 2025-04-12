using AutoMapper;
using CrudDapper.DTO;
using CrudDapper.models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;

namespace CrudDapper.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResponseModel<List<UsuarioListarDTO>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListarDTO>> response = new ResponseModel<List<UsuarioListarDTO>>();

            using (var connectioon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuariosBanco = await connectioon.QueryAsync<Usuario>("SELECT * FROM USUARIOS");

                if(usuariosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum usuario localizado!";
                    response.Status = false;
                    return response;
                }

                //Transformacao Mapper
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDTO>>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuarios localizados com sucesso!";

            }

            return response;
        }
    }
}
