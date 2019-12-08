FROM microsoft/aspnetcore-build as build-dev
WORKDIR	/code
COPY *.csproj /code
RUN dotnet restore
COPY . /code
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=build-dev /code/out /app

ENV ASPNETCORE_URLS http://+80
EXPOSE 80
ENTRYPOINT ["dotnet","User.API.dll"]

#FROM microsoft/aspnetcore-build as build-dev
#WORKDIR /code
#COPY *.csproj /code
#RUN dotnet restore
#COPY . /code
#RUN dotnet publish -c Release -o -out
#
#FROM microsoft/aspnetcore
#WORKDIR /app
#COPY --from=build-env /code/out ./
#
#EXPOSE 80
#RUN dotnet User.API.dll