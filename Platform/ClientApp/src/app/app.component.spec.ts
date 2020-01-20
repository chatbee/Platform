import { MatMenuModule } from '@angular/material/menu';
import { baseUrl } from './../test-helpers/baseUrl.spec';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { AuthService } from './core/services/auth.service';
import { CommonModule } from '@angular/common';

const authServiceStub: Partial<AuthService> = {
  isLoggedIn: () => {
    return true;
  }
};
describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MatSidenavModule,
        NoopAnimationsModule,
        HttpClientTestingModule,
        MatMenuModule,
        CommonModule
      ],
      providers: [
        baseUrl,
        {
          provide: AuthService,
          useValue: authServiceStub
        }
      ],
      declarations: [AppComponent]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Chatbees Platform'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('Chatbees Platform');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const el = fixture.debugElement.nativeElement.querySelector('span');
    expect(el.textContent).toContain('Chatbees Platform');
  });
});
