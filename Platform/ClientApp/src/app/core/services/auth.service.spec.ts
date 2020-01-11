import { AuthenticationResponse } from './../../models/authenticationResponse';
import { AuthenticationModel } from './../../models/authenticationModel';
import { RouterTestingModule } from '@angular/router/testing';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';

describe('AuthService', () => {
  let controller: HttpTestingController;
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule],
      providers: [
        {
          provide: 'BASE_URL',
          useFactory: () => {
            return 'https://test.com/';
          },
          deps: []
        },
        AuthService
      ]
    })
  );

  beforeEach(() => {
    controller = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    controller.verify();
  });

  it('should be created', () => {
    const service: AuthService = TestBed.get(AuthService);
    expect(service).toBeTruthy();
  });
  it('should emit a single post from login', () => {
    const service: AuthService = TestBed.get(AuthService);

    const fakeResponse = new AuthenticationResponse();
    fakeResponse.email = 'test@test.com';
    const fakeRequest = new AuthenticationModel('teestUser', 'password');

    const s = service.login(fakeRequest).subscribe(res => {
      expect(res).toBeTruthy();
      expect(res.email).toBe('test@test.com');
    });
    const req = controller.expectOne('https://test.com/api/users/authenticate');

    expect(req.request.method).toBe('POST');
    expect(req.request.body).toBe(fakeRequest);

    req.flush(fakeResponse);
    s.unsubscribe();
  });
});
