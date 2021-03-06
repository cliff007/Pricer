﻿namespace Pricer.Benchmark

open System
open Pricer
open Pricer.Core
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Diagnostics.Windows
                  
type BenchConfig () =
    inherit ManualConfig()
    do 
        base.Add Job.RyuJitX64
        #if MONO
        #else
        base.Add(new MemoryDiagnoser())
        #endif

[<Config (typeof<BenchConfig>)>]
type BinomialBenchmark () =    
    
    let stock = {
        Volatility = 0.05
        Rate = 0.03
        CurrentPrice = 230.0
    }

    let europeanCall = {
        Strike = 231.0
        Expiry = new DateTime(2015,12,12)
        Direction = 1.0
        Kind = Call
        Style = European
        PurchaseDate = new DateTime(2015,9,5)
    }

    [<Params (1000, 2000, 3000)>] 
    member val public Steps = 0 with get, set
   
    [<Benchmark>]
    member self.Binomial () = 
        Binomial.binomial stock europeanCall self.Steps Functional