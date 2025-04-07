using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MillyBeads.Infrastructure;
using MillyBeads.Models.Entities;
using MillyBeads.Models.ViewModels;
using MillyBeads.Services;

namespace MillyBeads.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseViewModel>> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

            if (user == null)
                return Unauthorized(new { message = "E-mail ou senha inválidos" });

            if (!_authService.VerifyPassword(model.Password, user.PasswordHash, user.Salt))
                return Unauthorized(new { message = "E-mail ou senha inválidos" });

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _authService.GenerateJwtToken(user);

            return Ok(new AuthResponseViewModel
            {
                Token = token,
                Name = user.Name,
                Email = user.Email
            });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthResponseViewModel>> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest(new { message = "Este e-mail já está em uso" });

            var (hash, salt) = _authService.HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = hash,
                Salt = salt,
                Role = "User", // Novos usuários sempre serão criados com papel "User"
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseViewModel
            {
                Token = _authService.GenerateJwtToken(user),
                Name = user.Name,
                Email = user.Email
            });
        }
    }
} 