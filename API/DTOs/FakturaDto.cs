
namespace API.DTOs
{
    public class FakturaDto
    {
        public int Broj { get; set; }
        public DateTime Datum { get; set; }
        public string Partner { get; set; }
        public float IznosBezPdv { get; set; }
        public float PostoRabata { get; set; }
        public float Rabat { get; set; }
        public float IznosSaRabatomBezPdv { get; set; }
        public float Pdv { get; set; }
        public float Ukupno { get; set; }
        public List<StavkaFaktureDto> StavkeFakture { get; set; }
    }
}