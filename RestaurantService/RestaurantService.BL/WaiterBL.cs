using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantService.DataAccess;
using System.Data.Entity.SqlServer;
using System.Data.SqlTypes;

namespace RestaurantService.BL
{
    /// <summary>
    /// Business layer for Waiter Service
    /// </summary>
    public class WaiterBL
    {
        IRestaurantContext context;

        public WaiterBL()
        {
            this.context = DbContextHelper.GetRestaurantContext();
        }

        public WaiterBL(IRestaurantContext Context)
        {
            this.context = DbContextHelper.GetRestaurantContext(Context);
        }

        /// <summary>
        /// Method to Place an Order
        /// </summary>
        /// <param name="addOrder">add order data</param>
        public void PlaceOrder(RestaurantService.Contracts.AddOrder addOrder)
        {
            CustomerOrder custOrder = new CustomerOrder();
            custOrder.TableNumber = addOrder.TableNumber;
            custOrder.StartTime = DateTime.Now;
            custOrder.CompletionTime = (DateTime)SqlDateTime.MinValue;

            var orderCreated = context.customerOrders.Add(custOrder);

            this.AddOrderToCustomer(orderCreated, addOrder);

            this.context.SaveChanges();
        }

        /// <summary>
        /// Method to Add the order to CustomerOrder Table
        /// </summary>
        /// <param name="order">customer order data</param>
        /// <param name="addOrder">addorder data</param>
        private void AddOrderToCustomer(CustomerOrder order, RestaurantService.Contracts.AddOrder addOrder)
        {
            order.Items = new List<ItemOrderXRef>();

            addOrder.Items.ForEach(y =>
            {
                var itemId = (from x in context.foodItems
                              where x.DishName == y.DishName
                              select x.FoodItemId).FirstOrDefault();
                order.Items.Add(new ItemOrderXRef() { FoodItemId = itemId, ItemQty = y.ItemQty, CustomerOrderId = order.CustomerOrderId });
            });
        }

        /// <summary>
        /// Method to Get all the orders in descending chronological order by time.
        /// </summary>
        /// <returns>list of customer orders</returns>
        public List<RestaurantService.Contracts.CustomerOrder> GetOrders()
        {
            var allOrders = this.context.customerOrders.AsQueryable().OrderByDescending(x => x.StartTime);
            return allOrders.Select(
                x => new RestaurantService.Contracts.CustomerOrder
                {
                    StartTime = x.StartTime,
                    CompletionTime = x.CompletionTime,
                    CustomerOrderId = SqlFunctions.StringConvert((double)x.CustomerOrderId),
                    TableId = x.TableNumber,
                    Status = (RestaurantService.Contracts.OrderStatus)x.Status
                }).ToList();
        }
    }
}