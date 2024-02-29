import { FakturaInterface } from './faktura.interface';

export interface FaktureStateInterface {
  isLoading: boolean;
  fakture: FakturaInterface[];
  error: string | null;
  trenutnaFaktura: FakturaInterface | null;
}
