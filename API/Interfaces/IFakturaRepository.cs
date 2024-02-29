using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IFakturaRepository
    {
        public void DodajFakturu(Faktura faktura);
        public void ObrisiFakturu(Faktura faktura);
        public Task<IEnumerable<FakturaDto>> DohvatiFakture();
        public Task<Faktura> DohvatiFakturu(int idFakture);
        public Task<Faktura> DohvatiFakturuIStavke(int brojFakture);
        public Task<bool> SacuvajIzmjene();
    }
}