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
using System.Linq;
using System.Collections.Generic;
using Wosad.Common.Entities;
using Wosad.Common.Section.Interfaces;
using Wosad.Common.Interfaces;



namespace Wosad.Concrete.ACI
{
    public abstract partial class ConcreteSectionLongitudinalReinforcedBase : ConcreteSectionBase, IConcreteSectionWithLongitudinalRebar
    {
        protected virtual SectionAnalysisResult GetSectionResult(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPosition)
        {
            ForceMomentContribution CForceRebarResultant = GetCompressionForceConcreteResultant(StrainDistribution, compFiberPosition);
            ForceMomentContribution CForceConcreteResultant = GetCompressionForceRebarResultant(StrainDistribution, compFiberPosition);
            
            ForceMomentContribution CForceResultant = CForceRebarResultant + CForceConcreteResultant;

            ForceMomentContribution TForceResultant = GetTensionForceResultant(StrainDistribution);
            SectionAnalysisResult result = new SectionAnalysisResult()
            {
                AxialForce = CForceResultant.Force + TForceResultant.Force,
                CForce = CForceResultant.Force,
                TForce = TForceResultant.Force,
                Moment = CForceResultant.Moment+ TForceResultant.Moment,
                Rotation = 0,
                StrainDistribution = StrainDistribution,
                CompressionRebarResults = CForceRebarResultant.RebarResults,
                TensionRebarResults =TForceResultant.RebarResults
            };
            return result;
        }

        protected virtual ForceMomentContribution GetCompressionForceConcreteResultant(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPosition)
        {
            ForceMomentContribution concreteContrib = GetConcreteForceResultant(StrainDistribution, compFiberPosition);

            return concreteContrib;
        }
        protected virtual ForceMomentContribution GetCompressionForceRebarResultant(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPosition)
        {
            ForceMomentContribution steelContrib = GetRebarResultant(StrainDistribution, ResultantType.Compression);

            return steelContrib;
        }

        protected virtual ForceMomentContribution GetTensionForceResultant(LinearStrainDistribution StrainDistribution)
        {
            ForceMomentContribution rebarContribution = GetRebarResultant(StrainDistribution, ResultantType.Tension);
            return rebarContribution;
        }

        ForceMomentContribution GetConcreteForceResultant(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPosition)
        {
            ForceMomentContribution concreteForceResultant = null;

            if (compFiberPosition == FlexuralCompressionFiberPosition.Top)
            {
                if (StrainDistribution.TopFiberStrain< this.MaxConcreteStrain)
                {
                    concreteForceResultant = GetConcreteParabolicStressForceResultant(StrainDistribution);
                }
                else
                {
                    if (StrainDistribution.TopFiberStrain == this.MaxConcreteStrain)
                    {
                        concreteForceResultant = GetConcreteWhitneyForceResultant(StrainDistribution, compFiberPosition);
                    }
                    else
                    {
                        throw new UltimateConcreteStrainExceededException();
                    }
                }
            }
            else
            {
                if (StrainDistribution.BottomFiberStrain < this.MaxConcreteStrain)
                {
                    concreteForceResultant = GetConcreteParabolicStressForceResultant(StrainDistribution);
                }
                else
                {
                    if (StrainDistribution.BottomFiberStrain == this.MaxConcreteStrain)
                    {
                        concreteForceResultant = GetConcreteWhitneyForceResultant(StrainDistribution,compFiberPosition);
                    }
                    else
                    {
                        throw new UltimateConcreteStrainExceededException();
                    }
                    
                }

            }

            return concreteForceResultant;
        }


     

        private IMoveableSection GetCompressedConcreteSection(LinearStrainDistribution StrainDistribution, FlexuralCompressionFiberPosition compFiberPos, double SlicingPlaneOffset)
        {
            IMoveableSection compressedPortion = null;
            ISliceableSection sec = this.Section.SliceableShape as ISliceableSection;

            if (StrainDistribution.TopFiberStrain >= 0 && StrainDistribution.BottomFiberStrain >= 0)
            {
                compressedPortion = this.Section.SliceableShape;
            }
            else
            {
                switch (compFiberPos)
                {
                    case FlexuralCompressionFiberPosition.Top:
                        compressedPortion = sec.GetTopSliceSection(SlicingPlaneOffset, SlicingPlaneOffsetType.Top); 
                        break;
                    case FlexuralCompressionFiberPosition.Bottom:
                        compressedPortion = sec.GetBottomSliceSection(SlicingPlaneOffset,SlicingPlaneOffsetType.Bottom); 
                        break;
                    default:
                        throw new CompressionFiberPositionException();
                }
            }

            return compressedPortion;
        }

        protected bool GetAnalysisResultConverged(SectionAnalysisResult sectionResult, double ConvergenceTolerance)
        {
            double C = Math.Abs(sectionResult.CForce);
            double T = Math.Abs(sectionResult.TForce);

            double MinForce = Math.Min(C, T);
            double ConvergenceToleranceDifference = ConvergenceTolerance * MinForce;

            if (Math.Abs(C - T) <= ConvergenceToleranceDifference)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected enum ResultantType
        {
            Tension,
            Compression
        }
    }
}
