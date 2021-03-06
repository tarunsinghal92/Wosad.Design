#region Copyright
   /*Copyright (C) 2015 Wosad Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wosad.Concrete.ACI;
using Wosad.Common.Entities;
using Wosad.Common.Reports; 
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces;

namespace Wosad.Concrete.ACI318_14
{
    public partial class Development : AnalyticalElement, IDevelopment
    {
//          Reduction in ld may be permitted by the ratio [(As required)/(As provided)] when excess reinforcement is provided
//          in a flexural member. Note that this reduction does not apply when the full fy development is required, as
//          for tension lap splices in 7.13, 12.15.1, and 13.3.8.5, development of positive moment reinforcement at supports
//          in 12.11.2, and for development of shrinkage and temperature reinforcement according to 7.12.2.3. Note also
//          that this reduction in development length is not permitted for reinforcement in structures located in regions of
//          high seismic risk or for structures assigned to high seismic performance or design categories (see 21.11.7.3 and
//          R21.11.7.3).


           
        public double CheckExcessReinforcement(double ld, bool IsTensionReinforcement, bool IsHook)
        {
            if (ExcessFlexureReinforcementRatio <= 1.0)
            {
                if (ExcessFlexureReinforcementRatio < 1.0)
                {
                    double AsReqdToAsProvided = excessFlexureReinforcementRatio;

                    if (IsTensionReinforcement!=true)
                    {
                            throw new Exception("Hooks should not be used to develop rebar in compression");

                    }
                    ld = excessFlexureReinforcementRatio * ld;
                }
            }
            else
            {
                throw new Exception("Exceess reinforcement ratio cannot be more than 1.0");

            }
            return ld;
        }
    }
}
