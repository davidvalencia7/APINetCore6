using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        readonly ApplicationDbContext contex;
        public LibrosController(ApplicationDbContext context)
        {
            this.contex = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await contex.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await contex.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
                return BadRequest($"No existe el autor de Id:{ libro.AutorId }");

            contex.Libros.Add(libro);
            await contex.SaveChangesAsync();
            return Ok();
        }
    }
}
