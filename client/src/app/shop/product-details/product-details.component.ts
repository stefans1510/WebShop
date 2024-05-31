import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CartService } from 'src/app/cart/cart.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  quantity = 1;
  quantityInCart = 0;
  
  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute, 
    private bcService: BreadcrumbService,
    private cartService: CartService
  ) {
    this.bcService.set('@productDetails', ' ')
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) this.shopService.getProduct(+id).subscribe({
      next: product => {
        this.product = product;
        this.bcService.set('@productDetails', product.name);
        this.cartService.cartSource$.pipe(take(1)).subscribe({
          next: cart => {
            const item = cart?.items.find(x => x.id === +id);

            if (item) {
              this.quantity = item.quantity;
              this.quantityInCart = item.quantity;
            }
          }
        })
      },
      error: error => console.log(error)
    })
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
  }
  }

  updateCart() {
    if (this.product) {
      if (this.quantity > this.quantityInCart) {
        const itemsToAdd = this.quantity - this.quantityInCart;
        this.quantityInCart += itemsToAdd;
        this.cartService.addItemToCart(this.product, itemsToAdd);
      } else {
        const itemsToRemove = this.quantityInCart - this.quantity;
        this.quantityInCart -= itemsToRemove;
        this.cartService.removeItemFromCart(this.product.id, itemsToRemove);
      }
    }
  }

  get buttonText() {
    return this.quantityInCart === 0 ? 'Add to cart': 'Update cart';
  }
}
