namespace BulkyBook.Utility
{
    public static class GlobalUti
    {
        public const string Role_User_Individual = "Individual Customer";
        public const string Role_User_Company = "Company Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        public const string ShoppingCartSession = "Shopping Cart Session";

        public static double GetPriceBasedOnQuantity(int quantity, double price, double price50, double price100)
        {
            if (quantity < 50) return price;
            if (quantity < 100) return price50;
            return price100;
        }
        
        public static string ConvertRawHtmlToPlainText(string source)
        {
            char[] contentArray = new char[source.Length];
            int contentCharCounter = 0;
            bool IsInsideTag = false;

            for (int i = 0; i < source.Length; i++)
            {
                char character = source[i];
                if (character == '<')
                {
                    IsInsideTag = true;
                    continue;
                }
                if (character == '>')
                {
                    IsInsideTag = false;
                    continue;
                }
                if (!IsInsideTag)
                {
                    contentArray[contentCharCounter] = character;
                    contentCharCounter++;
                }
            }
            return new string(contentArray, 0, contentCharCounter);
        }
    }
}