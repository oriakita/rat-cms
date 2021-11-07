if (!window.dropzoneBlazor) {
    window.dropzoneBlazor = {};
}


window.dropzoneBlazor = {

    init: (id, dotnetHelper, filetype, maxWidth, maxHeight, minWidth, minHeight) => {
        let dotnetObj = dotnetHelper;

        const doStuffAsync = async (file, done) => {
            //fetch('https://httpbin.org/get').then((response) => {
            //    file.dynamicUploadUrl = `https://This-URL-will-be-different-for-every-file${Math.random()}`
            //    done();//call the dropzone done
            //})

            let preSignedResult = await dotnetObj.invokeMethodAsync("requestPreSignedUrl", {
                originalFileName: file.upload.filename
            });

            file.id = preSignedResult.id
            file.isExceededMaximumFiles = preSignedResult.isExceededMaximumFiles;

            file.preSignedUrl = preSignedResult.preSignedUrl;

            done();//call the dropzone done
        }

        const checkFileCondition = (file) => {
            if (file.height > maxHeight || file.width > maxWidth) {
                alert(`Image size must be less than ${maxWidth} x ${maxHeight}`);

                dropzone.cancelUpload(file);

                dotnetObj.invokeMethodAsync("removeFile", {
                    Id: file.id
                });
            }

            if (file.height < minHeight || file.width < minWidth) {
                alert(`Image size must be more than ${minWidth} x ${minHeight}`);

                dropzone.cancelUpload(file);

                dotnetObj.invokeMethodAsync("removeFile", {
                    id: file.id
                });
            }
        }

        const generateUploadUrl = (files) => {
            let file = files[0];

            if (filetype == 'photo') {
                if (file.height == undefined) {
                    
                    setTimeout(() => {
                        checkFileCondition(file);
                    }, 1000);
                }
                else {
                    checkFileCondition(file);
                }
            }

            return file.preSignedUrl;
        }

        let acceptedFiles = '';
        switch (filetype)
        {
            case 'photo':
                acceptedFiles = '.jpg, .jpeg, .png, .gif, .svg, .mov';
                break;
            case 'icon':
                acceptedFiles = '.jpg, .jpeg, .png, .gif, .svg, .mov';
                break;
            case 'document':
                acceptedFiles = '.doc, .docx, .xls, .xlsx, .pdf, .jpeg,.jpg, .png, .zip';
                break;
            default:
                acceptedFiles = '.mp4, .webm';
                break;
        }
        let options = {
            url: generateUploadUrl,
            paramName: 'fileName',
            addRemoveLinks: false,
            acceptedFiles: acceptedFiles,
            maxFilesize: filetype == 'photo' ? 10 : 150, // MB,
            //autoProcessQueue: true,
            method: 'put',
            //headers: {
            //    "Cache-Control": "",
            //},
            accept: doStuffAsync,
            //maxFiles: 1,

            //previewTemplate: document.querySelector('#component_' + id + ' .template-container').innerHTML
        };

        //try {

        let dropzone = new Dropzone('#' + id, options);
        dropzone.submitRequest = function submitRequest(xhr, formData, files) {
            xhr.send(files[0]);
        };

        dropzone.on("processing", async (file) => {

        });

        dropzone.on("sending", async (file, xmlHttpRequest) => {
            // debugger;

            xmlHttpRequest.setRequestHeader('Content-Type', file.type);

        });

        dropzone.on("success", async (file, responseText) => {

            await dotnetObj.invokeMethodAsync("uploadedFileSuccess", {
                id: file.id
            });
        });

        dropzone.on("complete", async (file) => {
            dropzone.removeFile(file);

            if (file.isExceededMaximumFiles === true) {

                await dotnetObj.invokeMethodAsync("isExceededMaximumFiles", {
                    id: file.id
                });
            }

        });

        //} catch (e) {
        //    alert(e);
        //    console.log(e);
        //}

        //dropzone.on("maxfilesexceeded", function (file) { this.removeFile(file); });

    },

    removeExistingFile: (id, file) => {


        let dropzone = Dropzone.forElement("#" + id);

        var deletingFile = dropzone.files.find(p => p.name === file.fileName);

        if (deletingFile !== undefined) {
            dropzone.removeFile(deletingFile);
        }

    },

    displayTemporaryImages: (id, files) => {
        //let dropzone = Dropzone.forElement("#" + id);

        //setTimeout(() => {
        //    files.forEach(f => {
        //        var file = dropzone.files.find(p => p.id === f.id);

        //        if (file === undefined) {
        //            return;
        //        }

        //        $("#img_" + f.id).attr('src', file.dataURL);
        //    });
        //}, 500);
    },

    showHideElement: (selector, isShown) => {

        if (isShown) {
            $(selector).removeClass("d-none");
        } else {
            $(selector).removeClass("d-none").addClass("d-none");
        }

    },

    downloadAllFiles: function (listOfItems, zipFileName) {

        Swal.fire({
            title: 'Download',
            html: 'Downloading & zipping files..',
            allowEscapeKey: false,
            allowOutsideClick: false,
            onBeforeOpen: () => {
                Swal.showLoading();

            },
            onClose: () => {

            }
        });

        let showErrorMessage = () => {
            Swal.fire({
                type: 'error',
                title: 'Download',
                html: 'Failed to download all files',
                footer: '<a href>Contact Akshay for more detail!</a>'
            });
        };

        let promisses = [];
        listOfItems.forEach((file) => {

            let fetchingFile = fetch(file.signedUrl)
                .then(resp => resp.blob())
                .then(blob => {

                    return { "Blob": blob, "Url": file.signedUrl, "FileName": file.fileName };
                })
                .catch((e) => {

                    console.log(e);
                });

            promisses.push(fetchingFile);
        });

        Promise.all(promisses).then(responses => {
            // Download directly files from browser sucessfully. So, now we just need to zip them
            try {
                let zip = new JSZip();
                responses.forEach((file) => {
                    zip.file(file.FileName, file.Blob, { binary: true });
                });

                zip.generateAsync({ type: "blob" })
                    .then(function (blob) {
                        saveAs(blob, zipFileName + ".zip");

                        swal.close();
                    });
            } catch (e) {
                showErrorMessage();
            }

        });

    }

};
