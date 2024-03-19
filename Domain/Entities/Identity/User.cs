using Domain.Base;
using Domain.Entities.Catalog;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class User : Entity<int>
    {
        private ICollection<UserRole> _userRoles;
        //private ICollection<Cart> _carts;
        //private ICollection<Order> _orders;

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public ICollection<UserRole> UserRoles
        {
            get => _userRoles ??= new List<UserRole>();
            set => _userRoles = value;
        }

        //public ICollection<Cart> Carts
        //{
        //    get => _carts ??= new List<Cart>();
        //    set => _carts = value;
        //}
        
        //public ICollection<Order> Orders
        //{
        //    get => _orders ??= new List<Order>();
        //    set => _orders = value;
        //}
    }
}
