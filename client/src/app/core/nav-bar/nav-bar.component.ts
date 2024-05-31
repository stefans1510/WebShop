import { ViewportScroller } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
import { CartService } from 'src/app/cart/cart.service';
import { CartItem } from 'src/app/shared/models/cart';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})

export class NavBarComponent implements OnInit {
  isAboutSectionActive = false;
  isContactSectionActive = false;

  constructor(
    public cartService: CartService,
    public accountService: AccountService, 
    private router: Router,
    private viewportScroller: ViewportScroller
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const tree = router.parseUrl(router.url);
        if (tree.fragment === 'about') {
          this.isAboutSectionActive = true;
          this.viewportScroller.scrollToAnchor('about');
        } else {
          this.isAboutSectionActive = false;
        }
        if (tree.fragment === 'contact') {
          this.isContactSectionActive = true;
          this.viewportScroller.scrollToAnchor('contact');
        } else {
          this.isContactSectionActive = false;
        }
      }
    });
  }
  
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  getCount(items: CartItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0)
  }

  navigateToAboutSection() {
    this.router.navigate(['/'], { fragment: 'about' });
  }
}
