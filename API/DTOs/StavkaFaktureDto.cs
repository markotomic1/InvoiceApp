namespace API.DTOs
{
    public class StavkaFaktureDto
    {
        public int Rbr { get; set; }
        public string NazivArtikla { get; set; }
        public int Kolicina { get; set; }
        public double Cijena { get; set; }
        public double IznosBezPdv { get; set; }
        public double PostoRabata { get; set; }
        public double Rabat { get; set; }
        public double IznosSaRabatomBezPdv { get; set; }
        public double Pdv { get; set; }
        public double Ukupno { get; set; }
    }
}