using Microsoft.AspNetCore.Mvc;
using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Implementations;
using PWA_GREMIO_API.Core.Services.Interfaces;



namespace PWA_GREMIO_API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSignalRDataController : ControllerBase
    {
        private readonly IUserSignalRDataService _userSignalRDataService;

        public UserSignalRDataController(IUserSignalRDataService userSignalRDataService)
        {
            _userSignalRDataService = userSignalRDataService;
        }

        [HttpGet("groups/{userId}")]
        public async Task<IActionResult> GetGroupsOfUserAsync(int userId)
        {
            try
            {
                IEnumerable<AdminGroupListDto> groups = await _userSignalRDataService.AdminGroupsOfUser(userId);
                
                return Ok(groups);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("sent-announcements/{userId}")]
        public async Task<IActionResult> GetSentAnnoucementsFromUser(int userId)
        {
            try
            {
                IEnumerable<ReceiveAnnoucementDto> announcements = await _userSignalRDataService.AdminGetUserSentAnnoucements(userId);
                return Ok(announcements);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-announcements/{userId}")]
        public async Task<IActionResult> GetAnnoucementsFromUser(int userId)
        {
            try
            {
                IEnumerable<ReceiveAnnoucementDto> announcements = await _userSignalRDataService.GetAnnoucementsOfUser(userId);
                return Ok(announcements);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}


