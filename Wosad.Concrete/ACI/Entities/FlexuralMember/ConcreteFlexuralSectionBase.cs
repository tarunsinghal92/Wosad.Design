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
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Entities;
using Wosad.Common.Section.Interfaces;

 

namespace Wosad.Concrete.ACI
{
    /// <summary>
    /// Base class used for calculating nominal flexural strength. Utilized by beam or column sections
    /// </summary>
    public abstract partial class ConcreteFlexuralSectionBase : ConcreteSectionLongitudinalReinforcedBase, IConcreteFlexuralMember
    {

        public ConcreteFlexuralSectionBase(IConcreteSection Section, List<RebarPoint> LongitudinalBars, ICalcLog log, double ConvergenceToleranceStrain = 0.000002)
            : base(Section, LongitudinalBars, log)
        {
            this.ConvergenceToleranceStrain = ConvergenceToleranceStrain;

        }

        double ConvergenceToleranceStrain;



        public IStrainCompatibilityAnalysisResult GetNominalFlexuralCapacity
            (FlexuralCompressionFiberPosition CompressionFiberPosition )
        {


            //Step 1: Assume strain distribution with all bars below section centroid yielding
            LinearStrainDistribution TrialStrainDistribution =  GetInitialStrainEstimate(CompressionFiberPosition);
            SectionAnalysisResult TrialSectionResult = GetSectionResult(TrialStrainDistribution, CompressionFiberPosition);
            double Mn = 0;

                //check id T and C force are equal

                //if T<>C
                if (GetAnalysisResultConverged(TrialSectionResult, ConvergenceToleranceStrain) == false)
                {
                    SectionAnalysisResult IteratedResult = null;
                    try
                    {
                        TCIterationBound bound = GetSolutionBoundaries(TrialSectionResult, TrialStrainDistribution); //make sure solution exists
                        IteratedResult = FindPureMomentResult(CompressionFiberPosition,bound, ConvergenceToleranceStrain);
                        RebarPointResult controllingBar = GetMaxSteelStrainPoint(TrialSectionResult.TensionRebarResults);
                        Mn = IteratedResult.Moment;
                        return new ConcreteSectionFlexuralAnalysisResult(Mn, IteratedResult.StrainDistribution,controllingBar);
                    }
                    catch (SectionAnalysisFailedToConvergeException)
                    {
                        throw new SectionFailedToConvergeException();
                    }
                    
                }
                    //if T=C
                else
                {

                    IStrainCompatibilityAnalysisResult result = GetResult(TrialSectionResult);
                    return result;
                }

                    
        }

        protected IStrainCompatibilityAnalysisResult GetResult(SectionAnalysisResult SectionResult)
        {
                    RebarPointResult controllingBar = GetMaxSteelStrainPoint(SectionResult.TensionRebarResults);
                    double MaxSteelTensionStrain = controllingBar.Strain;
                    double Mn = SectionResult.Moment;
                    IStrainCompatibilityAnalysisResult result = new ConcreteSectionFlexuralAnalysisResult(Mn, SectionResult.StrainDistribution, controllingBar);
                    return result;
        }

        private RebarPointResult GetMaxSteelStrainPoint(List<RebarPointResult> rebarResult)
        {
            RebarPointResult maxStrainPoint= null;
            double MaxSteelStrain = double.NegativeInfinity;
            foreach (var bar in rebarResult)
            {
                if (bar.Strain >=MaxSteelStrain)
	        {
		         maxStrainPoint = bar;
	        }
                    }
                    return maxStrainPoint;
                }


    }
}
