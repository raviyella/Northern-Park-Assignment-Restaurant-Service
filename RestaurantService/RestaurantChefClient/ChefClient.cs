using RestaurantService.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;

namespace RestaurantChefClient
{
    public partial class ChefClient : Form
    {
        EndpointAddress chefServiceAddress;
        ChannelFactory<IChefService> chefServiceFactory;
        IChefService chefClient;

        System.Windows.Forms.Timer timer;

        public ChefClient()
        {
            this.InitializeComponent();

            this.InitializeChefChannel();
        }

        /// <summary>
        /// Method to channel factory for Chef Service
        /// </summary>
        private void InitializeChefChannel()
        {
            try
            {
                this.chefServiceAddress = new EndpointAddress(new Uri("net.tcp://localhost:4501/ChefService"));
                this.chefServiceFactory = new ChannelFactory<IChefService>(
                    new NetTcpBinding(),
                    this.chefServiceAddress);
                this.chefClient = this.chefServiceFactory.CreateChannel();
                this.timer = new System.Windows.Forms.Timer();
                this.timer.Interval = 15000;
                this.timer.Tick += new EventHandler(this.OrderStatus_TimerElapsed);
                this.timer.Start();
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show(ex.Message, "FaultException", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CommunicationException ex)
            {
                ((IClientChannel)chefClient).Abort();
                this.chefClient = null;
                MessageBox.Show(ex.Message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.chefServiceFactory.Abort();
                System.Environment.Exit(1);
            } 
        }

        /// <summary>
        /// Method that triggers on closing of Form.
        /// </summary>
        /// <param name="e"> Closing Event data</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                return;
            }

            // Confirm User wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Client UI Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    this.CleanUp();
                    break;
            }
        }

        /// <summary>
        /// Method to Clean up.
        /// </summary>
        private void CleanUp()
        {
            try
            {
                this.timer.Stop();
                ((IClientChannel)chefClient).Close();
                chefClient = null;
                chefServiceFactory.Close();
                chefServiceFactory = null;
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine(commProblem.Message);

                if (chefServiceFactory != null)
                {
                    chefServiceFactory.Abort();
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (chefServiceFactory != null)
                {
                    chefServiceFactory.Abort();
                }

                Console.ReadLine();
            }
            finally
            {
                if (chefClient != null)
                {
                    ((IClientChannel)chefClient).Close();
                }

                if (chefServiceFactory != null)
                {
                    chefServiceFactory.Abort();
                }
            }
        }

        /// <summary>
        /// Timer to get the status of all orders
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        public void OrderStatus_TimerElapsed(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                List<CustomerOrder> orders = this.chefClient.GetNewOrders();

                foreach (CustomerOrder order in orders)
                {
                    ListViewItem lvm1 = new ListViewItem(order.CustomerOrderId.Trim());
                    lvm1.SubItems.Add(order.TableId);
                    lvm1.SubItems.Add(order.StartTime.ToLongTimeString());
                    if (DateTime.Compare(order.CompletionTime, order.StartTime) > 0)
                    {
                        lvm1.SubItems.Add(order.CompletionTime.ToLongTimeString());
                        TimeSpan timeSpan = order.CompletionTime.Subtract(order.StartTime);
                        lvm1.SubItems.Add(timeSpan.ToString(@"hh\:mm\:ss"));
                    }
                    else
                    {
                        lvm1.SubItems.Add("00:00:00");
                        lvm1.SubItems.Add("00:00:00");
                    }

                    lvm1.SubItems.Add(((OrderStatus)order.Status).ToString());
                    listView1.Items.Add(lvm1);
                }
            }
            catch (FaultException<RestaurantService.Contracts.ServiceFaultDetails> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.ErrorMessage);
                this.timer.Stop();
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show("Communication Error: " + ex.Message);
                this.timer.Stop();
            }
        }

        /// <summary>
        /// Click event to Get All Order Details
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event data</param>
        private void BtnGetOrder_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                List<CustomerOrder> orders = this.chefClient.GetNewOrders();
                if (orders.Count == 0)
                {
                    MessageBox.Show("No Orders Available");
                }

                foreach (CustomerOrder order in orders)
                {
                    ListViewItem lvm1 = new ListViewItem(order.CustomerOrderId.Trim());
                    lvm1.SubItems.Add(order.TableId);
                    lvm1.SubItems.Add(order.StartTime.ToLongTimeString());
                    if (DateTime.Compare(order.CompletionTime, order.StartTime) > 0)
                    {
                        lvm1.SubItems.Add(order.CompletionTime.ToLongTimeString());
                        TimeSpan timeSpan = order.CompletionTime.Subtract(order.StartTime);
                        lvm1.SubItems.Add(timeSpan.ToString(@"hh\:mm\:ss"));
                    }
                    else
                    {
                        lvm1.SubItems.Add("00:00:00");
                        lvm1.SubItems.Add("00:00:00");
                    }

                    lvm1.SubItems.Add(((OrderStatus)order.Status).ToString());
                    listView1.Items.Add(lvm1);
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
        /// Click event to Approve the Order
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event data</param>
        private void BtnApproveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string retStatus = string.Empty;
                    string orderId = listView1.SelectedItems[0].SubItems[0].Text;
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        retStatus = this.chefClient.ApproveOrder(orderId);
                        if (retStatus == "Approved")
                        {
                            listView1.SelectedItems[0].SubItems[5].Text = "Approved";
                        }

                        MessageBox.Show(retStatus);
                    }
                }
                else
                {
                    MessageBox.Show("Please select the Order to Approve");
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
        /// Click event to Decline the Order
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event data</param>
        private void BtnDeclineOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string retStatus = string.Empty;
                    string orderId = listView1.SelectedItems[0].SubItems[0].Text;
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        retStatus = this.chefClient.DeclineOrder(orderId);
                        if (retStatus == "Declined")
                        {
                            listView1.SelectedItems[0].SubItems[5].Text = "Declined";
                        }

                        MessageBox.Show(retStatus);
                    }
                }
                else
                {
                    MessageBox.Show("Please select the Order to Decline");
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
        /// Click event Mark the Order as Complete.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event data</param>
        private void BtnOrderReady_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string retStatus = string.Empty;
                    string orderId = listView1.SelectedItems[0].SubItems[0].Text;
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        retStatus = this.chefClient.MarkOrderAsComplete(orderId);
                        if (retStatus == "Ready")
                        {
                            listView1.SelectedItems[0].SubItems[5].Text = "Ready";
                        }

                        MessageBox.Show(retStatus);
                    }
                }
                else
                {
                    MessageBox.Show("Please select the Order to Ready");
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
    }
}
