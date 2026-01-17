using UnityEngine;

namespace uk.vroad.xmpl
{
    public class UPlaySimExamplePrefab: UPlaySimExample
    {
        private static readonly int[] xrtValues =       { 0, 1, 2, 4, 10,};
        private static readonly int[] tspsValues =      { 0, 20, 10, 5, 5};

        [Range(0,4)]
        public int simulationSpeed = 1;

        private int prevSimulationSpeed = 0;
        
        protected override void Update()
        {
            if (sim != null && simulationSpeed != prevSimulationSpeed)
            {
                if (simulationSpeed == 0)
                {
                    runTraffic = false;
                }
                else
                {
                    runTraffic = true;
                    SetTargetRealTimeMultiplier(xrtValues[simulationSpeed]);
                    sim.SetTimeStepsPerSecond(tspsValues[simulationSpeed]);
                    EnforceSingleStepMode();
                }
                

                prevSimulationSpeed = simulationSpeed;
            }

            base.Update();
        }
    }
}