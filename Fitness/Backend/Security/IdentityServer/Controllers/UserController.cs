using AutoMapper;
using IdentityServer.DTOs;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace IdentityServer.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDetailsDto>>(users));
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Client")]
        [Authorize(Roles = "Trainer")]
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetUser(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == username);
            return Ok(_mapper.Map<UserDetailsDto>(user));
        }
    }
}
