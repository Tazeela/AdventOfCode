﻿using AdventLib;
using AdventOfCode2023;

Console.WriteLine("Hello, World 2023!");
IEnumerable<AdventSolver> solvers = new AdventSolver[] { new Day1() };


AdventRunner.ProcessRequests(solvers);