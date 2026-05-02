using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FellsideDigital.Components.Pages.Portal;

public partial class ProjectDetail : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private ClientProject? _project;
    private bool _notFound;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState.GetAuthenticationStateAsync();
        var user = await UserManager.GetUserAsync(authState.User);
        if (user is null) { _notFound = true; return; }

        _project = await ProjectService.GetByIdAsync(Id);
        if (_project is null || _project.ClientId != user.Id)
        {
            _notFound = true;
            _project = null;
        }
    }

    private static string StatusLabel(ProjectStatus s) => s switch
    {
        ProjectStatus.InProgress => "In Progress",
        ProjectStatus.OnHold => "On Hold",
        _ => s.ToString()
    };

    private static string StatusBadgeClass(ProjectStatus s) => s switch
    {
        ProjectStatus.InProgress => "bg-blue-50 text-blue-700 ring-1 ring-blue-600/20 dark:bg-blue-400/10 dark:text-blue-400",
        ProjectStatus.Completed  => "bg-green-50 text-green-700 ring-1 ring-green-600/20 dark:bg-green-400/10 dark:text-green-400",
        ProjectStatus.Blocked    => "bg-red-50 text-red-700 ring-1 ring-red-600/20 dark:bg-red-400/10 dark:text-red-400",
        ProjectStatus.OnHold     => "bg-yellow-50 text-yellow-700 ring-1 ring-yellow-600/20 dark:bg-yellow-400/10 dark:text-yellow-400",
        ProjectStatus.Pending    => "bg-slate-100 text-slate-600 ring-1 ring-slate-500/20 dark:bg-white/5 dark:text-neutral-400",
        _ => ""
    };

    private static string InvoiceBadgeClass(InvoiceStatus s) => s switch
    {
        InvoiceStatus.Paid    => "bg-green-50 text-green-700 ring-1 ring-green-600/20 dark:bg-green-400/10 dark:text-green-400",
        InvoiceStatus.Sent    => "bg-blue-50 text-blue-700 ring-1 ring-blue-600/20 dark:bg-blue-400/10 dark:text-blue-400",
        InvoiceStatus.Overdue => "bg-red-50 text-red-700 ring-1 ring-red-600/20 dark:bg-red-400/10 dark:text-red-400",
        InvoiceStatus.Draft   => "bg-slate-100 text-slate-600 ring-1 ring-slate-500/20 dark:bg-white/5 dark:text-neutral-400",
        _ => ""
    };
}
