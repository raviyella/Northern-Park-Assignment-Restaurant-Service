using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using RestaurantService.Contracts;

namespace RestaurantService.Host
{
    /// <summary>
    /// Restaurant Servicehost class
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost restaurantServiceHost = null;
            try
            {
                ////Base Address
                Uri baseAddress = new Uri("net.tcp://localhost:4500/RestaurantService");

                ////Instantiate ServiceHost
                restaurantServiceHost = new ServiceHost(typeof(RestaurantService), baseAddress);

                ////Add Endpoint to ServiceHost
                restaurantServiceHost.AddServiceEndpoint(typeof(IChefService), new NetTcpBinding(), "net.tcp://localhost:4501/ChefService");
                restaurantServiceHost.AddServiceEndpoint(typeof(IWaiterService), new NetTcpBinding(), "net.tcp://localhost:4502/WaiterService");

                ////Open the ServiceHost to start listening
                restaurantServiceHost.Open();

                Console.WriteLine("Restaurant Service is live now @ {0} : {1}", baseAddress, DateTime.Now);
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.ReadLine();

                ////Close the ServiceHost
                restaurantServiceHost.Close();
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine(timeProblem.Message);

                if (restaurantServiceHost != null)
                {
                    restaurantServiceHost.Abort();
                }

                Console.ReadLine();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine(commProblem.Message);

                if (restaurantServiceHost != null)
                {
                    restaurantServiceHost.Abort();
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (restaurantServiceHost != null)
                {
                    restaurantServiceHost.Abort();
                }

                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("Stopping the Restaurant Service, Please Wait...");

                if ((restaurantServiceHost != null) && (restaurantServiceHost.State != CommunicationState.Closed))
                {
                    restaurantServiceHost.Close();
                }
            }
        }
    }
}
