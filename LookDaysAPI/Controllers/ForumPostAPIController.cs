using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LookDaysAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumPostAPIController : ControllerBase
    {
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
