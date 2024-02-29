using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FakturaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFakturaRepository _fakturaRepository;
        private readonly DbContext _context;
        public FakturaController(IMapper mapper, IFakturaRepository fakturaRepository, DataContext context)
        {
            _fakturaRepository = fakturaRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("")]
        public async Task<ActionResult> DodajFakturu(FakturaDto fakturaDto)
        {
            var faktura = _mapper.Map<Faktura>(fakturaDto);

            _fakturaRepository.DodajFakturu(faktura);
            if (await _fakturaRepository.SacuvajIzmjene())
                return Ok("Faktura dodata");

            return BadRequest();


        }
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<FakturaDto>>> DohvatiFakture()
        {
            var fakture = await _fakturaRepository.DohvatiFakture();
            return Ok(fakture);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FakturaDto>> DohvatiFakturu(int id)
        {
            var faktura = await _fakturaRepository.DohvatiFakturuIStavke(id);
            var fakturaDto = _mapper.Map<FakturaDto>(faktura);

            if (faktura == null)
                return NotFound();

            return Ok(fakturaDto);
        }

        [HttpPut("")]
        public async Task<ActionResult> IzmjeniFakturu(FakturaDto fakturaUpdateDto)
        {
            var faktura = await _fakturaRepository
                .DohvatiFakturuIStavke(fakturaUpdateDto.Broj);

            if (faktura == null) return NotFound("Faktura ne postoji");

            _mapper.Map(fakturaUpdateDto, faktura);

            if (await _fakturaRepository.SacuvajIzmjene())
                return Ok("Izmjena obavljena");

            return BadRequest("Greska pri izmjeni");

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> ObrisiFakturu(int id)
        {
            var faktura = await _fakturaRepository.DohvatiFakturu(id);

            _fakturaRepository.ObrisiFakturu(faktura);

            if (await _fakturaRepository.SacuvajIzmjene())
                return Ok("Faktura obrisana");

            return BadRequest("Greska pri brisanju fakture");

        }
    }
}