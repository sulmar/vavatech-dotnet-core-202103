#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Vavatech.DotnetCore.WebApi/Vavatech.DotnetCore.WebApi.csproj", "Vavatech.DotnetCore.WebApi/"]
COPY ["Vavatech.DotnetCore.IServices/Vavatech.DotnetCore.IServices.csproj", "Vavatech.DotnetCore.IServices/"]
COPY ["Vavatech.DotnetCore.Models/Vavatech.DotnetCore.Models.csproj", "Vavatech.DotnetCore.Models/"]
COPY ["Vavatech.DotnetCore.FakeServices/Vavatech.DotnetCore.FakeServices.csproj", "Vavatech.DotnetCore.FakeServices/"]
COPY ["Vavatech.DotnetCore.Fakers/Vavatech.DotnetCore.Fakers.csproj", "Vavatech.DotnetCore.Fakers/"]
COPY ["Vavatech.DotnetCore.NBPExchangeRateService/Vavatech.DotnetCore.NBPExchangeRateService.csproj", "Vavatech.DotnetCore.NBPExchangeRateService/"]
COPY ["Vavatech.DotnetCore.Models.Validators/Vavatech.DotnetCore.Models.Validators.csproj", "Vavatech.DotnetCore.Models.Validators/"]
RUN dotnet restore "Vavatech.DotnetCore.WebApi/Vavatech.DotnetCore.WebApi.csproj"
COPY . .
WORKDIR "/src/Vavatech.DotnetCore.WebApi"
RUN dotnet build "Vavatech.DotnetCore.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vavatech.DotnetCore.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vavatech.DotnetCore.WebApi.dll"]