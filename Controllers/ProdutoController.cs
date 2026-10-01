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
            if(produto == null)
            {
                return NoContent();
            }
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
        [HttpDelete("{id:int}")]
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
        
    }
}