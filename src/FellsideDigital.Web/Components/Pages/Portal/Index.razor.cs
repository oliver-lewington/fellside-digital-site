using FellsideDigital.Web.Data;
using FellsideDigital.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FellsideDigital.Web.Components.Pages.Portal;

public partial class Index : ComponentBase
{
    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;

    private bool _loading = true;
    private string _firstName = "there";
    private string? _userId;

    private List<ClientProject> _projects = [];
    private List<Invoice>       _invoices = [];
    private List<(ProjectStatusUpdate Update, ClientProject Project)> _recentActivity = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState.GetAuthenticationStateAsync();
        _userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(_userId)) { _loading = false; return; }

        var user = await UserManager.GetUserAsync(authState.User);
        if (user is not null)
        {
            _firstName = !string.IsNullOrWhiteSpace(user.FirstName) ? user.FirstName
                : authState.User.FindFirstValue(ClaimTypes.Email) ?? "there";
        }

        _projects = await ProjectService.GetForClientAsync(_userId);
        _invoices = await InvoiceService.GetForClientAsync(_userId);

        _recentActivity = _projects
            .SelectMany(p => p.StatusUpdates.Select(u => (Update: u, Project: p)))
            .OrderByDescending(x => x.Update.CreatedAt)
            .ToList();

        _loading = false;
    }
}
