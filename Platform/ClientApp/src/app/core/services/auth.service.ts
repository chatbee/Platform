import { Injectable, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http/http';
import { map, share } from 'rxjs/Operators';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper = new JwtHelperService();
  private apiUrl = `${this.baseUrl}/api/users`;
  constructor(
    private router: Router,
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
