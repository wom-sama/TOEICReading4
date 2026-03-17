using Abp.Application.Services;
using TOEICReading4.Sessions.Dto;
using System.Threading.Tasks;

namespace TOEICReading4.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
