using Abp.Application.Services;
using TOEICReading4.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace TOEICReading4.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
