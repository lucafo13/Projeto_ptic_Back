namespace Models.Produto
{
    public class Produtos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Estoque { get; set; }
        public decimal Preco { get; set; }
        public int Min { get; set; }
        public string Status { get; set; }
    }
}