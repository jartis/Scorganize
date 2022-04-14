using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorganize
{
    public class Setlist
    {
        public List<SetlistEntry> Entries { get; set; }

        public Setlist()
        {
            Entries = new List<SetlistEntry>();
        }

        public Setlist(List<SetlistEntry> entries)
        {
            this.Entries = entries;
        }

        public void Add(SetlistEntry entry)
        {
            Entries.Add(entry);
        }

        public void Add(Song song, Songbook book)
        {
            //int songidx = book.Songs.IndexOf(song);
            //SetlistEntry entry = new SetlistEntry(song.Title, book.Filename, )
        }
    }

    public class SetlistEntry
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public int StartPage { get; set; }
        public int NumPages { get; set; }

        public SetlistEntry()
        {
            Title = String.Empty;
            Filename = String.Empty;
            StartPage = 0;
            NumPages = 0;
        }

        public SetlistEntry(string title, string filename, int startpage, int numpages)
        {
            Title = title;
            Filename = filename;
            StartPage = startpage;
            NumPages = numpages;
        }
    }
}
