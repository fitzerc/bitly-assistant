import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShortenUrlRequest } from './models/shorten-url-request';

@Injectable({ providedIn: 'root' })
export class ApiService {
    private url = 'api/bitlyassistant';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) {}

  shortenUrl(request: ShortenUrlRequest): void {
      console.log(request);
    this.http.post<ShortenUrlRequest>(this.url, request, this.httpOptions).subscribe();
  }
}
