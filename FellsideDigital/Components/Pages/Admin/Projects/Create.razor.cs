using System.ComponentModel.DataAnnotations;
using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FellsideDigital.Components.Pages.Admin.Projects;

public partial class Create : ComponentBase
{
    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private InputModel Input { get; set; } = new();
    private List<ApplicationUser> _clients = [];
    private string? _errorMessage;
    private bool _submitting;

    private const string InputClass =
        "block w-full rounded-md bg-white px-3 py-1.5 text-sm text-slate-900 outline-1 -outline-offset-1 outline-slate-300 " +
        "placeholder:text-slate-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-400 " +
        "dark:bg-white/5 dark:text-neutral-100 dark:outline-white/10 dark:placeholder:text-neutral-500 dark:focus:outline-orange-400";

    protected override async Task OnInitializedAsync()
    {
        var allUsers = UserManager.Users.ToList();
        var adminIds = (await UserManager.GetUsersInRoleAsync("SiteAdmin"))
            .Select(u => u.Id)
            .ToHashSet();
        _clients = allUsers.Where(u => !adminIds.Contains(u.Id)).ToList();
    }

    private async Task CreateAsync()
    {
        _submitting = true;
        _errorMessage = null;
        try
        {
            var authState = await AuthState.GetAuthenticationStateAsync();
            var admin = await UserManager.GetUserAsync(authState.User);
            if (admin is null) { _errorMessage = "Could not identify admin user."; return; }

            var project = new ClientProject
            {
                ClientId = Input.ClientId,
                Name = Input.Name,
                Description = Input.Description,
                Type = Input.Type,
                Status = Input.Status,
                PreviewUrl = string.IsNullOrWhiteSpace(Input.PreviewUrl) ? null : Input.PreviewUrl.Trim(),
                ProjectUrl = string.IsNullOrWhiteSpace(Input.ProjectUrl) ? null : Input.ProjectUrl.Trim(),
                DeploymentNotes = string.IsNullOrWhiteSpace(Input.DeploymentNotes) ? null : Input.DeploymentNotes.Trim()
            };

            await ProjectService.CreateAsync(project, admin.Id);
            NavigationManager.NavigateTo($"/Admin/Projects/{project.Id}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to create project: {ex.Message}";
        }
        finally
        {
            _submitting = false;
        }
    }

    private static string StatusLabel(ProjectStatus s) => s switch
    {
        ProjectStatus.InProgress => "In Progress",
        ProjectStatus.OnHold => "On Hold",
        _ => s.ToString()
    };

    private sealed class InputModel
    {
        [Required] public string ClientId { get; set; } = "";
        [Required] public string Name { get; set; } = "";
        [Required] public string Description { get; set; } = "";
        public ProjectType Type { get; set; } = ProjectType.Website;
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;
        public string? PreviewUrl { get; set; }
        public string? ProjectUrl { get; set; }
        public string? DeploymentNotes { get; set; }
    }
}
