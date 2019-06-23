using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using Nanoblog.Core.Data.Dto;
using Nanoblog.Core.Data.Commands.Entry;
using Nanoblog.Core.Data.Exception;
using Nanoblog.Api.Services;
using Nanoblog.Api.Data;
using Nanoblog.Api.Data.Models;

namespace Nanoblog.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/entries")]
    [ApiController]
    public class EntriesController : Controller
    {
        private readonly AppDbContext _context;

		private readonly IMapper _mapper;

		private readonly IEntryService _entryService;


        public EntriesController(AppDbContext context, IMapper mapper, IEntryService entryService)
        {
            _context = context;
			_mapper = mapper;
			_entryService = entryService;
		}

		// GET: api/entries/newest
		[HttpGet("newest")]
        public async Task<ActionResult<IEnumerable<EntryDto>>> GetNewestEntries()
        {
            var entries = await _entryService.GetNewestAsync();

            return entries.ToList();
        }

        // GET: api/entries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntryDto>> GetEntry(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
		
			return await _entryService.GetAsync(id);
		}

        // POST: api/entries
        [HttpPost, Authorize]
		public async Task<ActionResult<EntryDto>> AddEntry(AddEntry entry)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId is null)
			{
				return BadRequest(new ErrorDto("No user data!"));
			}

            return await _entryService.AddAsync(entry.Text, userId);
		}

        // DELETE: api/entries/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteEntry(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return BadRequest(new ErrorDto("No user data!"));
            }

            var entry = await _entryService.GetAsync(id);

			if (entry.AuthorId != userId)
			{
				return BadRequest(new ApiException("You are not author of this post!"));
			}

			await _entryService.RemoveAsync(id);

			return Ok();
        }
    }
}
