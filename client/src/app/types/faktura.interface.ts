import { StavkeFakture } from './stavkaFakture.interface';

export interface FakturaInterface {
  broj: number;
  datum: string;
  partner: string;
  iznosBezPdv: number;
  postoRabata: string;
  rabat: number;
  iznosSaRabatomBezPdv: number;
  pdv: number;
  ukupno: number;
  stavkeFakture: StavkeFakture[];
}
