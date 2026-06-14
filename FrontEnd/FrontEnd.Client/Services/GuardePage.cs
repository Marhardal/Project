using FrontEnd.Client.Services;
using Microsoft.AspNetCore.Components;
public abstract class PermissionGuardedPage : ComponentBase
{
    [Inject] protected PermissionService Permissions { get; set; } = default!;
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected virtual string PageSlug
    {
        get
        {
            var relativePath = Nav.ToBaseRelativePath(Nav.Uri);
            var firstSegment = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return (firstSegment ?? string.Empty).ToLowerInvariant();
        }
    }

    // Common action checks
    protected bool CanView => Permissions.Can(PageSlug, "View");
    protected bool CanCreate => Permissions.Can(PageSlug, "Create");
    protected bool CanEdit => Permissions.Can(PageSlug, "Edit");
    protected bool CanDelete => Permissions.Can(PageSlug, "Delete");
    protected bool CanExport => Permissions.Can(PageSlug, "Export");
    protected bool CanDetails => Permissions.Can(PageSlug, "Details");
    protected bool CanApprove => Permissions.Can(PageSlug, "Approve");

    // Composite checks
    protected bool HasTableActions => CanDetails || CanEdit || CanDelete;
    protected bool HasToolbarActions => CanCreate || CanExport;

    protected override async Task OnInitializedAsync()
    {
        await Permissions.InitializeAsync();
        if (!CanView)
        {
            Nav.NavigateTo("/access-denied");
            return;
        }
        await OnPageInitializedAsync();
    }

    protected virtual Task OnPageInitializedAsync() => Task.CompletedTask;
}
