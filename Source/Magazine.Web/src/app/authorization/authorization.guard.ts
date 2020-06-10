import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizationService } from './authorization.service';
import { tap, filter } from 'rxjs/operators';
import { ApplicationPaths, QueryParameterNames } from './authorization.constants';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationGuard implements CanActivate {

    constructor(private authorizeService: AuthorizationService,
                private router: Router) { }

    canActivate(_next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.authorizeService.isAuthenticated()
            .pipe(
                filter(isAuthenticated => !isAuthenticated),
                tap(() => {
                    this.router.navigate(ApplicationPaths.LoginPathComponents, {
                        queryParams: {
                            [QueryParameterNames.ReturnUrl]: state.url
                        }
                    });
                })
            );
    }
}
