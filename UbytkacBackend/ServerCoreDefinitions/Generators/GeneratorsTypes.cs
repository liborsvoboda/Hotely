namespace UbytkacBackend.ServerCoreStructure {


    //public class ApiStructureGenerator {

    //    public List<string> PanelPointList { get; set; }
    //    public bool GeneratePanelGlobalView { get; set; } = false;
    //    public List<string> PanelPointList { get; set; }

    //}


    /// <summary>
    /// WebFile Generators Request Dataset
    /// </summary>
    public class MDGeneratorCreateIndexRequest {
        public string WebRootFilePath { get; set; }

        /// <summary>
        /// Is Subfolder for WebrootFilePath AS multiple RootIndex
        /// </summary>
        public string IndexWebRootSubFolderPathName { get; set; } = null;
        public string FromType { get; set; }
        public string ToType { get; set; }
        public bool ScanRootOnly { get; set; }
        public bool IndexOnly { get; set; }
        public bool RewriteAllowed { get; set; }
        public string ServerLanguage { get; set; } = "cz";
        public bool IndexInFrameList { get; set; } = false;
        public string genHtmlIndexFileSuffix { get; set; }
        public bool FromSuffixOnly { get; set; } = false;
    }



    /// <summary>
    /// Class Definition for Server Emailer In List of this claas you can use Mass Emailer
    /// </summary>
    public class SendMailRequest {
        public string? Sender { get; set; } = null;
        public List<string>? Recipients { get; set; } = null;
        public string? Subject { get; set; } = null;
        public string? Content { get; set; } = null;
    }


    public partial class LicenseCheckRequest {
        public string UnlockCode { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
    }


    /// <summary>
    /// Server Process class for running external prrocesses
    /// </summary>
    public class RunProcessRequest {
        public string Command { get; set; }
        public string? WorkingDirectory { get; set; } = null;
        public string? Arguments { get; set; } = null;
        public bool WaitForExit = true;
    }


}