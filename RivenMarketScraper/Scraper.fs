namespace RivenMarketScraper

module Scraper =
    open FSharp.Data
    open System
    
    type ScraperConfiguration = {
        Platform: string;
        Limit: int;
        Polarity: string;
        Rank: string;
        Mastery: int;
        Weapon: string;
        Price: int;
        Rerolls: int;
    }

    let scrape { Platform=platform;
                 Limit=limit;
                 Polarity=polarity;
                 Rank=rank;
                 Mastery=mastery;
                 Weapon=weapon;
                 Price=price;
                 Rerolls=rerolls} =
        let sb = new System.Text.StringBuilder()
        let format str = Printf.bprintf sb str
        
        format "https://riven.market/_modules/riven/showrivens.php"
        format "?baseurl=aHR0cHM6Ly9yaXZlbi5tYXJrZXQv"
        format "&platform=%s" platform
        format "&limit=%d" limit
        format "&recency=-1"
        format "&veiled=false"
        format "&onlinefirst=true"
        format "&polarity=%s" polarity
        format "&rank=%s" rank
        format "&mastery=%d" mastery
        format "&weapon=%s" weapon
        format "&stats=Any"
        format "&neg=all"
        format "&price=%d" price
        format "&rerolls=%d" rerolls
        format "&sort=time"
        format "&direction=ASC"
        format "&page=1"
        format "&time=%d" (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        
        let results = HtmlDocument.Load(sb.ToString())
        results.Descendants ["div"] |> Seq.filter(fun n -> n.HasClass "riven") |> Seq.map Riven.parse