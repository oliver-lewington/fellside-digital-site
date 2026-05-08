using FellsideDigital.UI.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace FellsideDigital.Web.Components.Layout;

public partial class MainNavbar : ComponentBase, IAsyncDisposable
{
    private bool _mobileMenuOpen;
    private bool _hasNotifications = true;
    private string? currentUrl;
    private DotNetObjectReference<MainNavbar>? _dotNetRef;

    [Parameter] public NavbarLayoutMode LayoutMode { get; set; } = NavbarLayoutMode.Centered;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private string ContainerClass => LayoutMode == NavbarLayoutMode.Wide
        ? "w-full px-4 sm:px-6 lg:px-8"
        : "mx-auto max-w-7xl px-4 sm:px-6 lg:px-8";

    private static readonly (string Label, string Href)[] _navLinks =
    {
        ("Home", "/"),
        ("Contact", "/contact")
    };

    private static readonly (string Label, string Href)[] _servicesLinks =
    {
        ("Websites", "/websites"),
        ("Automation", "/automation"),
    };

    private static readonly (string Label, string Href)[] _mobileNavLinks =
    {
        ("Home", "/"),
        ("Websites", "/websites"),
        ("Automation", "/automation"),
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

        // Close sheet on any navigation (e.g. back/forward, programmatic navigate)
        if (_mobileMenuOpen)
        {
            _mobileMenuOpen = false;
            _ = JS.InvokeVoidAsync("fellsideNav.unlockScroll").AsTask();
        }

        StateHasChanged();
    }

    private async Task ToggleMobileMenuAsync()
    {
        _mobileMenuOpen = !_mobileMenuOpen;

        if (_mobileMenuOpen)
        {
            await JS.InvokeVoidAsync("fellsideNav.lockScroll");
            _dotNetRef ??= DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("fellsideNav.watchEscape", _dotNetRef);
        }
        else
        {
            await JS.InvokeVoidAsync("fellsideNav.unlockScroll");
        }
    }

    /// <summary>Called by JS when the user presses Escape.</summary>
    [JSInvokable]
    public async Task CloseMobileMenuFromJs()
    {
        _mobileMenuOpen = false;
        await JS.InvokeVoidAsync("fellsideNav.unlockScroll");
        await InvokeAsync(StateHasChanged);
    }

    private async Task CloseMobileMenuAsync()
    {
        if (!_mobileMenuOpen) return;
        _mobileMenuOpen = false;
        await JS.InvokeVoidAsync("fellsideNav.unlockScroll");
    }

    public async ValueTask DisposeAsync()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;

        if (_mobileMenuOpen)
        {
            try { await JS.InvokeVoidAsync("fellsideNav.unlockScroll"); }
            catch (JSDisconnectedException) { /* circuit already gone */ }
        }

        _dotNetRef?.Dispose();
    }
}
