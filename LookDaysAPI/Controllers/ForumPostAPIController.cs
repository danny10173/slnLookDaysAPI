using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LookDaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using LookDaysAPI.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LookDaysAPI.Models.DTO;
using System.Diagnostics;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumPostAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;
        private UserRepository _userRepository;
        public ForumPostAPIController(LookdaysContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        private string decodeJWT(string jwt)
        {
            jwt = jwt.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwt);
            string? username = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return username!;
        }

        [HttpGet("getByPoster"), Authorize(Roles = "user")]
        public async Task<IActionResult> getPostByUser()
        {
            try
            {
                string? jwt = HttpContext.Request.Headers["Authorization"];
                if (jwt == null || jwt == "") return BadRequest();
                string username = decodeJWT(jwt);

                if (username == null)
                {
                    return BadRequest();
                }

                User? user = await _userRepository.GetUserbyUsername(username);

                if (user == null)
                {
                    return NotFound("使用者不存在");
                }

                //var post = await _context.ForumPosts.FindAsync(user.UserId);
                var findpost = await _context.ForumPosts.Where(a => a.UserId == 10)
                    .Select(
                     fp => new ForumPostDTO()
                     {
                         UserId = fp.UserId,
                         Username = fp.User.Username,
                         PostId = fp.PostId,
                         PostTitle = fp.PostTitle,
                         PostTime = fp.PostTime,
                         PostContent = fp.PostContent,
                         Participants = fp.Participants
                     }
                    ).ToListAsync();
                return Ok(findpost);
            }
            catch (Exception)
            {
                return BadRequest("伺服器錯誤，請稍後再試");
            }
        }

        [HttpPost("PostByUser"),Authorize(Roles = "user")]
        public async Task<IActionResult> PostByUser(AddNewPostDTO addNewPostDTO)
        {
            try
            {
                string? jwt = HttpContext.Request.Headers["Authorization"];
                if (jwt == null || jwt == "") return BadRequest();
                string username = decodeJWT(jwt);

                if (username == null)
                {
                    return BadRequest();
                }

                User? user = await _userRepository.GetUserbyUsername(username);

                if (user == null)
                {
                    return NotFound("使用者不存在");
                }

                ForumPost forumPost = new ForumPost()
                {
                    PostTitle = addNewPostDTO.PostTitle,
                    UserId = user.UserId,
                    PostTime = addNewPostDTO.PostTime,
                    PostContent = addNewPostDTO.PostContent
                };
                _context.ForumPosts.Add(forumPost);
                await _context.SaveChangesAsync();
                return Ok(forumPost);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
    }


public static class ForumPostEndpoints
{
	public static void MapForumPostEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ForumPost").WithTags(nameof(ForumPost));

        group.MapGet("/", async (LookdaysContext db) =>
        {
            return await db.ForumPosts.ToListAsync();
        })
        .WithName("GetAllForumPosts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ForumPost>, NotFound>> (int postid, LookdaysContext db) =>
        {
            return await db.ForumPosts.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PostId == postid)
                is ForumPost model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetForumPostById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int postid, ForumPost forumPost, LookdaysContext db) =>
        {
            var affected = await db.ForumPosts
                .Where(model => model.PostId == postid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PostId, forumPost.PostId)
                  .SetProperty(m => m.UserId, forumPost.UserId)
                  .SetProperty(m => m.PostTitle, forumPost.PostTitle)
                  .SetProperty(m => m.PostTime, forumPost.PostTime)
                  .SetProperty(m => m.PostContent, forumPost.PostContent)
                  .SetProperty(m => m.Participants, forumPost.Participants)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateForumPost")
        .WithOpenApi();

        group.MapPost("/", async (ForumPost forumPost, LookdaysContext db) =>
        {
            db.ForumPosts.Add(forumPost);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ForumPost/{forumPost.PostId}",forumPost);
        })
        .WithName("CreateForumPost")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int postid, LookdaysContext db) =>
        {
            var affected = await db.ForumPosts
                .Where(model => model.PostId == postid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteForumPost")
        .WithOpenApi();
    }
}}
