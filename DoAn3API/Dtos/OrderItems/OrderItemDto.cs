namespace DoAn3API.Dtos.OrderItems
{

    public class OrderItemDto : BaseOrderItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }

        public double Total { get; set; }
        public int Status { get; set; }
    }
}
