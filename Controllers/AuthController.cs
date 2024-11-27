using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenAuthentication.DTO;
using TokenAuthentication.Functionality;
using TokenAuthentication.Models;

namespace TokenAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthReporitory repo;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public AuthController(IAuthReporitory _repo, IMapper _mapper, IConfiguration _config)
        {
            repo = _repo;
            mapper = _mapper;
            config = _config;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            registerDTO.Email = registerDTO.Email.ToLower();
            if (repo.UserExists(registerDTO.Email))
                return BadRequest("Email Already Exist");
            var userToCreate = mapper.Map<TblUser>(registerDTO);
            var CreatedUser = repo.Register(userToCreate, registerDTO.Password);
            return StatusCode(201, new { email = CreatedUser.Email, fullname = CreatedUser.FullName });
        }

        [HttpPost("Login")]
        public IActionResult LoginValidate(LoginDTO loginDTO)
        {
            // Once logging is successfully validated then, we have to generate the Token + identity.
            var userFromRepo = repo.Login(loginDTO.Email.ToLower(), loginDTO.Password);
            if (userFromRepo == null)
                return Unauthorized();
            //Now, Login done successfuly eligible to send the Token. role to be defined for user called identity.
            // provide identity example: email id, phone no or other credentials.
            //return Ok(userFromRepo.FullName + " " + userFromRepo.Email); for testing only
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userFromRepo.UserId.ToString()),
               new Claim(ClaimTypes.Name, userFromRepo.Email)
            };
            // Now, we need to retrieve the security key given in JSON File.
            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(config.GetSection("AppSettings:Token").Value));
            // These are the pre-requisites for Token: Claim and Key.
            // Steps for creating Token and adding Identity
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // Now, we have to attach Identity + Key in single object(Can you tell us the Class name?)

            var tokenDescriptor = new SecurityTokenDescriptor // Object Initializer Technique
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            // These are the pre-requisite to generate the Token(Identity, key:Hashing)
            var tokenHandler = new JwtSecurityTokenHandler();

            var finaltoken = tokenHandler.CreateToken(tokenDescriptor); // This is why Token is secure.
            return Ok(new{token = tokenHandler.WriteToken(finaltoken), email = userFromRepo.Email, fullname = userFromRepo.FullName});
        }
    }
}
