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
using Wosad.Common.Entities; 
using Wosad.Common.Section.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities;


namespace Wosad.Steel.AISC.AISC360v10.Compression
{
    public partial class CompressionMemberRectangle: ColumnDoublySymmetric
    {

            //    public CompressionMemberRectangle(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog) :
            //base(Section, L_x,L_y,K_x,K_y, CalcLog)

        public CompressionMemberRectangle(ISteelSection Section, double L_x, double L_y, double L_z,  ICalcLog CalcLog) :
            base(Section, L_x,L_y, L_z, CalcLog)
        {

        }
        public override double GetReductionFactorForStiffenedElementQa(double Fcr)
        {
            return 1.0; //local buckling reduction factor is not applicable
        }

        public override double GetReductionFactorForUnstiffenedElementQs()
        {
            return 1.0; //local buckling reduction factor is not applicable
        }



        public override double CalculateCriticalStress()
        {
            double FeFlexuralBuckling = GetFlexuralElasticBucklingStressFe(); 
            double FcrFlexuralBuckling = GetCriticalStressFcr(FeFlexuralBuckling, 1.0);
            return FcrFlexuralBuckling;
        }

        public override SteelLimitStateValue GetFlexuralBucklingStrength()
        {
            throw new NotImplementedException();
        }

        public override SteelLimitStateValue GetTorsionalAndFlexuralTorsionalBucklingStrength()
        {
            throw new NotImplementedException();
        }
    }
}
