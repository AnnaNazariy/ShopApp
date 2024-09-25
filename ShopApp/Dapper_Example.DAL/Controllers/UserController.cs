using Dapper_Example.DAL.Repositories.Interfaces;
using Dapper_Example.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Example.DAL.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IUnitOfWork _unitofWork;

        public UserController(ILogger<UserController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/User/GetAllUsers
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var results = await _unitofWork.UserRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all users from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllUsersAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/User/GetById/id
        [HttpGet("GetById/{id}", Name = "GetUserById")]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork.UserRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned user with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/User
        [HttpPost("PostUser")]
        public async Task<ActionResult> PostUserAsync([FromBody] User newUser)
        {
            try
            {
                if (newUser == null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid user object");
                }

                var created_id = await _unitofWork.UserRepository.AddAsync(newUser);
                var CreatedUser = await _unitofWork.UserRepository.GetAsync(created_id);
                _unitofWork.Commit();
                return CreatedAtRoute("GetUserById", new { id = created_id }, CreatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostUserAsync: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] User updateUser)
        {
            try
            {
                if (updateUser == null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid user object");
                }

                var UserEntity = await _unitofWork.UserRepository.GetAsync(id);
                if (UserEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                await _unitofWork.UserRepository.ReplaceAsync(updateUser);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var UserEntity = await _unitofWork.UserRepository.GetAsync(id);
                if (UserEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                await _unitofWork.UserRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
