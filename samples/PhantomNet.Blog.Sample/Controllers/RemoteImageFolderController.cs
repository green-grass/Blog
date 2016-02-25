using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using PhantomNet.RemoteFolder;

namespace PhantomNet.Blog.Sample.Controllers
{
    [Produces("application/json")]
    [Route("api/remote-image-folder")]
    public class RemoteImageFolderController : Controller
    {
        private readonly RemoteImageFolderService<RemoteImage> _remoteFolder;

        private readonly string _virtualPath = "~/content/blog/images";
        private readonly string _thumbsVirtualPath = "~/content/blog/thumbs";

        public RemoteImageFolderController(RemoteImageFolderService<RemoteImage> remoteFolder)
        {
            _remoteFolder = remoteFolder;
        }

        [HttpGet]
        public IEnumerable<RemoteImage> GetImages()
        {
            return _remoteFolder.List(_virtualPath, _thumbsVirtualPath, true);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> GetImage([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            var image = await Task.FromResult(new RemoteImage());

            if (image == null)
            {
                return HttpNotFound();
            }

            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(ICollection<IFormFile> files)
        {
            var result = await _remoteFolder.UploadAsync(files, _virtualPath);
            return CreatedAtRoute("GetImage", new { id = "abc" }, result);
        }
    }
}
