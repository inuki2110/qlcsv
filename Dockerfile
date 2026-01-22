# 1. Dùng bộ SDK của Microsoft để Build code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file dự án vào và khôi phục thư viện
COPY ["QLCSV.csproj", "./"]
RUN dotnet restore "QLCSV.csproj"

# Copy toàn bộ code còn lại và Build ra bản Release
COPY . .
WORKDIR "/src/."
RUN dotnet publish "QLCSV.csproj" -c Release -o /app/publish

# 2. Dùng bộ Runtime nhẹ để chạy ứng dụng (giúp tiết kiệm dung lượng)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Cấu hình cổng 8080 (Bắt buộc cho Render)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Lệnh chạy ứng dụng khi khởi động
ENTRYPOINT ["dotnet", "QLCSV.dll"]