using Microsoft.AspNetCore.Http;

namespace SharghPc.Application
{
    public static class SaveFile
    {
        public static async Task SaveFileToServer(this IFormFile file, string path, string FileName)
        {
            if (file != null)
            {
                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

                string pathfile = path + FileName;

                using (var stream = new FileStream(pathfile, FileMode.Create))
                {
                    if (!Directory.Exists(pathfile)) await file.CopyToAsync(stream);
                }
            }


        }
    }
}
