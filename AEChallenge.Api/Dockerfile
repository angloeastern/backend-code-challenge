# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AEChallenge.Api/AEChallenge.Api.csproj", "AEChallenge.Api/"]
COPY ["AEChallenge.Api.Tests/AEChallenge.Api.Tests.csproj", "AEChallenge.Api.Tests/"]
COPY ["AEChallenge.Application/AEChallenge.Application.csproj", "AEChallenge.Application/"]
COPY ["AEChallenge.Domain/AEChallenge.Domain.csproj", "AEChallenge.Domain/"]
COPY ["AEChallenge.Infrastructure/AEChallenge.Infrastructure.csproj", "AEChallenge.Infrastructure/"]
COPY ["AEChallenge.Application.Tests/AEChallenge.Application.Tests.csproj", "AEChallenge.Application.Tests/"]
COPY ["AEChallenge.Domain.Tests/AEChallenge.Domain.Tests.csproj", "AEChallenge.Domain.Tests/"]
COPY ["AEChallenge.Infrastructure.Tests/AEChallenge.Infrastructure.Tests.csproj", "AEChallenge.Infrastructure.Tests/"]
COPY ["AEChallenge.sln", "./"]

RUN dotnet restore "AEChallenge.sln"
COPY . .
# Run tests and collect coverage. If this step fails, the build fails.
RUN dotnet test "AEChallenge.sln" --verbosity normal --collect:"XPlat Code Coverage" --results-directory /app/testresults
WORKDIR "/src/AEChallenge.Api"
RUN dotnet build "./AEChallenge.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AEChallenge.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AEChallenge.Api.dll"]