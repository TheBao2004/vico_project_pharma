$(document).ready(function () {


    function runCode() {

        InOutPutSlug();

    }


    runCode();

    function getSlug(str) {


        let slug = str.trim().toLowerCase();

        slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẵ|ặ|ẳ|â|ấ|ầ|à|ẫ|ậ|ẩ/gi, 'a');
        slug = slug.replace(/é|è|ẻ|ẹ|ẽ|ê|ế|ề|ệ|ễ|ể/gi, 'e');
        slug = slug.replace(/í|ì|ỉ|ĩ|ị/gi, 'i');
        slug = slug.replace(/ó|ò|ọ|ỏ|õ|ô|ố|ồ|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ỡ|ở/gi, 'o');
        slug = slug.replace(/ù|ú|ụ|ũ|ủ|ư|ứ|ừ|ữ|ử|ự/gi, 'u');
        slug = slug.replace(/ỵ|ý|ỳ|ỷ|ỹ/gi, 'y');
        slug = slug.replace(/đ/gi, 'd');
        slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|\_/gi, '');

        slug = slug.trim();

        slug = slug.replaceAll(' ', '-');


        return slug;

    }


    function InOutPutSlug() {


        let inputSlug = document.querySelector('.inputSlug');
        let outputSlug = document.querySelector('.outputSlug');


        if (inputSlug != null && outputSlug != null) {

            if (sessionStorage.getItem('slug')) {
                sessionStorage.removeItem('slug');
            }

            let slug = '';

            if (outputSlug.value) {
                slug = getSlug(outputSlug.value);
                let linkSlug = slug + '.html';

            }

            let linkSlug = outputSlug.value + '.html';




            inputSlug.addEventListener('keyup', (e) => {
                if (sessionStorage.getItem('slug') == null) {
                    outputSlug.value = getSlug(e.target.value);
                }
            });


            inputSlug.addEventListener('change', (e) => {

                if (!sessionStorage.getItem('slug')) {
                    sessionStorage.setItem('slug', 1);
                }

                let linkSlug = outputSlug.value + '.html';



            });

            outputSlug.addEventListener('change', (e) => {
                if (outputSlug.value.trim() == '') {

                    sessionStorage.removeItem('slug');
                    outputSlug.value = getSlug(inputSlug.value);

                }

                let linkSlug = outputSlug.value + '.html';


            });


        }


    }   


    // $("#btnEditProduct").on('click', function(){

    //     let id = $("#imageProduct").data("pro_id");

    //     fetch(`/Product/getAllImage/${id}`)
    //     .then(res => res.json())
    //     .then((res) => {
    
    //  let arrImage = res.split(',');
    //     let showImage = [];
    //     arrImage.forEach((img, index) => {
    //         if(img != ""){
    //             showImage.push({
    //                 name: img,
    //                 url: "/uploads/" + img
    //             });
    //         }
    //     });
    //     $("#imageProduct").uploader({
    //         multiple: true,
    //         defaultValue: showImage
    //     })
    
    //     });
        
    // });


    let id = $("#imageProduct").data("pro_id");

    fetch(`/Product/getAllImage/${id}`)
    .then(res => res.json())
    .then((res) => {

        if(res != null){
            let arrImage = res.split(',');
            let showImage = [];
            arrImage.forEach((img, index) => {
                if(img != ""){
                    showImage.push({
                        name: img,
                        url: "/uploads/" + img
                    });
                }
            });
            $("#imageProduct").uploader({
                multiple: true,
                defaultValue: showImage
            })
        }else{
            $("#imageProduct").uploader({
                multiple: true,
            })
        }
    });
  


    $("#imageProduct")
        .on("uploader-init", function () {
            // do something
        })
        .on("before-upload", function (waitUploadFiles) {
            // do something
        })
        .on("uploading", function (file) {
        })
        .on("upload-success", function (file, data) {

            // let id = $(this).data("pro_id");
            // let rep = data;
            // console.log(file);
            // console.log(data.file);

            // var fileImage = new FormData();
            // fileImage.append('file', data.file);

            // $.ajax({
            //     url: "/Product/UploadProduct",
            //     type: "POST",
            //     processData: false,
            //     contentType: false,
            //     data: fileImage,
            //     success: function (res) {
            //         if (JSON.parse(res).length > 0) {
            //             let image = JSON.parse(res)[0];
            //             $.ajax({
            //                 url: "/Product/AddImageProduct",
            //                 type: "POST",
            //                 data: { "id": id, "image": image},
            //                 success: function (res) {
            //                     console.log(res);
                               
            //                 },
            //                 error: function (err) {
            //                     console.log("error");
            //                 }
            //             });
            //         }
            //     },
            //     error: function (err) {
            //         console.log("error");
            //     }
            // });

        })
        .on("upload-error", function (file, data) {
            let id = $(this).data("pro_id");
            let rep = data;
            // console.log(file);
            // console.log(data.file);

            var fileImage = new FormData();
            fileImage.append('file', data.file);
            // console.log(data.file);
            $.ajax({
                url: "/Product/UploadProduct",
                type: "POST",
                processData: false,
                contentType: false,
                data: fileImage,
                success: function (res) {
                    console.log(res)
                        $.ajax({
                            url: "/Product/AddImageProduct",
                            type: "POST",
                            data: { "id": id, "image": res},
                            success: function (res) {
                                // console.log(res);
                                // console.log(123);
                            },
                            error: function (err) {
                                console.log("error");
                            }
                        });
                },
                error: function (err) {
                    console.log("error");
                }
            });
          
        })
        .on("file-add", function () {
            // do something
        })
        .on("file-remove", function (file, data) {

            let id = $(this).data("pro_id");
            // console.log(id);
            let image = data.name;
            $.ajax({
                url: "/Product/RemoveImage",
                type: "POST",
                data: {"image": image, "id": id},
                success: function(res){
                    console.log(res)
                },
                error: function(){
                    console.log("error");
                }
            });
        })


        // new DataTable('#table_keyword', {
        //     // layout: {
        //     //     bottomEnd: {
        //     //         paging: {
        //     //             boundaryNumbers: false
        //     //         }
        //     //     }
        //     // }
        // });


}); 