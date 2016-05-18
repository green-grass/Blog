using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PhantomNet.Entities.EntityMarkers;

namespace PhantomNet.Blog
{
    public class Article : Article<Article, Category, Blogger> { }

    public class Article<TArticle, TCategory, TBlogger>
        : Article<TArticle, TCategory, TBlogger, int>
        where TArticle : Article<TArticle, TCategory, TBlogger>
        where TCategory : Category<TArticle, TCategory, TBlogger>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger>
    {
        public virtual int? CategoryId { get; set; }

        public virtual int? BloggerId { get; set; }
    }

    // Entity
    public partial class Article<TArticle, TCategory, TBlogger, TKey>
        : IIdWiseEntity<TKey>,
          IConcurrencyStampWiseEntity,
          ITimeWiseEntity,
          IIsActiveWiseEntity
        where TArticle : Article<TArticle, TCategory, TBlogger, TKey>
        where TCategory : Category<TArticle, TCategory, TBlogger, TKey>
        where TBlogger : Blogger<TArticle, TCategory, TBlogger, TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual DateTime DataCreateDate { get; set; }

        public virtual DateTime DataLastModifyDate { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string Language { get; set; }

        public virtual DateTime PublishDate { get; set; }

        public virtual string Author { get; set; }

        public virtual string SourceUrl { get; set; }

        public virtual string Title { get; set; }

        public virtual string UrlFriendlyTitle { get; set; }

        public virtual string ShortContent { get; set; }

        public virtual string Content { get; set; }

        public virtual string Tags { get; set; }

        public virtual string Series { get; set; }

        public virtual string DescriptionMeta { get; set; }

        public virtual string KeywordsMeta { get; set; }

        public virtual string Filters { get; set; }

        public virtual int? ViewCount { get; set; }
    }

    // Relationships
    partial class Article<TArticle, TCategory, TBlogger, TKey>
    {
        public virtual TCategory Category { get; set; }

        public virtual TBlogger Blogger { get; set; }

        public virtual ICollection<TArticle> RelatedArticles { get; set; }
    }

    // Customization
    partial class Article<TArticle, TCategory, TBlogger, TKey>
    {
        private StringProcessor stringProcessor { get; } = new StringProcessor();

        public string ProcessedShortContent => ShortContent == null ? null : ProcessContent(ShortContent);

        public virtual string PlainShortContent => stringProcessor.RemoveHtmlTags(ShortContent).Replace("\n", " ");

        public string ProcessedContent => Content == null ? null : ProcessContent(Content);

        #region Helpers

        protected virtual string ProcessContent(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            //if (Images == null) { return null; }

            //// Images
            //var images = Images.ToList();
            //var matches = Regex.Matches(content, @"\[image\s+name\s*=\s*('\s*((\w|\-)+.\w+)\s*'|""\s*((\w|\-)+.\w+)\s*"")(\s+alt=\s*('\s*([\s\S]*?)\s*'|""\s*([\s\S]*?)\s*""))*(\s+caption=\s*('\s*([\s\S]*?)\s*'|""\s*([\s\S]*?)\s*""))*(\s+description=\s*('\s*([\s\S]*?)\s*'|""\s*([\s\S]*?)\s*""))*\s*\]");
            //foreach (Match match in matches)
            //{
            //    var name = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[4].Value;
            //    var alt = match.Groups[8].Success ? match.Groups[8].Value : match.Groups[9].Value;
            //    var caption = match.Groups[12].Success ? match.Groups[12].Value : match.Groups[13].Value;
            //    var description = match.Groups[16].Success ? match.Groups[16].Value : match.Groups[17].Value;
            //    var src = UrlHelper.GenerateContentUrl(Path.Combine(VirtualPath, name),
            //        HttpContext.Current.Request.RequestContext.HttpContext);
            //    if (images.Contains(Path.Combine(VirtualPath, name)))
            //    {
            //        var replacement = string.Format(@"<figure><img alt=""{1}"" caption=""{2}"" description=""{3}"" src=""{0}"" /></figure>", src, alt, caption, description);
            //        content = content.Replace(match.Value, replacement);
            //    }
            //    else
            //    {
            //        content = content.Replace(match.Value, string.Empty);
            //    }
            //}

            // Captions
            var matches = Regex.Matches(content, @"\[label\][\s\S]*?\[\/label\]");
            foreach (Match match in matches)
            {
                var replacement = new Regex(@"<\/div>\s*<div>").Replace(match.Value, "<br />");
                content = content.Replace(match.Value, replacement);
            }
            content = content.Replace("[label]", "<figure><figcaption>");
            content = content.Replace("[/label]", "</figcaption></figure>");
            content = content.Replace("</figure><figure><figcaption>", "<figcaption>");

            // Youtube
            matches = Regex.Matches(content, @"\[youtube\]([a-zA-Z0-9]{11})\[\/youtube\]");
            foreach (Match match in matches)
            {
                var replacement = string.Format(@"<div class=""video-wrapper""><div class=""video-container""><iframe src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div></div>", match.Groups[1].Value);
                content = content.Replace(match.Value, replacement);
            }

            // Asides
            content = content.Replace("[aside]", "<div class=\"aside\">");
            content = content.Replace("[/aside]", "</div>");

            return content;
        }

        protected virtual bool DetectInvalidFormatting(string content)
        {
            if (content == null) { return false; }

            var invalidStrings = new string[]{
                @"<p class=""MsoNormal""",
                @"<span lang=""EN-GB""",
                @"<o:p>"
            };

            foreach (var item in invalidStrings)
            {
                if (content.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
