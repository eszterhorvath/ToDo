using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace ToDo.Core.Services
{
    public interface IMediaService
    {
        Task<MediaFile> CapturePhoto();
    }
}
