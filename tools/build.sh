#!/usr/bin/env bash
# Build Hotel Aurora from the command line (rules.md §5).
#
# Usage:
#   UNITY=/path/to/Unity ./tools/build.sh <profile> [-buildVersion X.Y.Z] [-outputPath DIR]
#
# <profile> is one of: web | web-dev | windows | windows-dev | steam
#
# Requires $UNITY to point at the Unity 6000.6.x Editor executable for this project.
# Fails if the Editor already has this project open (batchmode lock) — close it first.

set -euo pipefail

if [ -z "${UNITY:-}" ]; then
  echo "Set \$UNITY to the Unity 6000.6.x Editor executable path first." >&2
  exit 1
fi

PROFILE="${1:-}"
shift || true

case "$PROFILE" in
  web)          METHOD="Aurora.Editor.Build.BuildRunner.BuildWeb" ;;
  web-dev)      METHOD="Aurora.Editor.Build.BuildRunner.BuildWebDev" ;;
  windows)      METHOD="Aurora.Editor.Build.BuildRunner.BuildWindows" ;;
  windows-dev)  METHOD="Aurora.Editor.Build.BuildRunner.BuildWindowsDev" ;;
  steam)        METHOD="Aurora.Editor.Build.BuildRunner.BuildSteam" ;;
  *)
    echo "Usage: $0 <web|web-dev|windows|windows-dev|steam> [-buildVersion X.Y.Z] [-outputPath DIR]" >&2
    exit 1
    ;;
esac

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
mkdir -p "$PROJECT_ROOT/Logs"

echo "Building '$PROFILE' via $METHOD ..."
STATUS=0
"$UNITY" -batchmode -nographics -quit \
  -projectPath "$PROJECT_ROOT" \
  -executeMethod "$METHOD" \
  -logFile "$PROJECT_ROOT/Logs/build-$PROFILE.log" \
  "$@" || STATUS=$?

if [ $STATUS -ne 0 ]; then
  echo "Build failed (exit $STATUS). See Logs/build-$PROFILE.log" >&2
  exit $STATUS
fi

echo "Build '$PROFILE' finished. See Logs/build-$PROFILE.log and Builds/."
