#!/usr/bin/env bash

# https://www.willianantunes.com/blog/2021/05/production-ready-shell-startup-scripts-the-set-builtin/
set -eu -o pipefail

REPORTS_FOLDER_PATH=test-reports

dotnet test \
    --logger trx \
    --logger "console;verbosity=detailed" \
    --settings "runsettings.xml" \
    --results-directory $REPORTS_FOLDER_PATH