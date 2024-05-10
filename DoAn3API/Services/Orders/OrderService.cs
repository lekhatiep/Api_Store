using AutoMapper;
using Dapper;
using DoAn3API.Constants.Catalogs;
using DoAn3API.DataContext;
using DoAn3API.Dtos.OrderItems;
using DoAn3API.Dtos.Orders;
using DoAn3API.Services.Carts;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using Infastructure.Repositories.Catalogs.CartRepos;
using Infastructure.Repositories.Catalogs.OrderItemRepos;
using Infastructure.Repositories.Catalogs.OrderRepos;
using Infastructure.Repositories.ProductRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly DapperContext _dapperContext;

        public OrderService(
            IOrderRepository orderRepository,
            IMapper mapper,
            ICartService cartService,
            ICartRepository cartRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            DapperContext dapperContext
            )
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cartService = cartService;
            _cartRepository = cartRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _dapperContext = dapperContext;
        }

      
        public async Task<int> Checkout(CreateOrderDto createOrderDto)
        {
            var orderItems = new List<OrderItemDto>();

            foreach (var item in createOrderDto.OrderItems)
            {
                orderItems.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            var order = _mapper.Map<Order>(createOrderDto);
            order.Status = CatalogConst.OrderStatus.Processing;
            order.OrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            await _orderRepository.Insert(order);
            await _orderRepository.Save();

            return order.Id;

        }

        public async Task<List<OrderItemDto>> GetListHistoryOrderByUser(int userId, int status)
        {
            var query = from o in _orderRepository.List()
                        join oi in _orderItemRepository.List() on o.Id equals oi.OrderId into oio
                        from oi in oio.DefaultIfEmpty()
                        join p in _productRepository.List()
                                    .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete)) on oi.ProductId equals p.Id 
                        orderby oi.CreateTime descending
                        select new { o, oi, p };

            if (status == 0)
            {
                status = 0;

                var queryAllOrderItem = await (from q in query
                               where q.o.UserId == userId && q.o.IsDelete == false 

                               select new { q.oi, q.p , q.o })
                               .Select(x => new OrderItemDto
                               {
                                   Id = x.oi.Id,
                                   ProductId = x.p.Id,
                                   ImgPath = x.p.ProductImages.FirstOrDefault().ImagePath,
                                   Title = x.p.Title,
                                   Price = x.oi.Price,
                                   Quantity = x.oi.Quantity,
                                   Total = x.oi.Quantity * Convert.ToDouble(x.p.Price),
                                   Status = x.o.Status

                               }).ToListAsync(); ;

                return queryAllOrderItem;
            }

            var queryOrderItem = await (from q in query where q.o.UserId == userId &&
                                   q.o.Status == status && q.o.IsDelete == false
                                   select new { q.oi, q.p ,q.o})

                             .Select(x => new OrderItemDto
                             {
                                 Id = x.oi.Id, 
                                 ProductId = x.p.Id,
                                 ImgPath = x.p.ProductImages.FirstOrDefault().ImagePath,
                                 Title = x.p.Title,
                                 Price = x.oi.Price,
                                 Quantity = x.oi.Quantity,
                                 Total = x.oi.Quantity * Convert.ToDouble(x.p.Price),
                                 Status = x.o.Status

                             }).ToListAsync();

            return queryOrderItem;
        }

        public async Task<int> ProcessCheckoutOrder(int userId)
        {
            var cart = await _cartService.GetCartUserById(userId);

            if (cart == null)
            {
                return -1;

            }
            var listCart = await _cartService.GetUserListCartItemIsOrder(cart.Id);

            var orderItems = new List<OrderItemDto>();
            var createOrderDto = new CreateOrderDto();


            double subTotal = 0;
            double total = 0;
            double grandTotal = 0;
            double shipping = 0;
            double discountShop = 0; //Discount of Shop

            foreach (var item in listCart)
            {
                subTotal += Convert.ToDouble(item.Quantity * item.Price);
                total += (subTotal - item.Discount);

                orderItems.Add(new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Discount = item.Discount, //Discount supplier
                    CreateTime = DateTime.Now,
                    Price = item.Price,
                    Total = subTotal
                });
            }

            createOrderDto.OrderItems = orderItems;
            createOrderDto.GrandTotal = (total + shipping - discountShop);

            var order = _mapper.Map<Order>(createOrderDto);
            order.Status = CatalogConst.OrderStatus.Verify;
            order.OrderItems = _mapper.Map<List<OrderItem>>(orderItems);
            order.UserId = cart.UserId;
            order.CreateTime = DateTime.Now;

            await _orderRepository.Insert(order);
            await _orderRepository.Save();



            //Update current product quantity

            foreach (var item in listCart)
            {
                var product = await _productRepository.GetById(item.ProductId);
                product.Quantity--;

                await _productRepository.Update(product, product.Id);

            }

            //Update Cart 

            cart.Status = CatalogConst.CartStatus.SUCCESS;
            await _cartRepository.Update(cart, cart.Id);

            return order.Id;
        }

        public async Task<int> CancelOrder(int orderId)
        {
            var order = await _productRepository.GetById(orderId);
            order.IsDelete = true;

            await _productRepository.Update(order, order.Id);

            return 1;
        }

        public PagedList<OrderDto> GetListOderPaging(PagedOrderRequestDto pagedOrderRequest)
        {
            //Query
            var queryCategory = _orderRepository.List().Include(x => x.User);

            //List category

            var listOrder = queryCategory
                .Where(x => x.IsDelete == false);

            if (pagedOrderRequest.SearchOrderID > 0)
            {
                listOrder = listOrder.Where(x => x.Id == pagedOrderRequest.SearchOrderID);
            }
            if (!string.IsNullOrEmpty(pagedOrderRequest.SortValue))
            {
                listOrder = listOrder.OrderBy(x => pagedOrderRequest.SortValue);
            }

            if (!string.IsNullOrEmpty(pagedOrderRequest.SortBy) && pagedOrderRequest.SortBy.ToLower() == "desc")
            {
                listOrder = listOrder.OrderByDescending(x => pagedOrderRequest.SortValue);
            }

            var data = PagedList<Order>.ToPagedList(ref listOrder, pagedOrderRequest.PageNumber, pagedOrderRequest.PageSize);

            var dataResult = _mapper.Map<PagedList<OrderDto>>(data);

            return dataResult;
        }

        public async Task<List<OrderItemDto>> GetDetailOrder(int orderID)
        {
            if (orderID <0)
            {
                return null;
            }
            try
            {
                using (var con = _dapperContext.CreateConnection())
                {
                    var sql = $@"SELECT oi.*,oi.Quantity * (oi.Price - oi.Discount) as Total, p.Title, pi.ImagePath as ImgPath FROM OrderItems oi
	                            left join Products p on oi.ProductId = p.Id
	                            left join ProductImage pi on p.id =  pi.ProductId
	                            WHERE OrderId = {orderID}";

                    var data = await con.QueryAsync<OrderItemDto>(sql);
                    return data.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateStatusOrder(int orderID, int status)
        {
            if (orderID < 0)
            {
                return 0;
            }
            try
            {
                using (var con = _dapperContext.CreateConnection())
                {
                    var sql = $@"UPDATE Orders SET Status = {status} WHERE Id = {orderID}";

                    await con.ExecuteAsync(sql);

                    return 1;
                }
            }
            catch (Exception ex )
            {

                throw;
            }
           
        }
    }
}
