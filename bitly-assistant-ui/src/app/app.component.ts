import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiService } from './api.service';
import { ShortLinkModel } from './models/shorten-link-model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public items: ShortLinkModel[] = [];
  public currentUrl = '';

  subs: Subscription[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.getUrls();
  }

  private getUrls(): void {
    this.subs.push(this.api.getAllUrls().subscribe(res => this.items = res));
  }

  public submitClicked(): void {
    this.api.shortenUrl({
      Url: this.currentUrl,
      Domain: 'bit.ly'
    })

    this.getUrls();
    this.currentUrl = '';
  }
}
