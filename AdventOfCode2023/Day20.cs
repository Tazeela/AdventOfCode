using System.Reflection.Metadata;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pulse = AdventOfCode2023.Day20.Pulse;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 20](https://adventofcode.com/2023/day/20)
/// </summary>
public class Day20 : AdventSolver {
    public override string Day => "day20";

    private static readonly ModuleTrigger ButtonTrigger = new("", "button", Pulse.Low);

    public override void Solve() {
        ////   Part 1
        Dictionary<string, Module> part1Modules = BuildModuleMap();
        Console.WriteLine("Solution for part 1 is: " + CountTriggers(part1Modules, 1000));

        ////   Part 2
        // reload the modules so they reset
        Dictionary<string, Module> part2Modules = BuildModuleMap();
        Console.WriteLine("Solution for part 2 is = " + CalculatePeriod(part2Modules, "rx"));
    }


    public Dictionary<string, Module> BuildModuleMap() {
        List<Module> modules = [];

        // Parse everything
        foreach (string game in ReadInputAsIEnumerable()) {
            if (game.StartsWith("broadcaster")) {
                modules.Add(new BroadcasterModule(game.Split("->")[1].Split(",").Select(s => s.Trim()).ToList()));
            } else if (game.StartsWith("%")) {
                var kvp = game.Substring(1).Split(" -> ");
                modules.Add(new FlipFlopModule(kvp[0], kvp[1].Split(",").Select(s => s.Trim()).ToList()));
            } else if (game.StartsWith("&")) {
                var kvp = game.Substring(1).Split(" -> ");
                modules.Add(new ConjunctionModule(kvp[0], kvp[1].Split(",").Select(s => s.Trim()).ToList()));
            }
        }

        RxModule rx = new();

        modules.Add(new ButtonModule());
        modules.Add(rx);

        Dictionary<string, Module> moduleMap = modules.Select(module => (module.Name, module)).ToDictionary();

        // Register all sources
        foreach (var (key, module) in moduleMap) {
            foreach (var target in module.Targets) {
                if (moduleMap.TryGetValue(target, out Module? value)) {
                    value.RegisterSource(key);
                }
            }
        }
        return moduleMap;
    }

    public static long CountTriggers(Dictionary<string, Module> modules, int iterations) {
        long numLow = 0;
        long numHigh = 0;

        for (int x = 0; x < iterations; x++) {
            Queue<ModuleTrigger> triggers = [];
            triggers.Enqueue(ButtonTrigger);

            while (triggers.Count > 0) {
                var next = triggers.Dequeue();
                if (modules.TryGetValue(next.Target, out Module? module)) {
                    foreach (var triggered in module.Trigger(next.Source, next.pulse)) {
                        //Console.WriteLine("{0} -{1}-> {2}", trigger.source, trigger.IsLowPulse ? "low" : "high", trigger.Target);
                        if (triggered.pulse == Pulse.Low) {
                            numLow++;
                        } else {
                            numHigh++;
                        }

                        triggers.Enqueue(triggered);
                    }
                }
            }
        }

        return numLow * numHigh;
    }

    public static long CalculatePeriod(Dictionary<string, Module> modules, string target) {
        // Assumes that target has 1 Source, which is a 4x Conjunction
        Dictionary<string, long> watchlist = [];

        // Get the sources for the conjunction and watch them
        var conjunction = modules[modules[target].Sources[0]];
        var conjunctionSources = conjunction.Sources;

        // Run until we have 2 values for everything in the watchlist and can determine the period between them
        for (int iteration = 1; watchlist.Count < conjunctionSources.Count; iteration++) {
            Queue<ModuleTrigger> triggers = [];
            triggers.Enqueue(ButtonTrigger);

            while (triggers.Count > 0) {
                var next = triggers.Dequeue();
                if (modules.ContainsKey(next.Target)) {
                    foreach (var triggered in modules[next.Target].Trigger(next.Source, next.pulse)) {
                        if (!watchlist.ContainsKey(next.Source) && conjunctionSources.Contains(next.Source) && next.Target == conjunction.Name && next.pulse == Pulse.High) {
                            watchlist[next.Source] = iteration;
                        }
                        triggers.Enqueue(triggered);
                    }
                }
            }
        }

        // calculate the period between and generate an LCM
        return watchlist.Values.Lcm();
    }

    public enum Pulse {
        Low,
        High
    }

    public struct ModuleTrigger(string source, string target, Pulse pulse) {
        public readonly string Source = source;
        public readonly string Target = target;
        public readonly Pulse pulse = pulse;
    }

    public abstract class Module(string name, List<string> targets) {

        public readonly string Name = name;
        public readonly List<string> Targets = targets;

        public readonly List<string> Sources = [];

        public virtual IEnumerable<ModuleTrigger> Trigger(string source, Pulse pulse) {
            foreach (var target in Targets) {
                yield return new ModuleTrigger(Name, target, pulse);
            }
        }

        public virtual void RegisterSource(string source) {
            Sources.Add(source);
        }
    }

    public class RxModule() : Module("rx", []) {
        public bool hasReceivedLow = false;

        public override IEnumerable<ModuleTrigger> Trigger(string source, Pulse pulse) {
            if (pulse == Pulse.Low) hasReceivedLow = true;

            return base.Trigger(source, pulse);
        }
    }

    public class ButtonModule() : Module("button", ["broadcaster"]) {
    }

    public class FlipFlopModule(string name, List<string> targets) : Module(name, targets) {
        private bool lastIsLow = true;

        public override IEnumerable<ModuleTrigger> Trigger(string source, Pulse pulse) {
            if (pulse == Pulse.Low) {
                lastIsLow = !lastIsLow;
                // triggered after the flip, so the first time it sends high
                return base.Trigger(source, lastIsLow ? Pulse.Low : Pulse.High);
            } else {
                return [];
            }
        }
    }

    public class ConjunctionModule(string name, List<string> targets) : Module(name, targets) {
        private readonly Dictionary<string, Pulse> SourceStates = [];

        public override void RegisterSource(string name) {
            SourceStates.Add(name, Pulse.Low);
            base.RegisterSource(name);
        }

        public override IEnumerable<ModuleTrigger> Trigger(string source, Pulse pulse) {
            SourceStates[source] = pulse;
            if (SourceStates.Values.All(lastPulse => lastPulse == Pulse.High)) {
                // triggered after the flip, so the first time it sends high
                return base.Trigger(source, Pulse.Low);
            } else {
                return base.Trigger(source, Pulse.High);
            }
        }
    }

    public class BroadcasterModule(List<string> targets) : Module("broadcaster", targets) {
    }

    [TestClass]
    public class Day20Test {

        [TestMethod]
        public void Test1() {
        }
    }
}