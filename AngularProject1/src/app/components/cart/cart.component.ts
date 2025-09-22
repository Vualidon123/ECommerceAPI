import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { Cart, CartItem } from '../../models/cart.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class CartComponent implements OnInit {
  cart: Cart | null = null;
  loading = true;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    const user = this.authService.getCurrentUser();
    if (user) {
      this.loadCart(user.id);
    }
  }

  loadCart(userId: number): void {
    this.cartService.getCartByUserId(userId).subscribe({
      next: (cart) => {
        this.cart = cart;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading cart:', error);
        this.loading = false;
      }
    });
  }

  updateQuantity(item: CartItem, newQuantity: number): void {
    if (newQuantity < 1) {
      this.removeItem(item);
      return;
    }

    if (!this.cart) return;

    // Update the cart item quantity
    const cartItem = this.cart.cartItems?.find(ci => ci.id === item.id);
    if (cartItem) {
      cartItem.quantity = newQuantity;
    }

    // Update the cart on the server
    this.cartService.updateCart(this.cart).subscribe({
      next: (response) => {
        this.cartService.setCurrentCart(this.cart!);
      },
      error: (error) => {
        console.error('Error updating cart:', error);
        alert('Error updating cart');
      }
    });
  }

  removeItem(item: CartItem): void {
    if (!this.cart || !this.cart.cartItems) return;

    // Remove item from local cart
    this.cart.cartItems = this.cart.cartItems.filter(ci => ci.id !== item.id);

    // Update cart on server
    this.cartService.updateCart(this.cart).subscribe({
      next: (response) => {
        this.cartService.setCurrentCart(this.cart!);
      },
      error: (error) => {
        console.error('Error removing item:', error);
        alert('Error removing item from cart');
      }
    });
  }

  getCartTotal(): number {
    return this.cart ? this.cartService.getCartTotal(this.cart) : 0;
  }

  getCartItemCount(): number {
    return this.cart ? this.cartService.getCartItemCount(this.cart) : 0;
  }

  proceedToCheckout(): void {
    // For now, just show an alert
    // In a real app, you would navigate to a checkout page
    alert('Checkout functionality would be implemented here');
  }

  continueShopping(): void {
    this.router.navigate(['/products']);
  }
}
