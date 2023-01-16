using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiCatalago.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApiCatalogoDbContext _context;
        private const string CATEGORIA_NOTFOUND = "Categoria não encontrada";
        private const string CATEGORIA_ERROR = "Ocorreu um erro ao tratar a sua solicitação";

        public CategoriasController(ApiCatalogoDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                var produtos = await _context.Categorias.Include(p => p.Produtos).AsNoTracking().Take(5).ToListAsync();

                if (produtos is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, CATEGORIA_NOTFOUND);
                }
                return StatusCode(StatusCodes.Status200OK, produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasAsync()
        {
            try
            {
                var categorias = await _context.Categorias.Select(c => new { c.CategoriaId, c.Nome, c.ImagemURL }).AsNoTracking().Take(5).ToListAsync();

                if (categorias is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, CATEGORIA_NOTFOUND);
                }
                return StatusCode(StatusCodes.Status200OK, categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }


        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<IActionResult> GetCategoriaAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(id => id.CategoriaId.Equals(id));

                if (categoria is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, CATEGORIA_NOTFOUND);
                }
                return StatusCode(StatusCodes.Status200OK, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCategoriaAync([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutCategoriasAsync(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return StatusCode(StatusCodes.Status404NotFound, CATEGORIA_NOTFOUND);
                }

                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteCategoriaAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(id => id.CategoriaId.Equals(id));

                if (categoria is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, CATEGORIA_NOTFOUND);
                }

                _context.Categorias?.Remove(categoria);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CATEGORIA_ERROR);
            }
        }
    }
}
