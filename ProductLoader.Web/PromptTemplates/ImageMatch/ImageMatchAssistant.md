# Image Match Assistant

1. **Output Requirements:**

- For **this image**, provide a **result object** IN JSON format with the following structure:

```csharp
   public class Breakdown
    {
        [JsonProperty("item_number_match")]
        public int ItemNumberMatch { get; set; }

        [JsonProperty("manufacturer_match")]
        public int ManufacturerMatch { get; set; }

        [JsonProperty("description_similarity")]
        public int DescriptionSimilarity { get; set; }

        [JsonProperty("metadata_keywords")]
        public int MetadataKeywords { get; set; }

        [JsonProperty("score")]
        public int? ImageMatchScore { get; set; }

    }
```
