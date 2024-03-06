using ICSharpCode.AvalonEdit;
using System.Linq;

namespace EasyITSystemCenter.GlobalOperations {

    /// <summary>
    /// Centralized Library With Extension Functions Of Exterrnal Tools
    /// </summary>
    internal class ToolsOperations {

        /// <summary>
        /// Avaloneditor Content Search Function With HighLight
        /// </summary>
        /// <param name="searchQuery">        </param>
        /// <param name="foundedIndex">       </param>
        /// <param name="textEditor">         </param>
        /// <param name="ignoreCaseSensitive"></param>
        public static void AvalonEditorFindText(string searchQuery, ref int foundedIndex, ref TextEditor textEditor, bool ignoreCaseSensitive = true) {
            if (string.IsNullOrEmpty(searchQuery)) { foundedIndex = 0; return; }
            string editorText = ignoreCaseSensitive ? textEditor.Text.ToLower() : textEditor.Text;
            if (string.IsNullOrEmpty(editorText)) { foundedIndex = 0; return; }
            if (foundedIndex >= editorText.Count()) { foundedIndex = 0; return; }
            int nIndex = editorText.IndexOf(ignoreCaseSensitive ? searchQuery.ToLower() : searchQuery, foundedIndex);
            if (nIndex != -1) {
                textEditor.Select(nIndex, searchQuery.Length);
                foundedIndex = nIndex + searchQuery.Length;
            }
            else { foundedIndex = 0; }
        }

        /// <summary>
        /// AvalonEditor Replace Matched Strings in Editor
        /// </summary>
        /// <param name="sourceString">       </param>
        /// <param name="targetString">       </param>
        /// <param name="foundedIndex">       </param>
        /// <param name="textEditor">         </param>
        /// <param name="ignoreCaseSensitive"></param>
        /// <param name="selectedonly">       </param>
        public static void AvalonEditorReplaceText(string sourceString, string targetString, ref int foundedIndex, ref TextEditor textEditor, bool ignoreCaseSensitive = true, bool selectedonly = false) {
            int nIndex = -1;
            if (selectedonly) {
                nIndex = ignoreCaseSensitive ? textEditor.Text.ToLower().IndexOf(sourceString.ToLower(), textEditor.SelectionStart, textEditor.SelectionLength)
                    : textEditor.Text.IndexOf(sourceString, textEditor.SelectionStart, textEditor.SelectionLength);
            }
            else { nIndex = ignoreCaseSensitive ? textEditor.Text.ToLower().IndexOf(sourceString.ToLower()) : textEditor.Text.IndexOf(sourceString); }

            if (nIndex != -1) {
                textEditor.Document.Replace(nIndex, sourceString.Length, targetString);
                textEditor.Select(nIndex, textEditor.SelectionLength);
                foundedIndex = nIndex + targetString.Length;
            }
            else {
                foundedIndex = 0;
                _ = MainWindow.ShowMessageOnMainWindow(false, DBOperations.DBTranslation("StringWasReplaced").GetAwaiter().GetResult());
            }
        }
    }
}