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
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Steel.AISC.AISC360v10.Connections.Bolted;

namespace Wosad.Steel.AISC.AISC360v10.Connections.Bolted
{

    public class BoltBearingThreadedGeneral : BoltBearing
    {
        public BoltBearingThreadedGeneral(double Diameter, BoltThreadCase ThreadType, IBoltMaterial material,
            ICalcLog log=null)
            : base(Diameter, ThreadType, log)
        {
            this.material = material;
            nominalTensileStress = material.GetNominalTensileStress(ThreadType);
            nominalShearStress = material.GetNominalShearStress(ThreadType);

        }

        IBoltMaterial material;

        private double nominalTensileStress;

        public override double NominalTensileStress
        {
            get { return nominalTensileStress; }
        }

        private double nominalShearStress;

        public override double NominalShearStress
        {
            get { return nominalShearStress; }
        }

    }
}
