using Abp.Application.Services;
using TOEICReading4.MultiTenancy.Dto;

namespace TOEICReading4.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

