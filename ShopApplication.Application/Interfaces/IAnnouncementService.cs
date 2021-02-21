using System;
using System.Collections.Generic;
using System.Text;
using ShopApplication.Application.ViewModels.System;
using ShopApplication.Ultilities.Dtos;

namespace ShopApplication.Application.Interfaces
{
    public interface IAnnouncementService
    {
        PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id);
    }
}
