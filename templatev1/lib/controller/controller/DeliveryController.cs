using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class DeliveryController : abstractController
    {
        private readonly Database _database = new Database();
        private readonly Validator _validator = new Validator();

        public string GetDeliveryMap(string orderId, Size imageSize)
        {
            if (_validator.IsValidOrderId(orderId) == false)
            {
                return "Invalid orderId";
            }

            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                return "Invalid image size";
            }

            var orderStatus = ExecuteScalar("SELECT status FROM order_ WHERE OrderID = @orderId",
                new Dictionary<string, object> { { "@orderId", orderId } });

            switch (orderStatus)
            {
                case "Shipping":
                    string location = GetDeliveryRelay(orderId);
                    return GenerateMapUrl(location, imageSize, orderId);
                case "Shipped":
                    string shippingAddress = GetShippingAddress(orderId);
                    return GenerateMapUrl(shippingAddress, imageSize, orderId);
                case "Ready to Ship":
                case "Pending":
                case "Processing":
                    string readyToShipAddress = GetReadyToShipAddress();
                    return GenerateMapUrl(readyToShipAddress, imageSize, orderId);
                default:
                    return "Invalid order status";
            }
        }

        private string GetApiKey()
        {
            return Configuration.GoogleMapsApiKey ?? throw new Exception("Google Maps API key is not set");
        }

        private string GetReadyToShipAddress()
        {
            return "22.390715003644328, 114.19828146266907";
        }

        private string GetDeliveryRelay(string orderId)
        {
            var relayId = GetDeliveryRelayId(orderId);
            return ExecuteScalar("SELECT CONCAT(latitude, ',', longitude) FROM deliveryrelay WHERE RelayID = @relayId",
                new Dictionary<string, object> { { "@relayId", relayId } });
        }

        private string GetDeliveryRelayId(string orderId)
        {
            return ExecuteScalar("SELECT DeliveryRelayID FROM order_ WHERE OrderID = @orderId",
                new Dictionary<string, object> { { "@orderId", orderId } });
        }

        public int GetRelayId(string relayName)
        {
            return int.Parse(ExecuteScalar("SELECT RelayID FROM deliveryrelay WHERE RelayName = @relayName",
                new Dictionary<string, object> { { "@relayName", relayName } }));
        }

        public List<string> GetCitiesByProvince(string selectedProvince)
        {
            return GetListFromDatabase("SELECT DISTINCT city FROM deliveryrelay WHERE province = @provinceName",
                new Dictionary<string, object> { { "@provinceName", selectedProvince } });
        }

        public List<string> GetRelayNamesByCity(string selectedCity)
        {
            return GetListFromDatabase("SELECT RelayName FROM deliveryrelay WHERE city = @city",
                new Dictionary<string, object> { { "@city", selectedCity } });
        }

        private List<string> GetListFromDatabase(string query, Dictionary<string, object> parameters)
        {
            var dataTable = ExecuteDataTable(query, parameters);
            return dataTable.AsEnumerable().Select(row => row[0].ToString()).ToList();
        }

        private string ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            return _database.ExecuteScalarCommand(query, parameters).ToString();
        }

        private DataTable ExecuteDataTable(string query, Dictionary<string, object> parameters)
        {
            return _database.ExecuteDataTable(query, parameters);
        }

        public string GenerateMapUrl(string location, Size imageSize, string relayName)
        {
            string width = $"{imageSize.Width}";
            string height = $"{imageSize.Height}";

            return $"https://maps.googleapis.com/maps/api/staticmap?center={location}&zoom=15" +
                   $"&size={width}x{height}" +
                   "&maptype=roadmap" +
                   $"&markers=color:red%7Clabel:{relayName}%7C{location}" +
                   $"&key={GetApiKey()}";
        }

        public string GetRelayLocation(string relayName)
        {
            var dt = _database.ExecuteDataTable("SELECT * FROM deliveryrelay WHERE RelayName = @relayName",
                new Dictionary<string, object> { { "@relayName", relayName } });
            return $"{dt.Rows[0]["latitude"]},{dt.Rows[0]["longitude"]}";
        }

        public void EditRelay(string orderId, int relayId)
        {
            var sql = "UPDATE order_ SET DeliveryRelayID = @relayId WHERE OrderID = @orderId";
            _database.ExecuteNonQueryCommand(sql,
                new Dictionary<string, object> { { "@orderId", orderId }, { "@relayId", relayId } });
        }

        public List<string> GetProvinceList()
        {
            return GetListFromDatabase("SELECT DISTINCT province FROM deliveryrelay", null);
        }

        public string GetShippingAddress(string orderId)
        {
            string customerId = GetCustomerId(orderId);
            var parameters = new Dictionary<string, object> { { "@customerId", customerId } };
            var dt = _database.ExecuteDataTable(
                "SELECT CONCAT( CASE WHEN cdf.dfadd = 1 THEN c.warehouseAddress ELSE c.warehouseAddress2 END, ',', c.city, ',', c.province ) as warehouse_address FROM customer c JOIN customer_dfadd cdf ON c.customerID = cdf.customerID WHERE c.customerID = @customerId",
                parameters);
            string location = dt.Rows[0][0].ToString().Replace(" ", "+");
            return location;
        }

        private string GetCustomerId(string orderId)
        {
            string customerAccountID =
                ExecuteScalar("SELECT customerAccountID FROM order_ WHERE OrderID = @orderId",
                    new Dictionary<string, object> { { "@orderId", orderId } });
            string customerID = ExecuteScalar(
                "SELECT customerID FROM customer_account WHERE customerAccountID = @customerAccountID",
                new Dictionary<string, object> { { "@customerAccountID", customerAccountID } });
            return customerID;
        }
    }
}
