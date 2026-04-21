#!/usr/bin/env bash
set -euo pipefail

PKG="$(cd "$(dirname "$0")/.." && pwd)/src/Umbraco.Community.IntlTelInput"
cd "$PKG"

npm update intl-tel-input

LIB="node_modules/intl-tel-input/build"
cp "$LIB/js/intlTelInputWithUtils.min.js" wwwroot/js/
cp "$LIB/css/intlTelInput.css"            wwwroot/css/
cp "$LIB/img/flags.webp"                  wwwroot/img/
cp "$LIB/img/flags@2x.webp"               wwwroot/img/

VERSION=$(node -p "require('./node_modules/intl-tel-input/package.json').version")
echo "intl-tel-input bumped to $VERSION"
echo "Review the diff, bump <Version> in the .csproj, commit, tag, push."
