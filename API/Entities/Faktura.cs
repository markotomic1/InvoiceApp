
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    [Index(nameof(Broj))]
    public class Faktura
    {
        [Key]
        public int Id { get; set; }
        public int Broj { get; set; }
        public DateTime Datum { get; set; }
        public string Partner { get; set; }
        public double IznosBezPdv { get; set; }
        public double PostoRabata { get; set; }
        public double Rabat { get; set; }
        public double IznosSaRabatomBezPdv { get; set; }
        public double Pdv { get; set; }
        public double Ukupno { get; set; }
        public List<StavkaFakture> StavkeFakture { get; set; }
    }
}