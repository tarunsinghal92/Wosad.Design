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
using Wosad.Common.Entities;

namespace Wosad.Concrete.ACI
{
        public class ConcreteSectionIterationData
        {
            public LinearStrainDistribution PreviousStrainDistribution { get; set; }
            public LinearStrainDistribution StrainDistribution { get; set; }
            public SectionAnalysisResult Result { get; set; }

            public ConcreteSectionIterationData(LinearStrainDistribution StrainDistribution, LinearStrainDistribution PreviousStrainDistribution, 
                SectionAnalysisResult Result)
            {
                this.StrainDistribution = StrainDistribution;
                this.PreviousStrainDistribution = PreviousStrainDistribution;
                this.Result = Result;
            }
        }
    }

