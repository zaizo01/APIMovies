using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthDayPersonController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BirthDayPersonController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BirthDayPersonDTO>>> GetAll()
        {
            var entity = await context.BirthDayPersons.ToListAsync();
            return mapper.Map<List<BirthDayPersonDTO>>(entity);
        }

        [HttpGet("{id:int}", Name = "GetPerson")]
        public async Task<ActionResult<BirthDayPersonDTO>> GetBirthDayPersonById(int id)
        {
            var entity = await context.BirthDayPersons.FirstOrDefaultAsync(person => person.Id == id);
            if (entity == null) return NotFound();
            return mapper.Map<BirthDayPersonDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> PostBirthDayPerson([FromBody] BirthDayPersonPostDTO person)
        {
            var entity = mapper.Map<BirthDayPerson>(person);
            context.Add(entity);
            await context.SaveChangesAsync();
            var personDto = mapper.Map<BirthDayPersonDTO>(entity);
            return new CreatedAtRouteResult("GetActor", new { id = personDto.Id }, personDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutBirthDayPerson(int id, [FromBody] BirthDayPersonPostDTO person)
        {
            var entity = mapper.Map<BirthDayPerson>(person);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBirthDayPerson(int id)
        {
            var existPerson = await context.BirthDayPersons.AnyAsync(person => person.Id == id);
            if (!existPerson) return NotFound();
            context.Remove( new BirthDayPerson { Id = id});
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
