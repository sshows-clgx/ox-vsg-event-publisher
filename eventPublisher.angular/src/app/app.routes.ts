import { Routes, RouterModule } from "@angular/router";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { ForbiddenComponent } from "./components/forbidden/forbidden.component";
import { HomeComponent } from "./components/home/home.component";
import { LoginComponent } from "./components/login/login.component";

export const routes: Routes = [
	{
		path: '',
		children: [
			{ path: '', component: HomeComponent },  // default page
			{ path: 'forbidden', component: ForbiddenComponent },
            { path: 'notfound', component: NotFoundComponent },
            { path: 'login', component: LoginComponent },
		],
	},
	{ path: '**', component: NotFoundComponent }
];

export const routing = RouterModule.forRoot(routes);