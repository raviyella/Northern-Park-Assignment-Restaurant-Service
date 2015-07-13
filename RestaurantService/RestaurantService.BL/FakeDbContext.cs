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

    public class FakeDbSet<T> : IDbSet<T>
        where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();

        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public virtual T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Detach(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public System.Collections.ObjectModel.ObservableCollection<T> Local
        {
            get { return new System.Collections.ObjectModel.ObservableCollection<T>(_data); }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return _query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _query.Provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

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
