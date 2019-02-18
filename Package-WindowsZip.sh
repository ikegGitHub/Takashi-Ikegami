#!/bin/sh

WORK_DIR=$(mktemp -d)

BUILDS_ROOT=./UnityProject/Alter3/Builds/StandaloneWindows64

ARCHIVE_ROOT="${WORK_DIR}/XFLAG-Alter3Simulator-Win64"

mkdir -p "${ARCHIVE_ROOT}"

cp -r "${BUILDS_ROOT}"/* "${ARCHIVE_ROOT}"

cd "${WORK_DIR}" && zip -r XFLAG-Alter3Simulator-Win64.zip XFLAG-Alter3Simulator-Win64 && cd -
mv "${WORK_DIR}"/XFLAG-Alter3Simulator-Win64.zip .
