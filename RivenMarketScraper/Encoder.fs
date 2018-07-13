namespace RivenMarketScraper

module Encoder = 
    
    open RivenMarketScraper
    open System

    (*
        Name |> ignore
        Weapon |> binary // 257 weapons - 9 digits
        WeaponType |> onehot // 4 weapon types - 4 digits
        Price |> int
        Seller |> ignore
        Rank |> int
        MasteryRank |> int
        Polarity |> onehot // 5 polarities - 5 digits
        Rerolls |> int
        Stats.Name |> binary // 30 status types - 5 digits
        Stats.Value |> float
    *)

    let join separator (values:seq<'a>) =
        String.Join(separator, values)

    let binary (maximum:int) (value:int) =
        if (maximum <= 0) then invalidArg "maximum" ("Maximum must be greater than 0. Maximum: " + maximum.ToString())
        if (value < 0) then invalidArg "value" ("Value must be greater than or equal to 0. Value: " + value.ToString())
        if (maximum < value) then invalidArg "maximum" ("Maximum must be greater than or equal to value. Maximum: " + maximum.ToString() + ", Value: " + value.ToString())

        Convert.ToString(value, 2).PadLeft(Convert.ToString(maximum, 2).Length, '0').ToCharArray() |> join " "

    let onehot maximum value =
        Math.Min(value, 1).ToString().PadLeft(value, '0').PadRight(maximum, '0').ToCharArray() |> join " "
    
    let encodeData (method:int->int->string) collection value = 
        let index = match collection |> Seq.tryFindIndex (fun n -> n = value) with
                    | Some value -> value
                    | None -> 0
        method (collection |> Seq.length) index

    let encodeWeapon = encodeData binary WeaponNames
    let encodeWeaponType = encodeData onehot WeaponTypes
    let encodePolarity = encodeData onehot Polarities

    let encodeStat stat =
        let encodeStatName = encodeData binary StatNames
        let name = encodeStatName (fst stat)
        name + " " + (snd stat).ToString()

    let encode (riven:Riven) =
        let weapon = riven.Weapon |> encodeWeapon
        let weaponType = riven.WeaponType |> encodeWeaponType
        let price = riven.Price
        let rank = riven.Rank
        let mastery = riven.MasteryRank
        let polarity = riven.Polarity |> encodePolarity
        let rerolls = riven.Rerolls
        let stats = riven.Stats |> Seq.map encodeStat |> join " "
        
        let data = String.Join(" ", weapon, weaponType, price, rank, mastery, polarity, rerolls, stats)
        0