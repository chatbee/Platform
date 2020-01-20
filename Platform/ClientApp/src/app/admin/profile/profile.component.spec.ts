import { FormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { User } from './../../models/user';
import { ToastrModule } from 'ngx-toastr';
import { baseUrl } from './../../../test-helpers/baseUrl.spec';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileComponent } from './profile.component';
import { AuthenticationResponse } from 'src/app/models/authenticationResponse';
import { UserService } from 'src/app/core/services/user.service';

let userSericeStub: Partial<UserService> = {
  saveUser: u => {
    expect(u.id).toBe('1234');
    return of({});
  },
  updateLocalUser: u => {}
};
describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProfileComponent],
      imports: [
        HttpClientTestingModule,
        ToastrModule.forRoot(),
        NoopAnimationsModule,
        FormsModule
      ],
      providers: [baseUrl, { provide: UserService, useValue: userSericeStub }]
    }).compileComponents();
  }));

  beforeEach(() => {
    userSericeStub = {
      saveUser: u => {
        expect(u.id).toBe('1234');
        return of({});
      },
      updateLocalUser: u => {}
    };

    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('saves successfully', async () => {
    component.user = new AuthenticationResponse();
    component.user.id = '1234';
    component.save();
  });
  it('handles errors while saving', async () => {
    component.user = new AuthenticationResponse();
    component.user.id = '1234';
    userSericeStub = {
      saveUser: u => {
        return throwError('');
      }
    };
    expect(() => {
      component.save();
    }).not.toThrow();
  });
});
