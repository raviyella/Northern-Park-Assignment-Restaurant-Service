using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using RestaurantService.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using NUnit.Framework;

namespace RestaurantService.BL.UnitTests
{
    [TestFixture]
    public class RestaurantServiceWaiterBLTests 
    {
        IRestaurantContext fakeRestaurantContext;

        [SetUp]
        public void Initialize()
        {
            fakeRestaurantContext = new FakeRestaurantContext();
        }

        [TearDown]
        public void CleanUp()
        {
            fakeRestaurantContext = null;
        }

        private void VerifyResults(List<string> expectedTableNumber, string expectedOrderStatus, IRestaurantContext context)
        {
            Type type = context.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property == null) continue;

                bool isEnumerable = (from i in property.PropertyType.GetInterfaces()
                                     where i == typeof(IEnumerable)
                                     select i
                    ).Any();

                if (!isEnumerable) continue;

                try
                {
                    if (property.Name.Contains("customerOrders"))
                    {
                        int i = 0;
                        foreach (CustomerOrder custOrder in (IEnumerable)property.GetValue(context, null))
                        {
                            string ss = custOrder.StartTime.ToLongTimeString();
                            string tableNumber = custOrder.TableNumber;

                            NUnit.Framework.Assert.AreEqual(expectedTableNumber[i], tableNumber);
                            NUnit.Framework.Assert.AreEqual(expectedOrderStatus, ((OrderStatus)custOrder.Status).ToString());

                            foreach (ItemOrderXRef item in custOrder.Items)
                            {
                                NUnit.Framework.Assert.AreEqual(custOrder.CustomerOrderId, item.CustomerOrderId);
                            }

                            i++;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    if (ex.InnerException != null)
                        error += ex.InnerException.Message;
                    NUnit.Framework.Assert.Fail(error);
                }
            }
        }

        [Test(Description = "Waiter PlaceOrder Test")]
        public void WaiterBL_PlaceOrderTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "New";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Waiter Multiple PlaceOrder Test")]
        public void WaiterBL_MultipleOrderTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");
            lstExpectedTableId.Add("T02");

            string expectedOrderStatus = "New";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            order.TableNumber = "T02";
            waiter.PlaceOrder(order);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Waiter PlaceOrder Invalid Test")]
        [NUnit.Framework.ExpectedException(typeof(NullReferenceException))]
        public void WaiterBL_InvalidOrderTest()
        {
            Contracts.AddOrder order = new Contracts.AddOrder();

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "New";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef New Order Approved Test")]
        public void ChefBL_NewOrderApprovedTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "Approved";
            string expectedApproveOrderResult = "Approved";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);
            string actualApproveOrderResult = chef.ApproveOrder("1");
            NUnit.Framework.Assert.AreEqual(expectedApproveOrderResult, actualApproveOrderResult);
            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef New Order Cannot be Marked Completed Test")]
        public void ChefBL_NewOrderMarkedCompleted_InvalidTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "New";
            string expectedApproveOrderResult = "Cannot Complete the Order as the order is : New";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);
            string actualApproveOrderResult = chef.MarkOrderAsComplete("1");

            NUnit.Framework.Assert.AreEqual(expectedApproveOrderResult, actualApproveOrderResult);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef New Order Declined Test")]
        public void ChefBL_NewOrderDeclinedTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "Declined";
            string expectedDeclineOrderResult = "Declined";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);
            string actualDeclinedOrderResult = chef.DeclineOrder("1");

            NUnit.Framework.Assert.AreEqual(expectedDeclineOrderResult, actualDeclinedOrderResult);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef Approved Order Cannot be Declined Test")]
        public void ChefBL_ApprovedOrderDeclined_InvalidTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "Approved";
            string expectedApprovedOrderResult = "Cannot Decline the Order as the order is : Approved";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);
            chef.ApproveOrder("1");

            string actualApprovedOrderResult = chef.DeclineOrder("1");

            NUnit.Framework.Assert.AreEqual(expectedApprovedOrderResult, actualApprovedOrderResult);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef Approved Order Marked Completed Test")]
        public void ChefBL_ApprovedOrderMarkedCompletedTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "Ready";
            string expectedOrderCompleteResult = "Ready";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);
            chef.ApproveOrder("1");

            string actualOrderCompleteResult = chef.MarkOrderAsComplete("1");

            NUnit.Framework.Assert.AreEqual(expectedOrderCompleteResult, actualOrderCompleteResult);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

        [Test(Description = "Chef Declined Order Cannot be Marked Completed Test")]
        public void ChefBL_DeclinedOrderMarkedCompleted_InvalidTest()
        {
            List<Contracts.FoodItem> foodItem = new List<Contracts.FoodItem>()
            { 
                new Contracts.FoodItem(){ DishName="Chicken Biryani", ItemQty =4},
                new Contracts.FoodItem(){ DishName="Soup", ItemQty=5}
            };

            Contracts.AddOrder order = new Contracts.AddOrder();
            order.TableNumber = "T01";
            order.Items = foodItem;

            List<string> lstExpectedTableId = new List<string>();
            lstExpectedTableId.Add("T01");

            string expectedOrderStatus = "Declined";
            string expectedCompleteOrderResult = "Cannot Complete the Order as the order is : Declined";

            WaiterBL waiter = new WaiterBL(fakeRestaurantContext);
            waiter.PlaceOrder(order);

            ChefBL chef = new ChefBL(fakeRestaurantContext);

            chef.DeclineOrder("1");

            string actualCompleteOrderResult = chef.MarkOrderAsComplete("1");

            NUnit.Framework.Assert.AreEqual(expectedCompleteOrderResult, actualCompleteOrderResult);

            VerifyResults(lstExpectedTableId, expectedOrderStatus, fakeRestaurantContext);
        }

    }
}