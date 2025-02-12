using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTS.WSApi.Enums
{
    public enum XTSBookType
    {
        None = 0,
        Regular = 1,
        SpecialTerms = 2,
        StopLoss = 3,
        Negotiated = 4,
        OddLot = 5,
        Spot = 6,
        Auction = 7,
        CallAuction1 = 11,
        CallAuction2 = 12,
    }
}
