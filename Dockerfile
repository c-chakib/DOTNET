# Dockerfile for building and running the WinForms application in a Windows container
# Requires Docker Desktop switched to Windows containers

# Build stage
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /src

# Copy project files
COPY GestionBudgetWinForms/ ./GestionBudgetWinForms/
WORKDIR /src/GestionBudgetWinForms

# Restore NuGet packages
RUN nuget restore GestionBudgetWinForms.csproj

# Build
RUN msbuild /p:Configuration=Release

# Runtime stage
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8-windowsservercore-ltsc2019
WORKDIR /app

# Copy build output
COPY --from=build /src/GestionBudgetWinForms/bin/Release/ ./

# Default command (Run the exe)
ENTRYPOINT ["GestionBudgetWinForms.exe"]
