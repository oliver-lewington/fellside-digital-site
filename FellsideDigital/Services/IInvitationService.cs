using FellsideDigital.Data;

namespace FellsideDigital.Services;

public interface IInvitationService
{
    Task<(ClientInvitation? Invitation, string? EmailError)> CreateInvitationAsync(ClientInvitation model, string adminUserId);
    Task<ClientInvitation?> GetInvitationByTokenAsync(string token);
    Task AcceptInvitationAsync(Guid invitationId, string newUserId);
    Task<List<ClientInvitation>> GetAllInvitationsAsync();
    Task RevokeInvitationAsync(Guid id);
}
