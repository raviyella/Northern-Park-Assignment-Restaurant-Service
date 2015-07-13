using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestaurantService.Contracts;

namespace RestaurantWaiterClient
{
    public partial class OrderForm : Form
    {
        public OrderForm(List<CustomerOrder> orders)
        {
            this.InitializeComponent();

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
    }
}
