import { Component, OnInit, Inject } from '@angular/core';
import { CommentService } from 'src/app/services/comment/comment.service';
import { Comment } from 'src/app/models/comment/comment';
import { ArticleDetailComponent } from '../article/article-detail/article-detail.component';
import { ServerCommunicationService } from 'src/app/contracts/service/server-communication.service';
import { CommentServiceAddEvent } from 'src/app/events/comment/comment.service.add.event';
import { filter } from 'rxjs/operators';

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
                result => {
                    this.comments = result;
                },
                () => alert('Error while fetching comments')
            )
    }

    createComment(): void {
        let comment = new Comment();
        comment.body = 'new comment';
        comment.articleId = this.articleDetailComponent.article.id;

        this.commentService.addComment(comment).subscribe(
            (comment: Comment) => {
                this.comments.push(comment);
                this.serverCommunicationService.send(new CommentServiceAddEvent(comment.id));
            },
            response => {
                alert(`Error while creating comment. ${response.error.Message}`);
            }
        );
    }
}
