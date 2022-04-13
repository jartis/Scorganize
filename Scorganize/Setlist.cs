using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorganize
{
    public class Setlist
    {
        public List<SetlistEntry> entries;

        public Setlist()
        {
            entries = new List<SetlistEntry>();
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

        public SetlistEntry(string title, string filename, int start, int num)
        {
            Title = title;
            Filename = filename;
            StartPage = start;
            NumPages = num;
        }
    }
}
