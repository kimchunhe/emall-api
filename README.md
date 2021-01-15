
## Build and run the sample with Docker for Linux containers
```console
cd mall_api
docker build -t mall_api .
docker run -it --rm -p 8000:80 mall-api
```

## Build and run the sample locally
```console
cd mall_api
dotnet build
dotnet run
```