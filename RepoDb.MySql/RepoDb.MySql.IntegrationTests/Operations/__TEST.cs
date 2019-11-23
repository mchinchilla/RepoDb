﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using RepoDb.MySql.IntegrationTests.Setup;

namespace RepoDb.MySql.IntegrationTests.Operations
{
    [TestClass]
    public class __TEST
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        [TestMethod]
        public void ExtractDefinitions()
        {
            var definitions = string.Empty;
            using (var connection = new MySqlConnection(Database.ConnectionString).EnsureOpen())
            {
                using (var reader = connection.ExecuteReader("SELECT * FROM `completetable`;"))
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var columName = reader.GetName(i);
                        var type = reader.GetFieldType(i);
                        definitions = string.Concat(definitions, $"public {type.FullName.ToString()} {columName} {{ get; set; }}\n");
                    }
                }
            }
        }

        [TestMethod]
        public void ExtractFields()
        {
            var definitions = string.Empty;
            using (var connection = new MySqlConnection(Database.ConnectionString).EnsureOpen())
            {
                using (var reader = connection.ExecuteReader("SELECT * FROM `completetable`;"))
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var columName = reader.GetName(i);
                        var type = reader.GetFieldType(i);
                        definitions = string.Concat(definitions, $"{columName} = default({type.FullName.ToString()}),\n");
                    }
                }
            }
        }
    }
}
