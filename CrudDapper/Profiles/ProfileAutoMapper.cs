using AutoMapper;
using CrudDapper.DTO;
using CrudDapper.models;

namespace CrudDapper.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper()
        {
            CreateMap<Usuario, UsuarioListarDTO>();
        }
    }
}
