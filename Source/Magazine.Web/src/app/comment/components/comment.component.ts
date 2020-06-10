import { Component, OnInit, Inject } from '@angular/core';
import { CommentService } from 'src/app/comment/services/comment.service';
import { Comment } from 'src/app/comment/models/comment';
import { ArticleDetailComponent } from '../../article/components/article-detail/article-detail.component';
import { ServerCommunicationService } from 'src/app/server-communication/contracts/server-communication.service';
import { filter } from 'rxjs/operators';
import { CommentServiceAddEvent } from 'src/app/server-communication/events/comment/comment.service.add.event';

@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html',
    styleUrls: ['./comment.component.less']
})
export class CommentComponent implements OnInit {
    comments: Comment[];

    constructor(private commentService: CommentService,
                @Inject(ArticleDetailComponent) private articleDetailComponent: ArticleDetailComponent,
                private serverCommunicationService: ServerCommunicationService) {

        this.commentService.onAdd
            .pipe(filter(id => id === this.articleDetailComponent.article.id))
            .subscribe(id => {
                this.commentService.getComment(id).subscribe(comment => {
                    this.comments.push(comment);
                });
            });
     }

    ngOnInit() {
        this.commentService.getComments()
            .subscribe(
                comments => {
                    this.comments = comments;
                },
                error => {
                    console.log(error);
                    alert('Error while fetching comments');
                }
            )
    }

    createComment(): void {
        let comment = new Comment();
        comment.body = 'new comment';
        comment.articleId = this.articleDetailComponent.article.id;

        this.commentService.addComment(comment)
            .subscribe(
                comment => {
                    this.comments.push(comment);
                    this.serverCommunicationService.send(new CommentServiceAddEvent(comment.id));
                },
                error => {
                    console.log(error);
                    alert(`Error while creating comment`);
                }
        );
    }
}
