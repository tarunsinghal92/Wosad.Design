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
using Wosad.Steel.AISC.Entities;
using Wosad.Steel.AISC.Interfaces;

namespace  Wosad.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public abstract partial class RhsTrussBranchConnection
    {
        private double _Q_f;

        public double Q_f
        {
            get {
                _Q_f = GetQ_f();
                return _Q_f; }
            set { _Q_f = value; }
        }

        private double GetQ_f()
        {
            BranchForceType ForceType = ForceTypeMain;
            if (ForceType == BranchForceType.Tension)
            {
                return 1.0;
            }
            else
            {
                return GetQ_fInCompression();
            }
        }

        protected abstract double GetQ_fInCompression();

        private double _U;

        public double U
        {
            get {
                _U = GetU();
                return _U; }
            set { _U = value; }
        }

        private double GetU()
        {
            double A_g = Chord.Section.A;
            double S = Math.Min(Chord.Section.S_xBot, Chord.Section.S_xTop);
            double U = (((P_uChord) / (F_y*A_g)) + ((M_uChord) / (F_y * S)));
            return U;
        }
        
        
    }
}
