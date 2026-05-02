using System.Security.Claims;
using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FellsideDigital.Components.Pages.Portal;

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
