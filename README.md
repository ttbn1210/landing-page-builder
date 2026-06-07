# Landing Page Builder - .NET 10 Razor Pages

A modern, web-based landing page builder built with .NET 10 and Razor Pages. Create beautiful, responsive landing pages without writing code.

## Features

- 🎨 **Drag-and-Drop Editor** - Intuitive visual page builder
- 📱 **Mobile Responsive** - All pages are automatically optimized for mobile
- 🔐 **User Authentication** - Secure login with ASP.NET Identity
- 💾 **Database Storage** - Pages and components stored in SQL Server
- ⚡ **Fast Performance** - Built on .NET 10 for speed
- 📊 **Component Library** - Pre-built components (Hero, Features, Testimonials, etc.)
- 🌐 **SEO Friendly** - Meta tags and optimization support
- 📡 **REST API** - Full API for component management

## Tech Stack

- **Framework**: .NET 10
- **UI**: Razor Pages, Bootstrap 5
- **Database**: SQL Server (with Entity Framework Core)
- **Authentication**: ASP.NET Identity
- **Frontend**: HTML5, CSS3, JavaScript

## Project Structure

```
├── Controllers/           # API controllers
├── Data/                  # Database context and repositories
├── Models/                # Entity models
├── Pages/                 # Razor Pages
│   ├── Dashboard.cshtml   # User dashboard
│   ├── Index.cshtml       # Home page
│   └── Pages/
│       ├── Create.cshtml  # Create new page
│       ├── Edit.cshtml    # Edit page
│       └── View.cshtml    # View published page
├── wwwroot/               # Static files (CSS, JS, images)
└── Program.cs             # Application startup
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server 2019+ or LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ttbn1210/landing-page-builder.git
   cd landing-page-builder
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection**
   
   Edit `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=LandingPageBuilder;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
     }
   }
   ```

4. **Run migrations**
   ```bash
   dotnet ef database update
   ```

5. **Start the application**
   ```bash
   dotnet run
   ```

   The application will be available at `https://localhost:5001`

## Usage

### Create a Landing Page

1. Register or login to your account
2. Click "Create New Page" from the dashboard
3. Enter page title, slug, and metadata
4. Click "Create Page"
5. Use the editor to add components
6. Save and publish when ready

### Available Components

- **Hero Section** - Large banner with headline and CTA
- **Features** - Display key features in a grid
- **Testimonials** - Customer testimonials and reviews
- **Call to Action** - Button with headline and description
- **Text Content** - Rich text sections
- **Image Gallery** - Showcase images
- **Newsletter** - Email subscription form
- **Pricing** - Pricing plans comparison

## Database Schema

### Tables

- `AspNetUsers` - User accounts and profiles
- `LandingPages` - Landing page metadata
- `PageComponents` - Page components and content
- `ComponentTypes` - Available component types

## API Endpoints

### Pages API
- `GET /api/pages/{id}` - Get page details
- `DELETE /api/pages/{id}` - Delete a page

### Components API
- `POST /api/components` - Create new component
- `GET /api/components/{id}` - Get component
- `PUT /api/components/{id}` - Update component
- `DELETE /api/components/{id}` - Delete component

## Configuration

### Environment Variables

Create a `.env` file (for development):
```
ConnectionString=Server=(localdb)\mssqllocaldb;Database=LandingPageBuilder_Dev;Trusted_Connection=true;
```

## Development

### Running in Development Mode

```bash
dotnet watch run
```

This will automatically reload when you make changes.

### Database Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Revert to previous migration
dotnet ef database update PreviousMigrationName
```

## Deployment

### Azure App Service

1. Create an App Service in Azure
2. Create a SQL Database
3. Update connection string in Application settings
4. Deploy using Visual Studio or Azure CLI

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /src
COPY ["LandingPageBuilder.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LandingPageBuilder.dll"]
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, email support@landingpagebuilder.com or open an issue on GitHub.

## Roadmap

- [ ] Drag-and-drop visual builder
- [ ] More component templates
- [ ] Custom domain support
- [ ] Analytics integration
- [ ] Multi-language support
- [ ] Team collaboration features
- [ ] Mobile app

## Changelog

### v1.0.0 (2024)
- Initial release
- Basic page creation and editing
- Component management
- User authentication
