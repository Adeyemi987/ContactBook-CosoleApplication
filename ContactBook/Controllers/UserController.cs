using ContactBook.Core.Interfaces;
using ContactBook.Data.DTO;
using ContactBook.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;

        public UserController(IUserService userService, IConfiguration config, IImageService imageService)
        {
            _userService = userService;
            _config = config;
            _imageService = imageService;
        }

        [HttpGet("get=all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] Pagination pageParameters)
        {
            try
            {
                return Ok(await _userService.GetAllUsers(pageParameters));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("search Users")]
        public async Task<IActionResult> Search(Pagination parameterModel, string email = "")
        {
            var result = await _userService.Search(parameterModel, email);
            try
            {
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("get-User- by-id")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                return Ok(await _userService.GetUser(userId));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpGet("get-User-by-email")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmail(email));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("Update-user")]
        public async Task<IActionResult> Update(UpdateUserRequestDTO updateUserRequest)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.Update(userId, updateUserRequest);
                return NoContent();
            }
            catch (MissingMemberException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Regular")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await _userService.DeleteUser(userId);
                return NoContent();
            }
            catch (MissingMemberException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("photo/id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage([FromForm] AddImageDTO imageDto, string userId)
        {
            try
            {
                var response = "";
                var upload = await _imageService.UploadAsync(imageDto.Image);
                var imageProperties = new ImageAddedDTO()
                {
                    PublicId = upload.PublicId,
                    Url = upload.Url.ToString()
                };

                string url = imageProperties.Url.ToString();
                var result = await _userService.UploadImage(userId, url);


                if (result)
                {
                    response = "Photo updated successfully";
                }
                return Ok(response);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string firstName, string lastName, string email, string userName, string phoneNumber)
        {

            try
            {
                var result = await _userService.CreateAsync(firstName, lastName, email, userName, phoneNumber);
                return Created("", result);
            }
            catch (MissingFieldException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
