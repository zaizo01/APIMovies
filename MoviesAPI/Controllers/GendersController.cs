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
    [Route("api/Genders")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GendersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> GetAllGenders()
        {
            var entity = await context.Genders.ToListAsync();
            var genderDtos = mapper.Map<List<GenderDTO>>(entity);
            return genderDtos;
        }

        [HttpGet("{id:int}", Name = "GetGenderById")]
        public async Task<ActionResult<GenderDTO>> GetGenderById(int id)
        {
            var entity = await context.Genders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();
            var genderDto = mapper.Map<GenderDTO>(entity);
            return genderDto;
        }

        [HttpPost]
        public async Task<ActionResult> PostGender([FromBody] GenderPostDTO gender)
        {
            var entity = mapper.Map<Gender>(gender);
            context.Add(entity);
            await context.SaveChangesAsync();
            var genderDTO = mapper.Map<GenderDTO>(entity);
            return new CreatedAtRouteResult("GetGenderById", new { id = genderDTO.Id}, genderDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutGender(int id, [FromBody] GenderPostDTO gender)
        {
            var entity = mapper.Map<Gender>(gender);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGender(int id)
        {
            var genderExists = await context.Genders.AnyAsync(gender => gender.Id == id);
            if(!genderExists) return NotFound();
            context.Remove(new Gender { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
