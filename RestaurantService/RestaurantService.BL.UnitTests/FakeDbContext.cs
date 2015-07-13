using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RestaurantService.DataAccess;
using System.Collections.ObjectModel;
using System.Reflection;

namespace RestaurantService.BL
{
    /// <summary>
    /// FakeRestaurantContext for unit testing.
    /// </summary>
    public class FakeRestaurantContext : IRestaurantContext
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FakeRestaurantContext()
        {
            this.customerOrders = new FakeCustomerOrderSet();
            this.foodItems = new FakeFoodItemSet { new FoodItem { FoodItemId = 1, DishName = "Chicken Biryani", Price = 150.55 },
            new FoodItem { FoodItemId = 2, DishName = "Soup", Price = 120.45 },
            new FoodItem { FoodItemId = 3, DishName = "Chicken Fried Rice", Price = 200 }};
        }

        public IDbSet<CustomerOrder> customerOrders
        {
            get;
            set;
        }

        public IDbSet<FoodItem> foodItems
        {
            get;
            set;
        }

        public int SaveChanges()
        {
            return 0;
        }
    }

    /// <summary>
    /// This is an in-memory, list backed implementation 
    /// of Entity Framework's System.Data.Entity.IDbSet to use 
    /// for testing
    /// </summary>
    /// <typeparam name="T">The type of entity to store</typeparam>
    public class FakeDbSet<T> : IDbSet<T>
        where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();

        }

        /// <summary>
        /// Implements Find funtion of IDbSet
        /// Depends on the key collection being
        /// set to key types of the entity.
        /// </summary>
        /// <param name="keyValues">key values</param>
        /// <returns>entity</returns>
        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        /// <summary>
        /// Implement Add funtion of IDbSet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        /// <summary>
        /// Implement Remove funtion of IDbSet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        /// <summary>
        /// Implement Attach funtion of IDbSet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        /// <summary>
        /// Implement Detach funtion of IDbSet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Detach(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        /// <summary>
        /// Implement Create funtion of IDbSet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Implements create derived entity function of IDbSet
        /// </summary>
        /// <typeparam name="TDerivedEntity"></typeparam>
        /// <returns>derived entity</returns>
        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        /// <summary>
        /// local observable collection
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<T> Local
        {
            get { return new System.Collections.ObjectModel.ObservableCollection<T>(_data); }
        }

        /// <summary>
        /// Element type
        /// </summary>
        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        /// <summary>
        /// LINQ expression
        /// </summary>
        public System.Linq.Expressions.Expression Expression
        {
            get { return _query.Expression; }
        }

        /// <summary>
        /// LINQ query provider
        /// </summary>
        public IQueryProvider Provider
        {
            get { return _query.Provider; }
        }

        /// <summary>
        /// GetEnumerator - iterator over a generic collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator - iterator over a non-generic collection
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }

    public class FakeCustomerOrderSet : FakeDbSet<CustomerOrder>
    {
        public static int _Id = 1;

        public override CustomerOrder Find(params object[] keyValues)
        {
            return this.SingleOrDefault(d => d.CustomerOrderId == (int)keyValues.Single());
        }

        public override CustomerOrder Add(CustomerOrder entity)
        {
            entity.CustomerOrderId = _Id++;
            return base.Add(entity);
        }
    }

    public class FakeFoodItemSet : FakeDbSet<FoodItem>
    {
        public override FoodItem Find(params object[] keyValues)
        {
            return base.Find(keyValues);
        }
    }
    
}
