<img src="package/IntlTelInputIcon.png?raw=true" alt="Umbraco International Phone Input" width="250" align="right" />

# Umbraco.Community.IntlTelInput

An Umbraco Forms field type that provides an international phone-number input with country-code dropdown, flags, E.164 validation and as-you-type formatting. Powered by the [intl-tel-input](https://github.com/jackocnr/intl-tel-input) library.

![Screenshot](assets/screenshot-1.png?raw=true)

## Installation

Installation is done through NuGet:

`Install-Package Umbraco.Community.IntlTelInput`

Requires Umbraco CMS 17+, Umbraco Forms 17+ and .NET 10.

After installing, add the assets partial to your master template (inside `<head>` or before `</body>`):

`@await Html.PartialAsync("~/Views/Partials/IntlTelInput/Assets.cshtml")`

That's it — the field auto-registers via a composer and a new **International Phone** option will appear in the Forms field picker.

## Features

- International phone input with country-code dropdown and flag sprites
- Submitted value is always E.164 (e.g. `+14155552671`)
- Server-side and client-side validation
- 12 per-field settings: initial country, preferred countries, only/exclude lists, separate dial code, auto placeholder, national mode, strict mode, custom error message, and more
- Fully themeable via CSS custom properties
- RTL support out of the box

## Field settings

| Setting | Default | Description |
| ---- | ---- | ---- |
| Initial Country | `us` | ISO 3166-1 alpha-2 code |
| Preferred Countries | *(empty)* | Comma-separated codes shown at the top |
| Only Countries | *(empty)* | Restrict the dropdown to these codes |
| Exclude Countries | *(empty)* | Hide these codes from the dropdown |
| Separate Dial Code | `true` | Show dial code next to the selected country |
| Show Flags | `true` | Show country flags |
| Allow Dropdown | `true` | Let users change country |
| Auto Placeholder | `polite` | `polite` / `aggressive` / `off` |
| National Mode | `true` | Accept national-format entry |
| Format On Display | `true` | Format as the user types |
| Strict Mode | `false` | Enforce exact number length |
| Validation Error Message | `Please enter a valid phone number` | Shown when validation fails |

## Theming

Override any of these CSS custom properties in your site stylesheet:

```css
.intl-tel-input-field {
    --itif-border-color: #dfe1e4;
    --itif-border-color-focus: #0066ff;
    --itif-border-color-error: #d92d20;
    --itif-text-color: #111;
    --itif-bg: #fff;
    --itif-bg-hover: #f5f7fa;
    --itif-highlight-bg: #0066ff;
    --itif-highlight-color: #fff;
    --itif-height: 48px;
    --itif-font-size: 14px;
    --itif-border-radius: 6px;
    --itif-country-min-width: 95px;
}
```

## Re-initialising after AJAX

If you render forms dynamically (modal open, SPA page change) call:

`window.UmbracoIntlTelInput.init(document);`

## Building from source

Requires .NET 10 SDK only. The intl-tel-input library files are vendored into `wwwroot/` and committed to the repo.

```
git clone https://github.com/mesharideb/Umbraco.Community.IntlTelInput.git
cd Umbraco.Community.IntlTelInput
dotnet pack src/Umbraco.Community.IntlTelInput -c Release -o ./artifacts
```

To upgrade the bundled library: `./scripts/update-lib.sh`, then bump the package version, commit, and tag.

## Credits

- [intl-tel-input](https://github.com/jackocnr/intl-tel-input) by Jack O'Connor (MIT)
