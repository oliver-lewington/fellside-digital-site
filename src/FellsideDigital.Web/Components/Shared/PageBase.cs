using FellsideDigital.UI.Components.Feedback;
using Microsoft.AspNetCore.Components;

namespace FellsideDigital.Web.Components.Shared;

public abstract class PageBase : ComponentBase
{
    protected LoadingScreen? Loader;
    protected bool IsPageVisible { get; private set; }

    protected async Task FinishLoading()
    {
        if (Loader is not null)
            await Loader.HideAsync();

        IsPageVisible = true;
        StateHasChanged();
    }
}