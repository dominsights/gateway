using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Mapper
{
    public class MapperConfigurationFactory
    {
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return new AutoMapper.MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PaymentProfile>();
                });
            }
        }
    }
}
