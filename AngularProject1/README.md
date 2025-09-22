# ECommerce Angular Frontend

A modern, responsive Angular frontend application for the ECommerce API. This application provides a complete e-commerce experience with product browsing, user authentication, shopping cart functionality, and more.

## Features

- **Product Management**: Browse, search, and view product details
- **User Authentication**: Login and registration functionality
- **Shopping Cart**: Add, remove, and manage cart items
- **Responsive Design**: Mobile-first design that works on all devices
- **Modern UI**: Clean, professional interface with smooth animations
- **API Integration**: Full integration with the ECommerce API backend

## Prerequisites

- Node.js (version 18 or higher)
- npm (version 8 or higher)
- Angular CLI (version 20 or higher)
- ECommerce API backend running

## Installation

1. Navigate to the project directory:
   ```bash
   cd AngularProject1
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Configure the API URL in `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7000/api' // Update this to match your API URL
   };
   ```

## Running the Application

1. Start the development server:
   ```bash
   npm start
   ```

2. Open your browser and navigate to `http://localhost:4200`

3. The application will automatically reload when you make changes to the source files.

## Project Structure

```
src/
├── app/
│   ├── components/           # Angular components
│   │   ├── auth/            # Authentication components
│   │   │   ├── login/       # Login component
│   │   │   └── register/    # Registration component
│   │   ├── cart/            # Shopping cart component
│   │   ├── header/          # Navigation header
│   │   ├── home/            # Home page component
│   │   ├── product-detail/  # Product detail page
│   │   └── product-list/    # Product listing page
│   ├── models/              # TypeScript interfaces
│   │   ├── cart.model.ts
│   │   ├── order.model.ts
│   │   ├── product.model.ts
│   │   └── user.model.ts
│   ├── services/            # Angular services
│   │   ├── api.service.ts   # Main API service
│   │   ├── auth.service.ts  # Authentication service
│   │   ├── cart.service.ts  # Cart management service
│   │   └── product.service.ts # Product service
│   ├── app-module.ts        # Main app module
│   ├── app-routing-module.ts # Routing configuration
│   ├── app.html             # Main app template
│   ├── app.css              # Main app styles
│   └── app.ts               # Main app component
├── environments/            # Environment configurations
│   ├── environment.ts       # Development environment
│   └── environment.prod.ts  # Production environment
└── styles.css              # Global styles
```

## API Integration

The application integrates with the following API endpoints:

### Products
- `GET /api/Product` - Get all products
- `GET /api/Product/{id}` - Get product by ID
- `GET /api/Product/input?input={searchTerm}` - Search products
- `POST /api/Product` - Add new product (Admin only)
- `PUT /api/Product` - Update product (Admin only)
- `DELETE /api/Product/{id}` - Delete product (Admin only)

### Authentication
- `POST /api/Auth/login` - User login
- `POST /api/Auth/Register` - User registration

### Cart
- `GET /api/Cart/{userId}` - Get user's cart
- `POST /api/Cart` - Add/update cart
- `PUT /api/Cart` - Update cart
- `DELETE /api/Cart/{cartId}` - Delete cart

### Orders
- `GET /api/Order` - Get all orders
- `GET /api/Order/{id}` - Get order by ID
- `POST /api/Order` - Create new order
- `PUT /api/Order/{id}` - Update order

## Key Features

### Product Browsing
- View all available products
- Search products by name or description
- Filter products by category
- View detailed product information
- Add products to cart

### User Authentication
- User registration with validation
- Secure login functionality
- Session management with JWT tokens
- Protected routes for authenticated users

### Shopping Cart
- Add/remove items from cart
- Update item quantities
- View cart total and item count
- Persistent cart across sessions

### Responsive Design
- Mobile-first approach
- Responsive grid layouts
- Touch-friendly interface
- Optimized for all screen sizes

## Development

### Adding New Components
1. Generate a new component:
   ```bash
   ng generate component components/your-component-name
   ```

2. Add the component to the appropriate module
3. Update routing if needed

### Adding New Services
1. Generate a new service:
   ```bash
   ng generate service services/your-service-name
   ```

2. Implement the service logic
3. Inject the service where needed

### Styling
- Global styles are in `src/styles.css`
- Component-specific styles are in each component's CSS file
- Uses CSS Grid and Flexbox for layouts
- Responsive design with mobile-first approach

## Building for Production

1. Build the application:
   ```bash
   npm run build
   ```

2. The build artifacts will be stored in the `dist/` directory

3. Update the production API URL in `src/environments/environment.prod.ts`

## Troubleshooting

### Common Issues

1. **API Connection Issues**
   - Verify the API URL in environment files
   - Ensure the backend API is running
   - Check CORS settings on the backend

2. **Authentication Issues**
   - Verify JWT token handling
   - Check localStorage for stored tokens
   - Ensure proper error handling

3. **Build Issues**
   - Clear node_modules and reinstall: `rm -rf node_modules && npm install`
   - Update Angular CLI: `npm install -g @angular/cli@latest`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.