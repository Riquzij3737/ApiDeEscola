using ApiDeEscola.DataContext;
using ApiDeEscola.Models;
using ApiDeEscola.Models.Enums;
using ApiDeEscola.Models.TempModels;
using ApiDeEscola.Services;
using ApiDeEscola.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDeEscola.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataApplicationContext _context;
        private readonly ISecurityCrypService _crypservice;
        private readonly IJwtService _jwtService;

        public UsersController(DataApplicationContext context,
                                ISecurityCrypService security, 
                                IJwtService jwt)
        {
            _context = context;
            _crypservice = security;
            _jwtService = jwt;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(Guid? Id = null, string? cpf = null)
        {
            if (Id == null)
            {
                var senhas = _context.Users.ToListAsync().Result.Select(x => x.password).ToArray();
                var userdescp = await _context.Users.ToListAsync();

                foreach (var ss in userdescp)
                {
                    foreach (var t in senhas)
                    {
                        ss.password = _crypservice.DecryptData(t);
                    }
                }

                return Ok(userdescp);
            }
            else
            {
                var UserFind = await _context.Users.FindAsync(Id);
                var Employment = await _context.Employments.FindAsync(cpf);

                var Model = new UserAndEmployModel()
                {
                    Id = UserFind.Id,
                    Name = UserFind.Name,
                    Emaíl = UserFind.Emaíl,
                    password = _crypservice.DecryptData(UserFind.password),
                    Employment = Employment

                };

                return Ok(Model);
            }
        }

        [HttpPost]
        [Route("Subscribe")]
        public async Task<IActionResult> SubsCribeWithUsers([FromBody]UserAndEmployModel model)
        {
            if (model == null)
            {
                return BadRequest();
            } else
            {
                UsersModel users = new UsersModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Emaíl = model.Emaíl,
                    password = _crypservice.EncryptData(model.password)
                };


                model.Employment.Role = Enum.GetName<JobsEnum>(JobsEnum.None) ?? "None";

                EmploymentModel employment = model.Employment;

                string token = _jwtService.GenerationToken(employment);

                await _context.Users.AddAsync(users);
                await _context.Employments.AddAsync(employment);
                await _context.SaveChangesAsync();

                return Ok(new AuthenticationModel()
                {
                    Name = model.Name,
                    Token = token
                });                
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginWithUsers([FromBody]UsersModel users, [FromHeader]string Cpf)
        {
            if (users == null)
            {
                return BadRequest();
            } else
            {
                try
                {
                    var contexto = _context.Users.Where(x => _crypservice.DecryptData(x.password) == users.password).Select(x => x);

                    if (!contexto.Any())
                    {
                        return NotFound("usuario não encontrado");
                    } else
                    {
                        var employ = await _context.Employments.FindAsync(Cpf);

                        string token = _jwtService.GenerationToken(employ);
                                                            
                        return Ok(new AuthenticationModel(){
                            Name = employ.Name,
                            Token = token
                        });
                                                    
                    }

                 } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMyAcountAsync([FromQuery]string Nome, [FromQuery]string Senha)
        {
            if (!string.IsNullOrWhiteSpace(Nome) || !string.IsNullOrWhiteSpace(Senha))
            {                
                var usur = _context.Users.Where(x => x.Name == Nome && _crypservice.DecryptData(x.password) == Senha).Select(x => x);
                var emp = await _context.Employments.Where(x => x.Name == Nome).Select(x => x).SingleAsync();

                try
                {
                    _context.Users.Remove(await usur.SingleAsync());
                    _context.Employments.Remove(emp);

                    await _context.SaveChangesAsync();

                    return Ok();
                } catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            } else
            {
                return BadRequest("Tu num insiriu os dadu certu, Vai si fude");
            }
        }

    }
}
