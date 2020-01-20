import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TestBed, async, inject } from '@angular/core/testing';
import { AuthGuard } from './auth.guard';
import { AuthService } from '../services/auth.service';

describe('AuthGuard', () => {
  let authServiceStub: Partial<AuthService> = {
    isLoggedIn: () => {
      return false;
    }
  };
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule],
      providers: [
        AuthGuard,
        {
          provide: 'BASE_URL',
          useFactory: () => {
            return '';
          },
          deps: []
        },
        { provide: AuthService, useValue: authServiceStub }
      ]
    });
  });
  beforeEach(() => {
    authServiceStub = {
      isLoggedIn: () => {
        return false;
      }
    };
  });
  it('should create', inject([AuthGuard], (guard: AuthGuard) => {
    expect(guard).toBeTruthy();
  }));
  it('should detect it is not logged in and redirect', inject(
    [AuthGuard],
    async (guard: AuthGuard) => {
      authServiceStub = {
        isLoggedIn: () => {
          return false;
        }
      };
      expect(guard.canActivate(null, { url: null, root: null })).toBeFalsy();
    }
  ));
});
