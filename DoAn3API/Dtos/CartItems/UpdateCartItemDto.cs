using Domain.Base;
using System;

namespace DoAn3API.Dtos.CartItems
{
    public class UpdateCartItemDto : BaseCartItemDto, IEntity<Guid>
    {
        public Guid Id { get ; set ; }
    }
}
