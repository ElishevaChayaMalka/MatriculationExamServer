# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Set the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MatriculationExamsServer/MatriculationExamsServer.csproj", "MatriculationExamsServer/"]
RUN dotnet restore "MatriculationExamsServer/MatriculationExamsServer.csproj"
COPY . .
WORKDIR "/src/MatriculationExamsServer"
RUN dotnet build "MatriculationExamsServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MatriculationExamsServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MatriculationExamsServer.dll"]
