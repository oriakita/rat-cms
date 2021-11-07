namespace Raco.Blazor.DropzoneJs
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class DropzoneJs<IModal> where IModal : UploadComponentModal
    {
        private const string TEMPORARY_FOLDER = "temporary";

        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string DestinationFolder { get; set; }

        [Parameter]
        public int? MaxFiles { get; set; }

        [Parameter]
        public bool HideDeletingFiles { get; set; }

        [Parameter]
        public bool AllowToSort { get; set; } = true;

        [Parameter]
        public RenderFragment<IModal> ItemTemplate { get; set; }

        [Parameter]
        public RenderFragment<IModal> ActionTemplate { get; set; }

        [Parameter]
        public RenderFragment<RowTemplateModal<IModal>> RowTemplate { get; set; }

        [Parameter]
        public RenderFragment<DisplayTemplateModal<IModal>> DisplayTemplate { get; set; }

        [Parameter]
        public Func<List<IModal>> DataSource { get; set; }

        [Parameter]
        public EventCallback<DropzoneJs<IModal>> OnReady { get; set; }

        [Parameter]
        public string FileType { get; set; } = "photo";

        [Parameter]
        public int MaxWidth { get; set; } = 9999999;

        [Parameter]
        public int MaxHeight { get; set; } = 9999999;

        [Parameter]
        public int MinWidth { get; set; } = 100;

        [Parameter]
        public int MinHeight { get; set; } = 100;

        [Parameter]
        public string DownloadAllFileName { get; set; } = string.Empty;

        [Parameter]
        public bool CanRemoveAll { get; set; } = false;

        [Parameter]
        public bool CanDownloadAll { get; set; } = false;

        [Parameter]
        public bool IsShowFileDetail { get; set; } = false;

        public SectionTemplate RightTemplate { get; set; }

        private bool _isReadonly;
        [Parameter]
        public bool IsReadonly
        {
            get => _isReadonly;
            set
            {

                _isReadonly = value;

                StateHasChanged();


            }
        }

        protected override async Task OnInitializedAsync()
        {
            _elementRef = DotNetObjectReference.Create(this);

            await base.OnInitializedAsync();

            await this.OnReady.InvokeAsync(this);
        }

        private readonly JsonSerializerOptions _jsonSerializerOptions =
           new JsonSerializerOptions
           {
               PropertyNamingPolicy = JsonNamingPolicy.CamelCase
           };

        private DotNetObjectReference<DropzoneJs<IModal>> _elementRef;

        protected List<IModal> _fileItems = new List<IModal>();

        private bool _isProcessing => _fileItems.Any(p => p.IsUploading);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.init",
                      Id,
                      _elementRef,
                      FileType,
                      MaxWidth,
                      MaxHeight,
                      MinWidth,
                      MinHeight
                      );
            }

            //await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.displayTemporaryImages",
            //    Id,
            //    _fileItems.Where(p => p.IsTemporaryFile && p.NeedToDelete == false));

            await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.showHideElement", $"#component_{this.Id} .dropzone-section", !this.IsReadonly);

        }

        [JSInvokable("deletedFile")]
        public async Task<string> DeletedFile(IModal fileItem)
        {
            fileItem.NeedToDelete = true;
            _fileItems.Add(fileItem);

            StateHasChanged();

            return JsonSerializer.Serialize(fileItem, _jsonSerializerOptions);
        }


        [JSInvokable("uploadedFileSuccess")]
        public async Task<string> UploadedFileSuccess(IModal fileItem)
        {
            var file = _fileItems.Single(p => p.Id == fileItem.Id);
            file.IsUploading = false;

            StateHasChanged();

            return JsonSerializer.Serialize(file, _jsonSerializerOptions);
        }

        [JSInvokable("removeFile")]
        public void RemoveFileWhenDimensionLargeOrSmall(IModal fileItem)
        {
            var itemToDelete = _fileItems.SingleOrDefault(x => x.Id == fileItem.Id);
            if (itemToDelete != null)
            {
                _fileItems.Remove(itemToDelete);

                StateHasChanged();
            }

        }

        //[JSInvokable("requestPreSignedUrl")]
        //public dynamic RequestPreSignedUrl(IModal fileItem)
        //{
        //    var isExceededMaximumFiles = this.MaxFiles != null && _fileItems.Count(p => p.NeedToDelete == false) >= this.MaxFiles;

        //    fileItem.Id = Guid.NewGuid();

        //    var s3Key = $"{TEMPORARY_FOLDER}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";
        //    var preSignedUrl = _s3Uploader.GenerateSignedURLByKey(
        //        key: s3Key, isUpload: true);

        //    fileItem.IsTemporaryFile = true;
        //    fileItem.IsUploading = true;

        //    _fileItems.Add(fileItem);

        //    return new
        //    {
        //        id = fileItem.Id,
        //        preSignedUrl,
        //        s3Url = _s3Uploader.GenerateSignedURLByKey(s3Key, isUpload: true), // $"{ _s3Configuration.S3_BaseUrl}/temporary/{fileItem.Id}{Path.GetExtension(fileItem.FileName)}"
        //        isExceededMaximumFiles = isExceededMaximumFiles
        //    };
        //}

        [JSInvokable("isExceededMaximumFiles")]
        public async Task IsExceededMaximumFiles(IModal fileItem)
        {
            var removingFile = this._fileItems.SingleOrDefault(p => p.Id == fileItem.Id);
            if (removingFile != null)
                this._fileItems.Remove(removingFile);

            StateHasChanged();
        }

        public async Task RemoveFileAsync(IModal fileItem)
        {
            fileItem.NeedToDelete = true;
            //this._fileItems.Remove(fileItem);

            await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.removeExistingFile", Id, fileItem);

            await this.RefreshDisableFlagAsync();
        }

        public async Task RemoveAllFileAsync()
        {
            try
            {
                foreach (var item in this._fileItems)
                {
                    item.NeedToDelete = true;

                    await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.removeExistingFile", Id, item);
                }

                await this.RefreshDisableFlagAsync();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task OnDownloadClicked(IModal fileItem)
        {
            try
            {
                var files = new List<DownloadDto>
                    {
                        new DownloadDto { FileName = $"{fileItem.OriginalFileName}", SignedUrl = fileItem.SignedUrl}
                    };

                await DownloadAsync(files, fileItem.OriginalFileName.Split('.')[0]);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public async Task OnDownloadAllClicked()
        {
            try
            {
                var files = _fileItems.Select(p => new DownloadDto
                {
                    FileName = $"{p.OriginalFileName}",
                    SignedUrl = p.SignedUrl
                }).ToList();

                await DownloadAsync(files, DownloadAllFileName);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private async Task DownloadAsync(List<DownloadDto> files, string downloadFileName)
        {
            await _jsRuntime.InvokeVoidAsync(
                    "window.dropzoneBlazor.downloadAllFiles", files, downloadFileName);
        }

        private async Task RefreshDisableFlagAsync()
        {
            if (this.MaxFiles == null)
            {
                //await _jsRuntime.InvokeVoidAsync("window.dropzoneBlazor.setDisable", Id, false);

                return;
            }

            if (this._fileItems.Count(p => p.NeedToDelete == false) < this.MaxFiles)
            {
                // await this.SetDisableAsync(false);
            }
            else
            {
                //await this.SetDisableAsync(true);
            }
        }

        protected void MoveToTop(IModal fileItem)
        {
            for (var i = 0; i < _fileItems.Count; i++)
            {
                _fileItems[i].SortOrder = i + 1;
            }

            fileItem.SortOrder = 0;

            ReorderItems();

        }

        protected void MoveUp(IModal fileItem)
        {
            if (fileItem.SortOrder == 0)
                return;

            _fileItems[fileItem.SortOrder - 1].SortOrder++;
            fileItem.SortOrder--;

            _fileItems = _fileItems.OrderBy(p => p.SortOrder).ToList();
        }

        protected void MoveDown(IModal fileItem)
        {
            if (fileItem.SortOrder + 1 >= _fileItems.Count)
                return;

            _fileItems[fileItem.SortOrder + 1].SortOrder--;
            fileItem.SortOrder++;

            _fileItems = _fileItems.OrderBy(p => p.SortOrder).ToList();
        }

        private void ReorderItems()
        {
            _fileItems = _fileItems.OrderBy(p => p.SortOrder).ToList();

            for (var i = 0; i < _fileItems.Count; i++)
            {
                _fileItems[i].SortOrder = i;
            }

        }

        //public void SetDefaultImages(List<IModal> fileItems)
        //{
        //    _fileItems = fileItems;
        //    ReorderItems();

        //    StateHasChanged();
        //}

        public void RefreshDataSource()
        {
            if (DataSource == null) return;
            //if (DataSource != null)
            //{
            //    this.RefreshDataSource();
            //}
            this._fileItems = this.DataSource();

            ReorderItems();

            StateHasChanged();
        }

        public List<IModal> GetDeletingImages()
        {
            var deletingFiles = _fileItems
               .Where(p => p.NeedToDelete)
               .ToList();

            return deletingFiles;
        }

        private List<IModal> GetUploadingImages()
        {
            var uploadingFiles = _fileItems
               .Where(p => p.IsTemporaryFile == true && p.NeedToDelete == false)
               .ToList();

            return uploadingFiles;
        }

        public List<IModal> GetImages()
        {
            var currentImages = _fileItems
               .Where(p => p.NeedToDelete == false)
               .ToList();

            return currentImages;
        }

        public List<IModal> GetAllImages()
        {
            var currentImages = _fileItems
               .ToList();

            foreach (var image in currentImages)
            {
                image.AbsolutePath = GetOriginalUrlForImage(image);
            }

            return currentImages;
        }

        public async Task SaveAsync()
        {
            var deletingFiles = GetDeletingImages();
            foreach (var deletingFile in deletingFiles)
            {

            }

            var uploadingFiles = GetUploadingImages();
            if (uploadingFiles.Count == 0) return;

            foreach (var uploadingFile in uploadingFiles)
            {
                await _s3Uploader.MoveAndDeleteAsync(
                    sourceKey: $"{TEMPORARY_FOLDER}/{uploadingFile.S3FileName}",
                    destinationKey: $"{_s3Configuration.SecretOriginalPath}/{DestinationFolder}/{uploadingFile.S3FileName }");
            }
        }

        public string GetAbsoluteUrl(IModal fileItem)
        {
            if (fileItem.AbsolutePath != null)
            {
                fileItem.IsTemporaryFile = true;
                fileItem.NeedToDelete = false;

                return _s3Uploader.GetAbsolutePath(fileItem.AbsolutePath);
            }

            if (fileItem.IsTemporaryFile)
            {
                var s3Key = $"{TEMPORARY_FOLDER}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";

                return _s3Uploader.GetAbsolutePath(s3Key);
            }
            else
            {
                var s3Key = $"{_s3Configuration.SecretOriginalPath}/{DestinationFolder}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";

                return _s3Uploader.GetAbsolutePath(s3Key);
            }

        }

        public string GetThumbnailUrlForImage(IModal fileItem)
        {
            return $"{_cloudFrontConfiguration.Rootpath}/200/{DestinationFolder}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";
        }

        public string GetPreviewUrlForImage(IModal fileItem)
        {
            return $"{_cloudFrontConfiguration.Rootpath}/normal/{DestinationFolder}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";
        }

        public string GetOriginalUrlForImage(IModal fileItem)
        {
            return $"{_cloudFrontConfiguration.Rootpath}/CZywAfruoAgd/{DestinationFolder}/{fileItem.Id}{Path.GetExtension(fileItem.OriginalFileName)}";
        }

        public string GetSignedUrlForDocument(IModal fileItem)
        {
            return fileItem.SignedUrl;
        }
    }

    public class RowTemplateModal<IModal> where IModal : UploadComponentModal
    {
        public DropzoneJs<IModal> Dropzone { get; set; }

        public IModal Modal { get; set; }

    }

    public class DisplayTemplateModal<IModal> where IModal : UploadComponentModal
    {
        public DropzoneJs<IModal> Dropzone { get; set; }

        public List<IModal> Files { get; set; }

    }
    public class DownloadDto
    {
        public string FileName { get; set; } = string.Empty;

        public string SignedUrl { get; set; } = string.Empty;
    }
}
