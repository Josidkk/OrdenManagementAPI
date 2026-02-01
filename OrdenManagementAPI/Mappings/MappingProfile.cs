using AutoMapper;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Mappings
{
   
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Clientes y Productos (Bidireccional)
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();

            // Órdenes
            CreateMap<Orden, OrdenResponseDto>()
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetalleOrdens));
            CreateMap<DetalleOrden, DetalleResponseDto>();

            // Mapeo de Request a Entity (Para el POST)
            CreateMap<OrdenRequestDto, Orden>();
            CreateMap<DetalleRequestDto, DetalleOrden>();
        }
    }
}
