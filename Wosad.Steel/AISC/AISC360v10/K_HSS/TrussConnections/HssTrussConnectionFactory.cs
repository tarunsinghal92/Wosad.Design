﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.AISC360v10.K_HSS.TrussConnections;
using Wosad.Steel.AISC.Entities;
using Wosad.Steel.AISC.SteelEntities.Materials;
using Wosad.Steel.AISC.SteelEntities.Sections;

namespace Wosad.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public class HssTrussConnectionFactory
    {
        public IHssTrussBranchConnection GetConnection(HssTrussConnectionMemberType MemberType,
            HssTrussConnectionClassification Classification, ISectionHollow ChordSection, ISectionHollow MainBranchSection,
            ISectionHollow SecondaryBranchSection, double F_yChord, double F_yBranch,
            double thetaMainBranch, double thetaSecondaryBranch, BranchForceType ForceTypeMainBranch, 
            BranchForceType ForceTypeSecondaryBranch, bool IsTensionChord,
            double P_uChord, double M_uChord,
            double O_v=0)
        {
            if (MemberType == HssTrussConnectionMemberType.Rhs)
            {
                if (ChordSection is ISectionTube && MainBranchSection is ISectionTube && SecondaryBranchSection is ISectionTube)
                {

                    SteelMaterial matChord = new SteelMaterial(F_yChord);
                    SteelRhsSection Chord = new SteelRhsSection(ChordSection as ISectionTube, matChord);


                    SteelMaterial matBr = new SteelMaterial(F_yBranch);
                    SteelRhsSection MainBranch = new SteelRhsSection(MainBranchSection as ISectionTube, matBr);

                    SteelRhsSection SecondaryBranch = new SteelRhsSection(SecondaryBranchSection as ISectionTube, matBr);


                    switch (Classification)
                    {
                        case HssTrussConnectionClassification.T:
                            return new RhsTrussTConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            break;
                        case HssTrussConnectionClassification.Y:
                            return new RhsTrussYConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            
                            break;
                        case HssTrussConnectionClassification.X:
                            return new RhsTrussXConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch, ForceTypeMainBranch, ForceTypeSecondaryBranch,
                                IsTensionChord, P_uChord, M_uChord);
                            break;
                        case HssTrussConnectionClassification.GappedK:
                            return new RhsTrussGappedKConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch,
                                ForceTypeMainBranch, ForceTypeSecondaryBranch, IsTensionChord, P_uChord,M_uChord);
                            break;
                        case HssTrussConnectionClassification.OverlappedK:
                            return new RhsTrussOverlappedConnection(Chord, MainBranch, thetaMainBranch, SecondaryBranch, thetaSecondaryBranch,
                                ForceTypeMainBranch, ForceTypeSecondaryBranch, IsTensionChord, P_uChord, M_uChord,O_v);
                            break;
                        default:
                            throw new Exception("Connection classification not recognized.");
                            break;
                    }
                }
                else
                {
                    throw new Exception("One of the member section is not of type ISectionTube. Ensure that a rectangular hollow section object type is used for chord and branches.");
                }

            }
            else
            {
                throw new NotImplementedException("Circular HSS truss connections are not supported yet.");
            }
        }
    }




}
