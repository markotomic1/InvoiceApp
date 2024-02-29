import { createReducer, on } from '@ngrx/store';
import { FaktureStateInterface } from 'src/app/types/faktureState.interface';
import * as FaktureActions from './actions';
export const initialState: FaktureStateInterface = {
  isLoading: false,
  fakture: [],
  error: null,
  trenutnaFaktura: null,
};
export const reducers = createReducer(
  initialState,
  //dohvati fakture
  on(FaktureActions.dohvatiFakture, (state) => ({ ...state, isLoading: true })),
  on(FaktureActions.dohvatiFaktureSuccess, (state, { fakture }) => ({
    ...state,
    isLoading: false,
    fakture: fakture.map((faktura) => ({
      ...faktura,
      datum: faktura.datum.split('T')[0],
      stavkeFakture: [...faktura.stavkeFakture],
    })),
  })),
  on(FaktureActions.dohvatiFaktureFailure, (state, action) => ({
    ...state,
    isLoading: false,
    error: action.error,
  })),
  //obrisi fakturu
  on(FaktureActions.obrisiFakturu, (state, action) => ({
    ...state,
    isLoading: true,
  })),
  on(FaktureActions.obrisiFakturuSuccess, (state, action) => ({
    ...state,
    isLoading: false,
    fakture: state.fakture.filter((faktura) => faktura.broj !== action.id),
  })),
  on(FaktureActions.obrisiFakturuFailure, (state, action) => ({
    ...state,
    isLoading: false,
    error: action.error,
  })),
  //dodaj fakturu
  on(FaktureActions.dodajFakturu, (state, action) => ({
    ...state,
    isLoading: true,
  })),
  on(FaktureActions.dodajFakturuSuccess, (state, action) => ({
    ...state,
    isLoading: false,
    fakture: [
      ...state.fakture,
      { ...action.faktura, stavkeFakture: [...action.faktura.stavkeFakture] },
    ],
  })),
  on(FaktureActions.dodajFakturuFailure, (state, action) => ({
    ...state,
    isLoading: false,
    error: action.error,
  })),
  //izmjeni fakturu
  on(FaktureActions.izmjeniFakturu, (state, action) => ({
    ...state,
    isLoading: true,
  })),
  on(FaktureActions.izmjeniFakturuSuccess, (state, action) => ({
    ...state,
    isLoading: false,
    fakture: state.fakture.map((faktura) => {
      if (faktura.broj === action.faktura.broj) {
        console.log(action.faktura.stavkeFakture);
        return {
          ...faktura,
          ...action.faktura,
          stavkeFakture: [...action.faktura.stavkeFakture],
        };
      }
      return faktura;
    }),
  })),
  on(FaktureActions.izmjeniFakturuFailure, (state, action) => ({
    ...state,
    isLoading: false,
    error: action.error,
  })),
  //trenutna faktura
  on(FaktureActions.dohvatiFakturu, (state, action) => ({
    ...state,
    isLoading: true,
  })),
  on(FaktureActions.dohvatiFakturuSuccess, (state, { faktura }) => ({
    ...state,
    isLoading: false,
    trenutnaFaktura: {
      ...faktura,
      datum: faktura.datum.split('T')[0],
      stavkeFakture: [...faktura.stavkeFakture],
    },
  })),
  on(FaktureActions.dohvatiFakturuFailure, (state, action) => ({
    ...state,
    isLoading: false,
    error: action.error,
  }))
);
