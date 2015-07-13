using RestaurantService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.BL
{
    /// <summary>
    /// Restaurant Service CallBack class
    /// </summary>
    public static class RestaurantServiceCallback
    {
        public static Dictionary<Guid, INotifyOrderStatusCallback> registeredWaiters = new Dictionary<Guid, INotifyOrderStatusCallback>();

        /// <summary>
        /// Method to Send Order Status to Waiter Client
        /// </summary>
        /// <param name="order">customer order data</param>
        public static void SendOrderStatus(RestaurantService.Contracts.CustomerOrder order)
        {
            foreach (KeyValuePair<Guid, INotifyOrderStatusCallback> obj in registeredWaiters)
            {
                obj.Value.OnOrderNotification(order);
            }
        }

        /// <summary>
        /// Method to Register the waiter to receive notification
        /// </summary>
        /// <param name="id">guid</param>
        /// <param name="notifyWaiter">instance of NotifyOrderStatusCallback</param>
        public static void RegisterWaiter(Guid id, INotifyOrderStatusCallback notifyWaiter)
        {
            registeredWaiters.Add(id, notifyWaiter);
        }

        /// <summary>
        /// Method to De-Register the waiter from receiving notification.
        /// </summary>
        /// <param name="id">guid</param>
        public static void DegisterWaiter(Guid id)
        {
            registeredWaiters.Remove(id);
        }
    }
}
