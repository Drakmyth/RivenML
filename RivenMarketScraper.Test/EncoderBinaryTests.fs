namespace RivenMarketScraper.Tests

module ``Encoder binary Tests`` =
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
    let ``Output string must represent correct bit pattern`` maximum value =
        (validInput maximum value) ==> lazy

        let expected = value
        let bitstring = (binary maximum value).Replace(" ", "")
        let actual = Convert.ToInt32(bitstring, 2)
            
        Assert.Equal(expected, actual)

    [<Property>]
    let ``Output string must contain only 1's, 0's, or spaces`` maximum value =
        (validInput maximum value) ==> lazy

        let expected = ['0';'1';' '] |> HashSet
        let actual = binary maximum value |> Seq.distinct |> HashSet

        Assert.Subset(expected, actual)

    [<Property>]
    let ``Output string must have spaces between each bit`` maximum value =
        (validInput maximum value) ==> lazy

        let encoded = (binary maximum value).ToCharArray()
        let valid = encoded |> Seq.indexed |> Seq.forall (fun (i,n) -> match i % 2 = 0 with
                                                                       | true -> n <> ' '
                                                                       | false -> n = ' ')
        Assert.True(valid)

    [<Property>]
    let ``Output string must match maximum input's binary length`` maximum value =
        (validInput maximum value) ==> lazy

        let expected = Convert.ToString(maximum, 2).Length
        let actual = (binary maximum value).Replace(" ", "").Length

        Assert.Equal(expected, actual)

    [<Property>]
    let ``Input must be valid`` maximum value =
        not (validInput maximum value) ==> lazy

        Assert.Throws<ArgumentException>(fun () -> binary maximum value |> ignore) |> ignore
