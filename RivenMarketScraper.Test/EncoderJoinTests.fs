namespace RivenMarketScraper.Tests

module ``Encoder join Tests`` =
    open Xunit
    open RivenMarketScraper.Encoder
    open FsCheck
    open FsCheck.Xunit

    [<Property>]
    let ``Output string must have spaces between each bit`` separator =
        Arb.generate<string>
        |> Gen.arrayOf
        |> Arb.fromGen
        |> Prop.forAll <| fun values ->
            let joinedString = join separator values
            values |> Seq.fold (fun i n ->
                    let cleanN = match n with
                                 | null -> ""
                                 | _ -> n
                    let expectedValue = cleanN
                    let actualValue = joinedString.Substring(i, cleanN |> String.length)
                    Assert.Equal(expectedValue, actualValue)

                    let i2 = i + cleanN.Length

                    match i2 < joinedString.Length with
                    | true ->
                        let cleanSeparator = match separator with
                                             | null -> ""
                                             | _ -> separator
                        let expectedSeparator = cleanSeparator
                        let actualSeparator = joinedString.Substring(i2, cleanSeparator |> String.length)
                        Assert.Equal(expectedSeparator, actualSeparator)
                        i2 + cleanSeparator.Length
                    | false -> i2
                ) 0 |> ignore