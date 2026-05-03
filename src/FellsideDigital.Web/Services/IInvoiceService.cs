using FellsideDigital.Domain.Enums;
using FellsideDigital.Web.Data;
using Microsoft.AspNetCore.Components.Forms;

namespace FellsideDigital.Web.Services;

public interface IInvoiceService
{
    Task<Invoice> UploadAsync(Guid projectId, string title, string? description, decimal amount, string currency, DateTime? dueAt, IBrowserFile file);
    Task<List<Invoice>> GetForProjectAsync(Guid projectId);
    Task<List<Invoice>> GetForClientAsync(string clientId);
    Task<Invoice?> GetByIdAsync(Guid id);
    Task UpdateStatusAsync(Guid id, InvoiceStatus status);
    Task DeleteAsync(Guid id);
}
