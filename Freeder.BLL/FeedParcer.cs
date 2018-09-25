using Feeder.DAL;
using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Freeder.BLL
{
    public static class FeedParcer
    {
        public static IEnumerable<Feed> Parce(XDocument feedXML, string Type)
        {
            switch (Type)
            {
                case "RSS":
                    {
                        try
                        {
                            return from feed in feedXML.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                                   select new Feed()
                                   {
                                       Type = FeedType.RSS,
                                       Content = feed.Elements().First(i => i.Name.LocalName == "description").Value,
                                       Link = feed.Elements().First(i => i.Name.LocalName == "link").Value,
                                       PublishDate = feed.Elements().First(i => i.Name.LocalName == "pubDate").Value,
                                       Title = feed.Elements().First(i => i.Name.LocalName == "title").Value,
                                   };
                        }
                        catch
                        {
                            return null;
                        }
                    }
                case "Atom"://not tested
                    {
                        try
                        {
                            return from feed in feedXML.Root.Elements().Where(i => i.Name.LocalName == "entry")
                                   select new Feed()
                                   {
                                       Type = FeedType.Atom,
                                       Content = feed.Elements().First(i => i.Name.LocalName == "content").Value,
                                       Link = feed.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                                       PublishDate = feed.Elements().First(i => i.Name.LocalName == "published").Value,
                                       Title = feed.Elements().First(i => i.Name.LocalName == "title").Value,
                                   };
                        }
                        catch
                        {
                            return null;
                        }
                    }
                default: return null;
            }
        }
    }
}
