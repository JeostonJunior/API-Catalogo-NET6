using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalago.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    public string Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string Descricao { get; set; }

    [Required]
    [Column(TypeName = "Decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string ImagemUrl { get; set; }

    public float Estoque { get; set; }

    [SwaggerSchema(ReadOnly = true)]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    /// <summary>
    /// Mapeando o relacionamento de um para muitos. Onde produtos possui um ID de categoria e uma Categoria.
    /// </summary>
    public int CategoriaID { get; set; }

    [JsonIgnore]
    public Categoria Categoria { get; set; }
}
