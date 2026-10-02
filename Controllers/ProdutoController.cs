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
            var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Nome == produto.Nome);
            if(find != null)
            {
                return Conflict();
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

        [HttpGet("status/{Id:int}")]

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
        [HttpPost("status/{Id:int}")]
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
            await _AppDbContext.SaveChangesAsync();
            return Ok(find.Status);
        }
        [HttpPatch("estoque/{Id:int}")]
        public async Task<ActionResult> AddEstoque([FromBody]int add, int Id)
        {
            try
            {
                var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Id == Id);
                if (find == null)
                {
                    return NotFound();
                }
                if (add <= 0)
                {
                    return Unauthorized();
                }
                find.Estoque += add;
                await _AppDbContext.SaveChangesAsync();
                return Ok(find);
            }
            catch (Exception error)
            {
                return Forbid(error.Message);
            }


        }
        [HttpDelete("estoque/{Id:int}")]
        public async Task<ActionResult> RemEstoque([FromBody] int add, int Id)
        {
            try
            {
                var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Id == Id);
                if (find == null)
                {
                    return NotFound();
                }
                if (add <= 0)
                {
                    return Unauthorized();
                }
                if(find.Estoque - add < 0)
                {
                    return BadRequest();
                }
                find.Estoque -= add;
                await _AppDbContext.SaveChangesAsync();
                return Ok(find);

            }
            catch (Exception error)
            {
                return Forbid(error.Message);
            }


        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult> GetProduct(int Id)
        {
            try
            {
                var find = await _AppDbContext.produtos.FirstOrDefaultAsync(x => x.Id == Id);
                if(find == null)
                {
                    return NotFound();
                }
                return Ok(find);
            }
            catch(Exception error)
            {
                return Forbid(error.Message);
            }
        }
        [HttpGet("{nome}")]
        public async Task<ActionResult> GetNaimeProduct(string nome)
        {
            try
            {
                var find = await _AppDbContext.produtos.Where(x => x.Nome == nome).ToArrayAsync();
                if(find == null)
                {
                    return NotFound();
                }
                return Ok(find);
            }
            catch(Exception error)
            {
                return Forbid(error.Message);
            }
        }
        [HttpGet("ok")]
        public async Task<ActionResult> PegaOK()
        {
            try
            {
                var find = await _AppDbContext.produtos.Where(x => x.Status == "Ok!").ToListAsync();
                if(find == null)
                {
                    return NotFound();
                }   
                return Ok(find);
            }
            catch (Exception error)
            {
                
                return Forbid(error.Message);
            }
        }
        [HttpGet("atencao")]
        public async Task<ActionResult> PegaTencao()
        {
            try
            {
                var find = await _AppDbContext.produtos.Where(x => x.Status == "Atenção").ToListAsync();
                if(find == null)
                {
                    return NotFound();
                }   
                return Ok(find);
            }
            catch (Exception error)
            {
                
                return Forbid(error.Message);
            }
        }
        [HttpGet("critico")]
        public async Task<ActionResult> PegaCritico()
        {
            try
            {
                var find = await _AppDbContext.produtos.Where(x => x.Status == "Crítico").ToListAsync();
                if(find == null)
                {
                    return NotFound();
                }   
                return Ok(find);
            }
            catch (Exception error)
            {
                
                return Forbid(error.Message);
            }
        }
        [HttpGet("ncheck")]
        public async Task<ActionResult> PegaNcheck()
        {
            try
            {
                var find = await _AppDbContext.produtos.Where(x => x.Status == "Não Checado").ToListAsync();
                if(find == null)
                {
                    return NotFound();
                }   
                return Ok(find);    
            }
            catch (Exception error)
            {
                
                return Forbid(error.Message);
            }
        }
        [HttpGet("quantidade")]
        public async Task<ActionResult> PegaQnt()
        {
            var lista = await _AppDbContext.produtos.CountAsync();
            return Ok(lista);
        }
    }


}
