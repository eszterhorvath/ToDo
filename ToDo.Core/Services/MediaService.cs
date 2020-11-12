using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;

namespace ToDo.Core.Services
{
    public class MediaService : IMediaService
    {
        public async Task<MediaFile> CapturePhoto()
        {
            await CrossMedia.Current.Initialize();

            var options = new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 1000
            };

            return await CrossMedia.Current.TakePhotoAsync(options);
        }
    }
}
