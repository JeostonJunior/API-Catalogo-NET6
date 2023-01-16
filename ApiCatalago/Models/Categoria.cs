using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalago.Models;

[Table("Categorias")]
public class Categoria
{
    /// <summary>
    /// Contrutor para instaciar a coleção de produtos quando a classe é chamada.
    /// </summary>
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    [Key]
    [SwaggerSchema(ReadOnly = true)]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string ImagemURL { get; set; }

    /// <summary>
    /// Mapeando o relacionamento de um para muitos. Onde Categoria possui uma coleção de Produto.
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]

    public ICollection<Produto> Produtos { get; set; }
}