using CrudDapper.DTO;
using CrudDapper.models;

namespace CrudDapper.Services
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioListarDTO>>> BuscarUsuarios();
    }
}
