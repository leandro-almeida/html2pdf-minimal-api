# ------ BASE
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy-amd64 AS base

USER root
ARG DEBIAN_FRONTEND=noninteractive

# -- Instala pacotes adicionais para usar o wkhtmltox e ajusta o TimeZone
RUN apt update && apt install -y wget gdebi libgdiplus tzdata && ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN mv /etc/localtime /etc/localtime.old && cp /usr/share/zoneinfo/America/Belem /etc/localtime && date

# -- Instala o pacote wkhtmltox
RUN wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6.1-2/wkhtmltox_0.12.6.1-2.jammy_amd64.deb
RUN gdebi --n wkhtmltox_0.12.6.1-2.jammy_amd64.deb
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib64/libwkhtmltox.dll && ln -s /usr/local/lib/libwkhtmltox.so /usr/lib64/libwkhtmltox.so && ln -s /usr/local/lib/libwkhtmltox.so /usr/lib64/libwkhtmltox
RUN rm wkhtmltox_0.12.6.1-2.jammy_amd64.deb
USER 1001

WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://*:8080

# ------ BUILD 
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy-amd64 AS build
ENV DOTNET_CLI_HOME=/tmp/

WORKDIR /src
COPY ["src/TJPA.HtmlToPdfService.csproj", "."]
RUN dotnet restore "./TJPA.HtmlToPdfService.csproj"
COPY src/. .
RUN dotnet build "TJPA.HtmlToPdfService.csproj" -c Release -o /app/build

# ------ PUBLISH
FROM build AS publish
RUN dotnet publish "TJPA.HtmlToPdfService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------ FINAL
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TJPA.HtmlToPdfService.csproj.dll"]