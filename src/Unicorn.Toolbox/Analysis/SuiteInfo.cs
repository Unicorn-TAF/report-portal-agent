﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis
{
    [Serializable]
    public struct SuiteInfo
    {
        public const string NoFeature = "<FEATURE NOT SPECIFIED>";

        public SuiteInfo(string suiteName, IEnumerable<string> features, Dictionary<string, string> metadata)
        {
            Name = suiteName;
            TestsInfos = new List<TestInfo>();
            Features = new List<string>(features);
            Metadata = metadata;

            if (!Features.Any())
            {
                Features.Add(NoFeature);
            }
        }

        public string Name { get; set; }

        public List<TestInfo> TestsInfos { get; set; }

        public List<string> Features { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public string FeaturesString => string.Join("\n", Features).ToLowerInvariant();

        public string MetadataString => string.Join("\n", Metadata.Select(m => m.Key + ": " + m.Value));

        public void SetTestInfo(List<TestInfo> newInfos) =>
            TestsInfos = newInfos;
    }
}
