export interface StavkeFakture {
  rbr: number;
  nazivArtikla: string;
  kolicina: number;
  cijena: number;
  iznosBezPdv: number;
  postoRabata: string;
  rabat: number;
  iznosSaRabatomBezPdv: number;
  pdv: number;
  ukupno: number;
  [key: string]: string | number;
}
