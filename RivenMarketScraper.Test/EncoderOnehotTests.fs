namespace RivenMarketScraper.Tests

module ``Encoder onehot Tests`` =
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
    let ``Output string for value of 0 must have all bits set to 0`` maximum =
        let value = 0
        (validInput maximum value) ==> lazy

        let expected = ['0'] |> HashSet
        let bitstring = (onehot maximum value).Replace(" ", "")
        let actual = bitstring |> Seq.distinct |> HashSet
            
        Assert.Subset(expected, actual)

    [<Property>]
    let ``Output string for value greater than 0 must have exactly one bit set to 1`` maximum value =
        (validInput maximum value
        && value > 0) ==> lazy

        let expected = 1
        let actual = (onehot maximum value).ToCharArray() |> Seq.filter (fun n -> n = '1') |> Seq.length

        Assert.Equal(expected, actual)

    [<Property>]
    let ``Output string for value greater than 0 must have 1 bit in correct position`` maximum value =
        (validInput maximum value
        && value > 0) ==> lazy

        let expected = value - 1
        let actual = (onehot maximum value).Replace(" ", "").IndexOf('1')

        Assert.Equal(expected, actual)

    [<Property>]
    let ``Output string must contain only 1's, 0's, or spaces`` maximum value =
        (validInput maximum value) ==> lazy

        let expected = ['0';'1';' '] |> HashSet 
        let actual = (onehot maximum value) |> Seq.distinct |> HashSet

        Assert.Subset(expected, actual)

    [<Property>]
    let ``Output string must have spaces between each bit`` maximum value =
        (validInput maximum value) ==> lazy

        let encoded = (onehot maximum value).ToCharArray()
        let valid = encoded |> Seq.indexed |> Seq.forall (fun (i,n) -> match i % 2 = 0 with
                                                                       | true -> n <> ' '
                                                                       | false -> n = ' ')
        Assert.True(valid)

    [<Property>]
    let ``Output string length must match maximum`` maximum value =
        (validInput maximum value) ==> lazy

        let expected = maximum
        let actual = (onehot maximum value).Replace(" ", "").Length

        Assert.Equal(expected, actual)

    [<Property>]
    let ``Input must be valid`` maximum value =
        not (validInput maximum value) ==> lazy

        Assert.Throws<ArgumentException>(fun () -> onehot maximum value |> ignore) |> ignore
