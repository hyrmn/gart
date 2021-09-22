#!/usr/bin/env bash

dotnet restore && dotnet build -c Release && dotnet test && dotnet publish .\src\gart.csproj -o ./dist --no-self-contained