import { Component, OnInit } from '@angular/core';
import { AuthorizationService, AuthenticationResultStatus } from '../authorization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginActions, QueryParameterNames, ApplicationPaths, ReturnUrlType } from '../authorization.constants';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

    constructor(
        private authorizeService: AuthorizationService,
        private activatedRoute: ActivatedRoute,
        private router: Router) { }

    async ngOnInit() {
        const action = this.activatedRoute.snapshot.url[1];

        switch (action.path) {
            case LoginActions.Login:
                await this.login(this.getReturnUrl());
                break;
            case LoginActions.Register:
                this.redirectToRegister();
                break;
            case LoginActions.LoginCallback:
                await this.processLoginCallback();
                break;
            case LoginActions.LoginFailed:
                const message = this.activatedRoute.snapshot.queryParamMap.get(QueryParameterNames.Message);
                this.authorizeService.message.next(message);
                break;
            default:
                throw new Error(`Invalid action '${action}'`);
        }
    }

    private async login(returnUrl: string): Promise<void> {
        const state: INavigationState = { returnUrl };
        const result = await this.authorizeService.signIn(state);
        this.authorizeService.message.next(undefined);

        switch (result.status) {
            case AuthenticationResultStatus.Success:
                await this.router.navigateByUrl(returnUrl, {replaceUrl: true});
                break;
            case AuthenticationResultStatus.Fail:
                await this.router.navigate(ApplicationPaths.LoginFailedPathComponents, {
                    queryParams: { [QueryParameterNames.Message]: result.message }
                });
                break;
            case AuthenticationResultStatus.Redirect:
                break;
            default:
                throw new Error(`Incorrect status result ${(result as any).status}.`);
        }
    }

    private async processLoginCallback(): Promise<void> {
        const url = window.location.href;
        const result = await this.authorizeService.completeSignIn(url);

        switch (result.status) {
            case AuthenticationResultStatus.Success:
                this.authorizeService.isUserAuthenticated = true;
                await this.router.navigateByUrl(this.getReturnUrl(result.state), {replaceUrl: true});
                location.reload();
                break;
            case AuthenticationResultStatus.Fail:
                this.authorizeService.message.next(result.message);
                break;
            case AuthenticationResultStatus.Redirect:
                throw new Error('Should not redirect.');
        }
    }

    private redirectToRegister(): void {
        window.location.replace(`${environment.identityUrl}/${ApplicationPaths.IdentityRegisterPath}?returnUrl=${encodeURI(location.origin + '/' + ApplicationPaths.Login)}`);
    }

    private getReturnUrl(state?: INavigationState): string {
        const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;

        return (state && state.returnUrl) || fromQuery || ApplicationPaths.DefaultLoginRedirectPath;
    }
}

interface INavigationState {
    [ReturnUrlType]: string;
}
