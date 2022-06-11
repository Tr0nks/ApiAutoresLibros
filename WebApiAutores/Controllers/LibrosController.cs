using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase

    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /*[HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
           var libro =  await context.Libros.FirstOrDefaultAsync(librosDB => librosDB.Id == id);
            return mapper.Map<LibroDTO>(libro);


        }*/

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {

            var libro = await context.Libros.FirstOrDefaultAsync(librosBD => librosBD.Id == id);

            if (libro == null)
            {
                return NotFound();
            }
            return mapper.Map<LibroDTO>(libro);
        }


        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            //var existeautor = await context.autores.anyasync(x => x.id == libro.autorid);
            //if (!existeautor)
            //{
            //    return badrequest($"no existe el autor con el id : {libro.autorid}");
            //}

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();

        }



    }
}
