import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { ConfigService } from './config/config.service';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ArticleDetailComponent } from './articles/article-detail/article-detail.component';
import { ArticleListComponent } from './articles/article-list/article-list.component';
import { NewArticleComponent } from './articles/new-article/new-article.component';

@NgModule({
  declarations: [
    AppComponent,
    ArticleDetailComponent,
    ArticleListComponent,
    NewArticleComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: ArticleListComponent, pathMatch: 'full' },
      { path: 'articles', component: ArticleListComponent },
      { path: 'article/:id', component: ArticleDetailComponent },
      { path: 'articles/create', component: NewArticleComponent }
    ])
  ],
  providers: [
    ConfigService,
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [ConfigService],
      useFactory: (configService: ConfigService) => {
        return () => configService.load();
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
