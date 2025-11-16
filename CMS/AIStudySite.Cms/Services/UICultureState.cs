using System.Globalization;

namespace AIStudySite.Cms.Services;

public class UICultureState
{
    private CultureInfo _culture = CultureInfo.CurrentUICulture;

    public event EventHandler<string>? Changed;

    public string Culture => _culture.Name;

    public void SetCulture(string culture)
    {
        _culture = new CultureInfo(culture);
        CultureInfo.CurrentCulture = _culture;
        CultureInfo.CurrentUICulture = _culture;
        Changed?.Invoke(this, _culture.Name);
    }
}
