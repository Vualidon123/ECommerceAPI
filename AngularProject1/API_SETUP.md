# API Setup Guide

This guide will help you configure the Angular frontend to connect to your ECommerce API backend.

## Prerequisites

1. Ensure your ECommerce API backend is running
2. Note the API URL (typically `https://localhost:7000` for development)

## Configuration Steps

### 1. Update Environment Configuration

Edit `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7000/api' // Replace with your actual API URL
};
```

### 2. Update Production Environment

Edit `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-domain.com/api' // Replace with your production API URL
};
```

### 3. CORS Configuration

Ensure your API backend has CORS enabled for the Angular frontend. In your API's `Program.cs` or `Startup.cs`, add:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// In the pipeline
app.UseCors("AllowAngularApp");
```

### 4. Test the Connection

1. Start your API backend
2. Start the Angular frontend: `npm start`
3. Open the browser to `http://localhost:4200`
4. Check the browser's developer console for any API connection errors

## API Endpoints

The frontend expects these endpoints to be available:

### Products
- `GET /api/Product` - List all products
- `GET /api/Product/{id}` - Get product by ID
- `GET /api/Product/input?input={searchTerm}` - Search products

### Authentication
- `POST /api/Auth/login?email={email}&password={password}` - Login
- `POST /api/Auth/Register` - Register new user

### Cart
- `GET /api/Cart/{userId}` - Get user's cart
- `POST /api/Cart` - Add/update cart
- `PUT /api/Cart` - Update cart
- `DELETE /api/Cart/{cartId}` - Delete cart

### Orders
- `GET /api/Order` - List all orders
- `GET /api/Order/{id}` - Get order by ID
- `POST /api/Order` - Create order
- `PUT /api/Order/{id}` - Update order

## Troubleshooting

### Connection Refused
- Verify the API is running
- Check the API URL in environment files
- Ensure no firewall is blocking the connection

### CORS Errors
- Add the Angular app URL to CORS policy
- Ensure CORS is configured before other middleware

### 404 Errors
- Verify the API endpoints match exactly
- Check the API routing configuration
- Ensure the API is running on the correct port

### Authentication Issues
- Verify JWT token handling
- Check if the API returns tokens in the expected format
- Ensure proper error handling for authentication failures

## Development Tips

1. Use browser developer tools to monitor network requests
2. Check the console for any JavaScript errors
3. Verify API responses match the expected data structure
4. Test with different user roles if applicable
