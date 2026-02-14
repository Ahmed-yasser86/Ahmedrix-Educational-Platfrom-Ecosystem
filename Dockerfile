FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["OnlineCoursesPlatform.csproj", "./"]
RUN dotnet restore "OnlineCoursesPlatform.csproj"

COPY . .
RUN rm -rf obj bin

RUN dotnet publish "OnlineCoursesPlatform.csproj" -c Release -o /app/out /p:UseAppHost=false /p:GenerateUIFrameworkAttribute=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5076
EXPOSE 5076

ENTRYPOINT ["dotnet", "OnlineCoursesPlatform.dll"]