import { Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${this.baseUrl}api/users`;
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}
  /**
   * returns the user object for the specified userId
   */
  public getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }
  /**
   * saveUser
   */
  public saveUser(user: User) {
    return this.http.put(`${this.apiUrl}/${user.id}`, user);
  }
  public createUser(user: User) {
    return this.http.post(`${this.apiUrl}`, user);
  }
  /**
   * deleteUser
   */
  public deleteUser(id: string) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  /**
   * updateLocalUser
   */
  public updateLocalUser(user: User) {
    localStorage.setItem('userInfo', JSON.stringify(user));
  }
}
