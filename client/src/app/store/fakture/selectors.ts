import { createSelector } from '@ngrx/store';
import { AppStateInterface } from 'src/app/types/appState.interface';

export const selectFeature = (state: AppStateInterface) => state.fakture;

export const isLoadingSelector = createSelector(
  selectFeature,
  (state) => state.isLoading
);
export const faktureSelector = createSelector(
  selectFeature,
  (state) => state.fakture
);
export const errorSelector = createSelector(
  selectFeature,
  (state) => state.error
);
export const trenutnaFakturaSelector = createSelector(
  selectFeature,
  (state) => state.trenutnaFaktura
);
