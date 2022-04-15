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

        public int Version { get; set; }

        public Setlist()
        {
            Entries = new List<SetlistEntry>();
            Version = 1;
        }

        public Setlist(List<SetlistEntry> entries, int version)
        {
            this.Entries = entries;
            this.Version = version;
        }

        public void Add(SetlistEntry entry)
        {
            entry.SetlistPage = GetNextPageNum();
            Entries.Add(entry);
        }

        public void Add(Song song, Songbook book)
        {
            //int songidx = book.Songs.IndexOf(song);
            //SetlistEntry entry = new SetlistEntry(song.Title, book.Filename, )
        }

        public int GetNextPageNum()
        {
            return Entries.Sum(s => s.NumPages);
        }

        public void ResetPageNumbers()
        {
            int pageNum = 1;
            foreach (SetlistEntry setlistEntry in Entries)
            {
                setlistEntry.SetlistPage = pageNum;
                pageNum += setlistEntry.NumPages;
            }
        }
    }

    public class SetlistEntry
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public int StartPage { get; set; }
        public int NumPages { get; set; }

        public int SetlistPage { get; set; }

        public SetlistEntry()
        {
            Title = String.Empty;
            Filename = String.Empty;
            StartPage = 0;
            NumPages = 0;
            SetlistPage = 0;
        }

        public SetlistEntry(string title, string filename, int startpage, int numpages, int setlistpage)
        {
            Title = title;
            Filename = filename;
            StartPage = startpage;
            NumPages = numpages;
            SetlistPage = setlistpage;
        }
    }
}
