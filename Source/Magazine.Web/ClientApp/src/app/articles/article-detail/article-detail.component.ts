import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleEntity } from '../article';
import { ArticleService } from '../article.service';
import { MessageService } from '../../message/message.service';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.less']
})
export class ArticleDetailComponent implements OnInit {

  article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, account: null, accountId: 0, comments: null };
  isReadonly = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService,
    private messageService: MessageService) {

    this.messageService.broadcastMessageReceived.subscribe(message => {
      debugger;
      alert("This article was changed by another user. Refresh this article to get last changes.");
    });  
  }

  ngOnInit() {
    debugger;

    this.route.paramMap.subscribe(
      params => {
        debugger;
        if (params.get('id') !== null) {
          this.articleService.getArticle(+params.get('id'))
            .subscribe(
              result => {
                debugger;
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
    debugger;
    this.router.navigate(['/articles', { id: this.article !== null ? this.article.id : null }]);
  }

  createArticle() {

    // check body and title

    debugger;
    this.isReadonly = true;

    this.articleService.addArticle(this.article)
      .subscribe(
        result => this.article = result as ArticleEntity,
        response => {
          debugger;
          alert(`Error while creating article. ${response.error.Message}`);
          this.isReadonly = false;
        }
      );
  }

  updateArticle() {
    debugger;

    this.isReadonly = !this.isReadonly;

    if (this.isReadonly === false)
      return;

    this.articleService.updateArticle(this.article)
      .subscribe(
        result => {
          debugger;

          this.article = result as ArticleEntity;
          this.messageService.sendMessage("some message");
        },
        () => alert('Error while updating article')
      );
  }

  deleteArticle() {
    debugger;
    this.articleService.deleteArticle(this.article.id)
      .subscribe(
        () => this.navigateToArticles(),
        () => alert('Error while deleting article')
      );
  }
}
