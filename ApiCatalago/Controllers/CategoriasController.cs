using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApiCatalogoDbContext _context;
        private const string CATEGORIA_NOTFOUND = "Categoria não encontrada";

        public CategoriasController(ApiCatalogoDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).AsNoTracking().Take(5).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.AsNoTracking().Take(5).ToList();
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult Get(int id)
        {
            var categoria = _context.Categorias?.AsNoTracking().FirstOrDefault(id => id.CategoriaId.Equals(id));

            if (categoria is null)
                return NotFound(CATEGORIA_NOTFOUND);

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias?.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias?.FirstOrDefault(id => id.CategoriaId.Equals(id));

            if (categoria is null)
                return NotFound(CATEGORIA_NOTFOUND);

            _context.Categorias?.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
