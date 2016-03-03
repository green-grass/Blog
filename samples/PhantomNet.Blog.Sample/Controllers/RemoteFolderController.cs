using PhantomNet.RemoteFolder;

namespace PhantomNet.Blog.Sample.Controllers
{
    public class RemoteFolderController : RemoteImageFolderControllerBase
    {
        public RemoteFolderController(RemoteImageFolderService service)
            : base(service)
        { }

        protected override string Path => "~/images";
    }
}
