@echo off

FOR /F %%a IN ('POWERSHELL -COMMAND "$([guid]::NewGuid().ToString())"') DO ( SET APISECRET=%%a )

FOR /F %%a IN ('POWERSHELL -COMMAND "$([guid]::NewGuid().ToString())"') DO ( SET MVCSECRET=%%a )

FOR /F %%a IN ('POWERSHELL -COMMAND "$([guid]::NewGuid().ToString())"') DO ( SET ADMSECRET=%%a )

FOR /F %%a IN ('POWERSHELL -COMMAND "$([guid]::NewGuid().ToString())"') DO ( SET USRSECRET=%%a )

cd ./src/sharpcms.web.id/

dotnet user-secrets clear 

dotnet user-secrets set sharpcms-api-client %APISECRET%

dotnet user-secrets set sharpcms-mvc-client %MVCSECRET%

dotnet user-secrets set admin %ADMSECRET%

dotnet user-secrets set user %USRSECRET%

cd ../../

cd ./src/sharpcms.web.api/

dotnet user-secrets clear

dotnet user-secrets set sharpcms-api-client %APISECRET%

cd ../../

cd ./src/sharpcms.web.ui/

dotnet user-secrets clear

dotnet user-secrets set sharpcms-mvc-client %MVCSECRET%

cd ../../

