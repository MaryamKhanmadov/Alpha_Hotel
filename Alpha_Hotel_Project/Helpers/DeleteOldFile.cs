namespace Alpha_Hotel_Project.Helpers
{
    public static class DeleteOldFile
    {
        public static void DeleteOld(this string filename, string rootpath, string foldername)
        {
            string path = Path.Combine(rootpath, foldername, filename);
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
