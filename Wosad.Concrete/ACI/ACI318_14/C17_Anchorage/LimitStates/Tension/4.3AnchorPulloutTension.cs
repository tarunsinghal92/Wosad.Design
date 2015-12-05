﻿using Wosad.Concrete.ACI318_11.Anchorage.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wosad.Concrete.ACI318_11.Anchorage.LimitStates
{
    public class AnchorPulloutTension : AnchorageConcreteLimitState
    {
        //The pullout strength equations given in 17.4.3.4 and 17.4.3.5 are only applicable to cast-in headed and hooked anchors ; 
        //they are not applicable to expansion and undercut anchors that use various mechanisms for end anchorage 
        //unless the validity of the pullout strength equations are verified by tests.



        public bool IsHookedBolt { get; set; }
        public double d_a { get; set; }
        /// <summary>
        /// Cracked or uncracked concrete condition.
        /// </summary>
        public ConcreteCrackingCondition ConcreteCondition { get; set; }

        /// <summary>
        /// Distance from the inner surface of the shaft of a J- or L-bolt to the outer tip of the J- or L-bolt.
        /// </summary>
        public double e_h { get; set; }

        /// <summary>
        /// net bearing area of the head of stud, anchor bolt, or headed deformed bar
        /// </summary>
        public double A_brg { get; set; }

        /// <summary>
        /// Anchor category (1,2, or 3) replecting the sensitivity to installation and relaibility, per ACI 355.2 testing
        /// </summary>
        public AnchorReliabilityAndSensitivityCategory AnchorCategory { get; set; }

        /// <summary>
        /// Presence of supplementary reinforcement (Condition A or B) per ACI 318.
        /// </summary>
        SupplementaryReinforcmentCondition Condition { get; set; }

        public AnchorPulloutTension
            (int n, double h_eff, bool IsHookedBolt, double d_a, double e_h, double A_brg,
            AnchorReliabilityAndSensitivityCategory AnchorCategory, AnchorInstallationType AnchorType,
            SupplementaryReinforcmentCondition Condition,
            ConcreteCrackingCondition ConcreteCondition
            )
            : base(n, h_eff, AnchorType)
            {
                this.IsHookedBolt = IsHookedBolt;
                this.d_a = d_a;
                this.e_h = e_h;
                this.A_brg = A_brg;
                this.AnchorCategory = AnchorCategory;
                this.ConcreteCondition = ConcreteCondition;
                this.AnchorType = AnchorType;
                this.Condition = Condition;
            }

        double Np_n;

        public override double GetNominalStrength()
        {

                double gamma_c_P = GetGamma_cp(ConcreteCondition);
                double Np = GetNp();

                //17.4.3.1
                Np_n = Np * gamma_c_P;
                return Np_n;

        }
        public double GetDesignStrength(bool IsSeismicApplication = true)
        {
            if (Np_n == 0)
            {
                Np_n = GetNominalStrength();
            }

            double phi = 0.0;
            if (AnchorType == AnchorInstallationType.CastInPlace)
	        {
                phi = StrengthReductionFactorCastIn.GetStrengthReductionFactorForConcrete(Condition, AnchorCategory, AnchorLoadType.Tension);
	        }
            if (IsSeismicApplication == true)
            {
                //17.2.3.4.4  case (c)
                return 0.75* phi * Np_n;
            }
            else
            {
                return phi * Np_n;
            }
            
        }

        private double GetGamma_cp(ConcreteCrackingCondition ConcreteCondition)
        {

            double gamma_cp = 1.0;
            if (ConcreteCondition == ConcreteCrackingCondition.Cracked)
            {
                gamma_cp = 1.0;
            }
            else
            {
                gamma_cp = 1.4;
            }

            return gamma_cp;
        }

        /// <summary>
        /// The pullout strength in tension of a single headed stud or headed bolt (per 17.4.3.4 )
        /// </summary>
        /// <returns></returns>
        private double GetNp()
        {
            double Np = 0.0;
            if (IsHookedBolt == true)
            {
                Np = 0.9*fc*d_a*Math.Max(3.0*d_a,Math.Min(e_h,4.5*d_a));
            }
            else
	        {
                //studded bolt
                Np = (A_brg*8*fc)/1000;
	        }
            return Np;
        }


    }
}