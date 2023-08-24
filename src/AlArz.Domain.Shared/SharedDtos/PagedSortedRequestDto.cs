

using Volo.Abp.Application.Dtos;

namespace Application.Contracts.EntityDto
{
    public class PagedSortedRequestDto : PagedAndSortedResultRequestDto
    {
        public bool? SortDescending { get; set; }
    }

    public class PagedSortedDto
    {
        public bool? SortDescending { get; set; } = null;
    }

}
