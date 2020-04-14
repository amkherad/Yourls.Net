#!/usr/bin/env bash

API_KEY="$1"

if [ "$API_KEY"  == "" ] ; then
	echo "You should pass in the api key."
	exit;
fi

mkdir bin/

export AssemblyVersion=0.0.1

dotnet pack Yourls.Net\Yourls.Net.csproj -c Release -o bin
dotnet pack Yourls.Net.AspNet\Yourls.Net.AspNet.csproj -c Release -o bin
dotnet pack Yourls.Net.JsonNet\Yourls.Net.JsonNet.csproj -c Release -o bin

dotnet nuget push bin/Yourls.Net.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json
dotnet nuget push bin/Yourls.Net.AspNet.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json
dotnet nuget push bin/Yourls.Net.JsonNet.${AssemblyVersion}.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json