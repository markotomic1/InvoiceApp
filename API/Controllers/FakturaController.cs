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

            fakturaDto.IznosBezPdv = 0;
            fakturaDto.Rabat = 0;
            fakturaDto.IznosSaRabatomBezPdv = 0;
            fakturaDto.Pdv = 0;
            fakturaDto.Ukupno = 0;
            fakturaDto.PostoRabata = 0;
            foreach (var stavkaFakture in fakturaDto.StavkeFakture)
            {

                stavkaFakture.IznosBezPdv = Math.Round(stavkaFakture.Kolicina * stavkaFakture.Cijena, 2);
                stavkaFakture.Rabat = Math.Round(stavkaFakture.IznosBezPdv * stavkaFakture.PostoRabata / 100, 2);
                stavkaFakture.IznosSaRabatomBezPdv = stavkaFakture.IznosBezPdv - stavkaFakture.Rabat;
                stavkaFakture.Pdv = Math.Round(stavkaFakture.IznosSaRabatomBezPdv * 0.17, 2);
                stavkaFakture.Ukupno = stavkaFakture.IznosSaRabatomBezPdv + stavkaFakture.Pdv;

                fakturaDto.IznosBezPdv += stavkaFakture.IznosBezPdv;

                if (fakturaDto.IznosBezPdv > 0)
                {
                    fakturaDto.PostoRabata = Math.Round(stavkaFakture.Rabat * 100 / stavkaFakture.IznosBezPdv, 2);
                }
                fakturaDto.Rabat += stavkaFakture.Rabat;
                fakturaDto.IznosSaRabatomBezPdv += stavkaFakture.IznosSaRabatomBezPdv;
                fakturaDto.Pdv += stavkaFakture.Pdv;
                fakturaDto.Ukupno += stavkaFakture.Ukupno;
            }

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

            fakturaUpdateDto.IznosBezPdv = 0;
            fakturaUpdateDto.Rabat = 0;
            fakturaUpdateDto.IznosSaRabatomBezPdv = 0;
            fakturaUpdateDto.Pdv = 0;
            fakturaUpdateDto.Ukupno = 0;
            fakturaUpdateDto.PostoRabata = 0;
            foreach (var stavkaFakture in fakturaUpdateDto.StavkeFakture)
            {

                stavkaFakture.IznosBezPdv = Math.Round(stavkaFakture.Kolicina * stavkaFakture.Cijena, 2);
                stavkaFakture.Rabat = Math.Round(stavkaFakture.IznosBezPdv * stavkaFakture.PostoRabata / 100, 2);
                stavkaFakture.IznosSaRabatomBezPdv = stavkaFakture.IznosBezPdv - stavkaFakture.Rabat;
                stavkaFakture.Pdv = Math.Round(stavkaFakture.IznosSaRabatomBezPdv * 0.17, 2);
                stavkaFakture.Ukupno = stavkaFakture.IznosSaRabatomBezPdv + stavkaFakture.Pdv;

                fakturaUpdateDto.IznosBezPdv += stavkaFakture.IznosBezPdv;

                if (fakturaUpdateDto.IznosBezPdv > 0)
                {
                    fakturaUpdateDto.PostoRabata = Math.Round(stavkaFakture.Rabat * 100 / stavkaFakture.IznosBezPdv, 2);
                }
                fakturaUpdateDto.Rabat += stavkaFakture.Rabat;
                fakturaUpdateDto.IznosSaRabatomBezPdv += stavkaFakture.IznosSaRabatomBezPdv;
                fakturaUpdateDto.Pdv += stavkaFakture.Pdv;
                fakturaUpdateDto.Ukupno += stavkaFakture.Ukupno;
            }

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