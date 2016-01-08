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
////using Wosad.Analytics.Section;
 
 
using Wosad.Common.Mathematics;
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities.Sections;



namespace  Wosad.Steel.AISC360_10.HSS.ConcentratedForces
{
    public class ChsTransversePlateTandXAxial : ChsToPlateConnection
    {
        private double angle;

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public ChsTransversePlateTandXAxial(SteelChsSection Hss, SteelPlateSection Plate, double Angle, ICalcLog CalcLog)
            : base(Hss, Plate, CalcLog)
        {
            this.angle = Angle;
        }

        double GetAvailableStrength(bool ConnectingSurfaceInTension, double UtilizationRatio)
        {
            double R = 0.0;
            R = HssLocalYieldingLS(UtilizationRatio, ConnectingSurfaceInTension);
            return R;
        }

        double GetAvailableStrength(bool ConnectingSurfaceInTension, double RequiredAxialStrenghPro, double RequiredMomentStrengthMro)
        {
            ISteelSection s = GetHssSteelSection();
            double U = GetUtilizationRatio(s, RequiredAxialStrenghPro, RequiredMomentStrengthMro);
            return this.GetAvailableStrength( ConnectingSurfaceInTension, U);
        }

        double GetOutOfPlaneMomentForPlateBending(double UtilizationRatio,  bool ConnectingSurfaceInTension)
        {
            double M = 0.0;
            double Bp = Plate.Section.H;
            double R = HssLocalYieldingLS(UtilizationRatio,ConnectingSurfaceInTension);
            M = 0.5 * Bp * R; //note R already contains reduction factors

            return M;
        }

        double GetInPlaneMomentForPlateBending()
        {
            return 0.0;
        }

        internal double HssLocalYieldingLS(double UtilizationRatio, bool ConnectingSurfaceInTension )
        {
            double R = 0.0;

            double theta = this.angle;
            double sinTheta = Math.Sin(theta.ToRadians());

            double Fy = 0.0; double t = 0.0; double Bp = 0.0; double D = 0.0; double tp = 0.0;
            this.GetTypicalParameters(ref Fy, ref t, ref Bp, ref D, ref tp);
            
            
            double Qf = GetChordStressInteractionQf(UtilizationRatio, ConnectingSurfaceInTension);
            //(K1-1)
            double Rn = (Fy * Math.Pow(t, 2) * (5.5 / (1.0 - 0.81 * Bp / D)) * Qf)/sinTheta;

                R = 0.90 * Rn;


            return R;
        }

    }
}
