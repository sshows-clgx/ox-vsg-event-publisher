import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, Subscriber } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Injectable()
export class HttpService {
	constructor(private http: HttpClient, private router: Router, private loc: Location) {
	}

	public handleError: ((err: any, errorMsg?: string) => Observable<Response>) =
		((err, errMsg) => {
			if (err.status === 401 || err.status === 403) {

				// redirect you to login page
				this.router.navigate(['login']);

				return new Observable<Response>((subscriber: Subscriber<Response>) => {
					subscriber.complete();
				});
			} else {
				if (errMsg) {
					console.log(errMsg);
					//this.toastr.error(errMsg, 'Error');
				}

				return new Observable<Response>((subscriber: Subscriber<Response>) => {
					subscriber.error(err);
				});
			}
		}).bind(this);

	public get(url: string, errorMsg?: string) {
		return this.http.get(url, { headers: this.JSONHeaders() })
			.pipe(
				catchError(err => this.handleError(err, errorMsg))
			);
	}

	public post(url: string, data: any, errorMsg?: string) {
		return this.http.post(url, JSON.stringify(data), { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public put(url: string, data: any, errorMsg?: string) {
		return this.http.put(url, JSON.stringify(data), { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public delete(url: string, errorMsg?: string) {
		return this.http.delete(url, { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public getJwt() {
		let path = this.loc.path();
		let lastSlashIndex = path.lastIndexOf('/');
		return path.substring(lastSlashIndex + 1, path.length);
	}

	private JSONHeaders(): HttpHeaders {
		var headers = new HttpHeaders({
			'Content-Type': 'application/json',
			Accept: 'application/json',
		});

		return headers;
	}
}