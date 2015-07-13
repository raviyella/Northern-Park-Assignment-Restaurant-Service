using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace RestaurantService.Contracts
{
    /// <summary>
    /// Service contract for Waiter service.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(INotifyOrderStatusCallback))]
    public interface IWaiterService
    {
        /// <summary>
        /// Method to place an order
        /// </summary>
        /// <param name="order">order details</param>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        void PlaceOrder(AddOrder order);

        /// <summary>
        /// Method to get order details
        /// </summary>
        /// <returns>list of customer order</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDetails))]
        List<CustomerOrder> GetOrders();

        /// <summary>
        /// Method to register, to receive notification
        /// </summary>
        /// <param name="id">guid</param>
        [OperationContract(IsOneWay = true)]
        void RegisterWaiter(Guid id);

        /// <summary>
        /// Method to deregister the waiter from receiving notification
        /// </summary>
        /// <param name="id">guid</param>
        [OperationContract(IsOneWay = true)]
        void DeRegisterWaiter(Guid id);
    }

    /// <summary>
    /// Interface for OrderStatus Notification 
    /// </summary>
    public interface INotifyOrderStatusCallback
    {
        /// <summary>
        /// Method to send notification to chef of order completion
        /// </summary>
        /// <param name="order">order details</param>
        [OperationContract(IsOneWay = true)]
        void OnOrderNotification(CustomerOrder order);
    }
}
