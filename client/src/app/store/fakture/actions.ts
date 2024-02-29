import { createAction, props } from '@ngrx/store';
import { AxiosResponse } from 'axios';
import { FakturaInterface } from 'src/app/types/faktura.interface';

//Dohvati fakture

export const dohvatiFakture = createAction('[Fakture] Dohvati Fakture');
export const dohvatiFaktureSuccess = createAction(
  '[Fakture] Dohvati Fakture success',
  props<{ fakture: FakturaInterface[] }>()
);
export const dohvatiFaktureFailure = createAction(
  '[Fakture] Dohvati Fakture failure',
  props<{ error: string }>()
);

//Obrisi fakturu
export const obrisiFakturu = createAction(
  '[Fakture] Obrisi Fakturu',
  props<{ id: number }>()
);
export const obrisiFakturuSuccess = createAction(
  '[Fakture] Obrisi Fakturu success',
  props<{ id: number }>()
);
export const obrisiFakturuFailure = createAction(
  '[Fakture] Obrisi Fakturu failure',
  props<{ error: string }>()
);

//Dodaj fakturu
export const dodajFakturu = createAction(
  '[Fakture] Dodaj Fakturu',
  props<{ faktura: FakturaInterface }>()
);
export const dodajFakturuSuccess = createAction(
  '[Fakture] Dodaj Fakturu success',
  props<{ faktura: FakturaInterface }>()
);
export const dodajFakturuFailure = createAction(
  '[Fakture] Dodaj Fakturu failure',
  props<{ error: string }>()
);

//trenutna faktura

export const dohvatiFakturu = createAction(
  '[Fakture] Dohvati treunutnu fakturu',
  props<{ broj: number }>()
);
export const dohvatiFakturuSuccess = createAction(
  '[Fakture] Dohvati treunutnu fakturu Success',
  props<{ faktura: FakturaInterface }>()
);
export const dohvatiFakturuFailure = createAction(
  '[Fakture] Dohvati treunutnu fakturu Failure',
  props<{ error: string }>()
);
//izmjeni fakturu

export const izmjeniFakturu = createAction(
  '[Fakture] Izmjeni Fakturu fakturu',
  props<{ faktura: FakturaInterface }>()
);
export const izmjeniFakturuSuccess = createAction(
  '[Fakture] Izmjeni Fakturu fakturu Success',
  props<{ faktura: FakturaInterface }>()
);
export const izmjeniFakturuFailure = createAction(
  '[Fakture] Izmjeni Fakturu fakturu Failure',
  props<{ error: string }>()
);
