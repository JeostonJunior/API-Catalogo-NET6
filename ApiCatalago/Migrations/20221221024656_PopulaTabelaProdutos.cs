using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalago.Migrations
{
    public partial class PopulaTabelaProdutos : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaID)"+
                "Values('Coca-Cola','Bebida sabor cola, com aromatizantes', 7.00, 'coca.jpg',100,now(),1)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaID)" +
                "Values('Coxinha','Recheado com catupiry', 5.00, 'coxinha.jpg',5,now(),2)");

            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaID)" +
                "Values('Pudim ao leite','Pudim ao leite com calda de chocolate', 15.00, 'pudim.jpg',2,now(),3)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
