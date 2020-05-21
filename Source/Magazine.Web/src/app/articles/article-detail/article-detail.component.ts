import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleEntity } from '../article';
import { ArticleService } from '../../services/article/article.service';
import { MessageHubService } from '../../services/message/message-hub.service';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.less']
})
export class ArticleDetailComponent implements OnInit, AfterViewInit {

  article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, account: null, accountId: 0, comments: null };
  isReadonly = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private messageService: MessageHubService) { }

  ngAfterViewInit() {
    this.messageService.message.subscribe(message => {
      alert(message);
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(
      params => {
        if (params.get('id') !== null) {
          this.articleService.getArticle(+params.get('id'))
            .subscribe(
              result => {
                this.article = result as ArticleEntity;
                this.isReadonly = true;
              },
              () => alert('Error while opening article')
            );
        }
      }
    );
  }

  navigateToArticles() {
    this.router.navigate(['/articles', { id: this.article !== null ? this.article.id : null }]);
  }

  createArticle() {
    this.isReadonly = true;

    this.articleService.addArticle(this.article)
      .subscribe(
        result => this.article = result as ArticleEntity,
        response => {
          alert(`Error while creating article. ${response.error.Message}`);
          this.isReadonly = false;
        }
      );
  }

  updateArticle() {
    this.isReadonly = !this.isReadonly;

    if (this.isReadonly === false)
      return;

    this.articleService.updateArticle(this.article)
      .subscribe(
        result => {
          this.article = result as ArticleEntity;
          this.messageService.sendMessage("This article was changed by another user. Refresh this article to get last changes.");
        },
        () => alert('Error while updating article')
      );
  }

  deleteArticle() {
    this.articleService.deleteArticle(this.article.id)
      .subscribe(
        () => this.navigateToArticles(),
        () => alert('Error while deleting article')
      );
  }
}
