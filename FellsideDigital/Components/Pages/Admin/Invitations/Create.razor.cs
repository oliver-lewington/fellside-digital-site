using System.ComponentModel.DataAnnotations;
using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FellsideDigital.Components.Pages.Admin.Invitations;

public partial class Create : ComponentBase
{
    [Inject] private IInvitationService InvitationService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private InputModel Input { get; set; } = new();
    private string? _errorMessage;
    private string? _emailErrorDetails;
    private bool _submitting;

    private const string InputClass =
        "block w-full rounded-md bg-white px-3 py-1.5 text-sm text-slate-900 outline-1 -outline-offset-1 outline-slate-300 " +
        "placeholder:text-slate-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-400 " +
        "dark:bg-white/5 dark:text-neutral-100 dark:outline-white/10 dark:placeholder:text-neutral-500 dark:focus:outline-orange-400";

    private async Task CreateInvitationAsync()
    {
        _submitting = true;
        _errorMessage = null;
        _emailErrorDetails = null;

        try
        {
            var authState = await AuthState.GetAuthenticationStateAsync();
            var adminUser = await UserManager.GetUserAsync(authState.User);
            if (adminUser is null)
            {
                _errorMessage = "Could not determine the current admin user.";
                return;
            }

            var model = new ClientInvitation
            {
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                CompanyName = Input.CompanyName,
                ServiceType = Input.ServiceType,
                ProjectDescription = Input.ProjectDescription,
                Notes = Input.Notes
            };

            var result = await InvitationService.CreateInvitationAsync(model, adminUser.Id);

            if (!string.IsNullOrEmpty(result.EmailError))
            {
                _errorMessage = "Invitation created but email failed to send.";
                _emailErrorDetails = result.EmailError;
                return;
            }

            NavigationManager.NavigateTo("/Admin/Invitations?success=1");
        }
        catch (Exception ex)
        {
            _errorMessage = "Failed to create invitation. Please try again.";
            _emailErrorDetails = ex.Message;
        }
        finally
        {
            _submitting = false;
        }
    }

    private sealed class InputModel
    {
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string CompanyName { get; set; } = "";
        [Required] public string ServiceType { get; set; } = "";
        [Required] public string ProjectDescription { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}
