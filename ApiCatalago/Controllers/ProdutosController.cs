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

        public ProdutosController(ApiCatalogoDbContext context)
        {
            _context = context;
        }

        // Endpoint simples sem passagem de parametros
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos?.Include(c => c.Categoria).ToList();

            if (produtos == null)
                return NotFound(PRODUTO_NOTFOUND);

            return produtos;
        }

        // Se passado dentro {} é esperado um input no swagger
        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault(p => p.ProdutoId.Equals(id));

            if (produto is null)
                return NotFound(PRODUTO_NOTFOUND);

            return produto;
        }

        // SaveChanges é para salvar o produto na tabela do banco,
        // e CreatedAtRouteResult salva o resultado em uma rota e retorna ele;
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
                return BadRequest(PRODUTO_BADREQUEST);

            _context.Produtos?.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);

        }

        //Entry permite acesso a um objeto na tabela e permite alterar o seu estado a partir de EntityState.Modified
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
                return BadRequest(PRODUTO_IDERROR);

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        //Segue a mesma logica de get, mas com a diferença que remove da lista o produto, depois salva 
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault(p => p.ProdutoId.Equals(id));

            if (produto is null)
                return NotFound(PRODUTO_NOTFOUND);

            _context.Produtos?.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

    }
}
