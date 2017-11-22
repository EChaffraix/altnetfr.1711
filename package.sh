#!/bin/bash

dotnet_version=$(dotnet --version | cut -c 1-3)
#dest=$(pwd | sed -e 's/csharp.*//')
dest=$(pwd | sed -e 's/src\/.*//')

######################################################
# ARGUMENT PARSING
######################################################
while [[ $# -gt 1 ]]
do
key="$1"

case $key in
    -o|-output)
    OUTPUT="$(echo $2 | awk '{print tolower($0)}')"
    shift 
    ;;
    *)
            # unknown option
    ;;
esac
shift
done

if [ -z ${OUTPUT+x} ]; 	then error=1; echo "output is not defined";  fi

if [ ! -z ${error+x} ]; then 
    echo "Missing mandatory variables";  
    echo "Usage : $0"
    echo "\t-o, -output      OUTPUT : archive base file name"
    
    exit 1; 
fi


echo "Variables : "
echo -e "\tOUTPUT=${OUTPUT}"; 
echo -e "\tdotnet_version=${dotnet_version}"; 
echo -e "\tdest=${dest}"; 

######################################################
# Packing
######################################################
echo "Preparing..."
rm -rf bin
if [ -f package.json ]; then
  npm install

  if [ -f bower.json ]; then
      bower install
  fi

  if [ -f gulpfile.js ]; then
      gulp init
      gulp publish:before
  fi
fi

echo "Building"
#rm Web.config
dotnet publish -c Release

if [ -f gulpfile.js ]; then
    gulp publish:after
fi

echo "Creating artifact"
cd bin/Release/netcoreapp$dotnet_version/publish
tar czf $dest/$OUTPUT.tar.gz .
ls -sh $dest/$OUTPUT.tar.gz
