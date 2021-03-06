﻿// Copyright (c) Arjen Post. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Dapper.Mapper.Tests
{
    public abstract partial class SqlMapperTests
    {
        public class TheQueryAsync3Method : SqlMapperTests
        {
            public TheQueryAsync3Method()
                : base(columnNames: new[] { "FirstId", "SecondId", "ThirdId" })
            {
            }

            [Fact]
            public async Task ShouldPassCommandText()
            {
                // Arrange
                var commandText = "foo";

                // Act
                await this.Connection.QueryAsync<First, Second, Third>(sql: commandText, param: null, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: CommandType.Text);

                // Assert
                Mock.Get(this.Command)
                    .VerifySet(command => command.CommandText = commandText);
            }

            [Fact]
            public async Task ShouldPassParameters()
            {
                // Arrange
                var parameters = new { foo = 42 };

                // Act
                await this.Connection.QueryAsync<First, Second, Third>(sql: "@foo", param: parameters, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: CommandType.Text);

                // Assert
                Mock.Get(this.Parameter)
                    .VerifySet(parameter => parameter.ParameterName = "foo");
                Mock.Get(this.Parameter)
                    .VerifySet(parameter => parameter.Value = 42);
            }

            [Fact]
            public async Task ShouldPassTransaction()
            {
                // Act
                await this.Connection.QueryAsync<First, Second, Third>(sql: string.Empty, param: null, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: CommandType.Text);

                // Assert
                Mock.Get(this.Command)
                    .VerifySet(command => command.Transaction = this.Transaction);
            }

            [Fact]
            public async Task ShouldPassBuffered()
            {
                // Act
                var result = await this.Connection.QueryAsync<First, Second, Third>(sql: string.Empty, param: null, transaction: this.Transaction, buffered: false, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: CommandType.Text);

                // Assert
                Assert.IsNotType<List<First>>(result);
            }

            [Fact]
            public async Task ShouldPassSplitOn()
            {
                // Act
                var result = await this.Connection.QueryAsync<First, Second, Third>(sql: string.Empty, param: null, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: CommandType.Text);

                // Assert
                Assert.Equal(1, result.First().Second.Third.ThirdId);
            }

            [Fact]
            public async Task ShouldPassCommandTimeout()
            {
                // Arrange
                var commandTimeout = 42;

                // Act
                await this.Connection.QueryAsync<First, Second, Third>(sql: string.Empty, param: null, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: commandTimeout, commandType: CommandType.Text);

                // Assert
                Mock.Get(this.Command)
                    .VerifySet(command => command.CommandTimeout = commandTimeout);
            }

            [Fact]
            public async Task ShouldPassCommandType()
            {
                // Arrange
                var commandType = CommandType.TableDirect;

                // Act
                await this.Connection.QueryAsync<First, Second, Third>(sql: string.Empty, param: null, transaction: this.Transaction, buffered: true, splitOn: string.Join(",", this.ColumnNames), commandTimeout: null, commandType: commandType);

                // Assert
                Mock.Get(this.Command)
                    .VerifySet(command => command.CommandType = commandType);
            }
        }
    }
}
