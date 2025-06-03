$(document).ready(function () {
    function activateMenu(menuId) {
        $('.menu-link').removeClass('active');
        $('.link-dark').removeClass('active');
        $('.btn-toggle').removeClass('active');
        $('.collapse').removeClass('show');

        $('#' + menuId).addClass('active'); 
        $('#' + menuId).closest('.menu-link').addClass('active'); 

        var parentCollapse = $('#' + menuId).closest('.collapse');
        if (parentCollapse.length) {
            parentCollapse.addClass('show'); 
            parentCollapse.prev('.btn-toggle').attr('aria-expanded', 'true').addClass('active'); 
            parentCollapse.closest('.sub-item').addClass('active'); 
            parentCollapse.prev('.btn-toggle').removeClass('collapsed');
        }
    }

    var activeMenu = sessionStorage.getItem('activeMenu');
    if (activeMenu) {
        activateMenu(activeMenu);
    } else {
        activateMenu('dashboard');
    }

    $('.menu-link a, .submenu-item a').on('click', function (e) {
        e.preventDefault(); 
        var previousMenu = sessionStorage.getItem('activeMenu');
        if (previousMenu) {
            $('#' + previousMenu).removeClass('active'); 
            $('#' + previousMenu).closest('.menu-link').removeClass('active'); 
        }

        sessionStorage.removeItem('activeMenu');

        var menuId = $(this).attr('id');
        activateMenu(menuId);

        sessionStorage.setItem('activeMenu', menuId);

        window.location = $(this).attr('href');
    });

    $('#dashboard a').on('click', function (e) {
        e.preventDefault(); 

        activateMenu('dashboard');

        sessionStorage.setItem('activeMenu', 'dashboard');

        window.location = $(this).attr('href');
    });

    $('#show-sidebar').click(function () {
        $('.page-wrapper').addClass('toggled');
    });

    $('.sidebar-menus .close-btn').click(function () {
        $('.sidebar-icons ul li').removeClass('active');
    });

    $('.btn-toggle').on('click', function () {
        var targetCollapse = $(this).attr('data-bs-target');

        $(targetCollapse).collapse('toggle');

        $(this).attr('aria-expanded', $(targetCollapse).hasClass('show')).toggleClass('active', $(targetCollapse).hasClass('show'));
    });

    $('.collapse').on('shown.bs.collapse', function () {
        var btn = $(this).prev('.btn-toggle');
        btn.attr('aria-expanded', 'true').addClass('active'); 
    });

    $('.collapse').on('hidden.bs.collapse', function () {
        var btn = $(this).prev('.btn-toggle');
        btn.attr('aria-expanded', 'false').removeClass('active'); 
        sessionStorage.clear();
    });
    $('#logout').on('click', function (e) {
        sessionStorage.clear();
    });
});

//// Password Hide and Show ////

$(".toggle-password").click(function () {

    $(this).text(function (i, v) {

        return v === 'Show' ? 'Hide' : 'Show'

    });

    input = $(this).parent().find("input");

    if (input.attr("type") == "password") {

        input.attr("type", "text");

    } else {

        input.attr("type", "password");

    }

});


// ========== Header Search ========== //

$(".searchbtn").click(function () {

    $(".search-form").addClass("active");

});

$(".search-close").click(function () {

    $(".search-form.active").removeClass("active");
});

// Testimonials Slider //
var testimonialSwiper = new Swiper(".TestimonialSlider", {
    slidesPerView: 1,
    spaceBetween: 60,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
});

// Internship Slider //
var internshipSwiper = new Swiper(".internshipSlider", {
    slidesPerView: 4,
    spaceBetween: 30,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        '1360': { slidesPerView: 4, spaceBetween: 10 },
        '992': { slidesPerView: 3, spaceBetween: 10 },
        '640': { slidesPerView: 2, spaceBetween: 10 },
        '280': { slidesPerView: 1, spaceBetween: 10 },
    },
});

// Search Internship Slider //
var searchInternshipSwiper = new Swiper(".searchInternship", {
    slidesPerView: 6,
    spaceBetween: 20,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        '1360': { slidesPerView: 6, spaceBetween: 10 },
        '992': { slidesPerView: 4, spaceBetween: 10 },
        '640': { slidesPerView: 3, spaceBetween: 10 },
        '280': { slidesPerView: 2, spaceBetween: 10 },
    },
});

// Companies Slider //
var companiesSwiper = new Swiper(".CompaniesSection", {
    slidesPerView: 5,
    spaceBetween: 20,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        '1360': { slidesPerView: 5, spaceBetween: 10 },
        '992': { slidesPerView: 4, spaceBetween: 10 },
        '640': { slidesPerView: 3, spaceBetween: 10 },
        '280': { slidesPerView: 2, spaceBetween: 10 },
    },
});

// Internships by Company Slider //
var internshipsByCompanySwiper = new Swiper(".InternshipsByCompanys", {
    slidesPerView: 3,
    spaceBetween: 20,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        '1360': { slidesPerView: 3, spaceBetween: 10 },
        '992': { slidesPerView: 3, spaceBetween: 10 },
        '640': { slidesPerView: 2, spaceBetween: 10 },
        '280': { slidesPerView: 1, spaceBetween: 10 },
    },
});

// ========== Image Upload & Preview ========== //

function readURL(input) {

    let previewClass = $(input).attr('data-preview-class');

    console.log(previewClass);

    if (input.files && input.files[0]) {

        var reader = new FileReader();

        reader.onload = function (e) {

            $('.' + previewClass).css('background-image', 'url(' + e.target.result + ')');

            $('.' + previewClass).hide();

            $('.' + previewClass).fadeIn(650);

        }

        reader.readAsDataURL(input.files[0]);

    }

}

$(document).on("change", ".imageUpload", function () {

    readURL(this);

});

$(document).ready(function () {
 
    // Handle request options (Approve/Reject)
    $(".request-opt").click(function (e) {
        e.preventDefault();

        var appId = $(this).attr("app-id");
        var appName = $(this).attr("app-name");
        var appTxt = $(this).attr("app-txt");

        // Determine status based on appTxt value
        var status = appTxt.toLowerCase() === "approved" ? "Approved" : "Rejected";

        // Update form fields dynamically based on the appId
        $("#requestUser_" + appId).val(appName);
        $("#requestId_" + appId).val(appId);
        $("#requestStatus_" + appId).val(status);
        $("#requestNotes_" + appId).val("Status changed to " + status);

        // Create formData object for AJAX call
        var formData = {
            Id: appId,
            Notes: "Status changed to " + status,
            Status: status
        };

        // Send AJAX request
        $.ajax({
            url: '/Attachment/ApproveDecline',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(formData),
            success: function () {
                window.location.reload();
            },
            error: function () {
                window.location.reload();
            }
        });
    });

    // Handle the Terms and Conditions checkbox
    $('#flexCheckDefault').on('change', function () {
        $('#submitButton').prop('disabled', !this.checked);
    });

});
