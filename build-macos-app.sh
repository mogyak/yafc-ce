#!/bin/sh
set -eu

ROOT_DIR=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
BUILD_DIR="$ROOT_DIR/Build"
FINAL_APP_DIR="$BUILD_DIR/YAFC-CE.app"
VERSION=$(sed -n 's:.*<AssemblyVersion>\([^<]*\)</AssemblyVersion>.*:\1:p' "$ROOT_DIR/Yafc/Yafc.csproj" | head -n 1)

case $(uname -m) in
    arm64) RID=osx-arm64 ;;
    x86_64) RID=osx-x64 ;;
    *) echo "Unsupported macOS architecture: $(uname -m)" >&2; exit 1 ;;
esac

mkdir -p "$BUILD_DIR"
STAGING_ROOT=$(mktemp -d "$BUILD_DIR/.YAFC-CE.app.XXXXXX")
APP_DIR="$STAGING_ROOT/YAFC-CE.app"
CONTENTS_DIR="$APP_DIR/Contents"
MACOS_DIR="$APP_DIR/Contents/MacOS"
INFO_PLIST="$APP_DIR/Contents/Info.plist"
PKG_INFO="$APP_DIR/Contents/PkgInfo"
ICON_FILE="$APP_DIR/Contents/Resources/YAFC-CE.icns"
ICONSET_DIR="$APP_DIR/Contents/Resources/YAFC-CE.iconset"
BASE_ICON="$APP_DIR/Contents/Resources/YAFC-CE.png"

cleanup() {
    rm -rf "$STAGING_ROOT"
}
trap cleanup 0 HUP INT TERM

echo "Building YAFC-CE.app version ${VERSION:-local}..."

mkdir -p "$MACOS_DIR" "$CONTENTS_DIR/Resources"

dotnet publish "$ROOT_DIR/Yafc/Yafc.csproj" -r "$RID" -c Release --self-contained true -o "$MACOS_DIR" "$@"

find "$MACOS_DIR" -name "Yafc.I18n.Generator*" -delete
find "$MACOS_DIR" -name "yafc*.log" -delete

sips -s format png "$ROOT_DIR/Yafc/image.ico" --out "$BASE_ICON" >/dev/null
rm -rf "$ICONSET_DIR"
mkdir -p "$ICONSET_DIR"
sips -z 16 16 "$BASE_ICON" --out "$ICONSET_DIR/icon_16x16.png" >/dev/null
sips -z 32 32 "$BASE_ICON" --out "$ICONSET_DIR/icon_16x16@2x.png" >/dev/null
sips -z 32 32 "$BASE_ICON" --out "$ICONSET_DIR/icon_32x32.png" >/dev/null
sips -z 64 64 "$BASE_ICON" --out "$ICONSET_DIR/icon_32x32@2x.png" >/dev/null
sips -z 128 128 "$BASE_ICON" --out "$ICONSET_DIR/icon_128x128.png" >/dev/null
sips -z 256 256 "$BASE_ICON" --out "$ICONSET_DIR/icon_128x128@2x.png" >/dev/null
sips -z 256 256 "$BASE_ICON" --out "$ICONSET_DIR/icon_256x256.png" >/dev/null
sips -z 512 512 "$BASE_ICON" --out "$ICONSET_DIR/icon_256x256@2x.png" >/dev/null
sips -z 512 512 "$BASE_ICON" --out "$ICONSET_DIR/icon_512x512.png" >/dev/null
sips -z 1024 1024 "$BASE_ICON" --out "$ICONSET_DIR/icon_512x512@2x.png" >/dev/null
iconutil -c icns "$ICONSET_DIR" -o "$ICON_FILE"
rm -rf "$ICONSET_DIR" "$BASE_ICON"

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
	<key>CFBundleIconFile</key>
	<string>YAFC-CE.icns</string>
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

find "$APP_DIR" -type d -exec chmod 755 {} +
find "$APP_DIR" -type f -exec chmod 644 {} +
chmod 755 "$MACOS_DIR/Yafc"
find "$MACOS_DIR" -name "*.dylib" -exec chmod 755 {} +

xattr -cr "$APP_DIR" 2>/dev/null || true
codesign --force --deep --sign - "$APP_DIR"

rm -rf "$FINAL_APP_DIR"
mv "$APP_DIR" "$FINAL_APP_DIR"
trap - 0 HUP INT TERM
rm -rf "$STAGING_ROOT"

echo "Built $FINAL_APP_DIR"
echo "Open it with: open -n \"$FINAL_APP_DIR\""
