using System.ComponentModel.DataAnnotations;
using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;

namespace FellsideDigital.Components.Pages.Admin.Projects;

public partial class Edit : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private ClientProject? _project;
    private InputModel Input { get; set; } = new();
    private string? _errorMessage;
    private bool _submitting;

    private const string InputClass =
        "block w-full rounded-md bg-white px-3 py-1.5 text-sm text-slate-900 outline-1 -outline-offset-1 outline-slate-300 " +
        "placeholder:text-slate-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-400 " +
        "dark:bg-white/5 dark:text-neutral-100 dark:outline-white/10 dark:placeholder:text-neutral-500 dark:focus:outline-orange-400";

    protected override async Task OnInitializedAsync()
    {
        _project = await ProjectService.GetByIdAsync(Id);
        if (_project is not null)
        {
            Input.Name = _project.Name;
            Input.Description = _project.Description;
            Input.Type = _project.Type;
            Input.Status = _project.Status;
            Input.PreviewUrl = _project.PreviewUrl;
            Input.ProjectUrl = _project.ProjectUrl;
            Input.DeploymentNotes = _project.DeploymentNotes;
        }
    }

    private async Task SaveAsync()
    {
        if (_project is null) return;
        _submitting = true;
        _errorMessage = null;
        try
        {
            _project.Name = Input.Name;
            _project.Description = Input.Description;
            _project.Type = Input.Type;
            _project.Status = Input.Status;
            _project.PreviewUrl = string.IsNullOrWhiteSpace(Input.PreviewUrl) ? null : Input.PreviewUrl.Trim();
            _project.ProjectUrl = string.IsNullOrWhiteSpace(Input.ProjectUrl) ? null : Input.ProjectUrl.Trim();
            _project.DeploymentNotes = string.IsNullOrWhiteSpace(Input.DeploymentNotes) ? null : Input.DeploymentNotes.Trim();

            await ProjectService.UpdateAsync(_project);
            NavigationManager.NavigateTo($"/Admin/Projects/{Id}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to save: {ex.Message}";
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
        [Required] public string Name { get; set; } = "";
        [Required] public string Description { get; set; } = "";
        public ProjectType Type { get; set; } = ProjectType.Website;
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;
        public string? PreviewUrl { get; set; }
        public string? ProjectUrl { get; set; }
        public string? DeploymentNotes { get; set; }
    }
}
