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
using Wosad.Common.Entities; 
using Wosad.Common.Section.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
 using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces; 

using Wosad.Common.Section.SectionTypes;
using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.SteelEntities;
using Wosad.Steel.AISC.AISC360v10.General.Compactness;


namespace Wosad.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamCircularHss : FlexuralMemberChsBase
    {
        public BeamCircularHss(ISteelSection section, ICalcLog CalcLog)
            : base(section, CalcLog)
        {
            if (section is ISectionPipe)
            {
                SectionPipe = section as ISectionPipe;
            }
            else
            {
                throw new SectionWrongTypeException(typeof(ISectionTube));
            }
                
            
            GetSectionValues();
        }

       // This section applies to round HSS





        #region Limit States

        public virtual SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(GetYieldingMomentCapacity(), true);

            return ls;
        }



        public SteelLimitStateValue GetFlexuralWebOrWallBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {

            SteelLimitStateValue ls = new SteelLimitStateValue();

            ShapeCompactness.HollowMember Compactness = new ShapeCompactness.HollowMember(Section, CompressionLocation, MomentAxis.XAxis);
            CompactnessClassFlexure cClass = Compactness.GetWebCompactnessFlexure();

            if (cClass == CompactnessClassFlexure.Compact)
            {
                ls = new SteelLimitStateValue(-1, false);

            }
            else
            {
                double phiM_n = GetLocalBucklingCapacity();
                ls = new SteelLimitStateValue(phiM_n, true);
            }
            return ls;
        }



        #endregion







        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            F_y = Section.Material.YieldStress;

            D = SectionPipe.D;
            t = SectionPipe.t_des;

        }

        double E;
        double F_y;

        double D;
        double t;

    }
}
