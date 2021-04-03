using FluentAssertions;
using JsonMultidimensionalArrayExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace JsonmultidimensionalArrayExtesnsions.Tests
{
    public class MultidimensionalJsonConverterTests
    {
        [Theory]
        [MemberData(nameof(SerializeValidJsonTestData))]
        public void ShouldSerializeAJsonTextCorrectly(string jsonString, double[,] expected)
        {

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            var v = JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            v.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShoudThrowExceptionWithMesssageWhenInvalidJsonPassed()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            var jsonString = "[[1,2,3],[1,2,3], [1,2]]";
            Action act = () => JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            act.Should()
                .Throw<JsonException>()
                .WithMessage("the matrix must have teh same quantity of elements in each line");
        }

        [Theory]
        [MemberData(nameof(SerializeInvalidTestData))]
        public void ShoudThrowExceptionWhenInvalidJsonPassed(string jsonString)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            Action act = () => JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            act.Should()
                .Throw<JsonException>();
                //.WithMessage(string.Empty);
        }

        public static IEnumerable<object[]> SerializeValidJsonTestData() =>
            new[]
            {
                new object[] { "[[1,2,3],[1,2,3]]", new double[,] { { 1, 2, 3 }, { 1, 2, 3 } } },
                new object[] { "[[1,2],[1,2]]", new double[,] { { 1, 2 }, { 1, 2 } } },
                new object[] { "[[1,2,3,4,5],[1,2,3,4,5]]", new double[,] { { 1, 2, 3, 4, 5 }, { 1, 2, 3, 4, 5 } } },
                new object[] { "[[1,2,3,4,5],[5,4,3,2,1]]", new double[,] { { 1, 2, 3, 4, 5 }, { 5, 4, 3, 2, 1 } } },
            };

        public static IEnumerable<object[]> SerializeInvalidTestData() =>
            new[]
            {
                new object[] { "1,2,3],[1,2,3]]" },
                new object[] { "[1,2,3],[1,2,3]]" },
                new object[] { "[[1,2,3],[1,2,3" },
                new object[] { "[[1,2,3],[1,2,3]" },
            };
    }
}
