import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ArticleListComponent } from './article-list/article-list.component';
import { ArticleDetailComponent } from './article-detail/article-detail.component';
import { ArticlesRoutingModule } from './articles-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ArticlesRoutingModule
  ],
  declarations: [
    ArticleListComponent,
    ArticleDetailComponent
  ]
})
export class ArticlesModule { }
