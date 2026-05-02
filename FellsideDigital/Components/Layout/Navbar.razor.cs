using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace FellsideDigital.Components.Layout;

public enum NavbarLayoutMode
{
    Centered,
    Wide
}

public partial class Navbar : ComponentBase, IDisposable
{
    private bool _mobileMenuOpen;
    private bool _hasNotifications = true;

    private string? currentUrl;

    [Parameter] public NavbarLayoutMode LayoutMode { get; set; } = NavbarLayoutMode.Centered;

    private string ContainerClass => LayoutMode == NavbarLayoutMode.Wide
        ? "w-full px-2 sm:px-6 lg:px-8"
        : "mx-auto max-w-7xl px-2 sm:px-6 lg:px-8";

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private static readonly (string Label, string Href)[] _navLinks =
    {
        ("Home", "/"),
        ("Contact", "/contact")
    };

    protected override void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    private void ToggleMobileMenu() => _mobileMenuOpen = !_mobileMenuOpen;
    private void CloseMobileMenu() => _mobileMenuOpen = false;

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
