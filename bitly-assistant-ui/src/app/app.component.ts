import { Component, OnInit } from '@angular/core';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public items: string[] = [];
  public currentUrl = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
  }

  public submitClicked(): void {
    this.items.push(this.currentUrl);

    this.api.shortenUrl({
      Url: this.currentUrl
    })

    this.currentUrl = '';
  }
}
