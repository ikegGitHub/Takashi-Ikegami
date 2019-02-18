#!/bin/sh

if [ -z "$UNITY_PATH" ]; then
  echo "ERROR: Please specify UNITY_PATH"
  exit 1
fi

UNITY_EXEC=${UNITY_PATH}/Contents/MacOS/Unity

if ! which "$UNITY_EXEC"; then
  echo "ERROR: $UNITY_EXEC not found"
  exit 1
fi

${UNITY_EXEC} -batchmode -quit -projectPath 'UnityProject/Alter3' -executeMethod 'XFlag.Alter3SimulatorEditor.BatchBuild.BuildAll' -logFile

./Package.sh
./Package-WindowsZip.sh
