using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Utility
{
    public static class SD
    {

        public const string Role_User_Indi = "IndividualCustomer";
        public const string Role_User_Comp = "Company Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        public const string ssShopingCart = "Shoping Cart Session";

        public const string StautsPending = "Pending";
        public const string StautsApproved = "Approved";
        public const string StautsInProcess= "Processing";
        public const string StautsShipped = "Shipped";
        public const string StautsCancelled = "Cancelled ";
        public const string StautsRefund = "Refund ";


        public const string PaymentStautsPending = "Pending ";
        public const string PaymentStautsApproved= "Approved ";
        public const string PaymentStautsDelayedPayment = "ApprovedDelayedPayment ";
        public const string PaymentStautsRejected = "Rejected ";



        public static double GetPriceBasedOnQuality(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
            {
                return price;
            }

            else
            {

                if (quantity < 100)
                {

                    return price50;
                }
                else
                {

                    return price100;
                }
            }

        }

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }

}
