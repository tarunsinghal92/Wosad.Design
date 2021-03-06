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


namespace Wosad.Wood.NDS.NDS_2015
{
    public abstract partial class WoodMember : AnalyticalElement
    {
        //3.3.3 Beam Stability Factor, C_L 


        protected double GetC_L(double b, 
            double d,
            double l_e
            )
        {
            throw new NotImplementedException();
            double C_L = 1.0;
            if (b>=d)
            {
                C_L = 1.0;
            }
            else
            {
                bool isBraced = this.DetermineIfMemberIsLaterallyBraced();
                if (isBraced ==true)
                {
                    C_L = 1.0;
                }
                else
                {
                    double F_bE = GetBendingCriticalBucklingDesignValue(l_e, b,d);
                    double F_bStar = GetF_b_AdjustedForBeamStability();
                    double alpha = F_bE/F_bStar;

                    //NDS eq. (3.3-6) 
                    C_L=((1+alpha) / (1.9))-Math.Sqrt(Math.Pow((((1+alpha) / (1.9))), 2)-((alpha) / (0.95)));

                }
                return C_L;
            }
        }

        private double GetBendingCriticalBucklingDesignValue(double l_e, double b, double d)
        {
           double R_b=Math.Min(Math.Sqrt(((l_e*d) / (b*b))),50); //NDS eq. (3.3-5)
           double E_m_prime = GetModulusOfElasticityForBeamAndColumnStability();
           double F_bE=((1.20*E_m_prime) / (R_b*R_b)); //from section 3.3.3.6
           return F_bE;  
        }


        protected abstract double GetF_b_AdjustedForBeamStability();

        protected abstract double GetModulusOfElasticityForBeamAndColumnStability();

        protected abstract bool DetermineIfMemberIsLaterallyBraced();

    }
}
