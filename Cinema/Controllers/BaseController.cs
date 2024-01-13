using AutoMapper;
using Cinema.Models;
using Cinema.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class BaseController : ControllerBase
    {
        private static MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MoviesModel, MovieDTO>();
            cfg.CreateMap<MovieDTO, MoviesModel>();
            cfg.CreateMap<RoomsModel, RoomDTO>();
            cfg.CreateMap<RoomDTO, RoomsModel>();
        });
        protected Mapper mapper = new Mapper(mapperConfig);
        protected readonly AppDbContext _context;
        public BaseController(AppDbContext context)
        {
            _context = context;
        }
    }
}
