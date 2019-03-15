import { Injectable } from '@angular/core';
import { HttpService } from "./http.service";
import { UserCredentials } from '../models/user-credentials';
import { map } from 'rxjs/operators';
import { Configuration } from '../models/configuration';

@Injectable()
export class AdminService {
    constructor(private http: HttpService) { }

    public login(credentials: UserCredentials) {
		let url = `/api/login`;
		return this.http.post(url, credentials).pipe(map(res => <boolean>res));
	}

	public logout() {
		let url = `/api/logout`;
        return this.http.post(url, null).pipe(map(res => <boolean>res));
    }
    
    public getConfigurations() {
		let url = `/api/configurations`;
		return this.http.get(url).pipe(map(res => <Configuration[]>res));
	}
}
