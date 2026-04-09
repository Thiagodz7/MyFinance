using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Identity; // Referência para o ApplicationUser
using MyFinance.Shared.DTOs; // Referência para o PerfilUsuarioDto
using System.Security.Claims;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Só passa quem tiver logado!
    public class PerfilController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfilController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ObterMeuPerfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            var perfilDto = new PerfilUsuarioDto
            {
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Telefone = usuario.PhoneNumber,
                Ocupacao = usuario.Ocupacao,
                TetoGastosMensal = usuario.TetoGastosMensal,
                PlanoAtual = usuario.PlanoAtual,
                TokenAssinatura = usuario.TokenAssinatura,
                // MAPEAMENTO DAS NOVAS FLAGS
                NotificarEmail = usuario.NotificarEmail,
                NotificarTelefone = usuario.NotificarTelefone,
                NotificarPush = usuario.NotificarPush,
                NotificarWhatsapp = usuario.NotificarWhatsapp
            };

            return Ok(perfilDto);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] PerfilUsuarioDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            usuario.NomeCompleto = dto.NomeCompleto;
            usuario.PhoneNumber = dto.Telefone;
            usuario.Ocupacao = dto.Ocupacao;
            usuario.TetoGastosMensal = dto.TetoGastosMensal;
            usuario.TokenAssinatura = dto.TokenAssinatura;

            // ATUALIZANDO AS FLAGS NO BANCO
            usuario.NotificarEmail = dto.NotificarEmail;
            usuario.NotificarTelefone = dto.NotificarTelefone;
            usuario.NotificarPush = dto.NotificarPush;
            usuario.NotificarWhatsapp = dto.NotificarWhatsapp;

            var result = await _userManager.UpdateAsync(usuario);
            return result.Succeeded ? NoContent() : BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}