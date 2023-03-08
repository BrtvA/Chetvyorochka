#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Chetvyorochka.PL/Chetvyorochka.PL.csproj", "src/Chetvyorochka.PL/"]
COPY ["src/Chetvyorochka.BL/Chetvyorochka.BL.csproj", "src/Chetvyorochka.BL/"]
COPY ["src/Chetvyorochka.DAL/Chetvyorochka.DAL.csproj", "src/Chetvyorochka.DAL/"]
RUN dotnet restore "src/Chetvyorochka.PL/Chetvyorochka.PL.csproj"
COPY . .
WORKDIR "/src/src/Chetvyorochka.PL"
RUN dotnet build "Chetvyorochka.PL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chetvyorochka.PL.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chetvyorochka.PL.dll"]