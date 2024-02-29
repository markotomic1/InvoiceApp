import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { AppStateInterface } from '../types/appState.interface';
import { Observable } from 'rxjs';
import { isLoadingSelector } from '../store/fakture/selectors';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss'],
})
export class LoadingSpinnerComponent {
  isLoading$: Observable<boolean>;
  numberOfDots = 3;
  delay = 0.5; ///zadrska
  dots = Array.from({ length: this.numberOfDots });

  constructor(private store: Store<AppStateInterface>) {
    this.isLoading$ = this.store.pipe(select(isLoadingSelector));
  }
}
