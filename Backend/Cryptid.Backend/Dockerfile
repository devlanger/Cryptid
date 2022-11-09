#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cryptid.Backend/Cryptid.Backend.csproj", "Cryptid.Backend/"]
RUN dotnet restore "Cryptid.Backend/Cryptid.Backend.csproj"
COPY . .
WORKDIR "/src/Cryptid.Backend"
RUN dotnet build "Cryptid.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cryptid.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cryptid.Backend.dll"]