using System;
using System.Collections.Generic;
using System.Globalization;

namespace LMCIS.controller.Utilities
{
    public class dateHandler
    {
        private readonly IDatabase database;

        public string SystemDateTime { get; private set; }
        public string SystemDate { get; private set; }

        public dateHandler(IDatabase database = null)
        {
            this.database = database;
            SystemDateTime = GetSystemDateTime();
            SystemDate = GetSystemDate();
        }

        public int DayDifference(string orderID)
        {
            if (string.IsNullOrWhiteSpace(orderID)) throw new ArgumentException("An order ID is required.", nameof(orderID));

            IDatabase source = database ?? new Database();
            try
            {
                var dates = source.ExecuteDataTable(
                    "SELECT shippingDate FROM shipping_detail WHERE orderID = @orderID",
                    new Dictionary<string, object> { { "@orderID", orderID } });
                if (dates.Rows.Count == 0 || dates.Rows[0]["shippingDate"] == DBNull.Value)
                    throw new InvalidOperationException("The order has no shipping date.");

                return DayDifference(Convert.ToDateTime(dates.Rows[0]["shippingDate"], CultureInfo.CurrentCulture));
            }
            finally
            {
                if (database == null) source.Dispose();
            }
        }

        public int DayDifference(DateTime shippingDate)
        {
            return (shippingDate.Date - DateTime.Today).Days;
        }

        public string GetSystemDateTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
        }

        public string GetSystemDate()
        {
            return DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture);
        }
    }
}
