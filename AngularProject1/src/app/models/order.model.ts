export interface Order {
  id: number;
  userId: number;
  orderDate: string;
  status: OrderStatus;
  totalAmount: number;
  orderDetails?: OrderDetails[];
}

export interface OrderDetails {
  id: number;
  orderId: number;
  productId: number;
  quantity: number;
  price: number;
}

export enum OrderStatus {
  Pending = 0,
  Shipped = 1,
  Delivered = 2,
  Cancelled = 3
}
