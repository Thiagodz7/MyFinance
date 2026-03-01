using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Application.DTOs.Auth;
using MyFinance.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        // Injeção de Dependência clássica
        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserDto registerUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true // Para MVP, vamos assumir que tá confirmado
            };

            // O Identity cuida do Hash da senha aqui
            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                // Retorna o Token para ele já sair logado
                return Ok(await GerarJwt(user.Email));
            }

            foreach (var error in result.Errors)
            {
                // Retorna erros como "Senha muito curta", "Email já existe", etc.
                return BadRequest(error.Description);
            }

            return BadRequest("Falha ao registrar usuário");
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserDto loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // O último parâmetro (false) é para não bloquear a conta em caso de erro (lockout)
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("Usuário temporariamente bloqueado por tentativas inválidas");
            }

            return BadRequest("Usuário ou Senha inválidos");
        }

        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // 1. Definir quem é o dono do token (Claims)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único do token
            };

            // 2. Pegar a chave secreta do appsettings/env
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Segredo"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // 3. Montar o Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = creds,
                Issuer = _configuration["JwtSettings:Emissor"],
                Audience = _configuration["JwtSettings:Publico"]
            };

            // 4. Gerar a String final
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}