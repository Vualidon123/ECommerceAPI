import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Cart, CartItem } from '../models/cart.model';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartSubject = new BehaviorSubject<Cart | null>(null);
  public cart$ = this.cartSubject.asObservable();

  constructor(private apiService: ApiService) { }

  getCartByUserId(userId: number): Observable<Cart> {
    return this.apiService.getCartByUserId(userId);
  }

  addToCart(userId: number, productId: number, quantity: number = 1): Observable<Cart> {
    const cart: Cart = {
      id: 0,
      userId: userId,
      quantity: quantity,
      cartItems: [{
        id: 0,
        cartId: 0,
        productId: productId,
        quantity: quantity
      }]
    };

    return this.apiService.addCart(cart);
  }

  updateCart(cart: Cart): Observable<any> {
    return this.apiService.updateCart(cart);
  }

  removeFromCart(cartId: number): Observable<any> {
    return this.apiService.deleteCart(cartId);
  }

  getCartTotal(cart: Cart): number {
    if (!cart.cartItems) return 0;
    return cart.cartItems.reduce((total, item) => {
      return total + (item.product?.price || 0) * item.quantity;
    }, 0);
  }

  getCartItemCount(cart: Cart): number {
    if (!cart.cartItems) return 0;
    return cart.cartItems.reduce((total, item) => total + item.quantity, 0);
  }

  setCurrentCart(cart: Cart): void {
    this.cartSubject.next(cart);
  }

  getCurrentCart(): Cart | null {
    return this.cartSubject.value;
  }
}
