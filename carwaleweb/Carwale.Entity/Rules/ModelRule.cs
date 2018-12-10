using System;

namespace Carwale.Entity.Rules
{
    [Serializable]
    public class ModelRule
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public int ModuleEntryId { get; set; }
    }
}
