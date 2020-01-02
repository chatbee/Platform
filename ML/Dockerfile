FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Chatbee.ML/Chatbee.ML.csproj", "Chatbee.ML/"]
RUN dotnet restore "Chatbee.ML/Chatbee.ML.csproj"
COPY . .
WORKDIR "/src/Chatbee.ML"
RUN dotnet build "Chatbee.ML.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chatbee.ML.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chatbee.ML.dll"]