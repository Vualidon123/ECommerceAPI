import { Product } from './product.model';

export interface Cart {
  id: number;
  userId: number;
  quantity: number;
  cartItems?: CartItem[];
}

export interface CartItem {
  id: number;
  cartId: number;
  productId: number;
  quantity: number;
  product?: Product;
}
