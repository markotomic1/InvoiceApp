using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class StavkaFakture
    {
        [Key]
        public int Id { get; set; }
        public int Rbr { get; set; }
        public string NazivArtikla { get; set; }
        public int Kolicina { get; set; }
        public float Cijena { get; set; }
        public float IznosBezPdv { get; set; }
        public float PostoRabata { get; set; }
        public float Rabat { get; set; }
        public float IznosSaRabatomBezPdv { get; set; }
        public float Pdv { get; set; }
        public float Ukupno { get; set; }
        public int FakturaId { get; set; }
        public Faktura Faktura { get; set; }
    }
}