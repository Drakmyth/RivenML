module Program

open System
open RivenMarketScraper.Scraper

let printRiven riven =
    Console.WriteLine(riven.ToString())

[<EntryPoint>]
let main argv =

    #if DEBUG
    // Load System.Core.dll so IEnumerables can be expanded in the debugger
    System.Linq.Enumerable.Count([]) |> ignore
    #endif

    let options = {
        Platform="PC";
        Limit=25;
        Polarity="all";
        Rank="all";
        Mastery=16;
        Weapon="Any";
        Price=99999;
        Rerolls=(-1);
    }
    let rivens = scrape options
    rivens |> Seq.iter printRiven

    Console.ReadKey() |> ignore
    0 // return an integer exit code
