import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleDetailComponent } from './article-detail/article-detail.component';
import { NewArticleComponent } from './new-article/new-article.component';

const articlesRoutes: Routes = [
  { path: 'articles', component: ArticleListComponent, data: { animation: 'articles' } },
  { path: 'article/:id', component: ArticleDetailComponent, data: { animation: 'article' } },
  { path: 'articles/create', component: NewArticleComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(articlesRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class ArticlesRoutingModule { }
