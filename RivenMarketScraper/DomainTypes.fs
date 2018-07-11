namespace RivenMarketScraper

[<AutoOpen>]
module DomainTypes =
    
    type Stat = {
        Name: string;
        Value: float;
    }

    type Riven = {
        Name: string;
        Weapon: string;
        WeaponType: string;
        Price: int;
        Seller: string;
        Rank: int;
        MasteryRank: int;
        Polarity: string;
        Rerolls: int;
        Stats: seq<Stat>;
    }