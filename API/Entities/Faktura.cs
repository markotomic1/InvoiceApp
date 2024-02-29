
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
        public float IznosBezPdv { get; set; }
        public float PostoRabata { get; set; }
        public float Rabat { get; set; }
        public float IznosSaRabatomBezPdv { get; set; }
        public float Pdv { get; set; }
        public float Ukupno { get; set; }
        public List<StavkaFakture> StavkeFakture { get; set; }
    }
}