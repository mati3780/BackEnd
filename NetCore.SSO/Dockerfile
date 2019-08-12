#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 41766
EXPOSE 44328

FROM microsoft/dotnet:2.2-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["NetCore.SSO/NetCore.SSO.csproj", "NetCore.SSO/"]
RUN dotnet restore "NetCore.SSO/NetCore.SSO.csproj"
COPY . .
WORKDIR "/src/NetCore.SSO"
RUN dotnet build "NetCore.SSO.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetCore.SSO.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCore.SSO.dll"]
