using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace Umbraco.Community.IntlTelInput;

public sealed partial class IntlTelInputField : FieldType
{
    [GeneratedRegex(@"^\+[1-9]\d{1,14}$")] private static partial Regex E164();
    [GeneratedRegex(@"^[a-zA-Z]{2}$")] private static partial Regex CountryCode();

    public IntlTelInputField()
    {
        Id = new Guid("1da9341e-2d82-4caf-8fec-29ae76d9ed25");
        Alias = "intlTelInput";
        Name = "International Phone";
        Description = "Phone number input with country code selection";
        Icon = "icon-phone";
        DataType = FieldDataType.String;
        FieldTypeViewName = "FieldType.IntlTelInput.cshtml";
        SortOrder = 16;
        SupportsRegex = false;
    }

    [Setting("Initial Country", Description = "Default ISO 3166-1 alpha-2 code (us, gb, sa, ae)", View = "Umb.PropertyEditorUi.TextBox", DisplayOrder = 10)]
    public string InitialCountry { get; set; } = "us";

    [Setting("Preferred Countries", Description = "Comma-separated ISO codes shown at the top of the dropdown", View = "Umb.PropertyEditorUi.TextBox", DisplayOrder = 20)]
    public string PreferredCountries { get; set; } = string.Empty;

    [Setting("Only Countries", Description = "Comma-separated ISO codes to restrict the dropdown to", View = "Umb.PropertyEditorUi.TextArea", DisplayOrder = 30)]
    public string OnlyCountries { get; set; } = string.Empty;

    [Setting("Exclude Countries", Description = "Comma-separated ISO codes to hide from the dropdown", View = "Umb.PropertyEditorUi.TextArea", DisplayOrder = 40)]
    public string ExcludeCountries { get; set; } = string.Empty;

    [Setting("Separate Dial Code", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 50)]
    public string SeparateDialCode { get; set; } = "true";

    [Setting("Show Flags", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 60)]
    public string ShowFlags { get; set; } = "true";

    [Setting("Allow Dropdown", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 70)]
    public string AllowDropdown { get; set; } = "true";

    [Setting("Auto Placeholder", View = "Umb.PropertyEditorUi.Dropdown", PreValues = "polite,aggressive,off", DisplayOrder = 80)]
    public string AutoPlaceholder { get; set; } = "polite";

    [Setting("National Mode", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 90)]
    public string NationalMode { get; set; } = "true";

    [Setting("Format On Display", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 100)]
    public string FormatOnDisplay { get; set; } = "true";

    [Setting("Validation Error Message", View = "Umb.PropertyEditorUi.TextBox", DisplayOrder = 110)]
    public string ValidationErrorMessage { get; set; } = "Please enter a valid phone number";

    [Setting("Strict Mode", Description = "Enforce exact number length for the selected country", View = "Umb.PropertyEditorUi.Toggle", DisplayOrder = 120)]
    public string StrictMode { get; set; } = "false";

    public override List<Exception> ValidateSettings()
    {
        var errors = new List<Exception>();

        if (!string.IsNullOrWhiteSpace(InitialCountry) && !CountryCode().IsMatch(InitialCountry.Trim()))
        {
            errors.Add(new Exception($"Initial Country '{InitialCountry}' must be a 2-letter ISO code."));
        }

        ValidateCountryList(PreferredCountries, nameof(PreferredCountries), errors);
        ValidateCountryList(OnlyCountries, nameof(OnlyCountries), errors);
        ValidateCountryList(ExcludeCountries, nameof(ExcludeCountries), errors);

        return errors;
    }

    public override IEnumerable<string> ValidateField(
        Form form,
        Field field,
        IEnumerable<object> postedValues,
        HttpContext context,
        IPlaceholderParsingService placeholderParsingService,
        IFieldTypeStorage fieldTypeStorage)
    {
        var errors = new List<string>();
        var value = postedValues?.FirstOrDefault()?.ToString()?.Trim();

        if (!string.IsNullOrEmpty(value) && !E164().IsMatch(value))
        {
            var message = field.Settings?.FirstOrDefault(s => s.Key == nameof(ValidationErrorMessage)).Value;
            errors.Add(string.IsNullOrEmpty(message) ? ValidationErrorMessage : message);
        }

        return base.ValidateField(form, field, postedValues ?? [], context, placeholderParsingService, fieldTypeStorage, errors);
    }

    private static void ValidateCountryList(string value, string settingName, List<Exception> errors)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        foreach (var code in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!CountryCode().IsMatch(code))
            {
                errors.Add(new Exception($"{settingName}: '{code}' must be a 2-letter ISO code."));
            }
        }
    }
}
