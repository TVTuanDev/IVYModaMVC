$(function () {
  "use strict";

  /* HEADER NAVIGATION */

  $(".item.wallet .icon").click(function () {
    var index = $(".item.wallet .icon").index(this); // Lấy vị trí của button được click trong danh sách các button
    $(".sub-action")
      .not(":eq(" + index + ")")
      .not(":last")
      .hide(); // Ẩn tất cả các phần tử, ngoại trừ phần tử được click
    $(".sub-action:eq(" + index + ")").toggle("sub-action"); // Hiển thị hoặc ẩn phần tử được click
  });

  $(".item-cart .icon").click(function () {
    var subAction = $(this).parent().find(".sub-action.sub-action-cart");
    ToggleClass(subAction, "open");
  });

  $(".action-close").click(function () {
    ToggleClass($(this).parent(), "open");
  });

  $(".header-menu .menu-mobile").click(function () {
    ToggleClass($(this).parent(), "open");
  });

  $(".header-menu .sub-menu-mobile .close-menu i").click(function () {
    ToggleClass($(this).parents(".header-menu"), "open");
  });

  $(".header-menu .sub-menu-mobile ul li a i").click(function () {
    const subMenu = $(this).closest("li").find("ul").first();
    ToggleClass(subMenu, "open");
    ReplaceClass($(this), "fa-plus", "fa-minus");
  });

  function ToggleClass(element, nameClass) {
    element.hasClass(nameClass)
      ? element.removeClass(nameClass)
      : element.addClass(nameClass);
  }

  function ReplaceClass(element, classOld, classNew) {
    if (element.hasClass(classOld)) {
      element.removeClass(classOld);
      element.addClass(classNew);
    } else {
      element.removeClass(classNew);
      element.addClass(classOld);
    }
  }

  /* NAV BOTTOM */

  $(".nav-bottom .item-nav-bottom .item-nav").click(function () {
    $(this)
      .closest(".nav-bottom")
      .find(".sub-item-nav")
      .not($(this).next())
      .removeClass("open");
    ToggleClass($(this).next(), "open");
  });

  /* SLIDER BANNER */

  $(".slider-banner.owl-carousel").owlCarousel({
    loop: true,
    margin: 0,
    nav: true,
    navText: [
      '<i class="fa-solid fa-chevron-left"></i>',
      '<i class="fa-solid fa-chevron-right"></i>',
    ],
    dots: true,
    autoplay: true,
    autoplayTimeout: 7000,
    responsive: {
      0: {
        items: 1,
        nav: false,
        dots: false,
      },
      600: {
        items: 1,
        nav: false,
        dots: false,
      },
      1000: {
        items: 1,
      },
    },
  });

  $(".new-prod-slider.owl-carousel").owlCarousel({
    loop: false,
    margin: 30,
    nav: true,
    navText: [
      '<i class="fa-solid fa-chevron-left"></i>',
      '<i class="fa-solid fa-chevron-right"></i>',
    ],
    dots: false,
    autoplay: false,
    autoplayHoverPause: true,
    mouseDrag: false,
    autoplayTimeout: 7000,
    responsive: {
      0: {
        items: 2,
        nav: false,
        dots: false,
        margin: 15,
      },
      768: {
        items: 3,
      },
      1200: {
        items: 5,
      },
    },
  });

  /* TAB HOME */

  $(".exclusive-head li").click(function () {
    var tab_id = $(this).attr("data-tab");
    $(this)
      .parents(".exclusive-tabs")
      .find(".exclusive-head li")
      .removeClass("active");
    $(this)
      .parents(".exclusive-tabs")
      .find(".exclusive-inner")
      .removeClass("active");
    $(this).addClass("active");
    $("#" + tab_id).addClass("active active-hidden");
    setTimeout(function () {
      $("#" + tab_id).removeClass("active-hidden");
    }, 300);
  });

  const listColor = $(".list-products .list-color ul li");
  const favourite = $(".list-products .info-product .favourite");
  const iconShopping = $(".list-products .add-to-cart span").not(".bag-gray");
  const btnSize = $(".list-products .list-size li").not("unactive");

  listColor.click(function () {
    $(this).parent().find(".checked").removeClass("checked");
    $(this).addClass("checked");
  });

  favourite.click(function () {
    var heart = $(this).find(".fa-heart");
    ReplaceClass(heart, "fa-regular", "fa-solid");
  });

  iconShopping.click(function () {
    var listSize = $(this).parents(".product").find(".list-size");
    $(".list-size").not(listSize).removeClass("open");
    ToggleClass(listSize, "open");
  });

  btnSize.click(function () {
    var listSize = $(this).parents(".product").find(".list-size");

    toastr.options = {
      positionClass: "custom-toastr",
      closeButton: true,
      timeOut: 3000,
    };
    toastr.success("Thêm vào giỏ hàng thành công!");

    ToggleClass(listSize, "open");
  });

  /* BRAND */

  $(".slider-ads-brand.owl-carousel").owlCarousel({
    loop: true,
    margin: 30,
    nav: true,
    navText: [
      '<i class="fa-solid fa-chevron-left"></i>',
      '<i class="fa-solid fa-chevron-right"></i>',
    ],
    dots: false,
    autoplay: true,
    autoplayTimeout: 7000,
    responsive: {
      0: {
        items: 2,
        margin: 8,
      },
      600: {
        items: 2,
        margin: 8,
      },
      1000: {
        items: 2,
      },
    },
  });

  /* GALLERY */

  $(".list-gallery.owl-carousel").owlCarousel({
    loop: true,
    margin: 30,
    nav: false,
    dots: false,
    autoplay: true,
    autoplayTimeout: 7000,
    responsive: {
      0: {
        items: 2,
        margin: 8,
      },
      768: {
        items: 3,
        margin: 8,
      },
      1200: {
        items: 5,
      },
      1920: {
        items: 5,
      },
    },
  });

  /* PRODUCT */

  var listFilterProduct = $("#products .list-side .item-side-title");
  listFilterProduct.click(function () {
    ReplaceClass($(this).find("i"), "fa-plus", "fa-minus");
    $(this).parents(".item-side").find(".sub-list-side").slideToggle("active");
  });

  var listSizeProduct = $(
    "#products .list-side .item-side-size .item-sub-list"
  );
  listSizeProduct.click(function () {
    ToggleClass($(this), "checked");
  });

  var listColorProduct = $(
    "#products .list-side .item-side-color .item-sub-list"
  );
  listColorProduct.click(function () {
    ToggleClass($(this), "checked");
  });

  var listDiscountProduct = $(
    "#products .list-side .item-sub-list .item-sub-title"
  );
  listDiscountProduct.click(function () {
    ToggleClass($(this), "active");
    var checkParent = $(this).parents(".item-side-discount");
    if (checkParent.length == 1) {
      checkParent
        .find(".item-sub-title.active")
        .not($(this))
        .removeClass("active");
    }
  });

  var listFilterPlusProduct = $(
    "#products .list-side .item-sub-title.item-sub-pr"
  );
  listFilterPlusProduct.click(function () {
    ReplaceClass($(this).find("i"), "fa-plus", "fa-minus");

    var subSide = $(this).parents(".item-sub-list").find(".item-sub-side");

    subSide.slideToggle(function () {
      var atbCss = $(this).css("display");
      if (atbCss === "inline" || atbCss == "block") {
        subSide.css("display", "block");
      } else {
        subSide.css("display", "none");
      }
    });
  });

  $(".but_filter_remove").click(function () {
    location.reload();
  });

  $(".filter-search").click(function () {
    ToggleClass($(this).parent().next(), "open");
  });

  /* RANGE PRICE PRODUCT */

  let product_price_from =
    $("input[name=product_price_from]").val() != ""
      ? $("input[name=product_price_from]").val()
      : 0;
  let product_price_to =
    $("input[name=product_price_to]").val() != ""
      ? $("input[name=product_price_to]").val()
      : 10000000;

  $("#slider-range").slider({
    range: true,
    min: 0,
    max: 10000000,
    values: [product_price_from, product_price_to],
    slide: function (event, ui) {
      $("#amout-from").html(FormatCurrency(ui.values[0]) + "đ");
      $("#amout-to").html(FormatCurrency(ui.values[1]) + "đ");

      $("input[name=product_price_from]").val(ui.values[0]);
      $("input[name=product_price_to]").val(ui.values[1]);
    },
  });
  $("#slider-range-mb").slider({
    range: true,
    min: 0,
    max: 10000000,
    values: [product_price_from, product_price_to],
    slide: function (event, ui) {
      $("#amout-from-mb").html(FormatCurrency(ui.values[0]) + "đ");
      $("#amout-to-mb").html(FormatCurrency(ui.values[1]) + "đ");

      $("input[name=product_price_from]").val(ui.values[0]);
      $("input[name=product_price_to]").val(ui.values[1]);
    },
  });

  var FormatCurrency = function (number) {
    return number.toLocaleString("de-DE", { minimumFractionDigits: 0 });
  };

  /* MAIN PROD */

  $(".main-prod .top-main-prod .item-filter span").click(function () {
    ToggleClass($(this).parent(), "open");
    ToggleClass($(this).siblings(), "open");
  });

  /* PRODUCT DETAIL */

  new Swiper(".swiper-container", {
    direction: "vertical",
    loop: false,
    navigation: {
      nextEl: ".product-gallery-slide_nav .fa-chevron-down",
      prevEl: ".product-gallery-slide_nav .fa-chevron-up",
    },
    slidesPerView: 4,
    spaceBetween: 10,
    breakpoints: {
      769: {
        direction: "vertical",
      },
      0: {
        direction: "horizontal",
      },
    },
  });

  const listImgProduct = $(".product-detail .product-gallery ul li");
  const imgMainProduct = $(".product-detail .product-gallery-main img");

  listImgProduct.click(function () {
    imgMainProduct.attr("src", $(this).find("img").attr("src"));
  });

  /* PRODUCT INFO */

  const sizeInfo = $(".product-info-quantity .product-quantity");

  const favouriteDetail = $(
    ".product-detail .product-info .product-info-actions i"
  );
  const tabItemDetail = $(
    ".product-detail .product-info .product-info-tab .tab-item"
  );
  const showMore = $(
    ".product-detail .product-info .product-info-tab .show-more img"
  );

  sizeInfo.click(function () {
    const inputQuantity = $(this).parent().find("input");
    var valueInput = parseInt(inputQuantity.attr("value"));
    if ($(this).hasClass("product-detail__quantity--increase")) {
      if (valueInput >= parseInt(inputQuantity.attr("max"))) return;
      inputQuantity.val(++valueInput);
    } else {
      if (valueInput == 1) return;
      inputQuantity.val(--valueInput);
    }

    inputQuantity.attr("value", valueInput);
  });

  favouriteDetail.parent().click(function () {
    var heartDetail = $(this).find(".fa-heart");
    ReplaceClass(heartDetail, "fa-regular", "fa-solid");
  });

  tabItemDetail.click(function () {
    $(this).siblings().removeClass("active");
    $(this).addClass("active");
    const attrTab = $(this).data("tab");
    const tabEle = $(this).parents(".product-info-tab").find(`#${attrTab}`);
    tabEle.siblings().removeClass("active");
    tabEle.addClass("active");
  });

  showMore.click(function () {
    event.preventDefault();
    const tabShow = $(this)
      .parents(".product-info-tab_body")
      .find(".tab-content");

    ReplaceClass(tabShow, "hideContent", "showContent");
    ReplaceClass($(this), "show-img", "hide-img");
    ReplaceClass($(this).siblings(), "show-img", "hide-img");
  });

  /* MODAL PASSWORD */

  $(".my-account .user-info .order-block .form-buttons a").click(function () {
    event.preventDefault();
    ToggleClass($(".modal-info.change-password"), "active");

    $(".modal-info .modal-content input").first().focus();
  });

  $(".modal-info .modal-content .close-modal").click(function () {
    ToggleClass($(this).closest(".modal-info"), "active");
  });

  $(".modal-info .modal-inner .modal-content").click(function () {
    event.stopPropagation();
  });

  $(".modal-info .modal-inner").click(function () {
    ToggleClass($(this).closest(".modal-info"), "active");
  });

  /* ADDRESS */

  $(".my-account .checkout-address .order-block_title button").click(
    function () {
      event.preventDefault();
      ToggleClass($(".modal-info.add-address"), "active");

      $(".modal-info .modal-content input").first().focus();
    }
  );

    $(".my-account .checkout-address .checkout-actions button").click(
    function () {
      var parentFix = $(this).closest(".block-border");
      var valueId = parentFix.find(".idAddress").val();
      var nameUser = parentFix.find("h4").text();
      var numberPhone = parentFix.find(".phone-checkout span").last().text();
      var addressFix = parentFix.find(".address-checkout span").last().text();
      var defaultAddress = parentFix.find(".btn.btn--large.btn--outline");

      var modalInfo = $(".modal-info.fix-address");

      modalInfo.find(".modal-content input.idAddress").val(valueId);
      modalInfo.find(".modal-content input.user-name").val(nameUser);
      modalInfo.find(".modal-content input.phone-number").val(numberPhone);
      modalInfo.find(".modal-content input.address-details").val(addressFix);
      defaultAddress.text()
          ? modalInfo.find(".form-checkbox__input").prop("checked", false)
          : modalInfo.find(".form-checkbox__input").prop("checked", true);

      ToggleClass(modalInfo, "active");
    }
  );

  /* Q&A */

  $(".question-answer .order-block ul li a").click(function () {
    $(this).parent().find(".sub-menu-mb").slideToggle("active");

    ReplaceClass($(this).find("i"), "fa-plus", "fa-minus");
  });

  /* FOOTER */

  $(".site-bottom-mb .center-footer .item-ft-mb .title-footer").click(
    function () {
      ToggleClass($(this).parent(), "active");
      $(this).next().slideToggle("active");
    }
  );

  /* PROFILE */

  $(".my-account .order-sitemenu .order-sitemenu__user").click(function () {
    if ($(window).innerWidth() <= 992) {
      ToggleClass($(this), "active");

      $(this).parent().find(".order-sitemenu__menu").slideToggle("active");
    }
  });

  /* CART */

  $(".checkout .checkout-address-delivery .form-switch input").click(
    function () {
      ToggleClass(
        $(this).closest("div.pt-4").find(".ds__item__contact-info"),
        "d-block"
      );
    }
  );

  $(".checkout .view-more-product .btn").click(function () {
    ToggleClass($(this).parent().parent().find(".checkout-my-cart"), "d-block");
  });

  $(".checkout .checkout-address-delivery button").click(function () {
    ToggleClass($(this).next(), "active");
  });

  $(".checkout .checkout-address-delivery__action a").click(function () {
    ToggleClass($(".modal-info.change-address"), "active");
  });

  $(".checkout .cart-summary-voucher-title > h4")
    .last()
    .click(function () {
      ToggleClass($(this).next(), "active");
    });

  const labelPayment = $(".checkout .checkout-payment__options label");

  labelPayment.click(function () {
    if (!$(this).is(labelPayment.last())) {
      $(".checkout .cart-summary .cart-summary_button a").text(
        "Tiếp tục thanh toán"
      );
    } else {
      $(".checkout .cart-summary .cart-summary_button a").text("Hoàn thành");
    }
  });
});
