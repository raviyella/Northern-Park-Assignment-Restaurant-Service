using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantService.DataAccess;
using System.Data.Entity.SqlServer;
using System.Xml.Linq;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

namespace RestaurantService.BL
{
    /// <summary>
    /// Business layer for Chef Service
    /// </summary>
    public class ChefBL
    {
        IRestaurantContext context;
        XDocument xDocument;

        public ChefBL()
        {
            this.context = DbContextHelper.GetRestaurantContext();
            this.xDocument = XMLHelper.GetXDocument();
        }

        public ChefBL(IRestaurantContext Context)
        {
            this.context = DbContextHelper.GetRestaurantContext(Context);
        }

        /// <summary>
        /// Method to Approve the order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Approve order status</returns>
        public string ApproveOrder(string orderId)
        {
            string retStatus = string.Empty;
            int custOrderId;
            int.TryParse(orderId, out custOrderId);
            var orderToApprove = this.context.customerOrders.AsQueryable().FirstOrDefault(x => x.CustomerOrderId == custOrderId);
            if (orderToApprove.Status == OrderStatus.New)
            {
                orderToApprove.Status = OrderStatus.Approved;
                orderToApprove.CompletionTime = DateTime.Now;
                this.context.SaveChanges();
                retStatus = ((OrderStatus)orderToApprove.Status).ToString();
            }
            else
            {
                retStatus = "Cannot Approve the Order as the order is : " + ((OrderStatus)orderToApprove.Status).ToString();
            }

            return retStatus;
        }

        /// <summary>
        /// Method to Decline the order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Decline order status</returns>
        public string DeclineOrder(string orderId)
        {
            string retStatus = string.Empty;
            int custOrderId = int.Parse(orderId);
            var orderToDecline = this.context.customerOrders.AsQueryable().FirstOrDefault(x => x.CustomerOrderId == custOrderId);
            if (orderToDecline.Status == OrderStatus.New)
            {
                orderToDecline.Status = OrderStatus.Declined;
                orderToDecline.CompletionTime = DateTime.Now;
                this.context.SaveChanges();

                RestaurantService.Contracts.CustomerOrder order = new RestaurantService.Contracts.CustomerOrder();
                order.CustomerOrderId = Convert.ToString(orderToDecline.CustomerOrderId);
                order.Status = (RestaurantService.Contracts.OrderStatus)orderToDecline.Status;
                order.TableId = orderToDecline.TableNumber;

                RestaurantServiceCallback.SendOrderStatus(order);
                retStatus = ((OrderStatus)orderToDecline.Status).ToString();
            }
            else
            {
                retStatus = "Cannot Decline the Order as the order is : " + ((OrderStatus)orderToDecline.Status).ToString();
            }

            return retStatus;
        }

        /// <summary>
        /// Method to Get all orders except declined orders in descending chronological order by time.
        /// </summary>
        /// <returns>list of customer orders</returns>
        public List<RestaurantService.Contracts.CustomerOrder> GetNewOrders()
        {
            var pendingOrders = this.context.customerOrders.AsQueryable().Where(x => (x.Status != OrderStatus.Declined) && (x.Status != OrderStatus.Ready)).OrderByDescending(x => x.StartTime);
            return pendingOrders.Select(
                x => new RestaurantService.Contracts.CustomerOrder
                {
                    StartTime = x.StartTime,
                    CompletionTime = x.CompletionTime,
                    CustomerOrderId = SqlFunctions.StringConvert((double)x.CustomerOrderId),
                    TableId = x.TableNumber,
                    Status = (RestaurantService.Contracts.OrderStatus)x.Status
                }).ToList();
        }

        /// <summary>
        /// Method to Mark the order as Ready and Send a notification to waiter with order status.
        /// It also saves the order details to XML.
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>order complete status</returns>
        public string MarkOrderAsComplete(string orderId)
        {
            string retStatus = string.Empty;
            int custOrderId = int.Parse(orderId);
            var orderToComplete = this.context.customerOrders.AsQueryable().FirstOrDefault(x => x.CustomerOrderId == custOrderId);
            if (orderToComplete.Status == OrderStatus.Approved)
            {
                orderToComplete.Status = OrderStatus.Ready;
                orderToComplete.CompletionTime = DateTime.Now;
                this.context.SaveChanges();

                TimeSpan timeSpan = orderToComplete.CompletionTime.Subtract(orderToComplete.StartTime);

                ////Log the completed orders to XML file
                XMLHelper.SaveToXML(orderToComplete.CustomerOrderId, orderToComplete.TableNumber, timeSpan);

                RestaurantService.Contracts.CustomerOrder order = new RestaurantService.Contracts.CustomerOrder();
                order.CustomerOrderId = Convert.ToString(orderToComplete.CustomerOrderId);
                order.Status = (RestaurantService.Contracts.OrderStatus)orderToComplete.Status;
                order.TableId = orderToComplete.TableNumber;

                RestaurantServiceCallback.SendOrderStatus(order);
                retStatus = ((OrderStatus)orderToComplete.Status).ToString();
            }
            else
            {
                retStatus = "Cannot Complete the Order as the order is : " + ((OrderStatus)orderToComplete.Status).ToString();
            }

            return retStatus;
        }
    }
}
