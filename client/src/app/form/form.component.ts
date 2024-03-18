import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { FakturaInterface } from '../types/faktura.interface';
import { StavkeFakture } from '../types/stavkaFakture.interface';
import { ToastrService } from 'ngx-toastr';
import { Store, select } from '@ngrx/store';
import { AppStateInterface } from '../types/appState.interface';
import { Observable, Subscription } from 'rxjs';
import { trenutnaFakturaSelector } from '../store/fakture/selectors';
import * as FaktureActions from '../store/fakture/actions';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
})
export class FormComponent implements OnInit, OnDestroy {
  fakturaForm: FormGroup = new FormGroup({});
  @Output() closeModal = new EventEmitter<void>();
  faktura$: Observable<FakturaInterface | null>;
  @Input() modalType: 'add' | 'edit' | '' = '';
  private modalSubscription: Subscription = new Subscription();

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private store: Store<AppStateInterface>
  ) {
    this.faktura$ = this.store.pipe(select(trenutnaFakturaSelector));
  }
  ngOnDestroy(): void {
    this.modalSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.fakturaForm = this.fb.group({
      broj: new FormControl<number>(0, {
        validators: [Validators.required],
      }),
      datum: new FormControl<string>('', { validators: [Validators.required] }),
      partner: new FormControl<string>('', {
        validators: [Validators.required],
      }),
      iznosBezPdv: new FormControl<number>({ value: 0, disabled: true }),
      postoRabata: new FormControl<number>({ value: 0, disabled: true }),
      rabat: new FormControl<number>({ value: 0, disabled: true }),
      iznosSaRabatomBezPdv: new FormControl<number>({
        value: 0,
        disabled: true,
      }),
      pdv: new FormControl<number>({ value: 0, disabled: true }),
      ukupno: new FormControl<number>({ value: 0, disabled: true }),
      stavkeFakture: this.fb.array([]),
    });

    if (this.modalType === 'edit') {
      //popunjava podatke za izmjenu
      this.modalSubscription = this.faktura$.subscribe({
        next: (faktura: FakturaInterface | null) => {
          if (!faktura || !faktura.stavkeFakture) return;

          while (this.stavkeFakture.length) {
            this.stavkeFakture.removeAt(0);
          }
          faktura.stavkeFakture.forEach(() => this.dodajStavkuFakture());
          this.fakturaForm.patchValue(faktura);
        },
        error: (error) => {
          console.log(error);
        },
      });
    } else {
      this.dodajStavkuFakture();
    }
  }

  get stavkeFakture(): FormArray {
    return this.fakturaForm.get('stavkeFakture') as FormArray;
  }

  getStavkaFormGroup(stavkaGroup: AbstractControl): FormGroup {
    return stavkaGroup as FormGroup;
  }

  dodajStavkuFakture(): void {
    const novaStavka = this.fb.group({
      rbr: new FormControl<number>(0, { validators: [Validators.required] }),
      nazivArtikla: new FormControl<string>('', {
        validators: [Validators.required],
      }),
      kolicina: new FormControl<number>(0, {
        validators: [Validators.required],
      }),
      cijena: new FormControl<number>(0, { validators: [Validators.required] }),
      iznosBezPdv: new FormControl<number>(0, {
        validators: [Validators.required],
      }),
      postoRabata: new FormControl<number>(0, {
        validators: [Validators.required],
      }),
      rabat: new FormControl<number>(0, { validators: [Validators.required] }),
      iznosSaRabatomBezPdv: new FormControl<number>(0, {
        validators: [Validators.required],
      }),
      pdv: new FormControl<number>(0, { validators: [Validators.required] }),
      ukupno: new FormControl<number>({ value: 0, disabled: true }),
    });

    this.stavkeFakture.push(novaStavka);
  }

  obrisiStavkuFakture(index: number) {
    //brise fakturu i ponovo preracuna
    this.stavkeFakture.removeAt(index);
    this.onStavkaFaktureChange(index);
  }

  onSubmit(): void {
    if (!this.fakturaForm.valid) {
      this.toastr.error('Molimo vas unesite validne podatke');
      return;
    }

    this.fakturaForm.get('iznosBezPdv')?.enable();
    this.fakturaForm.get('postoRabata')?.enable();
    this.fakturaForm.get('rabat')?.enable();
    this.fakturaForm.get('iznosSaRabatomBezPdv')?.enable();
    this.fakturaForm.get('pdv')?.enable();
    this.fakturaForm.get('ukupno')?.enable();

    //omguci inpute da bi dosli do podataka
    for (let index = 0; index < this.stavkeFakture.length; index++) {
      this.stavkeFakture.at(index).get('ukupno')?.enable();
    }
    const formData: FakturaInterface = this.fakturaForm.value;
    this.modalType === 'add'
      ? this.store.dispatch(FaktureActions.dodajFakturu({ faktura: formData }))
      : this.store.dispatch(
          FaktureActions.izmjeniFakturu({ faktura: formData })
        );
    this.fakturaForm.reset();
    this.closeModal.emit();
  }

  onStavkaFaktureChange(index: number) {
    const kolicina = this.stavkeFakture.at(index).get('kolicina')?.value;
    const cijena = this.stavkeFakture.at(index).get('cijena')?.value;
    const postoRabata = this.stavkeFakture.at(index).get('postoRabata')?.value;

    const iznosBezPdv = kolicina * cijena;
    const rabat = parseFloat(((iznosBezPdv * postoRabata) / 100).toFixed(2));
    const iznosSaRabatomBezPdv = iznosBezPdv - rabat;
    const pdv = parseFloat((iznosSaRabatomBezPdv * 0.17).toFixed(2));
    const ukupno = this.addFloats(iznosSaRabatomBezPdv, pdv);

    this.stavkeFakture.at(index).patchValue({
      kolicina,
      cijena,
      iznosBezPdv,
      rabat,
      iznosSaRabatomBezPdv,
      pdv,
      ukupno,
    });

    //sume
    this.fakturaForm.patchValue({
      iznosBezPdv: 0,
      postoRabata: 0,
      rabat: 0,
      iznosSaRabatomBezPdv: 0,
      pdv: 0,
      ukupno: 0,
    });
    for (let j = 0; j < this.stavkeFakture.length; j++) {
      const iznosBezPdv =
        this.stavkeFakture.at(j).get('iznosBezPdv')?.value +
        this.fakturaForm.get('iznosBezPdv')?.value;

      const rabat = this.addFloats(
        this.stavkeFakture.at(j).get('rabat')?.value,
        this.fakturaForm.get('rabat')?.value
      );
      let postoRabata;
      if (iznosBezPdv > 0) {
        postoRabata = parseFloat(((rabat * 100) / iznosBezPdv).toFixed(2));
      }

      const iznosSaRabatomBezPdv = this.addFloats(
        this.stavkeFakture.at(j).get('iznosSaRabatomBezPdv')?.value,
        this.fakturaForm.get('iznosSaRabatomBezPdv')?.value
      );

      const pdv = this.addFloats(
        this.stavkeFakture.at(j).get('pdv')?.value,
        this.fakturaForm.get('pdv')?.value
      );

      const ukupno = this.addFloats(
        this.stavkeFakture.at(j).get('ukupno')?.value,
        this.fakturaForm.get('ukupno')?.value
      );

      this.fakturaForm.patchValue({
        iznosBezPdv,
        postoRabata,
        rabat,
        iznosSaRabatomBezPdv,
        pdv,
        ukupno,
      });
    }
  }

  private addFloats(num1: string | number, num2: string | number) {
    let float1 = parseFloat(num1.toString());
    let float2 = parseFloat(num2.toString());

    let sum = float1 + float2;

    let resultString = sum.toFixed(2);

    return parseFloat(resultString);
  }
}
