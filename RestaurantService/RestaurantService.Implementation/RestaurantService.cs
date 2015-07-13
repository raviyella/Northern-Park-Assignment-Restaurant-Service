using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RestaurantService.BL;
using RestaurantService.Contracts;

namespace RestaurantService
{
    /// <summary>
    /// Service class for Restaurant and it implements IChefService, IWaiterService interfaces
    /// </summary>
    public class RestaurantService : IChefService, IWaiterService
    {
        ChefBL chefBL = new ChefBL();
        WaiterBL waiterBL = new WaiterBL();

        #region IChefService Implementation
        /// <summary>
        /// Method to Approve the Order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Approve order status</returns>
        string IChefService.ApproveOrder(string orderId)
        {
            try
            {
                return this.chefBL.ApproveOrder(orderId);
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }

        /// <summary>
        /// Method to Decline the Order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Decline order status</returns>
        string IChefService.DeclineOrder(string orderId)
        {
            try
            {
                return this.chefBL.DeclineOrder(orderId);
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }

        /// <summary>
        /// Method to Get All Orders with its Details
        /// </summary>
        /// <returns>list of customer orders</returns>
        List<CustomerOrder> IChefService.GetNewOrders()
        {
            try
            {
                return this.chefBL.GetNewOrders();
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }

        /// <summary>
        /// Method to Mark Order as Complete
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>order complete status</returns>
        string IChefService.MarkOrderAsComplete(string orderId)
        {
            try
            {
                return this.chefBL.MarkOrderAsComplete(orderId);
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }
        #endregion

        #region IWaiterService Implementation
        /// <summary>
        /// Method to Place an Order
        /// </summary>
        /// <param name="order">order data</param>
        void IWaiterService.PlaceOrder(AddOrder order)
        {
            try
            {
                this.waiterBL.PlaceOrder(order);
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }

        /// <summary>
        /// Method to Get All Orders with its Details
        /// </summary>
        /// <returns>list of customer orders</returns>
        List<CustomerOrder> IWaiterService.GetOrders()
        {
            try
            {
                return this.waiterBL.GetOrders();
            }
            catch (Exception ex)
            {
                ServiceFaultDetails serviceException = new ServiceFaultDetails();
                serviceException.ErrorMessage = ex.Message;
                serviceException.Result = false;
                throw new FaultException<ServiceFaultDetails>(serviceException);
            }
        }

        /// <summary>
        /// Method to Register the Waiter so as to receive notification for order completion.
        /// </summary>
        /// <param name="id">guid</param>
        public void RegisterWaiter(Guid id)
        {
            INotifyOrderStatusCallback callback = OperationContext.Current.GetCallbackChannel<INotifyOrderStatusCallback>();
            RestaurantServiceCallback.RegisterWaiter(id, callback);
        }

        /// <summary>
        /// Method to De-Register Waiter from receiving notification of order completion.
        /// </summary>
        /// <param name="id">guid</param>
        public void DeRegisterWaiter(Guid id)
        {
            RestaurantServiceCallback.DegisterWaiter(id);
        }

        #endregion
    }
}
