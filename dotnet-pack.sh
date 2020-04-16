#!/usr/bin/env bash


AssemblyVersion="$1"
API_KEY="$2"

if [ "$API_KEY"  == "" ] ; then
	echo "You should pass in the api key."
	exit;
fi

mkdir ./bin/

export AssemblyVersion

dotnet pack ./Yourls.Net/Yourls.Net.csproj -c Release -o bin
dotnet pack ./Yourls.Net.AspNet/Yourls.Net.AspNet.csproj -c Release -o bin
dotnet pack ./Yourls.Net.JsonNet/Yourls.Net.JsonNet.csproj -c Release -o bin

dotnet nuget push ./bin/Yourls.Net.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json
dotnet nuget push ./bin/Yourls.Net.AspNet.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json
dotnet nuget push ./bin/Yourls.Net.JsonNet.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json
