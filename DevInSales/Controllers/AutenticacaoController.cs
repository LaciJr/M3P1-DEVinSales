using DevInSales.Context;
using DevInSales.DTOs;
using DevInSales.Models;
using DevInSales.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly SqlContext _context;

        public AutenticacaoController(SqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Faz login com email e senha.
        /// </summary>
        /// <returns>token de autenticação</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                var user = (User)_context.User.Where(ob => ob.Email.ToLower() == login.Email.ToLower() && ob.Password.ToLower() == login.Password.ToLower());

                if (user == null) return NotFound("Usuário ou senha incorreta");

                var token = TokenService.GenerateToken(user);

                return Ok(new {token});
            }
            catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }
    }
}
