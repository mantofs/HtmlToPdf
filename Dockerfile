FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
RUN apt update && apt install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so
ARG port=8080
ENV ASPNETCORE_URLS http://0.0.0.0:${port}
EXPOSE ${port}

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base as final
WORKDIR /app
COPY --from=build /app .
CMD ["/bin/bash", "-c", "dotnet HtmlToPdf.UI.dll"]
