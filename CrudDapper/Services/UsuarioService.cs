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

        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>("select * from usuarios");
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

        public async Task<ResponseModel<UsuarioListarDTO>> BuscarUsuarioId(int usuarioId)
        {
            ResponseModel<UsuarioListarDTO> response = new ResponseModel<UsuarioListarDTO>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM USUARIOS WHERE ID = @ID", new {Id = usuarioId});

                if(usuarioBanco == null)
                {
                    response.Mensagem = "Nenhum usuario localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UsuarioListarDTO>(usuarioBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuario localizado com sucesso!";

                return response;
            }
        }

        public async Task<ResponseModel<List<UsuarioListarDTO>>> CriarUsuario(UsuarioCriarDTO usuarioCriarDTO)
        {
            ResponseModel<List<UsuarioListarDTO>> response = new ResponseModel<List<UsuarioListarDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("Insert into Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao)" +
                                                                 "Values(@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriarDTO);

                if(usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDTO>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuario localizado com sucesso!";
               
            }

            return response;

        }

     

        public async Task<ResponseModel<List<UsuarioListarDTO>>> EditarUsuario(UsuarioEditarDTO usuarioEditarDTO)
        {
            ResponseModel<List<UsuarioListarDTO>> response = new ResponseModel<List<UsuarioListarDTO>>();

            using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("Update Usuarios set NomeCompleto = @NomeCompleto, " +
                                                                 "Email = @Email, Cargo = @Cargo, Salario = @Salario, " +
                                                                 "Situacao = @Situacao, CPF = @CPF " +
                                                                 "where Id = @Id", usuarioEditarDTO);
                if(usuarioBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }
                
                var usuarios = await ListarUsuarios(connection);

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDTO>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuario atualizados com sucesso!";
            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDTO>>> RemoverUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDTO>> response = new ResponseModel<List<UsuarioListarDTO>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("Delete from Usuarios where Id = @Id", new { Id = usuarioId });


                if (usuarioBanco == null)
                {
                    response.Mensagem = "Nenhum usuario localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuarioMapeado = _mapper.Map<List<UsuarioListarDTO>>(usuarios);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuario deletado com sucesso";
            }

            return response;    
        }

       
    }
}
