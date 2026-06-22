#!/bin/sh
set -eu

ROOT_DIR=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
APP_DIR="$ROOT_DIR/Build/YAFC-CE.app"
CONTENTS_DIR="$APP_DIR/Contents"
MACOS_DIR="$APP_DIR/Contents/MacOS"
INFO_PLIST="$APP_DIR/Contents/Info.plist"
PKG_INFO="$APP_DIR/Contents/PkgInfo"
VERSION=$(sed -n 's:.*<AssemblyVersion>\([^<]*\)</AssemblyVersion>.*:\1:p' "$ROOT_DIR/Yafc/Yafc.csproj" | head -n 1)

echo "Building YAFC-CE.app version ${VERSION:-local}..."

rm -rf "$APP_DIR"
mkdir -p "$MACOS_DIR" "$CONTENTS_DIR/Resources"

dotnet publish "$ROOT_DIR/Yafc/Yafc.csproj" -r osx-arm64 -c Release --self-contained true -o "$MACOS_DIR" "$@"

find "$MACOS_DIR" -name "Yafc.I18n.Generator*" -delete
find "$MACOS_DIR" -name "yafc*.log" -delete
find "$MACOS_DIR" -type d -exec chmod 755 {} +
find "$MACOS_DIR" -type f -exec chmod 644 {} +
chmod 755 "$MACOS_DIR/Yafc"
find "$MACOS_DIR" -name "*.dylib" -exec chmod 755 {} +

cat > "$INFO_PLIST" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
	<key>CFBundleDevelopmentRegion</key>
	<string>en</string>
	<key>CFBundleDisplayName</key>
	<string>YAFC-CE</string>
	<key>CFBundleExecutable</key>
	<string>Yafc</string>
	<key>CFBundleIdentifier</key>
	<string>com.github.yafc-ce.yafc-ce</string>
	<key>CFBundleName</key>
	<string>YAFC-CE</string>
	<key>CFBundlePackageType</key>
	<string>APPL</string>
	<key>CFBundleShortVersionString</key>
	<string>${VERSION:-local}</string>
	<key>CFBundleVersion</key>
	<string>${VERSION:-local}</string>
	<key>LSMinimumSystemVersion</key>
	<string>13.0</string>
	<key>NSHighResolutionCapable</key>
	<true/>
</dict>
</plist>
EOF
printf "APPL????" > "$PKG_INFO"

xattr -cr "$APP_DIR" 2>/dev/null || true
codesign --force --deep --sign - "$APP_DIR"

echo "Built $APP_DIR"
echo "Open it with: open -n \"$APP_DIR\""
