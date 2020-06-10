import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthorizationService } from '../authorization/authorization.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent  implements OnInit {
    public isAuthenticated: Observable<boolean>;
    public userName: Observable<string>;

    constructor(private authorizeService: AuthorizationService) { }

    ngOnInit() {
      this.isAuthenticated = this.authorizeService.isAuthenticated();
      this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    }
}
