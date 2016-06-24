namespace Lombiq.FeedAggregator.Models.NonPersistent
{
    public class Mapping
    {
        /// <summary>
        /// Format: NodeName, NodeName/NodeName, NodeName.AttributeName, NodeName/NodeName.AttributeName.
        /// </summary>
        public string FeedMapping { get; set; }

        /// <summary>
        /// Format: PartName, PartName.PartProperty, PartName.FieldName, ContentType.FieldName.
        /// </summary>
        public string ContentItemStorageMapping { get; set; }
    }
}