namespace EasyITSystemCenter.Classes {

    /// <summary>
    /// Program version Class
    /// </summary>
    public class AppVersion {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Private { get; set; }
    }

    /// <summary>
    /// Actual List Item informations for Controls each Page in MainView
    /// </summary>
    public class DataViewSupport {
        public int SelectedRecordId { get; set; } = 0;
        public bool FormShown { get; set; } = false;
        public string FilteredValue { get; set; } = null;
        public string AdvancedFilter { get; set; } = null;
    }

    /// <summary>
    /// Language definition support
    /// </summary>
    public class Language {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Report naming support
    /// </summary>
    public class ReportSelection {
        public string Name { get; set; }
    }
}