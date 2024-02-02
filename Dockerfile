FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Complaint-Report-Registering-API.csproj", ""]
RUN dotnet restore "./Complaint-Report-Registering-API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Complaint-Report-Registering-API.csproj" -c Release -o /app/build  

FROM build AS publish
RUN dotnet publish "Complaint-Report-Registering-API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Complaint-Report-Registering-API.dll"]