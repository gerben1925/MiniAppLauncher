using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniAppLauncher.Application.Interfaces.DataAccess;

namespace MiniAppLauncher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDbController : ControllerBase
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public TestDbController(IDbConnectionFactory dBConnectionFactory)
        {
           _dbConnectionFactory = dBConnectionFactory;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();

                var result = await connection.ExecuteScalarAsync<int>("SELECT 1");

                return Ok(new
                {
                    success = true,
                    message = "Database connection successful",
                    result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Database connection failed",
                    error = ex.Message
                });
            }
        }
    }

}
