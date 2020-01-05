import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, share, catchError } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationModel } from '../../models/authenticationModel';
import { AuthenticationResponse } from '../../models/authenticationResponse';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper = new JwtHelperService();
  private apiUrl = `${this.baseUrl}api/users`;
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}
  public login(credentials: AuthenticationModel) {
    return this.http
      .post<AuthenticationResponse>(`${this.apiUrl}/authenticate`, credentials)
      .pipe(
        map((response: AuthenticationResponse) => {
          if (response && response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('userInfo', JSON.stringify(response));
          }
          return response;
        }),
        catchError((e, c) => {
          if (e.status === 401) {
            return of(e.error.message);
          }
        }),
        share()
      );
  }
  public isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    if (!token) {
      return false;
    }
    const isExpired = this.jwtHelper.isTokenExpired(token);
    if (isExpired) {
      this.logout();
    }
    return !isExpired;
  }
  public logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userInfo');
  }
}
