using MatriculationExamsServer.DBs;
using MatriculationExamsServer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static MatriculationExamsServer.Types.Enums;
using Google.Apis.Util;
using Google.Apis.Sheets.v4.Data;
using MatriculationExamsServer.services;
using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using static Google.Apis.Requests.BatchRequest;
namespace MatriculationExamsServer.Controllers
{
    [Route("Login/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly GoogleSheetApiService _googleSheetApiService;
        private readonly LoginService _loginService;
        private string jsonPath;
        private string json;
        private Dictionary<string, string> ranges;
        private AuthenticationService _authenticationService;

        public LoginController(GoogleSheetApiService googleSheetApiService, LoginService loginService,AuthenticationService authenticationService)
        {
            _googleSheetApiService = googleSheetApiService;
            _loginService = loginService;
            jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Ranges.json");
            json = System.IO.File.ReadAllText(jsonPath);
            ranges = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            _authenticationService = authenticationService;
        }
        
        [HttpPost]
        //// get id and class and return token and user info 
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            string str = "";
            string sheet = "", rangeId = "", rangeData = "", stageClass = "";
            try
            {
                rangeId = user.ClassName + ranges["RangeIDGrade"];
                rangeData = user.ClassName + ranges["RangeUserDetailsGrade"];
                var data = await _loginService.GetUser(user, sheet, rangeId, rangeData);
                var token = _authenticationService.GenerateJwtToken(user.Id, user.ClassName);
                if (data != null)

                    return Ok(new
                    {
                        token,
                        data
                    });
                     return StatusCode(500, "Internal Server Error!" +user);

            }
            catch
            {
                return BadRequest(str+user);
            }
        }
       
        [HttpGet("GetData")]

        public async Task<IActionResult> GetData(string token)
        {
                
            var handler = new JwtSecurityTokenHandler();
            token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                var className = claims.FirstOrDefault(c => c.Type == "classRoomName")?.Value;
                var id = claims.FirstOrDefault(c => c.Type == "id")?.Value;

                string rangeDataSubject = className  + ranges[$"RangeSubjectGrade"];
                string rangDataExam = className + ranges["RangeExamsUser"];
                var data = await _loginService.GetExamResults(rangeDataSubject, rangDataExam, ranges,id, className);
                return Ok(data);
            }
            return BadRequest();


        }

        [HttpGet("GetInfo")]

        public async Task<IActionResult> GetInfo()
        {

            return Ok("hello world");

        }
    }
}
