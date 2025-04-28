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
                str += "a";
                rangeId = ranges[$"RangeID{user.ClassNameNumber}Grade"];
                str += "b";
                rangeData = ranges[$"RangeUserDetails{user.ClassNameNumber}Grade"];
                 str += "c";
                var data = await _loginService.GetUser(user, sheet, rangeId, rangeData);
                str += "d";
                var token = _authenticationService.GenerateJwtToken(user.Id, user.ClassName,user.ClassNameNumber);
                str += "h";

                if (data != null)

                    return Ok(new
                    {
                        token,
                        data
                    });
                     return StatusCode(500, "Internal Server Error!"+ str);

            }
            catch
            {
                return BadRequest(str);
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
                var classNameNumberClaim = claims.FirstOrDefault(c => c.Type == "classNameNumber")?.Value;
                var id = claims.FirstOrDefault(c => c.Type == "id")?.Value;

                string rangeDataSubject = ranges[$"RangeSubject{classNameNumberClaim}Grade"];
                var data = await _loginService.GetExamResults(rangeDataSubject, classNameNumberClaim, ranges,id);
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
