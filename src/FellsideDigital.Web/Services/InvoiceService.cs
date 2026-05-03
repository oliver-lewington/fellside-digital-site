using FellsideDigital.Web.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FellsideDigital.Web.Services;

public class InvoiceService(FellsideDigitalDbContext db, IWebHostEnvironment env) : IInvoiceService
{
    private static readonly string[] AllowedExtensions = [".pdf", ".png", ".jpg", ".jpeg"];

    public async Task<Invoice> UploadAsync(Guid projectId, string title, string? description, decimal amount, string currency, DateTime? dueAt, IBrowserFile file)
    {
        var ext = Path.GetExtension(file.Name).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"File type {ext} is not allowed. Use PDF or an image.");

        var uploadDir = Path.Combine(env.WebRootPath, "uploads", "invoices");
        Directory.CreateDirectory(uploadDir);

        var storedName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadDir, storedName);

        await using var stream = File.Create(fullPath);
        await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(stream);

        var invoice = new Invoice
        {
            ProjectId = projectId,
            Title = title,
            Description = description,
            Amount = amount,
            Currency = currency,
            DueAt = dueAt,
            FilePath = $"/uploads/invoices/{storedName}",
            FileName = file.Name,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Status = InvoiceStatus.Sent
        };

        db.Invoices.Add(invoice);
        await db.SaveChangesAsync();
        return invoice;
    }

    public async Task<List<Invoice>> GetForProjectAsync(Guid projectId)
        => await db.Invoices
            .Where(i => i.ProjectId == projectId)
            .OrderByDescending(i => i.IssuedAt)
            .ToListAsync();

    public async Task<List<Invoice>> GetForClientAsync(string clientId)
        => await db.Invoices
            .Include(i => i.Project)
            .Where(i => i.Project!.ClientId == clientId)
            .OrderByDescending(i => i.IssuedAt)
            .ToListAsync();

    public async Task<Invoice?> GetByIdAsync(Guid id)
        => await db.Invoices
            .Include(i => i.Project)
                .ThenInclude(p => p!.Client)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task UpdateStatusAsync(Guid id, InvoiceStatus status)
    {
        var invoice = await db.Invoices.FindAsync(id);
        if (invoice is null) return;
        invoice.Status = status;
        if (status == InvoiceStatus.Paid)
            invoice.PaidAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var invoice = await db.Invoices.FindAsync(id);
        if (invoice is null) return;

        if (!string.IsNullOrEmpty(invoice.FilePath))
        {
            var fullPath = Path.Combine(env.WebRootPath, invoice.FilePath.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        db.Invoices.Remove(invoice);
        await db.SaveChangesAsync();
    }
}
