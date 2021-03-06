using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); //every username are lowered so can't have duplicate

            if (await _repo.USerExists(userForRegisterDto.Username))
                return BadRequest("Username already exist");

                                        //<destination>  (source)
            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetailDto>(createdUser);
                 // GetUser is the name of httpGet in Users controller
                 // , new {...} specify the name of controller and the parameter passed
                 //,  object sent back
            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id}, userToReturn);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //first login() AuthRepository
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            //if login ok, create the token...

            //token gonna contains 2 claims: Id and username
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username.ToString())
            };

            //to verify that the token is valid when will come back, the server needs to sign it,
            //so we create a security key
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            //and use the key in the signin credentials, encrypting it in hash algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //create the token: create a tokendescriptor where pass the claims as subjects, pass exipry date, pass signin credentials 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            //create a token handler for actually create the token
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            //map properties into object user, for storage in localstorage
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            //send the response to the client with the token in it
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });

        }

    }
}