using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutoresController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(AplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        ////endpoint para probar el IConfiguratiobn
        //[HttpGet("configuraciones")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //    //return configuration["connectionStrings:defaultConnection"];
        //    return configuration["apellido"];

        //}



        [HttpGet] //Api/autores
        [AllowAnonymous]   

        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores =  await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {

            var  autor = await context.Autores.FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }
            return mapper.Map<AutorDTO>(autor);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);

        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO AutorDTO)
        {
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == AutorDTO.Nombre);

            if (existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {AutorDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(AutorDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")] // api/autores/1

        public async Task<ActionResult> Put(Autor autor, int id)
        {

            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el ID de la URL");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }



            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();


        }

        [HttpDelete("{id:int}")]  // api/autores/2//
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();


        }





    }
}
