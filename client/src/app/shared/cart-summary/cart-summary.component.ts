import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CartItem } from '../models/cart';
import { CartService } from 'src/app/cart/cart.service';

@Component({
  selector: 'app-cart-summary',
  templateUrl: './cart-summary.component.html',
  styleUrls: ['./cart-summary.component.scss']
})
export class CartSummaryComponent {
  @Output() addItem = new EventEmitter<CartItem>();
  @Output() removeItem = new EventEmitter<{id: number, quantity: number}>();
  @Input() isCart = true;

  constructor(public cartService: CartService) {}

  addCartItem(item: CartItem) {
    this.addItem.emit(item);
  }

  removeCartItem(id: number, quantity: number) {
    this.removeItem.emit({id, quantity});
  }
}
