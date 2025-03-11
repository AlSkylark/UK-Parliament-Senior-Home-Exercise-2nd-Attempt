import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root'
})
export class WarningsService extends ErrorService {

}
