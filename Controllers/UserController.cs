using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendJWTToken.Models;
using BackendJWTToken.Services;
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

        
    }
}