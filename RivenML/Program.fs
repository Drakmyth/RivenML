module Program

open System
open RivenMarketScraper.Scraper
open RivenMarketScraper.Encoder

let printRiven riven =
    Console.WriteLine(riven.ToString())

[<EntryPoint>]
let main _ =

    #if DEBUG
    // Load System.Core.dll so IEnumerables can be expanded in the debugger
    // See: http://blog.paranoidcoding.com/2010/09/10/improving-the-display-of-f-seq-lt-t-gt-s-in-the-debugger.html
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
    let riven = Seq.head rivens
    riven |> printRiven
    encode riven |> ignore

    Console.ReadKey() |> ignore
    0 // return an integer exit code
