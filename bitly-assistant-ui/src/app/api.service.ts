import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShortLinkModel } from './models/shorten-link-model';
import { ShortenUrlRequest } from './models/shorten-url-request';

@Injectable({ providedIn: 'root' })
export class ApiService {
    private url = 'api/bitlyassistant';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) {}

  shortenUrl(request: ShortenUrlRequest): void {
    this.http.post<ShortenUrlRequest>(this.url, request, this.httpOptions).subscribe();
  }

  getAllUrls(): Observable<ShortLinkModel[]> {
    return this.http.get<ShortLinkModel[]>(this.url, this.httpOptions);
  }
}
