#!/bin/sh

PACKAGE_ASSETS_PATH=PackageAssets/StandaloneOSX
SRC_PATH=UnityProject/Alter3/Builds/StandaloneOSX
RELEASE_PATH=./Releases/macOS
DMG_NAME=XFLAG-Alter3Simulator-macOS.dmg
DMG_PATH=${RELEASE_PATH}/${DMG_NAME}

cp -vr ${PACKAGE_ASSETS_PATH}/* ${SRC_PATH}

mkdir -p ${RELEASE_PATH}

if [ -e "${DMG_PATH}" ]; then
  rm "${DMG_PATH}"
fi

hdiutil create "${DMG_PATH}" -volname "XFLAG Alter3Simulator" -format UDBZ -srcfolder "${SRC_PATH}"
