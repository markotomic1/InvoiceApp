import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import * as FaktureActions from '../store/fakture/actions';
import { AppStateInterface } from '../types/appState.interface';
import { Observable } from 'rxjs';
import {
  errorSelector,
  faktureSelector,
  isLoadingSelector,
} from '../store/fakture/selectors';
import { FakturaInterface } from '../types/faktura.interface';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  isLoading$: Observable<boolean>;
  error$: Observable<string | null>;
  fakture$: Observable<FakturaInterface[]>;
  modalIsVisible = false;
  modalType: 'add' | 'edit' | '' = '';
  constructor(private store: Store<AppStateInterface>) {
    this.isLoading$ = this.store.pipe(select(isLoadingSelector));
    this.error$ = this.store.pipe(select(errorSelector));
    this.fakture$ = this.store.pipe(select(faktureSelector));
  }

  ngOnInit(): void {
    this.store.dispatch(FaktureActions.dohvatiFakture());
  }

  obrisiFakturu(id: number) {
    this.store.dispatch(FaktureActions.obrisiFakturu({ id }));
  }

  izmjeniFakturu(broj: number) {
    this.store.dispatch(FaktureActions.dohvatiFakturu({ broj }));
    this.openEditModal();
  }

  openEditModal() {
    this.modalType = 'edit';
    this.modalIsVisible = true;
  }
  openAddModal() {
    this.modalType = 'add';
    this.modalIsVisible = true;
  }
  closeModal() {
    this.modalIsVisible = false;
  }
}
