import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
/*import { Observable, throwError } from 'rxjs'*/;
//import { catchError, retry } from 'rxjs/operators';
//import { Config } from './config';
import { environment } from '../../environments/environment';
import { IConfig } from '../models/IConfig';

@Injectable()
export class ConfigService {

  static settings: IConfig;

  constructor(private http: HttpClient) { }

  load() {
    const jsonFile = `assets/config/config.${environment.name}.json`;

    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile).toPromise().then((response: IConfig) => {
        ConfigService.settings = <IConfig>response;
        resolve();
      }).catch((response: any) => {
        reject(`Could not load file '${jsonFile}': ${JSON.stringify(response)}`);
      });
    });
  }
}
