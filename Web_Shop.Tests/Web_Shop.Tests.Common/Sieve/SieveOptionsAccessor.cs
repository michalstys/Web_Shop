using Microsoft.Extensions.Options;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Web_Shop.Tests.Common.Sieve
{
    public class SieveOptionsAccessor : IOptions<SieveOptions>
    {
        public SieveOptions Value { get; }

        public SieveOptionsAccessor()
        {
            Value = new SieveOptions()
            {
                ThrowExceptions = true
            };
        }
    }
}
