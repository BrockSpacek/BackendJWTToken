using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendJWTToken.Models;
using BackendJWTToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendJWTToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController (UserService userService){
            _userService = userService;
        }

        [HttpPost]
        [Route("CreateUser")]

        public bool CreateUser([FromBody]UserDTO newUser){
            return _userService.CreateUser(newUser);
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login([FromBody]UserDTO user){
            string stringToken = _userService.Login(user);

            if(stringToken != null){
                return Ok(new { Token = stringToken });
            }else{
                return Unauthorized(new { Message = "Login was unsuccessful. Invalid Email or Password"});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("AuthenticUser")]
        public string AuthenticUserCheck(){
            return "You are logged in and allowed to be here, Yay!!";
        }

        [HttpPut]
        [Route("UpdatePassword")]

        public string UpdatePassword([FromBody] UserDTO user){
            return _userService.UpdatePassword(user);
        }
    }
}