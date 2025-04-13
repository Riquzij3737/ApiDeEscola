using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ApiDeEscola.DataContext;
using Microsoft.EntityFrameworkCore;
using ApiDeEscola.Models;
using ApiDeEscola.Models.Enums;

namespace ApiDeEscola.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EmploymentsController : ControllerBase
    {
        private readonly DataApplicationContext _context;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public EmploymentsController(DataApplicationContext context,
                                IHostApplicationLifetime lifetime,
                                ILogger<AdminController> logger)
        {
            _context = context;
            _lifetime = lifetime;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployments([FromHeader]string cpf)
        {
            if (String.IsNullOrWhiteSpace(cpf))
            {
                return Ok(await _context.Employments.ToListAsync());
            } else
            {
                var user = await _context.Employments.FindAsync(cpf);

                return Ok(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFuncionario([FromBody]EmploymentModel model, [FromHeader]JobsEnum job)
        {
            if (model == null)
            {
                return BadRequest();
            } else
            {
                Program.Select(ref model, job);

                await _context.Employments.AddAsync(model);

                await _context.SaveChangesAsync();

                return Ok();
            }
        }

        [HttpDelete]        
        public async Task<IActionResult> DeleteForCPF([FromQuery]string cpf)
        {
            if (String.IsNullOrWhiteSpace(cpf)) { return BadRequest(); } else
            {
                var user = await _context.Employments.FindAsync(cpf);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Employments.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
        }


    }
}