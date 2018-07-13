namespace RivenMarketScraper

module Riven =
    open FSharp.Data

    let getAttribute (node:HtmlNode) attribute = 
        node.AttributeValue(attribute)

    let parseStats node =
        let attrib = getAttribute node

        Seq.init 4 (fun i -> 
            let name = sprintf "data-stat%d" (i + 1)
            let value = sprintf "%sval" name

            attrib name,attrib value |> float
        )

    let parse node =
        let attrib = getAttribute node

        {
            Name=attrib "data-name";
            Weapon=attrib "data-weapon";
            WeaponType=attrib "data-wtype";
            Price=attrib "data-price" |> int;
            Seller=attrib "data-user";
            Rank=attrib "data-rank" |> int;
            MasteryRank=attrib "data-mr" |> int;
            Polarity=attrib "data-polarity";
            Rerolls=attrib "data-rerolls" |> int;
            Stats=parseStats node
        }