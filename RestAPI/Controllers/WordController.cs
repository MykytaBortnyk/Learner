using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Interfaces;
using RestAPI.Models;
using RestAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    public class WordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        //TODO:проверить принадлежность слова
        //TODO:расширить правки на коллекции
        public WordController(AppDbContext context, UserManager<AppUser> userManager)
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
                       return (from word in _context.Words
                               where word.UserId.ToString() == _userManager.GetUserId(User)
                               select word);
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
                    return (from word in _context.Words
                            where word.UserId.ToString() == _userManager.GetUserId(User)
                            && word.Id == id
                            select word);
                })
            );
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Word word)
        {
            if (ModelState.IsValid && word != null)
            {
                var user = _context.Users
                    .Where(u => u.Id == _userManager.GetUserId(User))
                    .Include(w => w.Words)
                    .Include(c => c.Collections)
                    .First();

                word.UserId = new Guid(_userManager.GetUserId(User));
                user.Words.Add(word);

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
        public async Task<IActionResult> Put(Guid id, [FromBody] Word value)
        {
            var word = await _context.Words
                .FirstOrDefaultAsync(w => w.Id == id &&
                w.UserId.ToString() == _userManager.GetUserId(User));

            if (word != null)
            {
                word = value;
                _context.Update(word);
                await _context.SaveChangesAsync();
            }
            return BadRequest($"The variable of a {value.GetType()} type is a null;");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var word = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);
            if (word != null)
            {
                _context.Remove(word);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound($"Could not find the word with Id {id};");
        }
    }
}
