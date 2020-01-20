import { AuthenticationResponse } from './../../models/authenticationResponse';
import { Subscription } from 'rxjs';
import { UserService } from './../../core/services/user.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {
  public user: AuthenticationResponse;
  private s: Subscription;
  constructor(
    private userService: UserService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem('userInfo'));
  }
  ngOnDestroy() {
    if (this.s) {
      this.s.unsubscribe();
    }
  }
  /**
   * save
   */
  public save() {
    this.s = this.userService.saveUser(this.user).subscribe(
      res => {
        this.userService.updateLocalUser(this.user);
        this.toastr.success('Saved profile successfully');
      },
      e => {
        console.error(e);
        this.toastr.error('An error occured while trying to save your profile');
      }
    );
  }
}
