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
    [Route("api/Actors")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActorController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> GetAllActors()
        {
            var entity = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entity);
        }

        [HttpGet("{id:int}", Name = "GetActor")]
        public async Task<ActionResult<ActorDTO>> GetActorById(int id)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if (entity == null) return NotFound();
            return mapper.Map<ActorDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> PostActor([FromForm] ActorPostDTO actor)
        {
            var entity = mapper.Map<Actor>(actor);
            context.Add(entity);
            //await context.SaveChangesAsync();
            var actorDto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("GetActor", new { id = actorDto.Id}, actorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutActor(int id, [FromForm] ActorPostDTO actor)
        {
            var entity = mapper.Map<Actor>(actor);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var actorExits = await context.Actors.AnyAsync(actor => actor.Id == id);
            if(!actorExits) return NotFound();
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
