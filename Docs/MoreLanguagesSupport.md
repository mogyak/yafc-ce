# YAFC support for more languages

[English](MoreLanguagesSupport.md) | [한국어](MoreLanguagesSupport.ko.md)

You can ask Yafc to display non-English text from the Welcome screen:
- On the Welcome screen, click the language name (probably "English") next to "In-game objects language:"
- Select your language from the drop-down that appears.
- If your language uses non-European glyphs, it may appear at the bottom of the list.
  - To use these languages, Yafc may need to do a one-time download of a suitable font.
Click "Confirm" if Yafc asks permission to download a font.
  - If you do not wish to have Yafc automatically download a suitable font, click "Select font" in the drop-down, and select a font file that supports your language.

If your language is supported by Factorio but does not appear in the Welcome screen, you can manually force YAFC to use the strings for your language:
- Open `yafc2.config` with a text editor. The usual locations are:
  - Windows: `%localappdata%\YAFC\yafc2.config`
  - Linux: `~/.local/share/YAFC/yafc2.config`
  - macOS: `~/Library/Application Support/yafc2.config`
- Find the `language` section and replace the value with your language code. Here are examples of language codes:
	- Chinese (Simplified): `zh-CN`
	- Chinese (Traditional): `zh-TW`
	- Korean: `ko`
	- Japanese: `ja`
	- Hebrew: `he`
	- Else: Look into the `Factorio/data/base/locale` folder and find the folder for your language.
- If your language uses glyphs that the bundled font cannot draw, use the "Select font" button in the language dropdown on the Welcome screen and select a font file that supports your language.
