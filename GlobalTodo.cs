/* Humberto H. C. Pinheiro Mon, 10 May 2010 09:07:55 
 * This tomboy plugin checks for notes containing the #TODO string and put 
 * a link to them inside a global TODO note.
 */

using System;
using Mono.Unix;
using Gtk;
using Tomboy;

namespace Tomboy.GlobalTodo
{
	public class GlobalTodoNoteAddin : NoteAddin
	{
		static string PATTERN = "#TODO";
		static string GLOBALTITLE = "Global Todo";
		static string GLOBALCONTENT = "<note-content><bold>Tasks to do ...</bold>\n\n</note-content>";
		static Note GlobalTodo;

		public override void Initialize ()
		{
			try {
				GlobalTodo = Note.Manager.Find(GLOBALTITLE); 

				if(GlobalTodo == null)
					GlobalTodo = Note.Manager.Create(GLOBALTITLE, GLOBALCONTENT);
			} catch (Exception e) {
				Console.WriteLine("Error at initialize: " + e);
			}
		}

		public override void Shutdown ()
		{
		}

		public override void OnNoteOpened ()
		{
			Note.Buffer.InsertText += OnInsertText;
			Note.Buffer.DeleteRange += OnDeleteRange;
			CheckRegion();
		}

		private void OnInsertText(object sender, Gtk.InsertTextArgs args) {
			CheckRegion();
		}

		private void OnDeleteRange(object sender, Gtk.DeleteRangeArgs args) {
			CheckRegion();
		}

		private void CheckRegion() {
			try {
				//Console.WriteLine("Trying note " + Note.Title);
				if(Note.Buffer.Text.IndexOf(PATTERN, 0) > -1) {
					if(GlobalTodo.Buffer.Text.IndexOf(Note.Title, 0) == -1){
						// insert note title in the global todo note
						//Console.WriteLine("Inserting " + Note.Title + " in global todo");
						GlobalTodo.Buffer.PlaceCursor(GlobalTodo.Buffer.EndIter);
						GlobalTodo.Buffer.InsertAtCursor(Note.Title + "\n");
					}
				}
				else {
					// this note doesn't have the #TODO tag, should not get an entry in the GLOBAL TODO note
					if(GlobalTodo.Buffer.Text.IndexOf(Note.Title, 0) > -1){						
						//Console.WriteLine("Note " + Note.Title + " doesn't hava the tag.");
						TextIter start, end;
						GlobalTodo.Buffer.StartIter.ForwardSearch(Note.Title, Gtk.TextSearchFlags.TextOnly, out start, out end, GlobalTodo.Buffer.EndIter);
						end.ForwardChar();
					    GlobalTodo.Buffer.Delete(ref start, ref end);							
					}
				}
			} catch (Exception e) {
				Console.WriteLine("Error at CheckRegion: " + e);
			}
		}
	}
}
