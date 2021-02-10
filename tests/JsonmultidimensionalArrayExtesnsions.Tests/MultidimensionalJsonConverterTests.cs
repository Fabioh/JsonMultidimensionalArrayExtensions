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
    }
}
