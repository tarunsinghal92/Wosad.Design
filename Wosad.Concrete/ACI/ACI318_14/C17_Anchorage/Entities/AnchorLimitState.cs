﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wosad.Concrete.ACI318_11.Anchorage
{
    public enum AnchorLimitState
    {
        AnchorPulloutTension,
        AnchorSteelShear,
        AnchorSteelTension,
        ConcreteBreakoutShear,
        ConcreteBreakoutTension,
        ConcretePryoutShear,
        ConcreteSideFaceBlowoutTension
    }
}