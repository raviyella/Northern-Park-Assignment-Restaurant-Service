using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestaurantService.DataAccess
{
    /// <summary>
    /// DbContext Helper
    /// </summary>
    public class DbContextHelper
    {
        private static IRestaurantContext restaurantContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        DbContextHelper() 
        { }

        /// <summary>
        /// Method to retrieve context instance of RestaurantContext.
        /// </summary>
        /// <returns>instance of restaurantcontext</returns>
        public static IRestaurantContext GetRestaurantContext()
        {
            if (null == restaurantContext)
            {
                restaurantContext = new RestaurantContext();
            }

            return restaurantContext;
        }

        /// <summary>
        /// Method to Retrieve the instance of Fake Restaurant Context
        /// </summary>
        /// <param name="context">restaurant context</param>
        /// <returns>instance of fake restaurantcontext</returns>
        public static IRestaurantContext GetRestaurantContext(IRestaurantContext context)
        {
            restaurantContext = context;
            return restaurantContext;
        }
    }
    /// <summary>
    /// XMLHelper class to load and save XML.
    /// </summary>
    public class XMLHelper
    {
        private static XDocument xDocument;

        /// <summary>
        /// Default constructor
        /// </summary>
        XMLHelper() 
        { }

        /// <summary>
        /// Instantiate XDocument object and creates XML file, if not instantiated.
        /// </summary>
        /// <returns>instance of XDocument</returns>
        public static XDocument GetXDocument()
        {
            if (null == xDocument)
            {
                xDocument = new XDocument(
                        new XDeclaration("1.0", "UTF-16", null),
                        new XElement("CustomerOrders", ""));

                xDocument.Save(@"C:\CustomerOrders.xml");
            }

            return xDocument;
        }

        /// <summary>
        /// Save Completion order details to XML.
        /// </summary>
        /// <param name="CustomerOrderId">Customer Id</param>
        /// <param name="TableNumber">Table Number</param>
        /// <param name="timeSpan">Duration</param>
        public static void SaveToXML(int CustomerOrderId, string TableNumber, TimeSpan timeSpan)
        {

            XElement xElement = XElement.Load(@"C:\CustomerOrders.xml");
            xElement.Add(new XElement("Order",
                                                new XAttribute("ID", CustomerOrderId),
                                                new XElement("Table",
                                                new XAttribute("ID", TableNumber),
                                                new XAttribute("Duration", timeSpan.ToString()))));

            xElement.Save(@"C:\CustomerOrders.xml");
        }
    }
}
