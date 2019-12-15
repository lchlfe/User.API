FROM microsoft/dotnet:3.1-sdk as build-dev
WORKDIR	/code
COPY *.csproj /code
RUN dotnet restore
COPY . /code
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:3.1-runtime
WORKDIR /app
COPY --from=build-dev /code/out /app

ENV ASPNETCORE_URLS http://+5001
EXPOSE 5001
ENTRYPOINT ["dotnet","webhello.API.dll"]