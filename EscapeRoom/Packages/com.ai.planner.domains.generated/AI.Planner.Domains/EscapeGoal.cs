using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.DomainLanguage.TraitBased;

namespace AI.Planner.Domains
{
    public struct EscapeGoal
    {
        public bool IsTerminal(StateData stateData)
        {
            var Character1Filter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Character>(),  };
            var Character2Filter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Character>(),  };
            var Character3Filter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Character>(),  };
            var Waypoint1Filter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Waypoint>(),[1] = ComponentType.ReadWrite<EscapePoint>(),  };
            var Waypoint2Filter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Waypoint>(),[1] = ComponentType.ReadWrite<EscapePoint>(),  };
            var Waypoint3Filter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Waypoint>(),[1] = ComponentType.ReadWrite<EscapePoint>(),  };
            var Character1ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Character1ObjectIndices, Character1Filter);
            var Character2ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Character2ObjectIndices, Character2Filter);
            var Character3ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Character3ObjectIndices, Character3Filter);
            var Waypoint1ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Waypoint1ObjectIndices, Waypoint1Filter);
            var Waypoint2ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Waypoint2ObjectIndices, Waypoint2Filter);
            var Waypoint3ObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(Waypoint3ObjectIndices, Waypoint3Filter);
            var CharacterBuffer = stateData.CharacterBuffer;
            
            for (int i0 = 0; i0 < Character1ObjectIndices.Length; i0++)
            {
                var Character1Index = Character1ObjectIndices[i0];
                var Character1Object = stateData.TraitBasedObjects[Character1Index];
                
                
                
                
                
                
            
            for (int i1 = 0; i1 < Character2ObjectIndices.Length; i1++)
            {
                var Character2Index = Character2ObjectIndices[i1];
                var Character2Object = stateData.TraitBasedObjects[Character2Index];
                
                if (!(stateData.GetTraitBasedObjectId(Character1Index) != stateData.GetTraitBasedObjectId(Character2Index)))
                    continue;
                
                
                
                
                
            
            for (int i2 = 0; i2 < Character3ObjectIndices.Length; i2++)
            {
                var Character3Index = Character3ObjectIndices[i2];
                var Character3Object = stateData.TraitBasedObjects[Character3Index];
                
                
                if (!(stateData.GetTraitBasedObjectId(Character2Index) != stateData.GetTraitBasedObjectId(Character3Index)))
                    continue;
                
                if (!(stateData.GetTraitBasedObjectId(Character1Index) != stateData.GetTraitBasedObjectId(Character3Index)))
                    continue;
                
                
                
            
            for (int i3 = 0; i3 < Waypoint1ObjectIndices.Length; i3++)
            {
                var Waypoint1Index = Waypoint1ObjectIndices[i3];
                var Waypoint1Object = stateData.TraitBasedObjects[Waypoint1Index];
                
                
                
                
                if (!(CharacterBuffer[Character1Object.CharacterIndex].Waypoint == stateData.GetTraitBasedObjectId(Waypoint1Index)))
                    continue;
                
                
            
            for (int i4 = 0; i4 < Waypoint2ObjectIndices.Length; i4++)
            {
                var Waypoint2Index = Waypoint2ObjectIndices[i4];
                var Waypoint2Object = stateData.TraitBasedObjects[Waypoint2Index];
                
                
                
                
                
                if (!(CharacterBuffer[Character2Object.CharacterIndex].Waypoint == stateData.GetTraitBasedObjectId(Waypoint2Index)))
                    continue;
                
            
            for (int i5 = 0; i5 < Waypoint3ObjectIndices.Length; i5++)
            {
                var Waypoint3Index = Waypoint3ObjectIndices[i5];
                var Waypoint3Object = stateData.TraitBasedObjects[Waypoint3Index];
                
                
                
                
                
                
                if (!(CharacterBuffer[Character3Object.CharacterIndex].Waypoint == stateData.GetTraitBasedObjectId(Waypoint3Index)))
                    continue;
                Character1Filter.Dispose();
                Character2Filter.Dispose();
                Character3Filter.Dispose();
                Waypoint1Filter.Dispose();
                Waypoint2Filter.Dispose();
                Waypoint3Filter.Dispose();
                return true;
            }
            }
            }
            }
            }
            }
            Character1ObjectIndices.Dispose();
            Character2ObjectIndices.Dispose();
            Character3ObjectIndices.Dispose();
            Waypoint1ObjectIndices.Dispose();
            Waypoint2ObjectIndices.Dispose();
            Waypoint3ObjectIndices.Dispose();
            Character1Filter.Dispose();
            Character2Filter.Dispose();
            Character3Filter.Dispose();
            Waypoint1Filter.Dispose();
            Waypoint2Filter.Dispose();
            Waypoint3Filter.Dispose();

            return false;
        }

        public float TerminalReward(StateData stateData)
        {
            var reward = 50f;

            return reward;
        }
    }
}
