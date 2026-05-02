using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FellsideDigital.Components.Pages.Admin.Invitations;

public partial class Index : ComponentBase
{
    [Inject] private IInvitationService InvitationService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private List<ClientInvitation>? _invitations;
    private string? _successMessage;

    [SupplyParameterFromQuery]
    private string? Success { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Success == "1")
            _successMessage = "Invitation sent successfully.";

        _invitations = await InvitationService.GetAllInvitationsAsync();
    }

    private async Task CopyLink(ClientInvitation inv)
    {
        var url = NavigationManager.ToAbsoluteUri(
            $"/Account/Register?token={Uri.EscapeDataString(inv.Token)}").ToString();
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", url);
    }

    private async Task RevokeAsync(Guid id)
    {
        await InvitationService.RevokeInvitationAsync(id);
        _invitations = await InvitationService.GetAllInvitationsAsync();
    }

    private static string StatusBadgeClass(InvitationStatus status) => status switch
    {
        InvitationStatus.Pending  => "bg-yellow-50 text-yellow-700 ring-1 ring-yellow-600/20 dark:bg-yellow-400/10 dark:text-yellow-400",
        InvitationStatus.Accepted => "bg-green-50 text-green-700 ring-1 ring-green-600/20 dark:bg-green-400/10 dark:text-green-400",
        InvitationStatus.Expired  => "bg-slate-100 text-slate-600 ring-1 ring-slate-500/20 dark:bg-white/5 dark:text-neutral-400",
        InvitationStatus.Revoked  => "bg-red-50 text-red-700 ring-1 ring-red-600/20 dark:bg-red-400/10 dark:text-red-400",
        _ => ""
    };
}
