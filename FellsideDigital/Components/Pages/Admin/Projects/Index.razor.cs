using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;

namespace FellsideDigital.Components.Pages.Admin.Projects;

public partial class Index : ComponentBase
{
    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private List<ClientProject>? _projects;

    protected override async Task OnInitializedAsync()
    {
        _projects = await ProjectService.GetAllAsync();
    }
}
