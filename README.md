# QR Commerce Platform

A complete startup-grade commercial software platform combining QR Order System, Customer Lead Collection, Android Telecom Gateway, SMS Confirmation, CRM, Campaign Analytics, Geo Tracking, and Multi-Location QR Management.

## Architecture

| Project | Description |
|---------|-------------|
| **QrCommerce.Web** | ASP.NET Core 9 Web Application (Razor Pages + Web API) |
| **QrCommerce.Android** | .NET MAUI Android App (Telecom Gateway) |
| **QrCommerce.Shared** | Shared models, DTOs, and enums |

## Technology Stack

- ASP.NET Core 9 Razor Pages
- ASP.NET Core Web API
- Entity Framework Core with SQLite
- .NET MAUI Android App
- Bootstrap 5
- Chart.js Analytics
- BackgroundService Workers
- Dependency Injection

## Getting Started

### Prerequisites

- .NET 9 SDK
- (For Android) .NET MAUI workload: `dotnet workload install maui`

### Run the Web Application

```bash
cd QrCommerce.Web
dotnet run
```

The application will start at `http://localhost:5000`. The database is auto-created with seed data.

### Admin Dashboard

Navigate to `/Admin/Dashboard` to access the admin panel.

### Customer Order Flow

Scan any QR code or navigate to `/q/{qrCode}` (e.g., `/q/QR-REST-T01`).

### API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/device/jobs` | GET | Get pending telecom jobs |
| `/api/device/report` | POST | Report job status |
| `/api/device/heartbeat` | POST | Device heartbeat |
| `/api/device/register` | POST | Register new device |
| `/api/orders` | POST | Create order |

### Android App

The Android app acts as a Telecom Gateway:
1. Configure server URL and API key in Settings
2. Start the polling service from Dashboard
3. The app polls for pending SMS jobs every 5 seconds
4. Uses the device's SIM card to send SMS confirmations

## Seed Data

The application comes pre-loaded with:
- 5 Marketing Campaigns
- 10 QR Locations (restaurants, schools, vehicles, malls, streets)
- 10 Products (Vietnamese food and drinks)
- 5 Customers
- 3 Telecom Devices
- Sample orders, telecom jobs, and scan logs

## Features

- QR code scanning and order placement
- Real-time campaign analytics with Chart.js
- Telecom gateway management
- SMS retry logic (max 3 retries)
- Device heartbeat monitoring
- Rate limiting and API key authentication
- Geo-location tracking
- Responsive mobile order page
