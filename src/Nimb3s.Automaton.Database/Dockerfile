#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Nimb3s.Automaton.Database/Nimb3s.Automaton.Database.csproj", "src/Nimb3s.Automaton.Database/"]
RUN dotnet restore "src/Nimb3s.Automaton.Database/Nimb3s.Automaton.Database.csproj"
COPY . .
WORKDIR "/src/src/Nimb3s.Automaton.Database"
RUN dotnet build "Nimb3s.Automaton.Database.csproj" -c Release -o /app/build
RUN ls -R

FROM mcr.microsoft.com/mssql/server:2019-latest 
# Elevate to root to install required packages
USER root
RUN apt-get -yq update \
    && apt-get install unzip libunwind8 libicu60 libssl1.0 -y

# Install SQLPackage for Linux and make it executable
RUN wget -progress=bar:force -q -O sqlpackage.zip https://go.microsoft.com/fwlink/?linkid=2113331 \
    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage \
    && chown -R mssql /opt/sqlpackage \
    && mkdir /tmp/db \
    && chown -R mssql /tmp/db
 
# Lower the privilege
USER mssql
 
# Add the DACPAC to the image
WORKDIR /tmp/db/
COPY --from=build /app/build/Nimb3s.Automaton.Database.dacpac /tmp/db/Nimb3s.Automaton.Database.dacpac
RUN ls -R

# Configure external build arguments to allow configurability.
ARG DBNAME=Automaton
ARG PASSWORD=1234qwerASDF
 
# Configure the required environmental variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=$PASSWORD
 
# Launch SQL Server, confirm startup is complete, deploy the DACPAC, then terminate SQL Server.
# See https://stackoverflow.com/a/51589787/488695
RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/sqlpackage/sqlpackage /a:Publish /tsn:. /tdn:${DBNAME} /tu:sa /tp:$SA_PASSWORD /sf:/tmp/db/Nimb3s.Automaton.Database.dacpac \
    && rm -r /tmp/db \
    && pkill sqlservr \
    && rm -r /opt/sqlpackage

#docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=1234qwerASDF' -e 'MSSQL_PID=Express' -p 1433:1433 -d nimb3s/identity.database
#docker-compose up -d --build --no-cache --progress plain --pull