import { ViewportScroller } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor (
    private route: ActivatedRoute,
    private viewportSroller: ViewportScroller
  ) {
    this.route.fragment.subscribe(fragment => {
      if (fragment) {
        this.viewportSroller.scrollToAnchor(fragment);
      }
    });
  }

  navigateToAboutSection() {
    
  }

  myInterval = 5000;
  activeSlideIndex = 0;
  slides: {title: string; image: string; text?: string}[] = [
    {title: "Unlock the Future", image: 'assets/images/laptop-banner.jpg', text: "VIEW MORE"},
    {title: "Elevate your gaming expirience",  image: 'assets/images/keyboard-banner.jpg', text: "VIEW MORE"},
    {title: "broaden your horizons",  image: 'assets/images/monitor-banner.jpg', text: "VIEW MORE"}
  ];
  showIndicator = false;
}
