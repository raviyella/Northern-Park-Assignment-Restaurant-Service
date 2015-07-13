using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Xml.Linq;
using System.IO;

namespace RestaurantService.DataAccess.UnitTests
{
    [TestFixture]
    public class DBContextHelperTest
    {
        [Test(Description = "Restaurant Constructor Test")]
        public void RestaurantConstructor_Test()
        {
            IRestaurantContext context = DbContextHelper.GetRestaurantContext();
            NUnit.Framework.Assert.IsNotNull(context);
        }

        [Test(Description = "Restaurant Constructor Multiple Instances Test")]
        public void Restaurant_MultipleInstancesTest()
        {
            IRestaurantContext context = DbContextHelper.GetRestaurantContext();
            IRestaurantContext contextSecond = DbContextHelper.GetRestaurantContext();
            NUnit.Framework.Assert.IsTrue(context == contextSecond);
        }

        [Test(Description = "XMLHelper Constructor Test")]
        public void XMLHelperConstructor_Test()
        {
            XDocument xDocument = XMLHelper.GetXDocument();
            NUnit.Framework.Assert.IsNotNull(xDocument);
        }

        [Test(Description = "XMLHelper Multiple Instances Test")]
        public void XMLHelper_MultipleInstancesTest()
        {
            XDocument xDocument = XMLHelper.GetXDocument();
            XDocument xDocumentSecond = XMLHelper.GetXDocument();
            NUnit.Framework.Assert.IsTrue(xDocument == xDocumentSecond);
        }

        [Test(Description = "Save XML Test")]
        public void SaveXML_Test()
        {
            int CustomerID = 1;
            string TableNumber = "T01";
            TimeSpan timeSpan = DateTime.Now.AddHours(1).Subtract(DateTime.Now);
            XMLHelper.SaveToXML(CustomerID, TableNumber, timeSpan);
        }

        [Test(Description = "Invalid XML path Test")]
        [NUnit.Framework.ExpectedException(typeof(FileNotFoundException))]
        public void SaveXML_InvalidXMLPathTest()
        {
            int CustomerID = 1;
            string TableNumber = "T01";
            TimeSpan timeSpan = DateTime.Now.AddHours(1).Subtract(DateTime.Now);
            XMLHelper.SaveToXML(CustomerID, TableNumber, timeSpan);
        }
    }
}
