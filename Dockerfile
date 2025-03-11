# ����� ������ ������ �� ASP.NET ���� 8.0 �����
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80

# ���� �� ������ �� SDK ������ ���������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
