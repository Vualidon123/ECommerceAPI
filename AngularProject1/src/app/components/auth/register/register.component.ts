import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { RegisterRequest } from '../../../models/user.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class RegisterComponent {
  user: RegisterRequest = {
    username: '',
    email: '',
    password: '',
    address: '',
    phoneNumber: ''
  };
  confirmPassword = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit(): void {
    if (!this.validateForm()) {
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.register(this.user).subscribe({
      next: (response) => {
        this.loading = false;
        alert('Registration successful! Please login.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.error = 'Registration failed. Please try again.';
        this.loading = false;
      }
    });
  }

  private validateForm(): boolean {
    if (!this.user.username || !this.user.email || !this.user.password || 
        !this.user.address || !this.user.phoneNumber) {
      this.error = 'Please fill in all fields';
      return false;
    }

    if (this.user.password !== this.confirmPassword) {
      this.error = 'Passwords do not match';
      return false;
    }

    if (this.user.password.length < 6) {
      this.error = 'Password must be at least 6 characters long';
      return false;
    }

    return true;
  }
}
