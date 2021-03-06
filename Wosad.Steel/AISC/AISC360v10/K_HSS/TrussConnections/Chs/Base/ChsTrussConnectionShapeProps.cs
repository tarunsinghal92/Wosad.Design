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
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.SteelEntities;
using Wosad.Steel.AISC.SteelEntities.Sections;
using Wosad.Common.Mathematics;
using Wosad.Steel.AISC.AISC360v10.K_HSS.TrussConnections;


namespace  Wosad.Steel.AISC.AISC360v10.HSS.TrussConnections
{

    public abstract partial class ChsTrussBranchConnection:  HssTrussConnection, IHssTrussBranchConnection
    {

        public double theta { get; set; }

        private double _sin_theta;

        public double sin_theta
        {
            get {
                _sin_theta = Math.Sin(theta.ToRadians());
                return _sin_theta; }
            set { _sin_theta = value; }
        }
        
        /// <summary>
        /// Width ratio  B_b/B
        /// </summary>
        private double _beta;

        protected double beta
        {
            get {
                _beta = Get_beta();
                return _beta; }
            set {
                
                _beta = value; }
        }

        private double Get_beta()
        {
           // return B_b / B;
            throw new NotImplementedException();
        }

        private double _gamma;

        /// <summary>
        /// Chord slenderness ratio
        /// </summary>
        protected double gamma
        {
            get {
                _gamma = Get_gamma();
                return _gamma; }
            set { _gamma = value; }
        }

        private double Get_gamma()
        {
            throw new NotImplementedException();
        }






        private double _t;

        protected double t
        {
            get
            {
                _t = Chord.Section.t_des;
                return _t;
            }
            set { _t = value; }
        }


        protected override double GetF_y()
        {
            return Chord.Material.YieldStress; 
        }

        protected override double GetF_yb()
        {
            return MainBranch.Material.YieldStress;
        }






        private double _t_b;

        public double t_b
        {
            get {

                SteelChsSection br = getBranch();
                _t_b = br.Section.t_des;
                return _t_b; }
            set { _t_b = value; }
        }


        private double _beta_eop;

        public double beta_eop
        {
            get {
                _beta_eop = Get_beta_eop();
                return _beta_eop; }
            set { _beta_eop = value; }
        }
        
        public double Get_beta_eop()
        {
            double beta_eop = ((5 * beta) / (gamma));
            beta_eop = beta_eop > beta ? beta : beta_eop;
            return beta_eop;
        }

        private double _E;

        public double E
        {
            get {
                _E = SteelConstants.ModulusOfElasticity;
                return _E; }
            set { _E = value; }
        }
        

        
        protected abstract SteelChsSection getBranch();

    }


}
