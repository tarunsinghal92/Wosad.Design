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
using Wosad.Common.Interfaces;
using Wosad.Common.Mathematics;

namespace Wosad.Steel.AISC.SteelEntities.Bolts
{
    public class BoltGroupGeneral:ConnectionGroup
    {
        public List<ILocationArrayElement>  Bolts {get; set;}


        public BoltGroupGeneral(List<ILocationArrayElement> Bolts)
        {
           this.Bolts =Bolts;
        }

        public BoltGroupGeneral()
        {

        }

        /// <summary>
        /// Calculates the capacity based on the combination of design shear and axial forces as well as moment, given single bolt shear strength
        /// </summary>
        /// <param name="V_u">Ultimate shear or axial force in the entire bolt group</param>
        /// <param name="M_u">Ultimate moment in the entire bolt group</param>
        /// <param name="phi_Rn"></param>
        /// <returns></returns>
       public Force GetForceAndMomentCapacity(double V_u, double P_u, double M_u, double phi_Rn)
        {

            Force Capacity = null;
            double phi_Mn;
            double C;

            if (P_u==0 && V_u==0)
            {
                //If forcesare zero
                //then elastic method is used
                C = this.CalculateElasticGroupMomentCoefficientC();
                phi_Mn = phi_Rn * C;

                Capacity = new Force(0, 0, 0, 0, 0, phi_Mn);
            }
            else
            {
                //BoltGroupGeneral adjustedGroup = GetAdjustedGroup();
                //double R = GetForceResultant(V_u, H_u);
                //double ex; //todo:

                //double AngleOfLoad = GetAngle(V_u, H_u);
                //C = adjustedGroup.FindUltimateStrengthCoefficient(e_x, AngleOfLoad);
                //double phi_Rn_Group = phi_Rn * C;

            }
            throw new NotImplementedException();
        }

        protected override double GetElementForce(ILocationArrayElement el, Point2D Center, ILocationArrayElement furthestBolt, double angle)
        {
            double Delta_u = furthestBolt.LimitDeformation;
            double LiMax = furthestBolt.GetDistanceToPoint(Center);
            double xi = el.Location.X - Center.X;
            double yi = el.Location.Y - Center.Y;
            double ri = Math.Sqrt(xi * xi + yi * yi);                   //radial distance from center to this element
            double Delta = Delta_u * ri / LiMax;                        //this bolt deformation
            double iRn = Math.Pow(1 - Math.Exp(-10.0 * Delta), 0.55);   //force developed by this element
            return iRn;
        }

        protected override List<ILocationArrayElement> GetICElements()
        {
            return Bolts;
        }

        private double FindLargestElementDistanceFromCenter(Point2D Center)
        {
            var MaxDistance = Bolts.Max(b => b.GetDistanceToPoint(Center));
            return MaxDistance;
        }

        protected override ILocationArrayElement FindUltimateDeformationElement(Point2D Center)
        {
            double LiMax  = FindLargestElementDistanceFromCenter(Center);
            double DeltaMax = 0.34;
            var ControllingBolt = Bolts.Where(b => b.GetDistanceToPoint(Center) == LiMax).FirstOrDefault();
            ControllingBolt.LimitDeformation = DeltaMax;
            return ControllingBolt;
        }
    }
}