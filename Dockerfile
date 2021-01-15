#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
#本地编译
FROM registry.cn-hangzhou.aliyuncs.com/newbe36524/sdk:3.1-alpine AS build
WORKDIR /app
ENV PORT=80

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
#本地编译
FROM registry.cn-hangzhou.aliyuncs.com/newbe36524/aspnet:3.1-alpine AS runtime

WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "mall_api.dll"]
EXPOSE $PORT