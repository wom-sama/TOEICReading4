FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/TOEICReading4.Web.Host/TOEICReading4.Web.Host.csproj", "src/TOEICReading4.Web.Host/"]
COPY ["src/TOEICReading4.Web.Core/TOEICReading4.Web.Core.csproj", "src/TOEICReading4.Web.Core/"]
COPY ["src/TOEICReading4.Application/TOEICReading4.Application.csproj", "src/TOEICReading4.Application/"]
COPY ["src/TOEICReading4.Core/TOEICReading4.Core.csproj", "src/TOEICReading4.Core/"]
COPY ["src/TOEICReading4.EntityFrameworkCore/TOEICReading4.EntityFrameworkCore.csproj", "src/TOEICReading4.EntityFrameworkCore/"]
WORKDIR "/src/src/TOEICReading4.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/TOEICReading4.Web.Host", "src/TOEICReading4.Web.Host"]
COPY ["src/TOEICReading4.Web.Core", "src/TOEICReading4.Web.Core"]
COPY ["src/TOEICReading4.Application", "src/TOEICReading4.Application"]
COPY ["src/TOEICReading4.Core", "src/TOEICReading4.Core"]
COPY ["src/TOEICReading4.EntityFrameworkCore", "src/TOEICReading4.EntityFrameworkCore"]
WORKDIR "/src/src/TOEICReading4.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "TOEICReading4.Web.Host.dll"]
