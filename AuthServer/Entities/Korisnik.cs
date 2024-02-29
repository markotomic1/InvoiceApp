namespace AuthServer.Entities
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public byte[] LozinkaHash { get; set; }
        public byte[] LozinkaSalt { get; set; }
    }
}