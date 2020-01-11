import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.authService.isLoggedIn) {
      const token = localStorage.getItem('token');
      req = this.addToken(req, token);
    }
    return next.handle(req);
  }

  addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: { Authorization: 'Bearer ' + token }
    });
  }
}
