using System.Collections.Generic;

namespace DbScripts.Processor
{
    public class InMemoryConfiguration : IConfiguration
    {
        public IEnumerable<FileMoveParams> MoveParams
        { 
            get
            {
                return new List<FileMoveParams>
                {
                    new FileMoveParams
                    {
                        Pattern = ".Schema.sql",
                        TargetFolderName = "010_Schemas"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".User.sql",
                        TargetFolderName = "020_Users"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".DatabaseRole.sql",
                        TargetFolderName = "030_Roles"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".UserDefinedDataType.sql",
                        TargetFolderName = "040_Types"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".Table.sql",
                        TargetFolderName = "050_Tables"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".Trigger.sql",
                        TargetFolderName = "070_Triggers"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".View.sql",
                        TargetFolderName = "080_Views"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".UserDefinedFunction.sql",
                        TargetFolderName = "090_Functions"
                    },
                    new FileMoveParams
                    {
                        Pattern = ".StoredProcedure.sql",
                        TargetFolderName = "100_Procedures"
                    },
                };
            }
        }

        public IEnumerable<string> DeletePatterns
        {
            get
            {
                return new string[]
                {
                    @"\.ExtendedProperties\.sql",
                    @"\.SqlAssembly\.sql",
                    @"\.Database\.sql",
                    @"\.\.sql"
                };
            }
        }

        public IEnumerable<string> DbObjectFolders
        {
            get
            {
                return new string[]
                {
                    "010_Schemas",
                    "020_Users",
                    "030_Roles",
                    "040_Types",
                    "050_Tables",
                    "060_TablesUpdates",
                    "070_Triggers",
                    "080_Views",
                    "090_Functions",
                    "100_Procedures",
                    "110_CustomScripts",
                };
            }
        }
    }
}
