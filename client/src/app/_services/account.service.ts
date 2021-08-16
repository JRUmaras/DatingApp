import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'
import { environment } from 'src/environments/environment';

import { IUser } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private httpClient: HttpClient) { }

  login(model: any){
    return this.httpClient.post(this.baseUrl + 'account/login', model).pipe(
      map((response: IUser) => {
        const user  = response;
        this.updateCurrentUser(user);
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: IUser) {
    this.updateCurrentUser(user);
  }

  register(model: any) {
    return this.httpClient.post(this.baseUrl + 'account/register', model).pipe(
      map((user: IUser) => {
        this.updateCurrentUser(user);
        return user;
      })
    );
  }

  private updateCurrentUser(user: IUser) {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    }
  }
}
