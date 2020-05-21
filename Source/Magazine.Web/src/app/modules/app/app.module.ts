import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from '../../components/app/app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ArticleDetailComponent } from '../../components/article/article-detail/article-detail.component';
import { ArticleListComponent } from '../../components/article/article-list/article-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ArticleDetailComponent,
    ArticleListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: ArticleListComponent, pathMatch: 'full' },
      { path: 'articles', component: ArticleListComponent },
      { path: 'article/:id', component: ArticleDetailComponent },
      { path: 'articles/create', component: ArticleDetailComponent }
    ])
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
