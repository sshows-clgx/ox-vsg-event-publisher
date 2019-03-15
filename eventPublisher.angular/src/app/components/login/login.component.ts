import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UserCredentials } from '../../models/user-credentials';
import { Router } from '@angular/router';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
	public username: string;
	public credential: string;

	constructor(private router: Router) { 
	}

	ngOnInit() {
	}

	submit() {
		let credentials = new UserCredentials();
		credentials.usernameOrEmail = this.username;
		credentials.credential = this.credential;
		// this.service.login(credentials)
		// 	.subscribe(x => {
		// 		this.router.navigate(['admin']);
		// 	}, err => {
		// 		this.toast.error("Invalid credentials", 'Error');
		// 	});
	}

}
