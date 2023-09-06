# Use .NET 6.0 SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0

# Set working directory
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else
COPY . ./

# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef

# Add dotnet tools path to PATH env variable 
ENV PATH="$PATH:/root/.dotnet/tools"

# Display PATH for debugging
RUN echo $PATH

# Run EF migrations 
RUN dotnet ef migrations add InitialMigration

# Publish app
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=builder /app/out .

ENTRYPOINT ["dotnet", "ERP.dll"]