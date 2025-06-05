$(document).ready(function () {

  
//   $(".header_menu_inner a").click(function(e) {
//     e.preventDefault();
//     var aid = $(this).attr("href");
//     $('html,body').animate({scrollTop: $(aid).offset().top}, 100);
// });


  setTimeout(function(){
    var element = document.querySelector(".app_page_submenu a.active");
    element.scrollIntoView({behavior: "smooth" , block: 'nearest', inline: "center"});        
  }, 100);

  $('#menu_mbl_CTA').on('click', function () {
    $('#app_Mainheader').addClass('MenuOpen');

    $('.Close_menu_overlay').on('click', function () {
      $('#app_Mainheader').removeClass('MenuOpen');
    });
    $('#Close_menu_CTA').on('click', function () {
      $('#app_Mainheader').removeClass('MenuOpen');
    });
  });



  $('#menu_mbl_AfterLogin').on('click', function () {
    $('.page-wrapper-panel').addClass('MenuOpen');

    $('.Close_menu_overlay').on('click', function () {
      $('.page-wrapper-panel').removeClass('MenuOpen');
    });
    $('#Close_menu_CTA').on('click', function () {
      $('.page-wrapper-panel').removeClass('MenuOpen');
    });
  });

  //Dropdown Start
  $('.hasChild a').on('click', function () {
    // $(this).toggleClass('SubMenuOpen');
    $(this).parents('li').toggleClass('SubMenuOpen');
    $(this).siblings('.submenuApp').slideToggle();
  });

  $('.hasChild.active').addClass('SubMenuOpen');
  $('.hasChild.active').find('.submenuApp').slideDown();
//Dropdown End

  var backtoTop = $('#backtoTop');

  $(window).scroll(function () {
    if ($(window).scrollTop() > 800) {
      backtoTop.addClass('show');
    } else {
      backtoTop.removeClass('show');
    }
  });

  backtoTop.on('click', function (e) {
    e.preventDefault();
    $('html, body').animate({ scrollTop: 0 }, '800');
  });




$('.footer_title_action').on('click', function(){
$(this).parents('.footer_title').siblings('.footer_content').slideToggle();;
});


});



// Filter open Start
function FilterOpenMbl() {
  const element = document.getElementById("page_have_filter");
  element.classList.add("filters_open");
}

function FilterCloseMbl() {
  var element = document.getElementById("page_have_filter");
  element.classList.remove("filters_open");
}
//  Search bar close End


window.onscroll = function () { myFunction() };

var header = document.getElementById("app_Mainheader");
var sticky = header.offsetTop;

function myFunction() {
  if (window.pageYOffset > sticky) {
    header.classList.add("sticky");
  } else {
    header.classList.remove("sticky");
  }
}




