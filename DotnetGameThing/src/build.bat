# Platforms
set win="win-x64"
set mac="osx-x64"
set lin="linux-x64"


echo "Building.. (win-x64, mac-x64, linux-x64)"
dotnet publish -r %win% --self-contained true --configuration Release
dotnet publish -r %mac% --self-contained true --configuration Release
dotnet publish -r %lin% --self-contained true --configuration Release
echo "Finished Building!"
