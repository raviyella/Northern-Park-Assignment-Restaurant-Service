using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace RestaurantService.DataAccess 
{
    /// <summary>
    /// Restaurant Context interface
    /// </summary>
    public interface IRestaurantContext
    {
        IDbSet<CustomerOrder> customerOrders { get; set; }
        IDbSet<FoodItem> foodItems { get; set; }
        int SaveChanges();
    }

    /// <summary>
    /// Restaurant Context class
    /// </summary>
    public class RestaurantContext : DbContext, IRestaurantContext
    {
        public RestaurantContext()
            : base("RestaurantContext")
        {
            Database.SetInitializer<RestaurantContext>(new RestaurantDbInitializer());
        }

        public IDbSet<CustomerOrder> customerOrders { get; set; }
        public IDbSet<FoodItem> foodItems { get; set; }

        /// <summary>
        /// Restaurant Db initializer
        /// </summary>
        public class RestaurantDbInitializer : DropCreateDatabaseAlways<RestaurantContext>
        {
            protected override void Seed(RestaurantContext context)
            {
                IList<FoodItem> masterItems = new List<FoodItem>()
                {
                    new FoodItem(){DishName="Chicken Biryani", Price = 150.55},
                    new FoodItem(){DishName="Chicken Soup", Price = 120.45},
                    new FoodItem(){DishName="Chicken Fried Rice", Price = 200},
                    new FoodItem(){DishName="French Fries", Price = 80},
                    new FoodItem(){DishName="Shrimp W. Broccoli", Price = 175.55},
                    new FoodItem(){DishName="Mixed Vegetable", Price = 150},
                    new FoodItem(){DishName="Home Style Tofu", Price = 200.30},
                    new FoodItem(){DishName="Spring Roll", Price = 100},
                    new FoodItem(){DishName="BBQ Boneless Ribs", Price = 245.45}
                };

                masterItems.ToList().ForEach(var => { context.foodItems.Add(var); });

                base.Seed(context);
            }
        }
    }
}
