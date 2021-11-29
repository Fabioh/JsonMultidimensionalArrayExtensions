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
        [Fact]
        public void ShouldSerializeAJsonTextCorrectly()
        {

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            var jsonString = "[[1,2,3],[1,2,3]]";
            var v = JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            v.Should().BeEquivalentTo(new double[,] { { 1, 2, 3 }, { 1, 2, 3 } }); 
        }

        [Fact]
        public void ShoudThrowExceptionWhenInvalidJsonPassed()
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
                .WithMessage("the matrix must have the same quantity of elements in each line");
        }

        [Fact]
        public void ShoudThrowExceptionWhenInvalidJsonPassedWithNoStartArraySymbol()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            var jsonString = "[1,2], [1,2], [1,2]]";
            Action act = () => JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            act.Should()
                .Throw<JsonException>();
        }

        [Fact]
        public void ShoudThrowExceptionWhenInvalidJsonPassedWithNoStartArraySymbol2()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new MultidimensionalJsonConverter<double>()
                }
            };

            var jsonString = "1,2], [1,2], [1,2]]";
            Action act = () => JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

            act.Should()
                .Throw<JsonException>();
        }

        //[Fact]
        //public void ShoudThrowExceptionWhenInvalidJsonPassedWithNoEndArraySymbol()
        //{
        //    var serializeOptions = new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        Converters =
        //        {
        //            new MultidimensionalJsonConverter<double>()
        //        }
        //    };

        //    var jsonString = "[[1,2], [1,2}, [1,2]]";
        //    Action act = () => JsonSerializer.Deserialize<double[,]>(jsonString, serializeOptions);

        //    act.Should()
        //        .Throw<JsonException>();
        //}
    }
}
