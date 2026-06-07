FROM mcr.microsoft.com/dotnet/aspnet:10.0 as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /src
COPY ["LandingPageBuilder.csproj", "./"]
RUN dotnet restore "LandingPageBuilder.csproj"
COPY . .
RUN dotnet build "LandingPageBuilder.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "LandingPageBuilder.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LandingPageBuilder.dll"]
