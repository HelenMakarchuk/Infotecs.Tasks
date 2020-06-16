import { Component, OnInit } from '@angular/core';
import { AuthenticationResultStatus, AuthorizationService } from '../authorization.service';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { LogoutActions, ApplicationPaths, ReturnUrlType } from '../authorization.constants';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit {
    public message = new BehaviorSubject<string>(null);

    constructor(
        private authorizeService: AuthorizationService,
        private activatedRoute: ActivatedRoute,
        private router: Router) { }

    async ngOnInit() {
        const action = this.activatedRoute.snapshot.url[1];
        switch (action.path) {
            case LogoutActions.Logout:
                await this.logout(this.getReturnUrl());
                break;
            case LogoutActions.LogoutCallback:
                await this.processLogoutCallback();
                break;
            case LogoutActions.LoggedOut:
                this.message.next('You successfully logged out!');
                break;
            default:
                throw new Error(`Incorrect action '${action}'`);
        }
    }

    private async logout(returnUrl: string): Promise<void> {
        this.message.next('The logout was not initiated from within the page.');
        const state: INavigationState = { returnUrl };
        const isauthenticated = await this.authorizeService.isAuthenticated().pipe(take(1)).toPromise();

        if (isauthenticated) {
            const result = await this.authorizeService.signOut(state);

            switch (result.status) {
                case AuthenticationResultStatus.Redirect:
                    break;
                case AuthenticationResultStatus.Success:
                    await this.navigateToReturnUrl(returnUrl);
                    break;
                case AuthenticationResultStatus.Fail:
                    this.message.next(result.message);
                    break;
                default:
                    throw new Error('Incorrect authentication result status.');
            }
        } else {
            this.message.next('You successfully logged out!');
        }
    }

    private async processLogoutCallback(): Promise<void> {
        const url = window.location.href;
        const result = await this.authorizeService.completeSignOut(url);

        switch (result.status) {
            case AuthenticationResultStatus.Redirect:
                throw new Error('Should not redirect.');
            case AuthenticationResultStatus.Success:
                await this.navigateToReturnUrl(this.getReturnUrl(result.state));
                break;
            case AuthenticationResultStatus.Fail:
                this.message.next(result.message);
                break;
            default:
                throw new Error('Incorrect authentication result status.');
        }
    }

    private async navigateToReturnUrl(returnUrl: string) {
        await this.router.navigateByUrl(returnUrl, {
            replaceUrl: true
        });
    }

    private getReturnUrl(state?: INavigationState): string {
        const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;

        return (state && state.returnUrl) || fromQuery || ApplicationPaths.LoggedOut;
    }
}

interface INavigationState {
    [ReturnUrlType]: string;
}
