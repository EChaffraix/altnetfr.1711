#!/bin/bash

rm -rf src/ test/ *.sln*

mkdir src
cd src

mkdir common
cd common
dotnet new classlib
cd ..

mkdir website
cd website
dotnet new mvc --auth None
dotnet add website.csproj reference ../common/common.csproj
cd ..

mkdir service
cd service
dotnet new mvc --auth None
dotnet add service.csproj reference ../common/common.csproj
rm -rf wwwroot Views
cd ../..

##########################################################################
# Tests
##########################################################################

mkdir test
cd test

mkdir website.tests
cd website.tests
dotnet new classlib -f netcoreapp2.0
dotnet add website.tests.csproj package NUnit
dotnet add website.tests.csproj package NSubstitute
dotnet add website.tests.csproj package Microsoft.NET.Test.Sdk
dotnet add website.tests.csproj package NUnit3TestAdapter
dotnet add website.tests.csproj reference ../../src/website/website.csproj
cd ..

mkdir service.tests
cd service.tests
dotnet new classlib -f netcoreapp2.0
dotnet add service.tests.csproj package NUnit
dotnet add service.tests.csproj package NSubstitute
dotnet add service.tests.csproj package Microsoft.NET.Test.Sdk
dotnet add service.tests.csproj package NUnit3TestAdapter
dotnet add service.tests.csproj reference ../../src/service/service.csproj
cd ../..


##########################################################################
# Solution
##########################################################################
dotnet new sln
dotnet sln altnetfr.sln add $(find . -name *.csproj)
