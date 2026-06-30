---
name: testing-trackqr
description: Test the TrackQR MVP end-to-end. Use when verifying admin dashboard, QR code routing, landing pages, lead capture, or analytics changes.
---

# Testing TrackQR

## Prerequisites

- .NET 9 SDK installed (`dotnet --version` should show 9.x)
- No external services needed — app uses SQLite (file-based) and MemoryCache

## Running the App Locally

```bash
cd /home/ubuntu/repos/TrackQR
dotnet run --urls="http://localhost:5050"
```

Note: Port 5000 is often in use on Ubuntu. Use 5050 or another free port.

The app auto-creates the SQLite database and seeds sample data on first run (2 campaigns, 3 placements, 2 landing pages, 3 QR codes).

## Key Test Flows

### 1. Admin Dashboard
- Navigate to `http://localhost:5050/Dashboard`
- Verify 4 stat cards render (Total Scans Today, Unique Visitors, Leads Today, Conversion Rate)
- Verify Top QR Codes and Top Campaigns tables show seed data

### 2. Campaign CRUD
- Navigate to `/Campaigns` → Click "New Campaign" → Fill form → Submit
- Verify new campaign appears in list with "Draft" status

### 3. Public QR Landing Page (Core Feature)
- Navigate to `http://localhost:5050/{shortCode}` (e.g. `/summer-car-d1`)
- Verify landing page HTML renders with Vietnamese content
- Verify `window.__TRACKQR__` is present with a `sessionKey` value
- Verify `tracker.js` is referenced in the page

### 4. Lead Capture End-to-End
- Get a session key by visiting a QR slug (check `window.__TRACKQR__.sessionKey` in console)
- POST to `/api/track/lead` with valid session key:
  ```bash
  curl -X POST http://localhost:5050/api/track/lead \
    -H "Content-Type: application/json" \
    -d '{"sessionKey":"<session_key>","fullName":"Test","phone":"0909999888","email":"test@test.com","message":"test"}'
  ```
- Verify lead appears in admin at `/Leads`

### 5. Tracking API
- POST to `/api/track/event` with valid session key:
  ```bash
  curl -X POST http://localhost:5050/api/track/event \
    -H "Content-Type: application/json" \
    -d '{"sessionKey":"<session_key>","eventType":"PageView","elementName":"test","value":""}'
  ```
- Should return 200 for valid session, 404 for invalid

### 6. Invalid QR Code 404
- Navigate to `http://localhost:5050/nonexistent-slug`
- Should show "QR Code not found or inactive."

## Known Issues

- `tracker.js` uses `App:BaseUrl` from appsettings.json as `apiBase`. For local dev, this might not match the running port. In production with a proper domain this works correctly. For local testing, either update `appsettings.Development.json` or test the tracking API via curl directly.

## Admin Pages Available

| Page | URL |
|------|-----|
| Dashboard | `/Dashboard` |
| Campaigns | `/Campaigns` |
| Placements | `/Placements` |
| Landing Pages | `/LandingPages` |
| QR Codes | `/QRCodes` |
| Leads | `/Leads` |
| Analytics | `/Analytics` |
| Settings | `/Settings` |

## Seed QR Short Codes

- `summer-car-d1` — Summer Sale 2024 / Car Sticker
- `summer-pho24` — Summer Sale 2024 / Restaurant Table Tent
- `school-hcmus` — Back to School / School Banner
