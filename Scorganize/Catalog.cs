using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scorganize
{
    public class Catalog
    {
        public Catalog()
        {
            Songbooks = new List<Songbook>();
            Version = 2;
        }

        public Catalog(int version, List<Songbook> songbooks)
        {
            Version = version;
            Songbooks = songbooks;
        }

        public List<Songbook> Songbooks { get; set; }

        public int Version { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int BookCount { get { return Songbooks.Count; } }

        public void Save(string catFile)
        {
            try
            {
                string catalogString = JsonSerializer.Serialize(this);
                using (StreamWriter sw = new StreamWriter(File.Open(catFile, FileMode.Create)))
                {
                    sw.Write(catalogString);
                    sw.Flush();
                }
            }
            catch
            {
                MessageBox.Show("Error saving catalog");
            }
        }

        public static Catalog Load(string catFile)
        {
            string jsonString = "";
            if (!File.Exists(catFile))
            {
                return new Catalog();
            }
            using (StreamReader r = new StreamReader(catFile))
            {
                jsonString = r.ReadToEnd();
            }
            Catalog c;
            try
            {
                c = JsonSerializer.Deserialize<Catalog>(jsonString) ?? new Catalog();
            }
            catch
            {
                File.Copy(catFile, catFile + ".bak");
                MessageBox.Show(String.Format("Error loading catalog file. A backup has been made at {0}.", catFile + ".bak"));
                return new Catalog();
            }
            c.Songbooks.Sort();
            return (c);
        }

        internal Songbook BookFromFilename(string filename)
        {
            return Songbooks.First(b => b.Filename == filename);
        }

        public void Remove(Songbook book)
        {
            Songbooks.Remove(book);
        }

        internal bool HasFile(string file)
        {
            return (Songbooks.Any(b => b.Filename == file));
        }

        internal void Add(Songbook book)
        {
            Songbooks.Add(book);
            Songbooks.Sort();
        }
    }

    public class Songbook : IComparable
    {
        public string Filename { get; set; }

        public string Title { get; set; }

        public List<Song> Songs { get; set; }

        public int pageCount { get; set; }

        public Songbook()
        {
            Filename = "";
            Title = "";
            Songs = new List<Song>();
            pageCount = 0;
        }

        public Songbook(string filename, string title, List<Song> songs, int pagecount)
        {
            Filename = filename;
            Title = title;
            Songs = songs;
            pageCount = pagecount;
        }

        public void Add(Song song)
        {
            Songs.Add(song);
            Songs.Sort();
        }

        public void Remove(Song song)
        {
            Songs.Remove(song);
            Songs.Sort();
        }

        public void Remove(string title)
        {
            Songs.Remove(Songs.First(s => s.Title == title));
            Songs.Sort();
        }

        public bool ReplaceBookmarksInFile()
        {
            try
            {
                // HACKY; I don't like this
                FileInfo myFile = new FileInfo(Filename);
                myFile.IsReadOnly = false;
                FileStream docStream = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite);
                PdfDocument document = PdfReader.Open(docStream);
                docStream.Close();
                document.Outlines.Clear();
                foreach (Song song in Songs.OrderBy(s => s.FirstPage))
                {
                    int bookPage = Math.Min(Math.Max(song.FirstPage, 0), pageCount);
                    document.Outlines.Add(new PdfOutline(song.Title, document.Pages[bookPage]));
                }
                document.Save(Filename);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Songbook? FromFile(string filename)
        {
            try
            {
                Songbook book = new Songbook();
                book.Filename = filename;
                book.Title = Path.GetFileNameWithoutExtension(filename);

                // Parse out the titles and pages and all of that whatnot
                using (FileStream docStream = File.OpenRead(filename))
                {
                    PdfDocument document;
                    document = PdfReader.Open(docStream, PdfDocumentOpenMode.ReadOnly);
                    book.pageCount = document.Pages.Count;
                    PdfOutlineCollection bookmarks = document.Outlines;
                    foreach (PdfOutline bookmark in bookmarks)
                    {
                        int pageNum = -1;
                        if (bookmark.DestinationPage != null)
                        {
                            if (bookmark.DestinationPage.Elements.ContainsKey("/StructParents"))
                            {
                                if (int.TryParse(bookmark.DestinationPage.Elements["/StructParents"].ToString(), out pageNum))
                                {
                                    pageNum += 1; // Zero-based
                                }
                            } else {
                                for (int i = 0; i < document.PageCount; i++)
                                {
                                    if (document.Pages[i].Equals(bookmark.DestinationPage))
                                    {
                                        // Hey it's a success!
                                        pageNum = i+1;
                                        break;
                                    }
                                }
                            }
                        }
                        if (pageNum > -1)
                        {
                            string bookmarkTitle = bookmark.Title;
                            string title = bookmark.Title;
                            string artist = "";
                            // TODO: Fix this so it's not gross
                            //if (bookmarkTitle.Contains(" - "))
                            //{
                            //    // Cut it at the hyphen with the spaces. Won't be perfect but works for right now.
                            //    // Right now we assume that it's 
                            //    var titleParts = bookmarkTitle.Split(" - ", 2, StringSplitOptions.TrimEntries);
                            //    title = titleParts[0];
                            //    artist = titleParts[1];
                            //}
                            book.Add(new Song(pageNum, 1, title, artist));
                        }
                    }
                    book.Songs.Sort();
                    book.FudgePageNumbers();
                    return book;
                }
            }
            catch
            {
                // Log the error
                return null;
            }
        }

        private void FudgePageNumbers()
        {
            for (int i = 0; i < Songs.Count - 1; i++)
            {
                Songs[i].NumPages = Math.Max(Songs[i + 1].FirstPage - Songs[i].FirstPage, 1);
            }
        }

        internal bool HasMarkerAt(int curPage)
        {
            return Songs.Any(s => s.FirstPage == curPage);
        }

        internal void AdjustPageNumbers(int change)
        {
            foreach (Song s in Songs)
            {
                s.FirstPage = Math.Min(pageCount, Math.Max(1, s.FirstPage + change));
            }
            Songs.Sort();
        }

        public int CompareTo(object? obj)
        {
            if (obj is Songbook)
            {
                return Title.CompareTo(((Songbook)obj).Title);
            }
            return Title.CompareTo(obj);
        }

        internal void Clear()
        {
            Songs.Clear();
        }

        internal Song SongFromTitle(string title)
        {
            return Songs.First(s => s.Title == title);
        }

        internal Song SongFromPage(int page)
        {
            return Songs.First(s => s.FirstPage == page);
        }
    }

    public class Song : IComparable
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int FirstPage { get; set; }

        public int NumPages { get; set; }

        public Song(int firstpage, int numpages, string title = "", string artist = "")
        {
            FirstPage = firstpage;
            Title = title;
            Artist = artist;
            NumPages = numpages;
        }

        public int CompareTo(object? obj)
        {
            if (obj is Song)
            {
                return FirstPage.CompareTo(((Song)obj).FirstPage);
            }
            return FirstPage.CompareTo(obj);
        }
    }

    public class TreeTag
    {
        public string Title { get; set; }
        public string Filename { get; set; }
        public int Page { get; set; }
        public int NumPages { get; set; }

        public TagType tagType { get; set; }

        public TreeTag(string title, string filename, int page, int numpages, TagType tt)
        {
            Title = title;
            Filename = filename;
            Page = page;
            NumPages = numpages;
            tagType = tt;
        }
    }

    public enum TagType
    {
        None,
        Song,
        Book,
    }
}
