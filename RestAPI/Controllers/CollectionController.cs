using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CollectionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CollectionController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get() =>
            Ok(_context.Collections
                .Where(w => w.AppUserId == _userManager.GetUserId(User)).Include(e => e.Words));

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is null;");
            }

            return Ok(await Task.Run(() =>
                _context.Collections.Where(x =>
                x.AppUserId == _userManager.GetUserId(User) &&
                x.Id == id).Include(e => e.Words)
            ));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CollectionViewModel value)
        {
            if (ModelState.IsValid && value != null)
            {
                var newCollection = new Collection(value, _userManager.GetUserId(User));

                await _context.Collections.AddAsync(newCollection);
                newCollection.Words = value.Words;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Collection value)
        {
            if (ModelState.IsValid && id == value.Id)
            {
                var isItemExists = await _context.Collections.AnyAsync(w =>
                                         w.Id == id &&
                                         w.AppUserId == _userManager.GetUserId(User));
                if (isItemExists)
                {
                    try
                    {
                        _context.Collections.Update(value);
                    }
                    catch (InvalidOperationException e)
                    {
                        return BadRequest($"{e.Message}\nMore then one entity with the same key;");
                    }
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                return BadRequest($"The entity with Id {id} not found;");
            }
            return BadRequest("Id mismatch or model error;");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var collection = await _context.Collections
                .FirstOrDefaultAsync(w => w.Id == id &&
                w.AppUserId == _userManager.GetUserId(User)
            );

            if (collection != null)
            {
                _context.Remove(collection);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound($"Could not find the word with Id {id};");
        }
    }
}
