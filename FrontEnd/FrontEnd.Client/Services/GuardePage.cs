using FrontEnd.Client.Services;
using Microsoft.AspNetCore.Components;

public abstract class PermissionGuardedPage : ComponentBase
{
    [Inject] protected PermissionService Permissions { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected abstract string PageSlug { get; }

    protected override async Task OnInitializedAsync()
    {
        await Permissions.InitializeAsync();
        if (!Permissions.CanView(PageSlug))
        {
            Nav.NavigateTo("/access-denied");
            return;
        }
        await OnPageInitializedAsync();
    }

    protected virtual Task OnPageInitializedAsync() => Task.CompletedTask;
}
