using System.Security.Claims;
using FellsideDigital.Web.Data;
using FellsideDigital.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FellsideDigital.Web.Components.Pages.Portal;

public partial class Index : ComponentBase
{
    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;

    private string? _userId;
    private string _displayName = "Client";
    private List<ClientProject> _projects = [];
    private List<Invoice> _invoices = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState.GetAuthenticationStateAsync();
        var user = authState.User;

        _userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(_userId)) return;

        var firstName = user.FindFirstValue(ClaimTypes.GivenName);
        var email = user.FindFirstValue(ClaimTypes.Email) ?? user.Identity?.Name;
        _displayName = !string.IsNullOrWhiteSpace(firstName) ? firstName : email ?? "Client";

        _projects = await ProjectService.GetForClientAsync(_userId);
        _invoices = await InvoiceService.GetForClientAsync(_userId);
    }
}
