@echo off

echo "sharpcms.config.tests =>"

dotnet test ./test/sharpcms.config.tests/ | findstr /C:"  Test Count:"

echo "sharpcms.database.tests =>"

dotnet test ./test/sharpcms.database.tests/ | findstr /C:"  Test Count:"

echo "sharpcms.content.tests =>"

dotnet test ./test/sharpcms.content.tests/ | findstr /C:"  Test Count:"

echo "sharpcms.api.tests =>"

dotnet test ./test/sharpcms.api.tests/ | findstr /C:"  Test Count:"
