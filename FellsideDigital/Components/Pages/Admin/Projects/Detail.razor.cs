using FellsideDigital.Data;
using FellsideDigital.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace FellsideDigital.Components.Pages.Admin.Projects;

public partial class Detail : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    [Inject] private IProjectService ProjectService { get; set; } = default!;
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthState { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private ClientProject? _project;
    private string _updateMessage = "";
    private string _newStatus = "";
    private bool _postingUpdate;

    private string _invoiceTitle = "";
    private decimal _invoiceAmount;
    private string _invoiceCurrency = "GBP";
    private DateTime? _invoiceDueDate;
    private IBrowserFile? _selectedFile;
    private bool _uploadingInvoice;
    private string? _invoiceError;

    private const string InputClass =
        "block w-full rounded-md bg-white px-3 py-1.5 text-sm text-slate-900 outline-1 -outline-offset-1 outline-slate-300 " +
        "placeholder:text-slate-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-400 " +
        "dark:bg-white/5 dark:text-neutral-100 dark:outline-white/10 dark:placeholder:text-neutral-500 dark:focus:outline-orange-400";

    protected override async Task OnInitializedAsync() => await LoadAsync();

    private async Task LoadAsync()
    {
        _project = await ProjectService.GetByIdAsync(Id);
    }

    private async Task PostUpdateAsync()
    {
        if (string.IsNullOrWhiteSpace(_updateMessage)) return;
        _postingUpdate = true;
        var authState = await AuthState.GetAuthenticationStateAsync();
        var admin = await UserManager.GetUserAsync(authState.User);
        ProjectStatus? status = Enum.TryParse<ProjectStatus>(_newStatus, out var parsed) ? parsed : null;
        await ProjectService.AddStatusUpdateAsync(Id, _updateMessage.Trim(), status, admin!.Id);
        _updateMessage = "";
        _newStatus = "";
        _postingUpdate = false;
        await LoadAsync();
    }

    private void OnFileSelected(InputFileChangeEventArgs e) => _selectedFile = e.File;

    private async Task UploadInvoiceAsync()
    {
        if (_selectedFile is null || string.IsNullOrWhiteSpace(_invoiceTitle)) return;
        _uploadingInvoice = true;
        _invoiceError = null;
        try
        {
            await InvoiceService.UploadAsync(Id, _invoiceTitle.Trim(), null, _invoiceAmount,
                _invoiceCurrency, _invoiceDueDate, _selectedFile);
            _invoiceTitle = "";
            _invoiceAmount = 0;
            _invoiceDueDate = null;
            _selectedFile = null;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _invoiceError = ex.Message;
        }
        finally
        {
            _uploadingInvoice = false;
        }
    }

    private async Task ChangeInvoiceStatusAsync(Guid invoiceId, ChangeEventArgs e)
    {
        if (Enum.TryParse<InvoiceStatus>(e.Value?.ToString(), out var status))
            await InvoiceService.UpdateStatusAsync(invoiceId, status);
        await LoadAsync();
    }

    private async Task DeleteInvoiceAsync(Guid invoiceId)
    {
        await InvoiceService.DeleteAsync(invoiceId);
        await LoadAsync();
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
