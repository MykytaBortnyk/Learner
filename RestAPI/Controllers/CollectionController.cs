using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
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
        public async Task<IActionResult> Get() =>
            Ok(
                   await Task.Run(() =>
                   {
                       return (from collection in _context.Collections
                               where collection.UserId.ToString() == _userManager.GetUserId(User)
                               select collection);
                   })
               );

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == null)
            {
                return BadRequest("Id is null;");
            }

            return Ok(
                await Task.Run(() =>
                {
                    return (from collection in _context.Collections
                            where collection.UserId.ToString() == _userManager.GetUserId(User)
                            && collection.Id == id
                            select collection);
                })
            );
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Collection collection)
        {
            if (ModelState.IsValid && collection != null)
            {
                var user = _context.Users
                    .Where(u => u.Id == _userManager.GetUserId(User))
                    .Include(w => w.Words)
                    .Include(c => c.Collections)
                    .First();

                collection.UserId = new Guid(_userManager.GetUserId(User));
                user.Collections.Add(collection);

                var result = await _userManager.UpdateAsync(user);
                //_context.Users.Update(user);
                if (result.Succeeded)
                    //await _context.SaveChangesAsync();
                    return Ok();
            }
            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Collection value)
        {
            var collection = await _context.Collections
                .FirstOrDefaultAsync(c => c.Id == id &&
                c.UserId.ToString() == _userManager.GetUserId(User));

            if (collection != null)
            {
                collection = value;
                _context.Update(collection);
                await _context.SaveChangesAsync();
            }
            return BadRequest($"The variable of a {value.GetType()} type is a null;");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
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
