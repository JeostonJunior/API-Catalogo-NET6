using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

        // Esse tipo de ActionResult tem como retorno um verbo HTTP ou o proprio objeto referenciado ActionResult<T> T é generico e pode ser "qualquer coisa"
        [HttpGet("GetProdutoActionResult/{id:int:min(1)}")]
        public ActionResult<Produto> GetProdutoActionResult(int id)
        {
            try
            {
                var produto = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId.Equals(id));

                if (produto is null)
                {
                    return NotFound(PRODUTO_NOTFOUND);
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, PRODUTO_ERROR);
            }
        }

        // IActionResult é uma interface, ActionResult implementa esssa interface, portanto é aconselhavel utilizar IActionResult para tratar os retornos do metodo
        [HttpGet("IActionResultProduto")]
        public IActionResult GetProduto()
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking().ToList();

                if (produto is null)
                {
                    return NotFound(produto);
                }
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, PRODUTO_ERROR);
            }
        }

        [HttpGet("categorias")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria()
        {           
            try
            {
                var produto = _context.Produtos.Include(p => p.Categoria).AsNoTracking().ToList();

                if (produto is null)
                    return NotFound(PRODUTO_NOTFOUND);

                return Ok(produto);
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
                var produto = _context.Produtos?.AsNoTracking().ToListAsync();
                
                if (produto is null)
                    return NotFound(PRODUTO_NOTFOUND);
                
                return await produto;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }            
        }

        // Se passado dentro {} é esperado um input no swagger
        // min() indica que o valor do id tem que ser maior que 0, caso seja indicado 0 não será realizado uma requisição na API
        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId.Equals(id));

                if (produto is null)
                {
                    return NotFound(PRODUTO_NOTFOUND);
                }           
                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, PRODUTO_ERROR);
            }
        }

        // SaveChanges é para salvar o produto na tabela do banco,
        // e CreatedAtRouteResult salva o resultado em uma rota e retorna ele;
        [HttpPost]
        public IActionResult Post(Produto produto)
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
        public IActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
                return BadRequest(PRODUTO_IDERROR);

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        //Segue a mesma logica de get, mas com a diferença que remove da lista o produto, depois salva 
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
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
