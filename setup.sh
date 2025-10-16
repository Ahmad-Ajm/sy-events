#!/bin/bash

# Event Management Platform - Setup Script for Linux/macOS

echo "====================================="
echo "Event Management Platform - Setup"
echo "====================================="
echo ""

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Check if command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Check .NET SDK
echo -e "${YELLOW}Checking .NET SDK...${NC}"
if command_exists dotnet; then
    DOTNET_VERSION=$(dotnet --version)
    echo -e "${GREEN}✓ .NET SDK found: $DOTNET_VERSION${NC}"
else
    echo -e "${RED}✗ .NET SDK not found. Please install from: https://dotnet.microsoft.com/download${NC}"
    exit 1
fi

# Check Node.js
echo -e "${YELLOW}Checking Node.js...${NC}"
if command_exists node; then
    NODE_VERSION=$(node --version)
    echo -e "${GREEN}✓ Node.js found: $NODE_VERSION${NC}"
else
    echo -e "${RED}✗ Node.js not found. Please install from: https://nodejs.org${NC}"
    exit 1
fi

# Check Docker
echo -e "${YELLOW}Checking Docker...${NC}"
if command_exists docker; then
    DOCKER_VERSION=$(docker --version)
    echo -e "${GREEN}✓ Docker found: $DOCKER_VERSION${NC}"
else
    echo -e "${YELLOW}⚠ Docker not found. Docker is optional but recommended.${NC}"
fi

# Install ABP CLI
echo ""
echo -e "${YELLOW}Installing ABP CLI...${NC}"
if dotnet tool install -g Volo.Abp.Cli 2>/dev/null; then
    echo -e "${GREEN}✓ ABP CLI installed successfully${NC}"
else
    echo -e "${YELLOW}ABP CLI already installed, trying to update...${NC}"
    dotnet tool update -g Volo.Abp.Cli
    echo -e "${GREEN}✓ ABP CLI updated successfully${NC}"
fi

# Verify ABP CLI
echo ""
echo -e "${YELLOW}Verifying ABP CLI...${NC}"
ABP_VERSION=$(abp --version)
echo -e "${GREEN}✓ ABP CLI version: $ABP_VERSION${NC}"

# Install Angular CLI
echo ""
echo -e "${YELLOW}Checking Angular CLI...${NC}"
if command_exists ng; then
    echo -e "${GREEN}✓ Angular CLI found${NC}"
else
    echo -e "${YELLOW}Installing Angular CLI...${NC}"
    npm install -g @angular/cli
    echo -e "${GREEN}✓ Angular CLI installed${NC}"
fi

# Create ABP Solution
echo ""
echo -e "${CYAN}=====================================${NC}"
echo -e "${CYAN}Creating ABP Solution...${NC}"
echo -e "${CYAN}=====================================${NC}"
echo ""
echo -e "${YELLOW}This may take several minutes...${NC}"
echo ""

SOLUTION_NAME="EventManagement"

# Check if solution already exists
if [ -f "./$SOLUTION_NAME.sln" ]; then
    echo -e "${YELLOW}⚠ Solution already exists!${NC}"
    read -p "Do you want to recreate it? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo -e "${YELLOW}Skipping solution creation.${NC}"
        exit 0
    else
        echo -e "${YELLOW}Removing existing solution...${NC}"
        rm -rf ./aspnet-core ./angular ./$SOLUTION_NAME.sln
    fi
fi

# Create ABP Application
abp new $SOLUTION_NAME \
    -t app \
    -u angular \
    -d ef \
    -dbms PostgreSQL \
    --mobile none \
    --pwa \
    --no-random-port

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✓ ABP Solution created successfully!${NC}"
else
    echo -e "${RED}✗ Failed to create ABP Solution${NC}"
    exit 1
fi

# Copy environment file
echo ""
echo -e "${YELLOW}Copying environment configuration...${NC}"
if [ -f ".env-template" ]; then
    cp .env-template .env 2>/dev/null
    echo -e "${GREEN}✓ .env file created${NC}"
fi

# Start Docker services
echo ""
echo -e "${CYAN}=====================================${NC}"
echo -e "${CYAN}Starting Docker Services...${NC}"
echo -e "${CYAN}=====================================${NC}"

if command_exists docker-compose; then
    read -p "Do you want to start PostgreSQL, Redis, and pgAdmin with Docker? (Y/n): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Nn]$ ]]; then
        echo -e "${YELLOW}Starting Docker containers...${NC}"
        docker-compose up -d postgres pgadmin redis
        echo -e "${GREEN}✓ Docker services started${NC}"
        echo ""
        echo -e "${CYAN}Services running at:${NC}"
        echo "  - PostgreSQL: localhost:5432"
        echo "  - pgAdmin:    http://localhost:5050"
        echo "  - Redis:      localhost:6379"
    fi
fi

# Final instructions
echo ""
echo -e "${GREEN}=====================================${NC}"
echo -e "${GREEN}Setup Complete!${NC}"
echo -e "${GREEN}=====================================${NC}"
echo ""
echo -e "${CYAN}Next steps:${NC}"
echo ""
echo -e "${YELLOW}1. Run Database Migrations:${NC}"
echo "   cd aspnet-core/src/EventManagement.DbMigrator"
echo "   dotnet run"
echo ""
echo -e "${YELLOW}2. Start Backend API:${NC}"
echo "   cd aspnet-core/src/EventManagement.HttpApi.Host"
echo "   dotnet run"
echo -e "   ${CYAN}Backend will be available at: https://localhost:44300${NC}"
echo ""
echo -e "${YELLOW}3. Start Frontend:${NC}"
echo "   cd angular"
echo "   npm install"
echo "   npm start"
echo -e "   ${CYAN}Frontend will be available at: http://localhost:4200${NC}"
echo ""
echo -e "${YELLOW}4. Default Login:${NC}"
echo "   Username: admin"
echo "   Password: 1q2w3E*"
echo ""
echo "For more information, see README.md and docs/getting-started.md"
echo ""

