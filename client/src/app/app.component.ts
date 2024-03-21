import { Component, OnInit } from '@angular/core';
import { Product } from './shared/models/products';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'PC Store';
  products: Product[] = [];

  constructor() {
  }

  ngOnInit(): void {
  }
}