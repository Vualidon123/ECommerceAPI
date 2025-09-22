import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { User, LoginRequest, RegisterRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
    // Check for stored token on service initialization
    const token = localStorage.getItem('token');
    if (token) {
      // You might want to validate the token here
      // For now, we'll just set a placeholder user
      this.currentUserSubject.next({} as User);
    }
  }

  login(email: string, password: string): Observable<string> {
    return this.apiService.login(email, password);
  }

  register(user: RegisterRequest): Observable<any> {
    return this.apiService.register(user);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }
}
