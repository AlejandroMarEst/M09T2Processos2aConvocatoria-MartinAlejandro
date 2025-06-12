using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BakerySimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var simulator = new CakeSimulator();

            Console.WriteLine("SEQUENTIAL SIMULATION:");
            var sequentialTime = simulator.RunSequentialSimulation();
            Console.WriteLine($"Total sequential time: {sequentialTime.TotalSeconds} seconds\n");

            Console.WriteLine("PARALLEL SIMULATION:");
            var parallelTime = await simulator.RunParallelSimulationAsync();
            Console.WriteLine($"Total parallel time: {parallelTime.TotalSeconds} seconds\n");

            Console.WriteLine($"Time saved: {(sequentialTime - parallelTime).TotalSeconds} seconds");
        }
    }

    public class CakeSimulator
    {
        private void BeatBatter() => SimulateWork("Beating batter", 8);
        private void PreheatOven() => SimulateWork("Preheating oven", 10);
        private void Bake() => SimulateWork("Baking", 15);
        private void PrepareIcing() => SimulateWork("Preparing icing", 5);
        private void CoolCake() => SimulateWork("Cooling cake", 4);
        private void Glaze() => SimulateWork("Glazing", 3);
        private void Decorate() => SimulateWork("Decorating", 2);

        // Helper method for synchronous simulation
        private void SimulateWork(string taskName, int seconds)
        {
            Console.WriteLine($"{taskName}... ({seconds} s)");
            Thread.Sleep(seconds * 1000);
        }

        // Helper method for asynchronous simulation
        private async Task SimulateWorkAsync(string taskName, int seconds)
        {
            Console.WriteLine($"{taskName}... ({seconds} s)");
            await Task.Delay(seconds * 1000);
        }

        // Sequential simulation
        public TimeSpan RunSequentialSimulation()
        {
            var stopwatch = Stopwatch.StartNew();

            BeatBatter();
            PreheatOven();
            Bake();
            PrepareIcing();
            CoolCake();
            Glaze();
            Decorate();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        // Parallel simulation
        public async Task<TimeSpan> RunParallelSimulationAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            // Beat batter and preheat oven in parallel
            Task beatBatterTask = SimulateWorkAsync("Beating batter", 8);
            Task preheatOvenTask = SimulateWorkAsync("Preheating oven", 10);
            await Task.WhenAll(beatBatterTask, preheatOvenTask);

            // Baking depends on batter and preheating being done
            await SimulateWorkAsync("Baking", 15);

            // Prepare icing and cool cake can run in parallel
            Task coolCakeTask = SimulateWorkAsync("Cooling cake", 4);
            Task prepareIcingTask = SimulateWorkAsync("Preparing icing", 5);
            await Task.WhenAll(coolCakeTask, prepareIcingTask);

            // Glazing after both previous steps are complete
            await SimulateWorkAsync("Glazing", 3);

            // Final decoration
            await SimulateWorkAsync("Decorating", 2);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
