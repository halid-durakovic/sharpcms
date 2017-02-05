@echo off

start dotnet run -p ./src/sharpcms.web.id/project.json ./src/sharpcms.web.id/

start dotnet run -p ./src/sharpcms.web.api/project.json ./src/sharpcms.web.api/

start dotnet run -p ./src/sharpcms.web.ui/project.json ./src/sharpcms.web.ui/