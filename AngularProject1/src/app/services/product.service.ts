import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiService } from './api.service';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productsSubject = new BehaviorSubject<Product[]>([]);
  public products$ = this.productsSubject.asObservable();

  constructor(private apiService: ApiService) { }

  getAllProducts(): Observable<Product[]> {
    return this.apiService.getProducts();
  }

  getProductById(id: number): Observable<Product> {
    return this.apiService.getProductById(id);
  }

  searchProducts(input: string): Observable<Product[]> {
    return this.apiService.searchProducts(input);
  }

  addProduct(product: Product): Observable<Product> {
    return this.apiService.addProduct(product);
  }

  updateProduct(product: Product): Observable<any> {
    return this.apiService.updateProduct(product);
  }

  deleteProduct(id: number): Observable<any> {
    return this.apiService.deleteProduct(id);
  }

  setProducts(products: Product[]): void {
    this.productsSubject.next(products);
  }

  getCurrentProducts(): Product[] {
    return this.productsSubject.value;
  }
}
