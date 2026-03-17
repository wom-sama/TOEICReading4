FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/TOEICReading4.Web.Mvc/TOEICReading4.Web.Mvc.csproj", "src/TOEICReading4.Web.Mvc/"]
COPY ["src/TOEICReading4.Web.Core/TOEICReading4.Web.Core.csproj", "src/TOEICReading4.Web.Core/"]
COPY ["src/TOEICReading4.Application/TOEICReading4.Application.csproj", "src/TOEICReading4.Application/"]
COPY ["src/TOEICReading4.Core/TOEICReading4.Core.csproj", "src/TOEICReading4.Core/"]
COPY ["src/TOEICReading4.EntityFrameworkCore/TOEICReading4.EntityFrameworkCore.csproj", "src/TOEICReading4.EntityFrameworkCore/"]
WORKDIR "/src/src/TOEICReading4.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/TOEICReading4.Web.Mvc", "src/TOEICReading4.Web.Mvc"]
COPY ["src/TOEICReading4.Web.Core", "src/TOEICReading4.Web.Core"]
COPY ["src/TOEICReading4.Application", "src/TOEICReading4.Application"]
COPY ["src/TOEICReading4.Core", "src/TOEICReading4.Core"]
COPY ["src/TOEICReading4.EntityFrameworkCore", "src/TOEICReading4.EntityFrameworkCore"]
WORKDIR "/src/src/TOEICReading4.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "TOEICReading4.Web.Mvc.dll"]
