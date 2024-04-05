namespace DoAn3API.Constants.Catalogs
{
    public static class CatalogConst
    {
        public class CartStatus
        {
            public const string PENDING = "Pending";
            public const string SUCCESS = "Success";
        }

        public class OrderStatus
        {
            public const int Processing = 1;
            public const int Verify = 2;
            public const int Delivering = 3;
            public const int Completed = 4;
            public const int Cancel = 5;
        }


    }
}
