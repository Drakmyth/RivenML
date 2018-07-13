namespace RivenMarketScraper.Tests

module ``Encoder Tests`` =
    open Xunit
    open System
    open RivenMarketScraper.Encoder
    open FsCheck
    open FsCheck.Xunit
    open System.Collections.Generic

    let validInput maximum value =
        (maximum > 0
        && value >= 0
        && maximum >= value)

    [<Property>]
    let ``Binary transformation outputs correct bit pattern`` maximum value =
        (validInput maximum value) ==> lazy

        let tVal = binary maximum value
        let tmp = tVal.Replace(" ", "")
        let output = Convert.ToInt32(tmp, 2)
            
        Assert.Equal(value, output)

    [<Property>]
    let ``Binary string should contains 1's, 0's, or spaces`` maximum value =
        (validInput maximum value) ==> lazy

        let tVal = binary maximum value
        let distinct = tVal |> Seq.distinct

        Assert.Subset(HashSet ['0';'1';' '], HashSet distinct)

    [<Property>]
    let ``Binary transformation should place spaces between each bit`` maximum value =
        (validInput maximum value) ==> lazy

        let tVal = binary maximum value
        let valid = tVal.ToCharArray() |> Seq.indexed |> Seq.forall (fun (i,n) -> match i % 2 = 0 with
                                                                                  | true -> n <> ' '
                                                                                  | false -> n = ' ')
        Assert.True(valid)

    [<Property>]
    let ``Binary transformation should match maximum length`` maximum value =
        (validInput maximum value) ==> lazy

        let tVal = binary maximum value
        let maxBinary = Convert.ToString(maximum, 2)

        Assert.Equal(maxBinary.Length, tVal.Replace(" ", "").Length)

    [<Property>]
    let ``Input must be valid`` maximum value =
        not (validInput maximum value) ==> lazy

        Assert.Throws<ArgumentException>(fun () -> binary maximum value |> ignore) |> ignore
