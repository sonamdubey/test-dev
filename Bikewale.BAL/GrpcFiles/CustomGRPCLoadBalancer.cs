using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GRPCLoadBalancer
{      
    static class CustomGRPCLoadBalancerWithSingleton
    {
        private static Channel ch;
        static string serverList = ConfigurationManager.AppSettings["GrpcArticleServerList"];
        static CustomGRPCLoadBalancerWithSingleton()
        {           
            ch = new Channel(serverList, ChannelCredentials.Insecure);           
        }

        
        internal static Channel GetWorkingChannel()
        {         
            return ch;
           //return new Channel(serverList1, ChannelCredentials.Insecure);
        }

    }
    
}