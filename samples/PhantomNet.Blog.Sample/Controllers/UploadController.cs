#if DNX451
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PhantomNet.Blog.Sample.Controllers
{
    public class UploadController : Controller
    {
        private const string Path = "~/images";
        private const int MaxImageWidth = 500;

        private readonly WebHelper _webHelper;

        public UploadController(
            IApplicationEnvironment appEnvironment,
            IUrlHelper urlHelper,
            IHttpContextAccessor contextAccessor)
        {
            _webHelper = new WebHelper(appEnvironment, urlHelper, contextAccessor);
        }

        public virtual JsonResult ImageList()
        {
            return Json(_webHelper.ImageList(Path));
        }

        [HttpPost]
        public virtual ActionResult UploadImage()
        {
            var ret = _webHelper.UploadImage(Path, maxWidth: MaxImageWidth);
            if (ret is string)
            {
                return Content((string)ret);
            }
            else
            {
                return Json(ret);
            }
        }

        [HttpPost]
        public virtual JsonResult RenameImage(string fileName, string newName)
        {
            return Json(_webHelper.RenameImage(Path, fileName, newName));
        }

        [HttpPost]
        public virtual JsonResult DeleteImage(string fileName)
        {
            return Json(_webHelper.DeleteImage(Path, fileName));
        }
    }

    public class WebHelper
    {
        protected readonly IApplicationEnvironment _appEnvironment;

        protected readonly IUrlHelper _url;

        protected readonly HttpContext _context;

        public WebHelper(
            IApplicationEnvironment appEnvironment,
            IUrlHelper urlHelper,
            IHttpContextAccessor contextAccessor)
        {
            _appEnvironment = appEnvironment;
            _url = urlHelper;
            _context = contextAccessor.HttpContext;
        }

        public object ImageList(string folder,
            bool includeImageDimensions = false,
            Func<IUrlHelper, string, string> formatThumb = null)
        {
            object ret;
            var path = MapPath(folder);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var urlHelper = _url;
                var list = new List<object>();
                Directory.GetFiles(path).ToList().ForEach(
                    file => {
                        if (includeImageDimensions)
                        {
                            Size size;
                            using (var image = Image.FromFile(file))
                            {
                                size = image.Size;
                            }
                            list.Add(new {
                                index = list.Count,
                                fileName = Path.GetFileName(file),
                                fileSize = new FileInfo(file).Length,
                                url = urlHelper.Content(Path.Combine(folder, Path.GetFileName(file))),
                                thumbUrl = formatThumb == null ? default(string) : formatThumb(urlHelper, file),
                                width = size.Width,
                                height = size.Height
                            });
                        }
                        else
                        {
                            list.Add(new {
                                index = list.Count,
                                fileName = Path.GetFileName(file),
                                fileSize = new FileInfo(file).Length,
                                url = urlHelper.Content(Path.Combine(folder, Path.GetFileName(file))),
                                thumbUrl = formatThumb == null ? default(string) : formatThumb(urlHelper, file)
                            });
                        }
                    }
                );
                ret = new { success = true, items = list };
            }
            catch
            {
                ret = new { success = false };
            }
            return ret;
        }

        public object UploadFile(string folder, string partsFolder = null,
            string fileNameWithoutExtension = null, bool urlFriendly = true,
            int? fileIndex = null)
        {
            if ((string)_context.Request.Headers["X-Part-Size"] != null)
            {
                partsFolder = partsFolder ?? folder;
                var success = SaveFilePart(folder, partsFolder,
                    fileNameWithoutExtension: fileNameWithoutExtension,
                    urlFriendly: urlFriendly);
                //return new JavaScriptSerializer().Serialize(new { success = success });
                return ToJson(new { success = success });
            }
            else
            {
                object ret;
                var files = _context.Request.Form.Files;
                if (files.Count() == 0)
                {
                    ret = new { success = false };
                }
                else
                {
                    try
                    {
                        bool success;
                        if (fileIndex.HasValue)
                        {
                            success = SaveFile(folder,
                                    files.Skip(fileIndex.Value).First(),
                                    fileNameWithoutExtension: fileNameWithoutExtension,
                                    urlFriendly: urlFriendly);
                        }
                        else
                        {
                            success = true;
                            foreach (var file in files)
                            {
                                success = success && SaveFile(folder,
                                    file,
                                    fileNameWithoutExtension: fileNameWithoutExtension,
                                    urlFriendly: urlFriendly);
                            }
                        }
                        ret = new { success = success };
                    }
                    catch
                    {
                        ret = new { success = false };
                    }
                }
                if (_context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return ret;
                }
                else
                {
                    //return new JavaScriptSerializer().Serialize(ret);
                    return ToJson(ret);
                }
            }
        }

        public object UploadImage(string folder, string fileNameWithoutExtension = null,
            bool cropToSquare = false, int? maxWidth = null, int? maxHeight = null)
        {
            object ret;
            var files = _context.Request.Form.Files;
            if (files.Count() == 0)
            {
                ret = new { success = false };
            }
            else
            {
                try
                {
                    bool success = true;
                    foreach (var file in files)
                    {
                        success = success && SaveImage(folder,
                            file,
                            fileNameWithoutExtension: fileNameWithoutExtension,
                            cropToSquare: cropToSquare, maxWidth: maxWidth, maxHeight: maxHeight);
                    }
                    ret = new { success = success };
                }
                catch
                {
                    ret = new { success = false };
                }
            }
            if (_context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return ret;
            }
            else
            {
                //return new JavaScriptSerializer().Serialize(ret);
                return ToJson(ret);
            }
        }

        public object RenameImage(string folder, string fileName, string newName)
        {
            object ret;
            var path = MapPath(Path.Combine(folder, fileName));
            var newPath = MapPath(Path.Combine(folder, newName));
            if (File.Exists(path))
            {
                try
                {
                    File.Move(path, newPath);
                    ret = new { success = true, newFileName = Path.GetFileName(newPath) };
                }
                catch
                {
                    ret = new { success = false };
                }
            }
            else
            {
                ret = new { success = false, fileNotFound = true };
            }
            return ret;
        }

        public object DeleteImage(string folder, string fileName)
        {
            object ret;
            var path = MapPath(Path.Combine(folder, fileName));
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    ret = new { success = true };
                }
                catch
                {
                    ret = new { success = false };
                }
            }
            else
            {
                ret = new { success = true };
            }
            return ret;
        }

        public object ClearImages(string folder)
        {
            object ret;
            var path = MapPath(folder);
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.GetFiles(path).ToList().ForEach(file => File.Delete(file));
                }
                ret = new { success = true };
            }
            catch
            {
                ret = new { success = false };
            }
            return ret;
        }

        private string MapPath(string virtualPath)
        {
            virtualPath = _url.Content(virtualPath).TrimStart('/').Replace('/', '\\');
            return Path.Combine(_appEnvironment.ApplicationBasePath, "wwwroot", virtualPath);
        }

        private bool SaveImage(string folder, IFormFile file, string fileNameWithoutExtension = null,
            bool cropToSquare = false, int? maxWidth = null, int? maxHeight = null)
        {
            // TODO:: FileName returns "fileName.ext"(with double quotes) in beta 3
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            // Path
            var extension = Path.GetExtension(originalFileName);
            if (extension == ".gif")
            {
                extension = "gif";
            }
            else
            {
                extension = "jpg";
            }
            var fileName = string.Format("{0}.{1}",
                fileNameWithoutExtension = ToUrlFriendly(fileNameWithoutExtension ?? Path.GetFileNameWithoutExtension(originalFileName)),
                extension);
            var path = MapPath(folder);
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch { return false; }
            }
            string fullPath;
            if (File.Exists(fullPath = Path.Combine(path, fileName)))
            {
                var index = 1;
                while (File.Exists(fullPath = Path.Combine(path, string.Format("{0}-{1}.{2}", fileNameWithoutExtension, index, extension))))
                {
                    index++;
                }
            }

            // Load image
            Image image;
            try { image = Image.FromStream(file.OpenReadStream()); }
            catch { return false; }

            // Size
            if (cropToSquare)
            {
                image = ImageHelper.SquareImage(image);
            }
            if (maxWidth != null || maxHeight != null)
            {
                image = ImageHelper.ScaleImage(image, maxWidth, maxHeight);
            }

            // Encoding
            if (extension == "gif")
            {
                try { image.Save(fullPath, ImageFormat.Gif); }
                catch { return false; }
            }
            else
            {
                var encoder = ImageCodecInfo.GetImageEncoders()
                    .Where(m => m.FormatID == ImageFormat.Jpeg.Guid).FirstOrDefault();
                var parameters = new EncoderParameters(1);
                var parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                parameters.Param[0] = parameter;
                try { image.Save(fullPath, encoder, parameters); }
                catch { return false; }
            }

            return true;
        }

        private bool SaveFile(string folder, IFormFile file,
            string fileNameWithoutExtension = null, bool urlFriendly = true)
        {
            // TODO:: FileName returns "fileName.ext"(with double quotes) in beta 3
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            var path = MapPath(folder);
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch { return false; }
            }

            fileNameWithoutExtension = fileNameWithoutExtension ?? Path.GetFileNameWithoutExtension(originalFileName);
            if (urlFriendly)
            {
                fileNameWithoutExtension = ToUrlFriendly(fileNameWithoutExtension);
            }
            var extension = Path.GetExtension(originalFileName);
            var fileName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
            string fullPath;
            if (File.Exists(fullPath = Path.Combine(path, fileName)))
            {
                var index = 1;
                while (File.Exists(fullPath = Path.Combine(path, string.Format("{0}-{1}{2}", fileNameWithoutExtension, index, extension))))
                {
                    index++;
                }
            }
            try { file.SaveAs(fullPath); }
            catch { return false; }
            return true;
        }

        private bool SaveFilePart(string folder, string partsFolder,
            string fileNameWithoutExtension = null, bool urlFriendly = true)
        {
            var partPath = MapPath(partsFolder);
            if (!Directory.Exists(partPath))
            {
                try { Directory.CreateDirectory(partPath); }
                catch { return false; }
            }

            var fileId = WebUtility.UrlDecode(_context.Request.Headers["X-File-ID"]);
            var partIndex = int.Parse(_context.Request.Headers["X-Part-Index"]);
            var partCount = int.Parse(_context.Request.Headers["X-Part-Count"]);
            var partSize = int.Parse(_context.Request.Headers["X-Part-Size"]);
            var buff = new byte[partSize];
            var partFullPath = Path.Combine(partPath, fileId);

            _context.Request.Body.Read(buff, 0, buff.Length);
            try
            {
                var stream = File.OpenWrite(partFullPath);
                stream.Position = stream.Length;
                stream.Write(buff, 0, buff.Length);
                stream.Close();
            }
            catch { return false; }

            if (partIndex + 1 < partCount)
            {
                return true;
            }

            // Save the completed file
            var path = MapPath(folder);
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch { return false; }
            }

            var fileName = WebUtility.UrlDecode(_context.Request.Headers["X-File-Name"]);
            fileNameWithoutExtension = fileNameWithoutExtension ?? Path.GetFileNameWithoutExtension(fileName);
            if (urlFriendly)
            {
                fileNameWithoutExtension = ToUrlFriendly(fileNameWithoutExtension);
            }
            var extension = Path.GetExtension(fileName);
            fileName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
            string fullPath;
            if (File.Exists(fullPath = Path.Combine(path, fileName)))
            {
                var index = 1;
                while (File.Exists(fullPath = Path.Combine(path, string.Format("{0}-{1}{2}", fileNameWithoutExtension, index, extension))))
                {
                    index++;
                }
            }
            try
            {
                File.Move(partFullPath, fullPath);
            }
            catch { return false; }
            return true;
        }

        private string ToAscii(string source)
        {
            string unicode = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴđĐ",
                ascii = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYdD";

            for (var i = 0; i < unicode.Length; i++)
            {
                source = source.Replace(unicode[i], ascii[i]);
            }
            return source;
        }

        private string ToUrlFriendly(string source)
        {
            source = ToAscii(source).Trim().ToLower();
            source = source.Replace(' ', '-');
            source = source.Replace("&nbsp;", "-");
            source = new Regex("[^0-9a-z-]").Replace(source, string.Empty);
            while (source.IndexOf("--") > -1)
            {
                source = source.Replace("--", "-");
            }
            return source;

        }

        private string ToJson(object value)
        {
            var writer = new StringWriter();
            new JsonOutputFormatter().WriteObject(writer, value);
            return writer.ToString();
        }
    }

    public static class ImageHelper
    {
        public static Image SquareImage(Image image)
        {
            var size = Math.Min(image.Width, image.Height);
            var newImage = new Bitmap(size, size);
            using (var graphic = Graphics.FromImage(newImage))
            {
                graphic.DrawImageUnscaledAndClipped(image, new Rectangle(0, 0, size, size));
            }
            return newImage;
        }

        public static Image ScaleImage(Image image, int? maxWidth, int? maxHeight)
        {
            var ratioX = maxWidth == null ? double.MaxValue : (double)maxWidth.Value / image.Width;
            var ratioY = maxHeight == null ? double.MaxValue : (double)maxHeight.Value / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            if (ratio >= 1)
            {
                return image;
            }

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphic = Graphics.FromImage(newImage))
            {
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ToSize(Image image, int width, int height)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt32((width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt32((height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = Convert.ToInt32(sourceWidth * nPercent);
            int destHeight = Convert.ToInt32(sourceHeight * nPercent);
            if (width - destWidth <= 2)
            {
                destWidth = width;
                destX = 0;
            }
            if (height - destHeight <= 2)
            {
                destHeight = height;
                destY = 0;
            }

            return DrawImage(image, width, height, sourceX, sourceY, sourceWidth, sourceHeight,
                destX, destY, destWidth, destHeight, true);
        }

        private static Bitmap DrawImage(Image image, int width, int height,
            int sourceX, int sourceY, int sourceWidth, int sourceHeight,
            int destX, int destY, int destWidth, int destHeight,
            bool clear = false)
        {
            using (var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var grPhoto = Graphics.FromImage(bmPhoto))
                {
                    if (clear)
                    {
                        grPhoto.Clear(Color.White);
                    }
                    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grPhoto.DrawImage(image,
                        new Rectangle(destX, destY, destWidth, destHeight),
                        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                        GraphicsUnit.Pixel);
                }
                return bmPhoto;
            }
        }
    }
}
#endif
