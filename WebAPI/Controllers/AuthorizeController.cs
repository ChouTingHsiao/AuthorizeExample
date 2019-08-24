using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        [HttpPost]
        public async Task<Out_ApiResponse> RequestCookie([FromBody]InLogin inLogin)
        {

            ClaimsIdentity identity = new ClaimsIdentity("myLogin");

            identity.AddClaim(new Claim(ClaimTypes.Name, "Chou"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            identity.AddClaim(new Claim("Name", "周庭孝"));
            identity.AddClaim(new Claim("DEPOT", "TEST"));



            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            string cookie = string.Empty;

            foreach (var headers in HttpContext.Response.Headers.Values)
                foreach (var header in headers)
                    if (header.Contains("test"))
                    {
                        cookie = header;
                    }

            return new Out_ApiResponse(HttpStatusCode.OK, cookie, "");

        }

        [HttpPost]
        public Out_ApiResponse RequestToken([FromBody]InLogin inLogin)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,"Chou"),
                new Claim(ClaimTypes.Role,"User"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is a jwt key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "CoreJWT.vulcan.net",
                audience: "Core RESTful API",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);


            return new Out_ApiResponse(HttpStatusCode.OK,
             new
             {
                 access_token = new JwtSecurityTokenHandler().WriteToken(token),
                 token_type = "Bearer",
             }, "");

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "User")]
        public Out_ApiResponse UserAuth()
        {

            return new Out_ApiResponse(HttpStatusCode.OK, "你有User權限", "");

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public Out_ApiResponse AdminAuth()
        {

            return new Out_ApiResponse(HttpStatusCode.OK, "你有Admin權限", "");

        }

    }

}
