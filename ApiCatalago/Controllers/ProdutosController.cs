using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    //Talvez precise alterar futuramente
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiCatalogoDbContext _context;
        private const string PRODUTO_NOTFOUND = "Produto não encontrado";
        private const string PRODUTO_BADREQUEST = "Produto não pode ser nulo";
        private const string PRODUTO_IDERROR = "O id do produto não coincide";
        private const string PRODUTO_ERROR = "Ocoreu um erro ao tratar a sua solicitação";

        public ProdutosController(ApiCatalogoDbContext context)
        {
            _context = context;
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosCategoriaAsync()
        {
            try
            {
                var produto = await _context.Produtos.Include(p => p.Categoria).AsNoTracking().ToListAsync();

                if (produto is null)
                    return StatusCode(StatusCodes.Status404NotFound, produto);

                return StatusCode(StatusCodes.Status200OK, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }
        }

        // Endpoint simples sem passagem de parametros
        // Utilização do metodo async Task<>, realiza requisições em paralelo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produto = await _context.Produtos?.AsNoTracking().ToListAsync();

                if (produto is null)
                    return StatusCode(StatusCodes.Status404NotFound, produto); ;

                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }
        }

        // Se passado dentro {} é esperado um input no swagger
        // min() indica que o valor do id tem que ser maior que 0, caso seja indicado 0 não será realizado uma requisição na API
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos?.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId.Equals(id));

                if (produto is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, produto);
                }

                return StatusCode(StatusCodes.Status200OK, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }
        }

        // SaveChanges é para salvar o produto na tabela do banco,
        // e CreatedAtRouteResult salva o resultado em uma rota e retorna ele;
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Produto produto)
        {
            try
            {
                _context.Produtos?.Add(produto);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, PRODUTO_ERROR);
            }
        }

        //Entry permite acesso a um objeto na tabela e permite alterar o seu estado a partir de EntityState.Modified
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                    return StatusCode(StatusCodes.Status400BadRequest, produto);

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }
        }

        //Segue a mesma logica de get, mas com a diferença que remove da lista o produto, depois salva
        //Retorna um stuscode 200 caso sucesso e 404 caso insucesso
        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos?.FirstOrDefaultAsync(p => p.ProdutoId.Equals(id));

                if (produto is null)
                    return StatusCode(StatusCodes.Status404NotFound, produto);

                _context.Produtos?.Remove(produto);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, PRODUTO_ERROR);
            }
        }
    }
}
