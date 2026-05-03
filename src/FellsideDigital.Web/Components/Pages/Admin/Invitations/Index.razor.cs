using FellsideDigital.Web.Data;
using FellsideDigital.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FellsideDigital.Web.Components.Pages.Admin.Invitations;

public partial class Index : ComponentBase
{
    [Inject] private IInvitationService InvitationService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private List<ClientInvitation>? _invitations;
    private string? _successMessage;
    private string? _copiedMessage;

    [SupplyParameterFromQuery]
    private string? Success { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Success == "1")
            _successMessage = "Invitation created and sent successfully.";

        _invitations = await InvitationService.GetAllInvitationsAsync();
    }

    private async Task CopyLink(ClientInvitation inv)
    {
        var url = NavigationManager.ToAbsoluteUri(
            $"/Account/Register?token={Uri.EscapeDataString(inv.Token)}").ToString();
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", url);
        _copiedMessage = $"Invitation link for {inv.FirstName} copied to clipboard.";
        StateHasChanged();
        await Task.Delay(3000);
        _copiedMessage = null;
        StateHasChanged();
    }

    private async Task RevokeAsync(Guid id)
    {
        await InvitationService.RevokeInvitationAsync(id);
        _invitations = await InvitationService.GetAllInvitationsAsync();
    }
}
