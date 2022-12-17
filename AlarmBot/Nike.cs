using System;
using System.Collections.Generic;
using System.Linq;

namespace AlarmBot
{
    public class Nike : Brand
    {
        public Nike(EBrand brand, string url)
            : base(brand, url)
        {
        }

        public override int GetNewProduct()
        {
            return 0;
        }
    }
}
