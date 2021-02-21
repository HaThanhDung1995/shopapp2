using System;
using System.Collections.Generic;
using System.Text;
using ShopApplication.Data.Enums;

namespace ShopApplication.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
