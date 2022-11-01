var fileInput = document.getElementById("FileInput");
var UploadProgressBox = document.getElementById("UploadProgressBox");
var UploadProgressBar = document.getElementById("UploadProgressBar");
var Interval;

fileInput.addEventListener("change", function (event) {
    // files validation
    const files = fileInput.files;
    if (!files.length) {
        return;
    }

    // create and add file to formData
    const formData = new FormData();
    formData.append("file", files[0]);

    // show progress bar
    UploadProgressBar.classList.remove("bg-success");
    UploadProgressBar.classList.add("bg-primary");
    UploadProgressBox.classList.remove("d-none");

    // set interval and send ajax to get current progress value 
    Interval = setInterval(
        function () {
            $.post("/Home/GetProgress", function (progress) {
                UploadProgressBar.innerText = progress + "%";
                UploadProgressBar.style.width = progress + "%";
            });
        },
        10
    );

    // send file with ajax
    $.ajax({
        url: "/Home/UploadFile",
        data: formData,
        processData: false,
        contentType: false,
        type: "POST",
        success: function (fileName) {
            clearInterval(Interval);
            UploadProgressBar.classList.remove("bg-primary");
            UploadProgressBar.classList.add("bg-success");
            console.info(`File Uploaded Successfully => filename : ${fileName}`);
        }
    });
});

