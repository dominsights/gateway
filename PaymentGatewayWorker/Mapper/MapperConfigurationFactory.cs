using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Mapper
{
    public class MapperConfigurationFactory
    {
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PaymentProfile>();
                });
            }
        }
    }
}
