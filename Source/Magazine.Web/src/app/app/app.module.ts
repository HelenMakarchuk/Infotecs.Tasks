import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PortalModule } from '@angular/cdk/portal';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import 'src/polyfills'
import { SignalrService } from '../server-communication/services/signalr.service';
import { ServerCommunicationService } from '../server-communication/contracts/server-communication.service';
import { CommentComponent } from '../comment/components/comment.component';
import { ArticleDetailComponent } from '../article/components/article-detail/article-detail.component';
import { ArticleListComponent } from '../article/components/article-list/article-list.component';
import { AuthorizationGuard } from '../authorization/authorization.guard';
import { LoginComponent } from '../authorization/login/login.component';
import { LogoutComponent } from '../authorization/logout/logout.component';
import { CommonModule } from '@angular/common';
import { ApplicationPaths } from '../authorization/authorization.constants';

@NgModule({
    declarations: [
        AppComponent,
        ArticleDetailComponent,
        ArticleListComponent,
        CommentComponent,
        LoginComponent,
        LogoutComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        CommonModule,
        HttpClientModule,
        FormsModule,
        MatNativeDateModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSelectModule,
        BrowserAnimationsModule,
        RouterModule.forRoot([
            { path: '', component: ArticleListComponent, pathMatch: 'full' },
            { path: 'articles', component: ArticleListComponent },
            { path: 'article/:id', component: ArticleDetailComponent, canActivate: [AuthorizationGuard] },
            { path: 'articles/create', component: ArticleDetailComponent, canActivate: [AuthorizationGuard] },
            { path: ApplicationPaths.Register, component: LoginComponent },
            { path: ApplicationPaths.Profile, component: LoginComponent },
            { path: ApplicationPaths.Login, component: LoginComponent },
            { path: ApplicationPaths.LoginFailed, component: LoginComponent },
            { path: ApplicationPaths.LoginCallback, component: LoginComponent },
            { path: ApplicationPaths.LogOut, component: LogoutComponent },
            { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
            { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
        ])
    ],
    exports: [
        CdkStepperModule,
        MatAutocompleteModule,
        MatBadgeModule,
        MatBottomSheetModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatStepperModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatTabsModule,
        MatToolbarModule,
        PortalModule,
        ScrollingModule,
        LoginComponent,
        LogoutComponent
    ],
    entryComponents: [AppComponent],
    bootstrap: [AppComponent],
    providers: [
        { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'fill' } },
        { provide: ServerCommunicationService, useClass: SignalrService },
    ]
})
export class AppModule { }
