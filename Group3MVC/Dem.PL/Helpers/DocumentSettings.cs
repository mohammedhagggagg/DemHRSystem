namespace Dem.PL.Helpers
{
    public static class DocumentSettings
    {
        //Upload
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            // 2. Get File Name and Make it Unique 
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            // 3. Get File Path [Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);
            // 4. Save File As Streams
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            // 5. Return File Name
            return FileName;
        }
        //Delete 
        public static void DeleteFile(string FileName, string FolderName)
        {
            // 1. Get Located Folder Path 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            // 2. Get File Path [Folder Path + FileName]
            string FilePAth = Path.Combine(FolderPath, FileName);
            // 3. Check If File Exists
            if (File.Exists(FilePAth))
            {
                // 4. Delete File
                File.Delete(FilePAth);
            }

        }
    }
}
