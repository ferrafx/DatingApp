import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl='http://localhost:5000/api/auth/'

constructor(private http: HttpClient) { }

login(model:any) {
  return this.http.post(this.baseUrl + 'login', model) //the login method of AuthController.cs API
  .pipe(
    map((response:any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
      }
    })
  );
}

register(model:any){ //the register method of AuthController.cs API, takes UserForRegisterDto
  return this.http.post(this.baseUrl + 'register', model);
}

}
