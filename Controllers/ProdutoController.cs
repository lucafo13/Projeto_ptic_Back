using API_PTIC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Models.Produto;
namespace csharpBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _AppDbContext;
        public ProdutoController(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> CadProduct(Produtos produto)
        {
            if (produto == null)
            {
                return NoContent();
            }
            produto.Status = "Não Checado";
            _AppDbContext.produtos.Add(produto);

            await _AppDbContext.SaveChangesAsync();
            var lista = await _AppDbContext.produtos.ToListAsync();
            return Ok(lista);
        }
        [HttpGet]
        public async Task<ActionResult> ShowAll()
        {
            var lista = await _AppDbContext.produtos.ToListAsync();
            return Ok(lista);

        }
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Des(int id)
        {
            var produto = await _AppDbContext.produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _AppDbContext.produtos.Remove(produto);
            await _AppDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{Id:int}")]

        public async Task<ActionResult> GetStatus(int Id)
        {
            var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Id == Id);
            if (find == null)
            {
                return NotFound();
            }
            var Status = find.Status;
            return Ok(Status);

        }
        [HttpPost("{Id:in}")]
        public async Task<ActionResult> CheckStatus(int Id)
        {
            var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Id == Id);
            if (find == null)
            {
                return NotFound();
            }
            if (find.Estoque <= find.Min || find.Estoque < find.Min + 1)
            {
                find.Status = "Crítico";


            }
            else if (find.Estoque > find.Min && find.Estoque < find.Min + 10)
            {
                find.Status = "Atenção";
            }
            else
            {
                find.Status = "Ok!";
            }

            return Ok(find.Status);
        }

    }
}