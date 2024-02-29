import { Injectable } from '@angular/core';
import { AxiosService } from './axios.service';
import { environment } from 'src/environments/environment.development';
import { FakturaInterface } from '../types/faktura.interface';
import { AxiosResponse } from 'axios';

@Injectable({
  providedIn: 'root',
})
export class FakturaService {
  baseUrl = environment.baseUrl;
  private fetchedData = false;
  constructor(private axios: AxiosService) {}

  dohvatiFakture() {
    return this.axios.instance.get<FakturaInterface[]>(
      this.baseUrl + '/api/faktura'
    );
  }
  dohvatiFakturu(broj: number) {
    return this.axios.instance.get<FakturaInterface>(
      this.baseUrl + '/api/faktura/' + broj
    );
  }
  obrisiFakturu(id: number) {
    return this.axios.instance.delete(
      this.baseUrl + '/api/faktura/' + id.toString()
    );
  }
  dodajFakturu(faktura: FakturaInterface) {
    return this.axios.instance.post(this.baseUrl + '/api/faktura', faktura);
  }
  izmjeniFakturu(faktura: FakturaInterface) {
    return this.axios.instance.put(this.baseUrl + '/api/faktura', faktura);
  }
}
