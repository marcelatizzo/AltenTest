FROM mcr.microsoft.com/mssql/server:2019-latest

ENV CHECK_POLICY=OFF
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=M4rcel#2022
ENV MSSQL_PID=Express

COPY ./scripts/create-database.sql /tmp/create-database.sql

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ${SA_PASSWORD} -i /tmp/create-database.sql \
    && pkill sqlservr

CMD ["/opt/mssql/bin/sqlservr"]