import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { User, LoginRequest, RegisterRequest, AuthResponse } from '../models/user.model';
import { Cart, CartItem } from '../models/cart.model';
import { Order } from '../models/order.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Product endpoints
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}/Product`);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/Product/${id}`);
  }

  searchProducts(input: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}/Product/input?input=${encodeURIComponent(input)}`);
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(`${this.baseUrl}/Product`, product);
  }

  updateProduct(product: Product): Observable<any> {
    return this.http.put(`${this.baseUrl}/Product`, product);
  }

  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Product/${id}`);
  }

  // Auth endpoints
  login(email: string, password: string): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/Auth/login?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`, {});
  }

  register(user: RegisterRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/Auth/Register`, user);
  }

  // Cart endpoints
  getCartByUserId(userId: number): Observable<Cart> {
    return this.http.get<Cart>(`${this.baseUrl}/Cart/${userId}`);
  }

  addCart(cart: Cart): Observable<Cart> {
    return this.http.post<Cart>(`${this.baseUrl}/Cart`, cart);
  }

  updateCart(cart: Cart): Observable<any> {
    return this.http.put(`${this.baseUrl}/Cart`, cart);
  }

  deleteCart(cartId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Cart/${cartId}`);
  }

  // Order endpoints
  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/Order`);
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}/Order/${id}`);
  }

  createOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(`${this.baseUrl}/Order`, order);
  }

  updateOrder(id: number, order: Order): Observable<any> {
    return this.http.put(`${this.baseUrl}/Order/${id}`, order);
  }
}
