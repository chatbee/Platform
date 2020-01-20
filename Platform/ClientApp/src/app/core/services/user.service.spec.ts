import { User } from './../../models/user';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
const u = new User('1234', 'test', 'test2', 'test@test.com');
describe('UserService', () => {
  let controller: HttpTestingController;
  let svc: UserService;
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: 'BASE_URL',
          useFactory: () => {
            return 'api/';
          },
          deps: []
        },
        UserService
      ]
    })
  );

  beforeEach(async () => {
    controller = TestBed.get(HttpTestingController);
    svc = TestBed.get(UserService);
  });

  afterEach(async () => {
    controller.verify();
  });
  it('should be created', () => {
    const service: UserService = TestBed.get(UserService);
    expect(service).toBeTruthy();
  });
  it('should GET a certain user', async () => {
    const s = svc.getUser(u.id).subscribe(res => {
      expect(res).toBeTruthy();
      expect(res).toEqual(u);
    });
    const req = controller.expectOne('api/api/users/1234');
    expect(req.request.method).toBe('GET');
    req.flush(u);

    s.unsubscribe();
  });
  it('should PUT a certain user', async () => {
    const s = svc.saveUser(u).subscribe(res => {
      expect(res).toBeTruthy();
    });
    const req = controller.expectOne('api/api/users/1234');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toBe(u);
    req.flush(u);

    s.unsubscribe();
  });
  it('should POST a new user', async () => {
    const s = svc.createUser(u).subscribe(res => {
      expect(res).toBeTruthy();
    });
    const req = controller.expectOne('api/api/users');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toBe(u);
    req.flush(u);

    s.unsubscribe();
  });
  it('should DELETE a user', async () => {
    const s = svc.deleteUser(u.id).subscribe(res => {
      expect(res).toBeTruthy();
    });
    const req = controller.expectOne('api/api/users/1234');
    expect(req.request.method).toBe('DELETE');
    req.flush(u);

    s.unsubscribe();
  });
  it('updates local user', async () => {
    svc.updateLocalUser(null);
  });
});
