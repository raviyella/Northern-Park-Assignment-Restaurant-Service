using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestaurantService.Contracts;
using System.ServiceModel;

namespace RestaurantWaiterClient
{
    public partial class WaiterClient : Form
    {
        static int itemCount = 0;
        double total = 0;
        EndpointAddress waiterServiceAddress;
        ChannelFactory<IWaiterService> waiterServiceFactory;
        IWaiterService waiterClient;
        InstanceContext callbackInstance;
        Guid guid;
        ErrorProvider errorProvider;

        public WaiterClient()
        {
            this.InitializeComponent();

            this.InitializeWaiterChannel();
        }

        /// <summary>
        /// Method to Instantiate Duplex channel factory for Waiter Service
        /// </summary>
        private void InitializeWaiterChannel()
        {
            try
            {
                this.waiterServiceAddress = new EndpointAddress(new Uri("net.tcp://localhost:4502/WaiterService"));
                this.callbackInstance = new InstanceContext(new OrderNotifitcation());
                this.waiterServiceFactory = new DuplexChannelFactory<IWaiterService>(
                    this.callbackInstance,
                    new NetTcpBinding(),
                    this.waiterServiceAddress);
                this.waiterClient = this.waiterServiceFactory.CreateChannel();
                this.guid = Guid.NewGuid();
                this.waiterClient.RegisterWaiter(this.guid);
                this.errorProvider = new ErrorProvider();
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show(ex.Message, "FaultException", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CommunicationException ex)
            {
                ((IClientChannel)waiterClient).Abort();
                waiterClient = null;
                MessageBox.Show(ex.Message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                waiterServiceFactory.Abort();
                System.Environment.Exit(1);
            }
        }

        /// <summary>
        /// Method to Clean up.
        /// </summary>
        private void CleanUp()
        {
            try
            {
                errorProvider = null;
                waiterClient.DeRegisterWaiter(guid);
                ((IClientChannel)waiterClient).Close();
                waiterClient = null;
                waiterServiceFactory.Close();
                waiterServiceFactory = null;
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine(commProblem.Message);

                if (waiterServiceFactory != null)
                {
                    waiterServiceFactory.Abort();
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (waiterServiceFactory != null)
                {
                    waiterServiceFactory.Abort();
                }

                Console.ReadLine();
            }
            finally
            {
                if (waiterClient != null)
                {
                    ((IClientChannel)waiterClient).Close();
                }

                if (waiterServiceFactory != null)
                {
                    waiterServiceFactory.Abort();
                }
            }
        }

        /// <summary>
        /// Method that triggers on closing of Form.
        /// </summary>
        /// <param name="e"> Closing Event data</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            //Confirm User wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Waiter UI Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    CleanUp();
                    break;
            }
        }

        /// <summary>
        /// Click event to handle selection of the Order
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void BtnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                    (!string.IsNullOrEmpty(txtQty.Text) &&
                    (!string.IsNullOrEmpty(Convert.ToString(comboBox1.SelectedItem)))) &&
                    (listView1.SelectedItems.Count > 0)
                    )
                {
                    if (!Regex.IsMatch(txtQty.Text, @"^[1-9]*$"))
                    {
                        errorProvider.SetError(txtQty, "Only Digits");
                    }
                    else
                    {
                        errorProvider.Clear();
                        double price = double.Parse(listView1.SelectedItems[0].SubItems[1].Text, CultureInfo.InvariantCulture);

                        int quantity = int.Parse(txtQty.Text);
                        double subTotal = price * quantity;
                        string item = listView1.SelectedItems[0].Text;

                        listView2.Items.Add(item);
                        listView2.Items[itemCount].SubItems.Add(price.ToString());
                        listView2.Items[itemCount].SubItems.Add(quantity.ToString());
                        listView2.Items[itemCount].SubItems.Add(subTotal.ToString());
                        itemCount++;
                        
                        total += subTotal;
                        lblTotal.Text = total.ToString();

                        txtQty.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Table Number, Menu and Order the Item");
                    txtQty.Clear();
                }
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.ErrorMessage);
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show("Communication Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Click event to handle Addition of Order
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void BtnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                itemCount = 0;
                AddOrder order = new AddOrder();
                List<FoodItem> lstFoodItem = new List<FoodItem>();
                string tableNumber = Convert.ToString(comboBox1.SelectedItem);
                if (!string.IsNullOrEmpty(tableNumber) &&
                    (listView2.Items.Count > 0) )
                {
                    order.TableNumber = tableNumber;
                    int i = 0;
                    foreach (ListViewItem lvItem in listView2.Items)
                    {
                        string item = listView2.Items[i].SubItems[0].Text.ToString();
                        double price = double.Parse(listView2.Items[i].SubItems[1].Text, CultureInfo.InvariantCulture);
                        int quantity = int.Parse(listView2.Items[i].SubItems[2].Text);
                        double subTotal;
                        double.TryParse(listView2.Items[i].SubItems[2].Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out subTotal);

                        lstFoodItem.Add(new FoodItem() { DishName = item, ItemQty = quantity });
                        i++;
                    }

                    if (lstFoodItem.Count > 0)
                    {
                        order.Items = lstFoodItem;
                        waiterClient.PlaceOrder(order);
                        listView2.Items.Clear();
                        lblTotal.Text = "";
                        total = 0;
                        MessageBox.Show("Order Placed Successfully");
                    }
                }
                else
                {
                    MessageBox.Show("Order list is Empty. Place an Order");
                }
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.ErrorMessage);
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        /// <summary>
        /// Click event to handle retrieval of all Order details
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void BtnGetAllOrders_Click(object sender, EventArgs e)
        {
            try
            {
                List<CustomerOrder> orders = waiterClient.GetOrders();

                OrderForm orderForm = new OrderForm(orders);
                orderForm.Show();
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.ErrorMessage);
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        /// <summary>
        /// Click event to handle deletion of Order from the list. 
        /// It will not actually delete the order from DB
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.SelectedItems != null && listView2.SelectedItems.Count > 0)
                {
                    double deduct = 0;
                    var confirm = MessageBox.Show("Are Sure You Want to Delete the Item", "DELETE ITEM", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        for (int i = 0; i < listView2.Items.Count; i++)
                        {
                            if (listView2.Items[i].Selected)
                            {
                                double.TryParse(listView2.Items[i].SubItems[3].Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out deduct);
                                listView2.Items[i].Remove();
                                i--;
                                itemCount--;
                            }
                        }
                    }
                    total = total - deduct;
                    lblTotal.Text = total.ToString();
                }
                else
                {
                    MessageBox.Show("Select Item to Delete", "DELETE ITEM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException formatException)
            {
                MessageBox.Show("Format Exception: ", formatException.Message);
            }
        }
    }

    /// <summary>
    /// Order Completion Notification from Chef
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class OrderNotifitcation : INotifyOrderStatusCallback
    {
        public void OnOrderNotification(CustomerOrder order)
        {
            MessageBox.Show("Order Id = " + order.CustomerOrderId + " "
                + "Table Id = " + order.TableId + " "
                + "Status = " + ((OrderStatus)order.Status).ToString(), "Order Status");
        }
    }
}
