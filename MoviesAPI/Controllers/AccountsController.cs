using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoviesAPI.Services;
using AutoMapper;

namespace MoviesAPI.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IMapper mapper)
           
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("UserRegister", Name = "UserRegister")]
        public async Task<ActionResult<ResponseAuthentication>> UserRegister([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded) return await BuildToken(userCredentials); 
            else return BadRequest(result.Errors);
        
        }

        [HttpPost("UserLogin", Name = "UserLogin")]
        public async Task<ActionResult<ResponseAuthentication>> UserLogin([FromBody] UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) return await BuildToken(userCredentials);
            else return BadRequest("Login Failed");
        }

        [HttpGet("RenewToken", Name = "RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseAuthentication>> RenewToken()
        {
            var userInfo = new UserCredentials
            {
                Email = HttpContext.User.Identity.Name
            };

            return await BuildToken(userInfo);
        }

        private async Task<ResponseAuthentication> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userCredentials.Email),
                new Claim(ClaimTypes.Email, userCredentials.Email),
            };

            var identityUser = await _userManager.FindByEmailAsync(userCredentials.Email);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyJwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new ResponseAuthentication()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpGet("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers([FromQuery] PaginacionDTO paginationDTO)
        {
            var queryable = context.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            return mapper.Map<List<UserDTO>>(queryable);
        }

        [HttpPost("MakeAdminRol", Name = "MakeAdminRol")]
        public async Task<ActionResult> MakeAdminRol(UserEditRolDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdminRol", Name = "RemoveAdminRol")]
        public async Task<ActionResult> RemoveAdminRol(UserEditRolDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            await _userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }
    }
}
