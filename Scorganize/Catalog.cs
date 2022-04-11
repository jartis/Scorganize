using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scorganize
{
    public class Catalog
    {
        public Catalog()
        {
            Songbooks = new List<Songbook>();
            Version = 1;
        }

        public Catalog(int version, List<Songbook> songbooks)
        {
            Version = version;
            Songbooks = songbooks;
        }

        public List<Songbook> Songbooks { get; set; }

        public int Version { get; set; }


        public void Save(string catFile)
        {
            string catalogString = JsonSerializer.Serialize(this);
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(catFile)))
            {
                sw.Write(catalogString);
                sw.Flush();
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
            return (JsonSerializer.Deserialize<Catalog>(jsonString) ?? new Catalog());
        }
    }

    public class Songbook
    {
        public string Filename { get; set; }

        public string Title { get; set; }

        public List<Song> Songs { get; set; }

        public Songbook()
        {
            Filename = "";
            Title = "";
            Songs = new List<Song>();
        }

        public void Add(Song song)
        {
            Songs.Add(song);
        }

        public static Songbook FromFile(string filename)
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
                                };
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
                            book.Add(new Song(pageNum, title, artist));
                        }
                    }

                    return book;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return null;
            }
        }
    }

    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Page { get; set; }

        public Song(int page, string title = "", string artist = "")
        {
            Page = page;
            Title = title;
            Artist = artist;
        }
    }

    public class TreeTag
    {
        public string Filename { get; set; }
        public int Page { get; set; }

        public TagType tagType { get; set; }

        public TreeTag(string filename, int page, TagType tt)
        {
            Filename = filename;
            Page = page;
            tagType = tt;
        }
    }

    public enum TagType
    {
        Song,
        Book,
    }
}
