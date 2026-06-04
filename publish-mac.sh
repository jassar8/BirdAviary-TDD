#!/usr/bin/env bash
# Builds a macOS .app bundle (BirdAviaryManagement.app) for Apple Silicon or Intel Macs.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")" && pwd)"
PROJECT="$ROOT/BirdAviaryManagement.App/BirdAviaryManagement.App.csproj"
STAGING="$ROOT/.build-mac-publish"
OUTPUT_ROOT="$ROOT/publish-mac"
APP_NAME="BirdAviaryManagement.app"
APP_PATH="$OUTPUT_ROOT/$APP_NAME"
EXEC_NAME="BirdAviaryManagement"

ARCH="$(uname -m)"
if [ "$ARCH" = "arm64" ]; then
  RID="osx-arm64"
else
  RID="osx-x64"
fi

echo "Publishing Bird Aviary Management for macOS ($RID)..."

rm -rf "$STAGING" "$OUTPUT_ROOT"
mkdir -p "$STAGING"

dotnet publish "$PROJECT" \
  -c Release \
  -r "$RID" \
  --self-contained true \
  -p:UseAppHost=true \
  -p:PublishSingleFile=true \
  -o "$STAGING"

MACOS_DIR="$APP_PATH/Contents/MacOS"
mkdir -p "$MACOS_DIR"
cp -R "$STAGING"/* "$MACOS_DIR/"

if [ ! -f "$MACOS_DIR/$EXEC_NAME" ]; then
  echo "Publish failed: $MACOS_DIR/$EXEC_NAME was not created." >&2
  exit 1
fi

chmod +x "$MACOS_DIR/$EXEC_NAME"

cat > "$APP_PATH/Contents/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleExecutable</key>
    <string>$EXEC_NAME</string>
    <key>CFBundleIdentifier</key>
    <string>com.birdaviary.management</string>
    <key>CFBundleName</key>
    <string>BirdAviaryManagement</string>
    <key>CFBundleDisplayName</key>
    <string>Bird Aviary Management</string>
    <key>CFBundleVersion</key>
    <string>1.0.0</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0.0</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>NSHighResolutionCapable</key>
    <true/>
    <key>LSMinimumSystemVersion</key>
    <string>11.0</string>
</dict>
</plist>
EOF

if command -v codesign >/dev/null 2>&1; then
  codesign --force --sign - --deep "$APP_PATH"
fi

xattr -cr "$APP_PATH" 2>/dev/null || true

rm -rf "$STAGING"

echo ""
echo "Done!"
echo "Open the app:"
echo "  open \"$APP_PATH\""
echo ""
echo "If macOS still blocks it (Safari download / \"damaged\" message), run:"
echo "  xattr -dr com.apple.quarantine \"$APP_PATH\""
