using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantService.Contracts
{
    /// <summary>
    /// Service contract for Chef service
    /// </summary>
    [ServiceContract]
    public interface IChefService
    {
        /// <summary>
        /// Method to Approve the order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Approve order status</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        string ApproveOrder(string orderId);

        /// <summary>
        /// Method to decline the order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Decline order status</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        string DeclineOrder(string orderId);

        /// <summary>
        /// Method to get all new orders
        /// </summary>
        /// <returns>list of customer orders</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        List<CustomerOrder> GetNewOrders();

        /// <summary>
        /// Method to mark order as complete
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>order complete status</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        string MarkOrderAsComplete(string orderId);
    }   

    /// <summary>
    /// Customer order class
    /// </summary>
    [DataContract]
    public class CustomerOrder
    {
        /// <summary>
        /// Gets or sets customer order id
        /// </summary>
        [DataMember]
        public string CustomerOrderId { get; set; }

        /// <summary>
        /// Gets or sets table id
        /// </summary>
        [DataMember]
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets items
        /// </summary>
        [DataMember]
        public List<FoodItem> Items { get; set; }

        /// <summary>
        /// Gets or sets start time
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets completion time
        /// </summary>
        [DataMember]
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets status
        /// </summary>
        [DataMember]
        public OrderStatus Status { get; set; }
    }

    /// <summary>
    /// Food item class
    /// </summary>
    [DataContract]
    public class FoodItem
    {
        /// <summary>
        /// Gets or sets dish name
        /// </summary>
        [DataMember]
        public string DishName { get; set; }

        /// <summary>
        /// Gets or sets item quantity
        /// </summary>
        [DataMember]
        public int ItemQty { get; set; }
    }

    /// <summary>
    /// Add order class
    /// </summary>
    [DataContract]
    public class AddOrder
    {
        /// <summary>
        /// Gets or sets table number
        /// </summary>
        [DataMember]
        public string TableNumber { get; set; }

        /// <summary>
        /// Gets or sets items
        /// </summary>
        [DataMember]
        public List<FoodItem> Items { get; set; }
    }

    /// <summary>
    /// Enum for Order status
    /// </summary>
    [DataContract]
    public enum OrderStatus
    {
        [EnumMember]
        New,

        [EnumMember]
        Approved,

        [EnumMember]
        Declined,

        [EnumMember]
        Ready 
    }

    /// <summary>
    /// Fault details class
    /// </summary>
    [DataContract]
    public class ServiceFaultDetails
    {
        /// <summary>
        /// Gets or sets result
        /// </summary>
        [DataMember]
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
