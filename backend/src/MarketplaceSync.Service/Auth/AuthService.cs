using System.Security.Cryptography;
using System.Text;
using MarketplaceSync.Domain.Entities;
using MarketplaceSync.Domain.Interfaces;

namespace MarketplaceSync.Service.Auth
{
    public interface IAuthService
    {
        Task<(bool success, string token, string message)> RegisterAsync(string email, string password, string lojaName);
        Task<(bool success, string token, string message)> LoginAsync(string email, string password);
        Task<bool> ValidateTokenAsync(string token);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<(bool success, string token, string message)> RegisterAsync(
            string email, string password, string lojaName)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Email e senha são obrigatórios");

            if (password.Length < 6)
                return (false, null, "Senha deve ter no mínimo 6 caracteres");

            // Verificar se email já existe
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            if (existingUser.Any())
                return (false, null, "Este email já está registrado");

            // Criar novo usuário
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = HashPassword(password),
                LojaName = lojaName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var token = _tokenService.GenerateAccessToken(user);
            return (true, token, "Usuário registrado com sucesso");
        }

        public async Task<(bool success, string token, string message)> LoginAsync(string email, string password)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Email e senha são obrigatórios");

            // Buscar usuário
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();

            if (user == null)
                return (false, null, "Email ou senha incorretos");

            // Verificar senha
            if (!VerifyPassword(password, user.PasswordHash))
                return (false, null, "Email ou senha incorretos");

            if (!user.IsActive)
                return (false, null, "Usuário inativo");

            // Gerar token
            var token = _tokenService.GenerateAccessToken(user);
            return (true, token, "Login realizado com sucesso");
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                _tokenService.GetPrincipalFromExpiredToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
