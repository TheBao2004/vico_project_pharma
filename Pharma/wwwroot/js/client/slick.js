$(document).ready(function(){

    $('.slice_home').slick();

    $('.slide_categories').slick({
        slidesToShow: 6,
        slidesToScroll: 1,
        // autoplay: true,
        autoplaySpeed: 2000,
      });

      $('.list_btn_detail_product').slick({
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 4
      });

});