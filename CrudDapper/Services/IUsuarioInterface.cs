using CrudDapper.DTO;
using CrudDapper.models;

namespace CrudDapper.Services
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioListarDTO>>> BuscarUsuarios();
        Task<ResponseModel<UsuarioListarDTO>> BuscarUsuarioId(int usuarioId);
        Task<ResponseModel<List<UsuarioListarDTO>>> CriarUsuario(UsuarioCriarDTO usuarioCriar); //nao precisa de id para ser criado
        Task<ResponseModel<List<UsuarioListarDTO>>> EditarUsuario(UsuarioEditarDTO usuarioEditarDTO);
        Task<ResponseModel<List<UsuarioListarDTO>>> RemoverUsuario(int usuarioId);
    }
}
