import { Component, OnInit } from '@angular/core';
import { CommentService } from 'src/app/services/comment/comment.service';
import { Comment } from 'src/app/models/comment/comment';

@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html',
    styleUrls: ['./comment.component.less']
})
export class CommentComponent implements OnInit {
    comments: Comment[];

    constructor(private commentService: CommentService) { }

    ngOnInit() {
        this.commentService.getComments()
            .subscribe(
                result => {
                    this.comments = result;
                },
                () => alert('Error while fetching comments')
            )
    }
}
