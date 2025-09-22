export interface User {
  id: number;
  username: string;
  email: string;
  password?: string;
  address: string;
  phoneNumber: string;
  role: Role;
  cartId: number;
}

export enum Role {
  Customer = 0,
  Seller = 1,
  Admin = 2
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  address: string;
  phoneNumber: string;
}

export interface AuthResponse {
  token: string;
  user?: User;
}
