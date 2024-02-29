using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FakturaRepository : IFakturaRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FakturaRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void DodajFakturu(Faktura faktura)
        {
            _context.Fakture.Add(faktura);
        }

        public async Task<IEnumerable<FakturaDto>> DohvatiFakture()
        {
            return await _context.Fakture.ProjectTo<FakturaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<Faktura> DohvatiFakturuIStavke(int brojFakture)
        {
            return await _context.Fakture.Where(f => f.Broj == brojFakture)
                .Include(x => x.StavkeFakture).FirstOrDefaultAsync();
        }
        public async Task<Faktura> DohvatiFakturu(int idFakture)
        {
            return await _context.Fakture.FirstOrDefaultAsync(x => x.Broj == idFakture);
        }



        public void ObrisiFakturu(Faktura faktura)
        {
            _context.Fakture.Remove(faktura);
        }

        public async Task<bool> SacuvajIzmjene()
        {
            return await _context.SaveChangesAsync() > 0;
        }


    }
}