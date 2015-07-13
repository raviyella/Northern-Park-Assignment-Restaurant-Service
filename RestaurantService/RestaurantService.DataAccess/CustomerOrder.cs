using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DataAccess
{
    /// <summary>
    /// CustomerOrder class
    /// </summary>
    public class CustomerOrder
    {
        /// <summary>
        /// Gets or sets customer order id
        /// </summary>
        public int CustomerOrderId { get; set; }

        /// <summary>
        /// Gets or sets table number
        /// </summary>
        public string TableNumber { get; set; }

        /// <summary>
        /// Gets or sets list of items
        /// </summary>
        [ForeignKey("CustomerOrderId")]
        public List<ItemOrderXRef> Items { get; set; }

        /// <summary>
        /// Gets or sets start time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets completion time
        /// </summary>
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets status
        /// </summary>
        public OrderStatus Status { get; set; }
    }

    /// <summary>
    /// FoodItem class
    /// </summary>
    public class FoodItem
    {
        /// <summary>
        /// Gets or sets food item id
        /// </summary>
        public int FoodItemId { get; set; }

        /// <summary>
        /// Gets or sets dish name
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// Gets or sets price
        /// </summary>
        public double Price { get; set; }
    }

    /// <summary>
    /// Enum for Order status
    /// </summary>
    public enum OrderStatus
    {
        New,
        Approved,
        Declined,
        Ready
    }

    /// <summary>
    /// ItemOrder class
    /// </summary>
    public class ItemOrderXRef
    {
        /// <summary>
        /// Gets or sets item order reference id
        /// </summary>
        public int ItemOrderXRefId { get; set; }

        /// <summary>
        /// Gets or sets food item id
        /// </summary>
        public int FoodItemId { get; set; }

        /// <summary>
        /// Gets or sets item quantity
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// Gets or sets customer order id
        /// </summary>
        public int CustomerOrderId { get; set; }
    }
}
