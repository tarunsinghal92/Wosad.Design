﻿#region Copyright
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
using System.Threading.Tasks;
using Wosad.Steel.AISC.SteelEntities;

namespace Wosad.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElement : SteelDesignElement
    {
        public double GetMinimumThicknessOfMaterialSupportingFilletWeld(double w_weld, double F_u)
        {
           double t_min = ((3.09 * w_weld) / (16 * F_u));
           return t_min;
        }
    }
}
