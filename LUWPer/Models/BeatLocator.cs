using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LUWPer.Models
{
    static class BeatLocator
    {
       
        public static async Task<List<BeatInfo>> FindBeats()
        {
            var beats = new List<BeatInfo>();
            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            string path = root + @"\Assets\Loops";
            var folder = await StorageFolder.GetFolderFromPathAsync(path);
            foreach (var file in await folder.GetFilesAsync())
            {
                beats.Add(new BeatInfo(file.DisplayName, file.Path));
            }
            return beats;
        }
    }
}
