using Microsoft.AspNetCore.Mvc;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Services.Interfaces;



namespace PWA_GREMIO_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _userSignalRDataService;

        public UserDataController(IUserDataService userSignalRDataService)
        {
            _userSignalRDataService = userSignalRDataService;
        }



        [HttpGet("user-data/{userId}")]
        public async Task<IActionResult> GetUserData(int userId)
        {
            try
            {
                UserData userData = await _userSignalRDataService.GetUserData(userId);
                if (userData == null)
                {
                    return NotFound("UserData not found");
                }
                return Ok(userData);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }


        [HttpPost("create-user-data")]
        public async Task<IActionResult> CreateUserData([FromBody] UserData user)
        {
            try
            {
                await _userSignalRDataService.CreateUserData(user);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("update-user-data")]
        public async Task<IActionResult> UpdateUserData([FromBody] UserData user)
        {
            try
            {
                await _userSignalRDataService.UpdateUserData(user);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("delete-user-data/{userId}")]
        public async Task<IActionResult> DeleteUserData(int userId)
        {
            try
            {
                await _userSignalRDataService.DeleteUserData(userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}


