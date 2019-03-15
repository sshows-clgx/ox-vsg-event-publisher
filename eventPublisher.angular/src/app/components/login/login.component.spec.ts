import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { BehaviorSubject, Subject, Observable } from "rxjs";
import { AdminService } from '../../services/admin.service';
import { HttpService } from '../../../services/http.service';
import { ResponseOptions, Response, RequestOptions, RequestMethod, Headers } from "@angular/http";
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrService } from 'ngx-toastr';

describe('LoginComponent', () => {
	let component: LoginComponent;
	let fixture: ComponentFixture<LoginComponent>;
	let mockAdminService: any
	let mockHttpService: any
	let mockViewContainer: any;
	let router: any;
	let mockToastr: any;

	beforeEach(async(() => {

		let response = new BehaviorSubject<Response>(new Response(new ResponseOptions()));
		mockHttpService = jasmine.createSpyObj("http", ["get", "post", "put", "delete"]);
		mockAdminService = jasmine.createSpyObj("AdminService", ["logout", "login"]);

		mockAdminService.logout.and.returnValue(response.asObservable());
		mockAdminService.login.and.returnValue(response.asObservable());
		mockViewContainer = jasmine.createSpyObj('vRef', ['root']);

		mockToastr = jasmine.createSpyObj('ToastrService', ['success', 'error']);

		TestBed.configureTestingModule({
			declarations: [LoginComponent],
			imports: [
				FormsModule,
				RouterTestingModule
			],
			providers: [
				{ provide: HttpService, useValue: mockHttpService },
				{ provide: AdminService, useValue: mockAdminService },
				{ provide: ToastrService, useValue: mockToastr }
			]
		})
			.compileComponents();

		router = TestBed.get(Router);
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(LoginComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
		router.initialNavigation();
		spyOn(router, 'navigate');
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	describe('submit', () => {
		it('should call service login', () => {
			component.submit();
			expect(mockAdminService.login).toHaveBeenCalled();
			expect(router.navigate).toHaveBeenCalledWith(['admin']);
		});
	});
});
