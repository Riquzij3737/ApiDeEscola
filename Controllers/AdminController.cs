using ApiDeEscola.DataContext;
using ApiDeEscola.Models.Enums;
using ApiDeEscola.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDeEscola.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly DataApplicationContext _context;        
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public AdminController(DataApplicationContext context,                                
                                IHostApplicationLifetime lifetime,
                                ILogger<AdminController> logger)
        {
            _context = context;            
            _lifetime = lifetime; 
            _logger = logger;
        }

        [HttpDelete]
        [Authorize(Roles = "Coordinator,Director,Deputy_director")]
        public async Task<IActionResult> DeleteTable(string tableName)
        {
            try
            {
                switch (tableName)
                {
                    case "users":
                        _context.RemoveRange(_context.Users);
                        await _context.SaveChangesAsync();
                        return Ok("Deu certo paizão");

                    case "employments":
                        _context.RemoveRange(_context.Employments);
                        await _context.SaveChangesAsync();
                        return Ok("Deu certo paizão");
                    default:
                        return NotFound("Q poha é essa?");
                }
                
            }
            catch (Exception ex)
            {

                return BadRequest($"ex: {ex.Message}");
            }

        }

        [HttpDelete]
        [Authorize(Roles = "Director")]
        [Route("ForAuthorize")]
        public async Task<IActionResult> DeleteDatabaseEndAPP([FromHeader]string? message)
        {
            _logger.LogWarning("ATENÇÃO!\n\nEsta rota é apenas para usuarios autenticados, pois ela ira deletar o banco de dados, Para acessa-la, você precisa de um token JWT que apenas o diretor da escola possui, caso haja qualquer tentativa de invasão ou uso de um recurso restrito, a pessoa que tentou fazer isto, sera detido, obrigado por sua comprensão");

            try
            {
                await _context.Database.EnsureDeletedAsync();

                _logger.LogInformation($"Banco de dados apagado com sucesso\n\nmotivo da exclusão: {message}");

                _lifetime.StopApplication();

                return Ok();
            } catch (Exception ex)
            {
                _logger.LogError("Erro na exclusão do banco de dados");
                _logger.LogError($"ex: {ex.Message}");

                return BadRequest();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Coordinator,Director,Deputy_director")]
        public async Task<IActionResult> DarAumento([FromHeader]string cpf,[FromQuery]int Aumento)
        {
            if (cpf == null)
            {
                return NotFound();
            } else
            {
                var funcionario = await _context.Employments.FindAsync(cpf);
                float aumentodosalario = (funcionario.Salary * Aumento) / 100;

                funcionario.Salary += aumentodosalario;

                _context.Employments.Update(funcionario);

                await _context.SaveChangesAsync();

                return Ok("Deu certo nego");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Director")]
        [Route("ForDirector")]
        public async Task<IActionResult> PromoverACordenador([FromHeader] string cpf, [FromQuery] JobsEnum jobs)
        {
            if (cpf == null)
            {
                return NotFound();
            }
            else
            {
                var funcionario = await _context.Employments.FindAsync(cpf);

                
                

                _context.Employments.Update(funcionario);

                await _context.SaveChangesAsync();

                return Ok("Deu certo nego");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Coordinator,Director,Deputy_director")]
        [Route("ForAuthorize")]
        public async Task<IActionResult> DarPromoção([FromHeader] string cpf, [FromQuery]JobsEnum jobs)
        {
            if (cpf == null)
            {
                return NotFound();
            }
            else
            {
                var funcionario = await _context.Employments.FindAsync(cpf);
                
                 Program.Select(ref funcionario, jobs);

                _context.Employments.Update(funcionario);

                await _context.SaveChangesAsync();

                return Ok("Deu certo nego");
            }
        }
    }
}
