import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as FaktureActions from './actions';
import { catchError, concatMap, from, map, mergeMap, of } from 'rxjs';
import { FakturaService } from 'src/app/services/faktura.service';
@Injectable()
export class FaktureEffects {
  constructor(
    private actions$: Actions,
    private faktureService: FakturaService
  ) {}

  //Dohvati fakture
  getFakture$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FaktureActions.dohvatiFakture),
      mergeMap(() =>
        from(this.faktureService.dohvatiFakture()).pipe(
          map((response) =>
            FaktureActions.dohvatiFaktureSuccess({ fakture: response.data })
          ),
          catchError((error) =>
            of(FaktureActions.dohvatiFaktureFailure({ error: error.message }))
          )
        )
      )
    )
  );

  //Obrisi fakturu

  obrisiFakturu$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FaktureActions.obrisiFakturu),
      mergeMap((action) =>
        from(this.faktureService.obrisiFakturu(action.id)).pipe(
          map(() => FaktureActions.obrisiFakturuSuccess({ id: action.id })),
          catchError((error) =>
            of(FaktureActions.obrisiFakturuFailure({ error: error.message }))
          )
        )
      )
    )
  );

  //Dodaj Fakturu
  dodajFakturu$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FaktureActions.dodajFakturu),
      mergeMap((action) =>
        from(this.faktureService.dodajFakturu(action.faktura)).pipe(
          map(() =>
            FaktureActions.dodajFakturuSuccess({ faktura: action.faktura })
          ),
          catchError((error) =>
            of(FaktureActions.dodajFakturuFailure({ error: error.message }))
          )
        )
      )
    )
  );

  //dohvati fakturu

  dohvatiFakturu$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FaktureActions.dohvatiFakturu),
      concatMap((action) => {
        return from(this.faktureService.dohvatiFakturu(action.broj)).pipe(
          map((response) =>
            FaktureActions.dohvatiFakturuSuccess({
              faktura: response.data,
            })
          ),
          catchError((error) =>
            of(FaktureActions.dohvatiFakturuFailure({ error: error.message }))
          )
        );
      })
    )
  );

  //izmjeni Fakturu

  izmjeniFakturu$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FaktureActions.izmjeniFakturu),
      mergeMap((action) =>
        from(this.faktureService.izmjeniFakturu(action.faktura)).pipe(
          map(() =>
            FaktureActions.izmjeniFakturuSuccess({ faktura: action.faktura })
          ),
          catchError((error) =>
            of(FaktureActions.izmjeniFakturuFailure({ error: error.message }))
          )
        )
      )
    )
  );
}
