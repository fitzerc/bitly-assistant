import { Component, OnInit, Input } from '@angular/core';
import { ShortLinkModel } from '../models/shorten-link-model';

@Component({
  selector: 'app-link-table',
  templateUrl: './link-table.component.html',
  styleUrls: ['./link-table.component.scss']
})
export class LinkTableComponent implements OnInit {
  @Input() Links: ShortLinkModel[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
