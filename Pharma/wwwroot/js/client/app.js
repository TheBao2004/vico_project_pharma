
function alertSuccess(value) {
    bs4Toast.success("Pharma", value);
}

function alertError(value) {
    bs4Toast.error("Pharma", value);
}

$(document).ready(function () {

    $('#show_form_time_cart').on('change', function () {

        let ds = $('.form_time_cart');

        if (ds.hasClass("un_time_cart")) {
            ds.removeClass("un_time_cart");
        } else {
            ds.addClass("un_time_cart");
        }

    });

    $("#add_to_cart").on('click', function () {



    });

    $("#form_detail_product").on('submit', function (event) {
        event.preventDefault();

        let quantity = $("#quantity_detail_product").val();
        let pro_id = $("#pro_id_detail_product").val();

        // let truong = $(this).serializeArray();
        // let quantity = 0;
        // let pro_id = 0;

        // truong.forEach(function(item){
        //     if(item.name = "quantity") quantity = item.value;
        //     if(item.name = "pro_id") pro_id = item.value;
        // });

        $.ajax({
            url: "/Home/AddToCartDetail",
            type: "POST",
            data: { "pro_id": pro_id, "quantity": quantity },
            success: function (res) {
                if (res != "") {
                    alertSuccess(res);
                } else {
                    alertError("Thêm sản phẩm thất bại");
                }
            },
            error: function (err) {
                console.log("error");
            }
        });



    });

    $("#add_detail_product").on('click', function () {

        let inputQuantity = $("#quantity_detail_product");

        let quantity = inputQuantity.val();

        ++quantity;

        inputQuantity.val(quantity);

    });

    $("#minus_detail_product").on('click', function () {

        let inputQuantity = $("#quantity_detail_product");

        let quantity = inputQuantity.val();

        --quantity;

        if (quantity <= 0) quantity = 1;

        inputQuantity.val(quantity);

    });

    $("#quantity_detail_product").on('change', function () {

        let value = $(this).val();

        if (!value.match(/^[0-9]+$/) || value == "0") $(this).val(1);

    });

    $(".quantity_table_cart").on('submit', function (event) {
        event.preventDefault();
    });

    function quantityToCart(pro_id, quantity, itemTotal) {

        console.log(`${pro_id} - ${quantity}`);

        $.ajax({
            url: "/Home/UpdateCartItem",
            type: "POST",
            data: { "pro_id": pro_id, "quantity": quantity },
            success: function (res) {
                console.log(res);
                if (res == "") {
                    alertError("Đã có lỗi xảy ra !!!");
                } else {
                    let box = itemTotal.closest(".item_product_table_cart");
                    let boxbox = box.closest(".list_product_table_cart");
                    if (res.empty == 1) {
                        box.css({
                            "display": "none"
                        });
                    }
                    if (res.total <= 0) {
                        boxbox.html(`<div colspan="10" class="text-danger text-center py-3">Chưa có sản phẩm</div>`);
                    }
                    itemTotal.html(`<span>${res.itemTotal.toLocaleString()} đ</span>`);
                    $(".total_cart").html(`
                    <span>Tổng tiền:</span>
                    <span>${res.total.toLocaleString()} đ</span>
                    `);
                }
            },
            error: function () {
                alertError("Đã có lỗi xảy ra !!!");
            }
        });
    }

    $(".add_to_cart").on('click', function () {

        let quantityHtml = $(this).prev();
        let quantity = Number(quantityHtml.val()) + 1;
        quantityHtml.val(quantity);
        let itemTotal = quantityHtml.closest(".item_product_table_cart").find(".total_product_table_cart");

        let pro_id = quantityHtml.data("pro_id");

        quantityToCart(pro_id, quantity, itemTotal);

    });

    $(".minus_to_cart").on('click', function () {

        let quantityHtml = $(this).next();
        let quantity = Number(quantityHtml.val()) - 1;
        quantityHtml.val(quantity);
        let itemTotal = quantityHtml.closest(".item_product_table_cart").find(".total_product_table_cart");

        let pro_id = quantityHtml.data("pro_id");

        quantityToCart(pro_id, quantity, itemTotal);

    });

    $(".quantity_to_cart").on('change', function () {

        let quantity = $(this).val();
        let pro_id = $(this).data("pro_id");
        let itemTotal = $(this).closest(".item_product_table_cart").find(".total_product_table_cart");


        if (!quantity.match(/^[0-9]+$/) || quantity == "0") $(this).val(1);

        quantityToCart(pro_id, quantity, itemTotal);

    });


    $("#form_AddAddress").on('submit', function (event) {

        let error = true;

        let address_id = $(this).attr("data-address_id");

        // let truong = $(this).serializeArray();

        let fullname = $('input[name="fullname"]');
        let phone = $('input[name="phone"]');
        let email = $('input[name="email"]');
        let address = $('input[name="address"]');
        let city = $('select[name="city"]');
        let district = $('select[name="district"]');
        let ward = $('select[name="ward"]');

        if ($.trim(fullname.val()) == "") {
            fullname.next().remove();
            fullname.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false
        } else {
            fullname.next().remove();
        }

        if ($.trim(phone.val()) == "") {
            phone.next().remove();
            phone.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false;
        } else {
            phone.next().remove();
            if (!phone.val().match(/^0[0-9]{9}$/)) {
                phone.next().remove();
                phone.after(`<div class="text-danger">Phải là số điện thoại việt nam</div>`);
                error = false;
            } else {
                phone.next().remove();
            }
        }

        // if($.trim(email.val()) == ""){
        //     email.next().remove();
        //     email.after(`<div class="text-danger">Vui lòng điền</div>`);
        // }else{
        //     email.next().remove();
        // }

        if ($.trim(address.val()) == "") {
            address.next().remove();
            address.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false;
        } else {
            address.next().remove();
        }

        if ($.trim(city.val()) == "") {
            city.next().remove();
            city.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false;
        } else {
            city.next().remove();
        }

        if ($.trim(district.val()) == "") {
            district.next().remove();
            district.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false;
        } else {
            district.next().remove();
        }

        if ($.trim(ward.val()) == "") {
            ward.next().remove();
            ward.after(`<div class="text-danger">Vui lòng điền</div>`);
            error = false;
        } else {
            ward.next().remove();
        }

        if (error) {

        } else {
            event.preventDefault();
            $("#AddAddress").modal("show");
        }

    });


    $("#pharma_district").attr("disabled", true);
    $("#pharma_ward").attr("disabled", true);

    $("#pharma_city").on('change', function () {

        let city = $(this).val();

        if (city == "") {
            $("#pharma_district").attr("disabled", true);
            $("#pharma_ward").attr("disabled", true);
        } else {
            $("#pharma_district").attr("disabled", false);
            $("#pharma_ward").attr("disabled", true);
            $("#pharma_ward").html(`<option value="">Chọn xã</option>`);
            $.ajax({
                url: "/Home/GetDistrict",
                type: "POST",
                data: { "id": city },
                success: function (res) {
                    if (res == "") {
                        $("#pharma_district").attr("disabled", true);
                    } else {
                        let option = res.map(function (item) {
                            return `
                                <option value="${item.id}">${item.name}</option>
                            `;
                        }).join("");

                        $("#pharma_district").html(`<option value="">Chọn huyện</option>${option}`);
                    }
                },
                error: function (err) {
                    console.log("error");
                }
            });
        }

    });

    $("#pharma_district").on('change', function () {

        let district = $(this).val();

        if (district == "") {
            $("#pharma_ward").attr("disabled", true);
        } else {
            $("#pharma_ward").attr("disabled", false);
            $.ajax({
                url: "/Home/GetWard",
                type: "POST",
                data: { "id": district },
                success: function (res) {
                    if (res == "") {
                        $("#pharma_ward").attr("disabled", true);
                    } else {
                        let option = res.map(function (item) {
                            return `
                                <option value="${item.id}">${item.name}</option>
                            `;
                        }).join("");

                        $("#pharma_ward").html(`<option value="">Chọn xã</option>${option}`);
                    }
                },
                error: function (err) {
                    console.log("error");
                }
            });
        }

    });


    $(".edit_item_address_account").click(function () {

        let address_id = $(this).data("address_id");
        $("#EditAddress").modal("show");

        $.ajax({

            url: "/Home/InforAddress",
            type: "POST",
            data: { "id": address_id },
            success: function (res) {
                // console.log(res);
                $("#EditAddress").html(res);

                // $("#pharma_district_edit").attr("disabled", true);
                // $("#pharma_ward_edit").attr("disabled", true);

                $("#pharma_city_edit").on('change', function () {

                    let city = $(this).val();

                    if (city == "") {
                        // $("#pharma_district_edit").attr("disabled", true);
                        // $("#pharma_ward_edit").attr("disabled", true);
                    } else {
                        // $("#pharma_district_edit").attr("disabled", false);
                        // $("#pharma_ward_edit").attr("disabled", true);
                        $("#pharma_ward_edit").html(`<option value="">Chọn xã</option>`);
                        $.ajax({
                            url: "/Home/GetDistrict",
                            type: "POST",
                            data: { "id": city },
                            success: function (res) {
                                if (res == "") {
                                    $("#pharma_district_edit").attr("disabled", true);
                                } else {
                                    let option = res.map(function (item) {
                                        return `
                                            <option value="${item.id}">${item.name}</option>
                                        `;
                                    }).join("");

                                    $("#pharma_district_edit").html(`<option value="">Chọn huyện</option>${option}`);
                                }
                            },
                            error: function (err) {
                                console.log("error");
                            }
                        });
                    }

                });

                $("#pharma_district_edit").on('change', function () {

                    let district = $(this).val();

                    if (district == "") {
                        // $("#pharma_ward_edit").attr("disabled", true);
                    } else {
                        // $("#pharma_ward_edit").attr("disabled", false);
                        $.ajax({
                            url: "/Home/GetWard",
                            type: "POST",
                            data: { "id": district },
                            success: function (res) {
                                if (res == "") {
                                    // $("#pharma_ward_edit").attr("disabled", true);
                                } else {
                                    let option = res.map(function (item) {
                                        return `
                                            <option value="${item.id}">${item.name}</option>
                                        `;
                                    }).join("");

                                    $("#pharma_ward_edit").html(`<option value="">Chọn xã</option>${option}`);
                                }
                            },
                            error: function (err) {
                                console.log("error");
                            }
                        });
                    }

                });


                $("#form_EditAddress").on('submit', function (event) {

                    let error = true;

                    // let address_id = $(this).attr("data-address_id");

                    // let truong = $(this).serializeArray();

                    let fullname = $('input[name="fullname_edit"]');
                    let phone = $('input[name="phone_edit"]');
                    let email = $('input[name="email_edit"]');
                    let address = $('input[name="address_edit"]');
                    let city = $('select[name="city_edit"]');
                    let district = $('select[name="district_edit"]');
                    let ward = $('select[name="ward_edit"]');


                    if ($.trim(fullname.val()) == "") {
                        fullname.next().remove();
                        fullname.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        fullname.next().remove();
                    }

                    if ($.trim(phone.val()) == "") {
                        phone.next().remove();
                        phone.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        phone.next().remove();
                        if (!phone.val().match(/^0[0-9]{9}$/)) {
                            phone.next().remove();
                            phone.after(`<div class="text-danger">Phải là số điện thoại việt nam</div>`);
                            error = false;

                        } else {
                            phone.next().remove();
                        }
                    }

                    // if($.trim(email.val()) == ""){
                    //     email.next().remove();
                    //     email.after(`<div class="text-danger">Vui lòng điền</div>`);
                    // }else{
                    //     email.next().remove();
                    // }

                    if ($.trim(address.val()) == "") {
                        address.next().remove();
                        address.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        address.next().remove();
                    }


                    if ($.trim(city.val()) == "") {
                        city.next().remove();
                        city.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        city.next().remove();
                    }


                    if ($.trim(district.val()) == "") {
                        district.next().remove();
                        district.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        district.next().remove();
                    }

                    if ($.trim(ward.val()) == "") {
                        ward.next().remove();
                        ward.after(`<div class="text-danger">Vui lòng điền</div>`);
                        error = false;
                    } else {
                        ward.next().remove();
                    }

                    if (error) {

                    } else {
                        event.preventDefault();
                        $("#UpdateAddress").modal("show");
                    }

                });

            },
            error: function (err) {
                console.log("error");
            }

        });


        $("#EditAddress").attr("data-address_id", address_id)

    });

    // $('#add_address_infor').click(function(){

    //     $("#form_AddAddress").removeAttr("data-address_id");

    //     $.ajax({

    //         url: "/Home/InforAddress",
    //         type: "POST",
    //         data: {"id": 0},
    //         success: function(res){
    //             $("#AddAddress").html(res);
    //         },
    //         error: function(err){
    //             console.log("error");
    //         }

    //     });

    // });

    // $("#pharma_district_bill").attr("disabled", true);
    // $("#pharma_ward_bill").attr("disabled", true);

    $("#pharma_city_bill").on('change', function () {

        let city = $(this).val();

        if (city == "") {
            $("#pharma_district_bill").attr("disabled", true);
            $("#pharma_ward_bill").attr("disabled", true);
        } else {
            $("#pharma_district_bill").attr("disabled", false);
            $("#pharma_ward_bill").attr("disabled", true);
            $("#pharma_ward_bill").html(`<option value="">Chọn xã</option>`);
            $.ajax({
                url: "/Home/GetDistrict",
                type: "POST",
                data: { "id": city },
                success: function (res) {
                    if (res == "") {
                        $("#pharma_district_bill").attr("disabled", true);
                    } else {
                        let option = res.map(function (item) {
                            return `
                                <option value="${item.id}">${item.name}</option>
                            `;
                        }).join("");

                        $("#pharma_district_bill").html(`<option value="">Chọn huyện</option>${option}`);
                    }
                },
                error: function (err) {
                    console.log("error");
                }
            });
        }

    });

    $("#pharma_district_bill").on('change', function () {

        let district = $(this).val();

        if (district == "") {
            $("#pharma_ward_bill").attr("disabled", true);
        } else {
            $("#pharma_ward_bill").attr("disabled", false);
            $.ajax({
                url: "/Home/GetWard",
                type: "POST",
                data: { "id": district },
                success: function (res) {
                    if (res == "") {
                        $("#pharma_ward_bill").attr("disabled", true);
                    } else {
                        let option = res.map(function (item) {
                            return `
                                <option value="${item.id}">${item.name}</option>
                            `;
                        }).join("");

                        $("#pharma_ward_bill").html(`<option value="">Chọn xã</option>${option}`);
                    }
                },
                error: function (err) {
                    console.log("error");
                }
            });
        }

    });


    $("#btn_voucher_bill").on('click', function () {
        let code = $("#code_voucher_bill").val();

        if (code.trim() != "") {
            $("#error_voucher_bill").html("");

            $.ajax({
                url: "/Home/ApplyVoucher",
                type: "POST",
                data: { code: code },
                success: function (res) {
                    let old = $('#old_voucher_bill').val();
                    if (!isNaN(res)) {
                        $("#error_voucher_bill").html("");
                        $('#total_voucher_bill').html(`${res.toLocaleString()}đ`);
                        $('#box_voucher_bill').html(`
                            <span>Mã giảm giá</span>
                            <span>${(old - res).toLocaleString()}đ</span>
                        `);
                        $('#box_voucher_bill').removeClass('d-none');
                    } else {
                        $("#error_voucher_bill").html(res);
                        $('#total_voucher_bill').html(`${old.toLocaleString()}đ`);
                        $('#box_voucher_bill').addClass('d-none');
                    }
                },
                error: function (err) {

                }

            });

        } else {
            $("#error_voucher_bill").html("Vui lòng nhập code voucher");
        }

    });


    $('input[name="filter_category_price"]').on('change', function () {

        let strValue = "";
        let arrValue = [];
        let strSlug = "NULL";
        // console.log($("#category_slug").val());
        if ($("#category_slug").val() != undefined) {
            strSlug = $("#category_slug").val();
        }

        // console.log(strSlug + "âsdas");

        $('input[name="filter_category_price"]').each(function (item) {
            if ($(this).is(":checked")) {
                strValue += `,${$(this).val()}`;
                arrValue.push($(this).val());
            }
        });

        strValue = strValue.slice(1);

        // console.log(strValue);
        // console.log(arrValue);
        // console.log(JSON.stringify(arrValue));
        // console.log(JSON.parse(JSON.stringify(arrValue)));
        // console.log(strSlug);

        $.ajax({
            url: "/Home/CategoryProductFilter",
            type: "POST",
            // data: {str_value: strValue, arr_value: JSON.stringify(arrValue)},
            data: { arr_value: JSON.stringify(arrValue), str_slug: strSlug },
            success: function (res) {
                // console.log(res);

                let html = res.map(function (item) {

                    // console.log(GetImageOne(item.images));

                    let Discount = item.discount;
                    let Price = item.price;
                    let khuyenMai = "";
                    let Gia = "";
                    if (Discount > 0) {
                        khuyenMai = `<span class="smart_item_product_component">${PhanTramGiam(Discount, Price)} %</span>`;
                        Gia = `
                            <span class="discount_item_product_component">${Discount.toLocaleString()} đ</span>
                            <span class="price_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    } else {
                        Gia = `
                            <span class="discount_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    }

                    return `


                    <div class="item_product_category">
                    <div class="item_product_component">
                        <div class="img_item_product_component">
                            ${khuyenMai}
                            <img src="/uploads/${GetImageOne(item.images)}" />
                        </div>
                        <div class="content_item_product_component">
                            <p><span><a href="/san-pham/${item.slug}" class="text-dark text-decoration-none">${item.name}</a></span></p>
                            <div class="sub_content_item_product_component row mx-0 row-cols-2">
                                <div>
                                    ${Gia}
                                </div>
                                <div>
                                    <button class="btn btn-success add_to_cart_item_product" data-id="${item.id}">
                                        <i class="fa fa-cart-plus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            

                    `;
                }).join("");

                if (html.length == 0) {
                    $('.list_product_category').html(`
                        <div class="col-12 alert alert-warning">
                            Không có sản phẩm   
                        </div>
                    `);
                } else {
                    $('.list_product_category').html(html);
                }

            },
            error: function (err) {
                console.log(err);
            }
        });

    });

    $('.add_to_cart_item_product').on('click', function () {

        let pro_id = $(this).data('id');

        $.ajax({
            url: "/Home/AddToCartDetail",
            type: "POST",
            data: { "pro_id": pro_id, "quantity": 1 },
            success: function (res) {
                if (res != "") {
                    alertSuccess(res);
                } else {
                    alertError("Thêm sản phẩm thất bại");
                }
            },
            error: function (err) {
                console.log("error");
            }
        });

    });


    $("#form_comment_detail_product").on('submit', function (event) {

        let comment = $("#textarea_comment_detail_product").val();
        let error = $("#error_comment_detail_product");

        if (comment.trim() == "") {
            $("#error_comment_detail_product").html("Vui lòng nhập");
            console.log(error);
            event.preventDefault();
        } else {
            error.html("");
        }

    });


    $(".form_reply_detail_product").each(function (index, item) {

        $(this).hide();

        let form_reply = index + "_form_reply";
        let open_reply = index + "_open_reply";
        let close_reply = index + "_close_reply";
        let content_reply = index + "_content_reply";
        let error_reply = index + "_error_reply";

        $("." + open_reply).click(function () {
            $("." + form_reply).show();
        });

        $("." + close_reply).click(function () {
            $("." + form_reply).hide();
        });

        $(this).on('submit', function (event) {

            if ($("." + content_reply).val().trim() == "") {
                event.preventDefault();
                $("." + error_reply).text("Vui lòng nhập");
            }

        });

    });


    $(".product_home_keyword_1").click(function () {

        let keyword = $(this).data('keyword');

        console.log(keyword);

        $.ajax({
            url: "/Home/KeyWordProduct",
            type: "POST",
            data: { keyword: keyword },
            success: function (res) {
                console.log(res);


                let html = res.map(function (item) {

                    // console.log(GetImageOne(item.images));

                    let Discount = item.discount;
                    let Price = item.price;
                    let khuyenMai = "";
                    let Gia = "";
                    if (Discount > 0) {
                        khuyenMai = `<span class="smart_item_product_component">${PhanTramGiam(Discount, Price)} %</span>`;
                        Gia = `
                            <span class="discount_item_product_component">${Discount.toLocaleString()} đ</span>
                            <span class="price_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    } else {
                        Gia = `
                            <span class="discount_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    }

                    return `


                    <div class="item_product_tab_home">
                    <div class="item_product_component">
                        <div class="img_item_product_component">
                            ${khuyenMai}
                            <img src="/uploads/${GetImageOne(item.images)}" />
                        </div>
                        <div class="content_item_product_component">
                            <p><span><a href="/san-pham/${item.slug}" class="text-dark text-decoration-none">${item.name}</a></span></p>
                            <div class="sub_content_item_product_component row mx-0 row-cols-2">
                                <div>
                                    ${Gia}
                                </div>
                                <div>
                                    <button class="btn btn-success add_to_cart_item_product" data-id="${item.id}">
                                        <i class="fa fa-cart-plus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            

                    `;
                }).join("");



                if (html.length == 0) {
                    $('.list_product_tab_home').html(`
                        <div class="col-12 alert alert-warning">
                            Không có sản phẩm   
                        </div>
                    `);
                } else {
                    $('.list_product_tab_home').html(html);
                }


            },
            error: function (err) {
                console.log("error");
            }
        });

    });



    $(".product_home_keyword_2").click(function () {

        let keyword = $(this).data('keyword');

        console.log(keyword);

        $.ajax({
            url: "/Home/KeyWordProduct",
            type: "POST",
            data: { keyword: keyword },
            success: function (res) {

                console.log(res);

                console.log(1231);


                let html = res.map(function (item) {

                    // console.log(GetImageOne(item.images));

                    let Discount = item.discount;
                    let Price = item.price;
                    let khuyenMai = "";
                    let Gia = "";
                    if (Discount > 0) {
                        khuyenMai = `<span class="smart_item_product_component">${PhanTramGiam(Discount, Price)} %</span>`;
                        Gia = `
                            <span class="discount_item_product_component">${Discount.toLocaleString()} đ</span>
                            <span class="price_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    } else {
                        Gia = `
                            <span class="discount_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    }

                    return `


                    <div class="item_product_group_right">
                    <div class="item_product_component">
                        <div class="img_item_product_component">
                            ${khuyenMai}
                            <img src="/uploads/${GetImageOne(item.images)}" />
                        </div>
                        <div class="content_item_product_component">
                            <p><span><a href="/san-pham/${item.slug}" class="text-dark text-decoration-none">${item.name}</a></span></p>
                            <div class="sub_content_item_product_component row mx-0 row-cols-2">
                                <div>
                                    ${Gia}
                                </div>
                                <div>
                                    <button class="btn btn-success add_to_cart_item_product" data-id="${item.id}">
                                        <i class="fa fa-cart-plus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            

                    `;
                }).join("");



                if (html.length == 0) {
                    $('.list_product_group_right_other_1').html(`
                        <div class="col-12 alert alert-warning">
                            Không có sản phẩm   
                        </div>
                    `);
                } else {
                    $('.list_product_group_right_other_1').html(html);
                }


            },
            error: function (err) {
                console.log("error");
            }
        });

    });


    $(".product_home_keyword_3").click(function () {

        let keyword = $(this).data('keyword');

        console.log(keyword);

        $.ajax({
            url: "/Home/KeyWordProduct",
            type: "POST",
            data: { keyword: keyword },
            success: function (res) {

                console.log(res);

                console.log(1231);


                let html = res.map(function (item) {

                    // console.log(GetImageOne(item.images));

                    let Discount = item.discount;
                    let Price = item.price;
                    let khuyenMai = "";
                    let Gia = "";
                    if (Discount > 0) {
                        khuyenMai = `<span class="smart_item_product_component">${PhanTramGiam(Discount, Price)} %</span>`;
                        Gia = `
                            <span class="discount_item_product_component">${Discount.toLocaleString()} đ</span>
                            <span class="price_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    } else {
                        Gia = `
                            <span class="discount_item_product_component">${Price.toLocaleString()} đ</span>
                        `;
                    }

                    return `


                    <div class="item_product_group_right">
                    <div class="item_product_component">
                        <div class="img_item_product_component">
                            ${khuyenMai}
                            <img src="/uploads/${GetImageOne(item.images)}" />
                        </div>
                        <div class="content_item_product_component">
                            <p><span><a href="/san-pham/${item.slug}" class="text-dark text-decoration-none">${item.name}</a></span></p>
                            <div class="sub_content_item_product_component row mx-0 row-cols-2">
                                <div>
                                    ${Gia}
                                </div>
                                <div>
                                    <button class="btn btn-success add_to_cart_item_product" data-id="${item.id}">
                                        <i class="fa fa-cart-plus"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            

                    `;
                }).join("");



                if (html.length == 0) {
                    $('.list_product_group_right_other_2').html(`
                        <div class="col-12 alert alert-warning">
                            Không có sản phẩm   
                        </div>
                    `);
                } else {
                    $('.list_product_group_right_other_2').html(html);
                }


            },
            error: function (err) {
                console.log("error");
            }
        });

    });


});




















function toast() {
    bs4Toast.primary('Primary Toast', 'This is Primary toast example.');
}

function toastS() {
    bs4Toast.success('Primary Toast', 'This is Primary toast example.');
}

function dangerT() {
    bs4Toast.error('Danger Toast', 'This is danger toast example.');
}

function warning() {
    bs4Toast.warning('Warning Toast', 'This is warning toast example.');
}
function imaget() {
    bs4Toast.show('Toast with Icon', 'This is toast with buttons example.',
        {
            // delay: 2000,
            icon: {
                type: 'image',
                // src: 'https://via.placeholder.com/150'
                src: 'https://yt3.ggpht.com/a/AATXAJwph2ZkKpM9wNqAERG0JnTYf9B5C1jbVG3ylA=s900-c-k-c0xffffffff-no-rj-mo'
            }

        });
}

function icont() {
    bs4Toast.show('Toast with Icon', 'This is toast with buttons example.',
        {
            delay: 2000,
            icon: {
                type: 'fontawesome',
                class: 'fa-bell'
            }

        });
}

function buttontoast() {
    bs4Toast.show('Toast with Button', 'This is toast with buttons example.',
        {
            delay: 2000,
            buttons: [
                {
                    text: 'Button 1',
                    class: 'btn btn-success btn-sm mr-2',
                    callback: () => {
                        alert('Button 1 clicked');
                    }
                },
                {
                    text: 'Button 2',
                    class: 'btn btn-primary btn-sm',
                    callback: () => {
                        alert('Button 2 clicked');
                    }
                }
            ],
            icon: {
                type: 'fontawesome',
                src: 'https://via.placeholder.com/150',
                class: 'fa-bell'
            }

        }

    );
}
