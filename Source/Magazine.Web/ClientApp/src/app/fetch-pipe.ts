import { Pipe, PipeTransform, Injectable } from "@angular/core";

@Pipe({
  name: 'fetch'
})
@Injectable({
  providedIn: 'root'
})
export class FetchPipe implements PipeTransform {
  transform(result: any, key: string) {
    debugger;

    return result.data[key];
  }
}
