using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Models;
using Newtonsoft.Json;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;

        public ActivitiesAPIController(LookdaysContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
        {
          if (_context.Activities == null)
          {
              return NotFound();
          }
            return await _context.Activities.ToListAsync();
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var activity = await _context.Activities
                                        .Include(a => a.ActivitiesAlbums)
                                        .Include(a => a.Reviews)
                                        .ThenInclude(a => a.User)
                                        .FirstOrDefaultAsync(a => a.ActivityId == id);

            if (activity == null)
            {
                return NotFound();
            }

            // 初始化描述項目列表
            List<DescriptionItem> descriptionItems = null;

            // 檢查並反序列化 JSON 描述
            if (!string.IsNullOrEmpty(activity.DescriptionJson))
            {
                descriptionItems = JsonConvert.DeserializeObject<List<DescriptionItem>>(activity.DescriptionJson);
            }

            var selectedActivity = new
            {
                activity.ActivityId,
                activity.Name,
                activity.Description,
                descriptionJson = descriptionItems, // 添加結構化描述，如果為 null 則不包含
                activity.Price,
                activity.Date,
                activity.CityId,
                activity.Remaining,
                activity.HotelId,
                activity.Address,

                photo = activity.ActivitiesAlbums.Select(album => album.Photo != null ? Convert.ToBase64String(album.Photo) : null).ToList(),
                photoDesc = activity.ActivitiesAlbums.Select(album => album.PhotoDesc).ToList(),
                reviews = activity.Reviews.Select(r => new
                {
                    r.ReviewId,
                    username = r.User.Username,
                    r.Comment,
                    r.Rating,
                    UserPic = r.User.UserPic != null ? Convert.ToBase64String(r.User.UserPic) : null
                }).ToList(),
            };

            return Ok(selectedActivity);
        }



        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
          if (_context.Activities == null)
          {
              return Problem("Entity set 'LookdaysContext.Activities'  is null.");
          }
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.ActivityId }, activity);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            if (_context.Activities == null)
            {
                return NotFound();
            }
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(int id)
        {
            return (_context.Activities?.Any(e => e.ActivityId == id)).GetValueOrDefault();
        }

        // GET: api/ActivitiesAPI/ModelTags
        [HttpGet("ModelTags")]
        public async Task<ActionResult<IEnumerable<ModelTag>>> GetModelTags()
        {
            try
            {
                var modelTags = await _context.ModelTags.ToListAsync();
                if (modelTags == null || modelTags.Count == 0)
                {
                    return NotFound("No model tags found.");
                }

                return Ok(modelTags);
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving model tags: {ex.Message}");
            }
        }
        [HttpGet("{id}/ActivitiesModels")]
        public async Task<ActionResult<IEnumerable<ActivitiesModel>>> GetActivitiesModelsForActivity(int id)
        {
            try
            {
                var activitiesModels = await _context.ActivitiesModels
                    .Where(am => am.ActivityId == id)
                    .Select(am => new ActivitiesModel
                    {
                        ModelId = am.ModelId,
                        ActivityId = am.ActivityId,
                        ModelName = am.ModelName,
                        ModelPrice = am.ModelPrice,
                        ModelContent = am.ModelContent
                    })
                    .ToListAsync();

                if (activitiesModels == null || activitiesModels.Count == 0)
                {
                    return NotFound($"No activities models found for activity with id {id}.");
                }

                return Ok(activitiesModels);
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving activities models: {ex.Message}");
            }
        }
    }


}
