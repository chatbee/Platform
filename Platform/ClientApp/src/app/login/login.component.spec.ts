import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from '../core/services/auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceStub: Partial<AuthService>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      providers: [
        {
          provide: 'BASE_URL',
          useFactory: () => {
            return 'test.com/';
          },
          deps: []
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    authServiceStub = {
      login: u => {
        return of({ token: 'asdf' });
      }
    };
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should run ngOnDestroy', async () => {
    component.subscription = of(null).subscribe(res => {});
    expect(() => {
      component.ngOnDestroy();
    }).not.toThrow();
  });
  it('should run redirect', async () => {
    expect(() => {
      component.redirect();
    }).not.toThrow();
  });
  it('should login successfully', async () => {
    component.login();
  });
});
