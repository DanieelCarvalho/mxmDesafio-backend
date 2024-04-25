using AutoMapper;
using RevendaCarros_Dc.Domain.Dto;
using RevendaCarros_Dc.Domain.Model;

namespace RevendaCarros_Dc.Domain.Profiles;

public class RevendedorProfile : Profile
{
    public RevendedorProfile()
    {
        CreateMap<CriarAnuncioDto, Carro>()
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());


        CreateMap< Carro, CarViewDto>();
      

    }
}
