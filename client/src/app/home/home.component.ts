import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  myInterval = 1500;
  activeSlideIndex = 0;
  slides: {image: string; text?: string}[] = [
    {image: 'assets/images/hero3.jpg'},
    {image: 'assets/images/hero2.jpg'},
    {image: 'assets/images/hero1.jpg'}
  ];
  showIndicator = false;
}
