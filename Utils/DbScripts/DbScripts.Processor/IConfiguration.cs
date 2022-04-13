using System.Collections.Generic;

namespace DbScripts.Processor
{
    public interface IConfiguration
    {
        IEnumerable<FileMoveParams> MoveParams { get; }
        IEnumerable<string> DeletePatterns { get; }
        IEnumerable<string> DbObjectFolders { get; }
    }
}
