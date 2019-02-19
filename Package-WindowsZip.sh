#!/bin/sh

PACKAGE_ASSETS_COMMON_PATH=PackageAssets/Common

WORK_DIR=$(mktemp -d)

BUILDS_ROOT=./UnityProject/Alter3/Builds/StandaloneWindows64

ARCHIVE_ROOT="${WORK_DIR}/XFLAG-Alter3Simulator-Win64"
RELEASE_PATH=./Releases/win64

mkdir -p "${ARCHIVE_ROOT}"

cp -vr "${BUILDS_ROOT}"/* "${ARCHIVE_ROOT}"
cp -vr "${PACKAGE_ASSETS_COMMON_PATH}"/* "${ARCHIVE_ROOT}"

cd "${WORK_DIR}" && zip -r XFLAG-Alter3Simulator-Win64.zip XFLAG-Alter3Simulator-Win64 && cd -

mkdir -p "${RELEASE_PATH}"
mv "${WORK_DIR}"/XFLAG-Alter3Simulator-Win64.zip "${RELEASE_PATH}"
