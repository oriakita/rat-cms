namespace Raco.Blazor.DropzoneJs
{
    using System;
    using System.IO;

    public class UploadComponentModal
    {
        public Guid Id { get; set; }

        public string OriginalFileName { get; set; }

        public string S3FileName
        {
            get => $"{Id}{Path.GetExtension(OriginalFileName)}";
        }

        public double FileSize { get; set; }

        public string AbsolutePath { get; set; }

        public bool IsTemporaryFile { get; set; }

        public bool NeedToDelete { get; set; }

        public bool IsUploading { get; set; }

        public bool IsImage
        {
            get
            {
                switch (Path.GetExtension(OriginalFileName).ToLower())
                {
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".svg":
                    case ".heic":
                    case ".mov":
                    case ".plist":
                    case ".webp":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsVideo
        {
            get
            {
                switch (Path.GetExtension(OriginalFileName).ToLower())
                {
                    case ".mp4":
                    case ".webm":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsGifImage
        {
            get
            {
                switch (Path.GetExtension(OriginalFileName).ToLower())
                {
                    case ".gif":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public int SortOrder { get; set; }

        public string Title { get; set; } = string.Empty;

        public string AlternateText { get; set; } = string.Empty;

        public string SignedUrl { get; set; } = string.Empty;
    }
}
