$(document).ready(function () {
    //отобразить/скрыть панель Login
    $(".header-user-bar-login-icon").click(function () {
        if ($(".login-form-container").hasClass("login-form-container-hidden")) {
            $(".login-form-container").addClass("login-form-container-visible");
            $(".login-form-container").removeClass("login-form-container-hidden");
        }
        else if ($(".login-form-container").hasClass("login-form-container-visible")) {
            $(".login-form-container").addClass("login-form-container-hidden");
            $(".login-form-container").removeClass("login-form-container-visible");
        }
    });

    //отобразить панель добавления отзыва
    $(".add-review-button").click(function () {
        $(".add-review-block").addClass("add-review-block-visible");
        $(".add-review-block").removeClass("add-review-block-hidden");
    });

    //закрыть панель добавления отзыва
    $(".add-review-cancel-button").click(function () {
        $(".add-review-block").addClass("add-review-block-hidden");
        $(".add-review-block").removeClass("add-review-block-visible");
    });
    
    //отобразить панель добавления сообщения в дискуссию
    $(".add-message-button").click(function () {
        $(".add-message-block").addClass("add-message-block-visible");
        $(".add-message-block").removeClass("add-message-block-hidden");
        $(".discussion-group").attr("value", "-1");
    });

    //закрыть панель добавления сообщения в дискуссию
    $(".add-message-cancel-button").click(function () {
        $(".add-message-block").addClass("add-message-block-hidden");
        $(".add-message-block").removeClass("add-message-block-visible");
        $(".discussion-group").attr("value", "");
    });

    //отобразить панель добавления сообщения в дискуссию (REPLY)
    $(".discussions-list-item-bottom-reply").click(function () {
        $(".add-message-block").addClass("add-message-block-visible");
        $(".add-message-block").removeClass("add-message-block-hidden");
        $(".message-textarea").val($(this).attr("reply-to") + ", ");
        $(".discussion-group").attr("value", $(this).attr("discussion-group"));
        $(".message-textarea").focus();
        
    });

    //закрыть панель добавления сообщения в дискуссию (REPLY)
    $(".add-message-cancel-button").click(function () {
        $(".add-message-block").addClass("add-message-block-hidden");
        $(".add-message-block").removeClass("add-message-block-visible");
        $(".message-textarea").val("");
        $(".discussion-group").attr("value", "");
    });

    //отобразить изображение товара в большом окне
    $(".small-good-image-container").click(function () {
        var smallImageSrc = $(this).find('img').attr("src");
        $(".big-good-image-container img").attr("src", smallImageSrc);        
    });

    //выбрать цвет товара
    $(".select-color").on('change', function (e) {
        window.location = this.value;
    });
    
    //добавить товар в корзину
    $(".add-to-cart-button").click(function () {
        var goodId = $(this).attr("good-id");
        if ( $(".add-to-cart-button").hasClass("good-not-in-cart") ) {
            $.get("/Cart/AddGoodToCart/" + goodId, function (goodIdList) {

            });
            //изменить отображение кнопки добавить/удалить товар
            $(".add-to-cart-button").addClass("good-in-cart");
            $(".add-to-cart-button").removeClass("good-not-in-cart");
            $(".add-to-cart-button").html("Remove from cart");
        }
        else if ( $(".add-to-cart-button").hasClass("good-in-cart") ) {
            $.get("/Cart/RemoveGoodFromdCart/" + goodId, function (goodIdList) {

            });
            //изменить отображение кнопки добавить/удалить товар
            $(".add-to-cart-button").addClass("good-not-in-cart");
            $(".add-to-cart-button").removeClass("good-in-cart");
            $(".add-to-cart-button").html("Add to cart");
        }        
    });

    //отобразить больше дискуссий
    $(".show-more-discussions").click(function () {
        var goodId = $(".discussions-list").attr("good-id");
        var showedDiscussionsNumber = $(".discussions-list").attr("showed-discussions");
        var addedDiscussionsNumber = 2;        
        $.get("/Catalog/ShowMoreDiscussions/" + goodId + "/" + showedDiscussionsNumber + "/" + addedDiscussionsNumber, function (jsonResult) {
            addedDiscussionsList = JSON.parse(jsonResult[1]);
            var discussions;
            addedDiscussionsList.forEach(function (group) {
                group.forEach(function (item) {
                    if (item.FirstDiscussionMessage === true) {
                        discussions = `
                            <div class="discussions-list-item">
                                <div class="discussions-list-item-top">
                                    ` + item.Message + `
                                </div>
                                <div class="discussions-list-item-bottom">
                                    <div class="discussions-list-item-bottom-author">
                                        <div class="discussions-list-item-bottom-author-photo"></div>
                                        <div class="discussions-list-item-bottom-author-name">` + item.AuthorName + `</div>
                                    </div>
                                    <div class="discussions-list-item-bottom-date">
                                        <div class="discussions-list-item-bottom-date-icon"></div>
                                        <div class="discussions-list-item-bottom-date-text">` + item.StringDate + `</div>
                                    </div>
                                    <div class="discussions-list-item-bottom-reply" reply-to="` + item.AuthorName + `" discussion-group="` + item.DiscussionGroup + `">
                                        <div class="discussions-list-item-bottom-reply-icon"></div>
                                        <div class="discussions-list-item-bottom-reply-text">Reply</div>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                    else {
                        discussions = `
                            <div class="discussions-list-item-reply">
                                <div class="discussions-list-item-top">`
                                    + item.Message +
                                `</div>
                                <div class="discussions-list-item-bottom">
                                    <div class="discussions-list-item-bottom-author">
                                        <div class="discussions-list-item-bottom-author-photo"></div>
                                        <div class="discussions-list-item-bottom-author-name">` + item.AuthorName + `</div>
                                    </div>
                                    <div class="discussions-list-item-bottom-date">
                                        <div class="discussions-list-item-bottom-date-icon"></div>
                                        <div class="discussions-list-item-bottom-date-text">` + item.StringDate + `</div>
                                    </div>
                                    <div class="discussions-list-item-bottom-reply" reply-to="` + item.AuthorName + `" discussion-group="` + item.DiscussionGroup + `">
                                        <div class="discussions-list-item-bottom-reply-icon"></div>
                                        <div class="discussions-list-item-bottom-reply-text">Reply</div>
                                    </div>
                                </div>
                            </div>
                        `;
                    }
                    $(discussions).insertBefore(".show-more-discussions");
                });
            });
            if (jsonResult[0] === "true") {
                $(".show-more-discussions").addClass("hidden");
                showedDiscussionsNumber = $('.discussions-list-item').length;
                $(".discussions-list").attr("showed-discussions", showedDiscussionsNumber);
            }
            else {
                alert();
                showedDiscussionsNumber = Number(showedDiscussionsNumber) + Number(addedDiscussionsNumber);
                $(".discussions-list").attr("showed-discussions", showedDiscussionsNumber);
            }
        });        
    });

    //отобразить больше отзывов корзину
    $(".show-more-reviews").click(function () {
        var goodId = $(".reviews-list").attr("good-id");
        var showedReviewsNumber = $(".reviews-list").attr("showed-reviews");
        var addedReviewsNumber = 2;

        $.get("/Catalog/ShowMoreReviews/" + goodId + "/" + showedReviewsNumber + "/" + addedReviewsNumber, function (jsonResult) {
            addedReviewsList = JSON.parse(jsonResult[1]);
                        
            addedReviewsList.forEach(function (item) {
                var review = `
                    <div class="reviews-list-item">
                        <div class="reviews-list-item-top">
                            <div class="reviews-list-item-top-mark">
                                <div class="reviews-list-item-top-mark-icon">★ </div>
                                <div class="reviews-list-item-top-mark-number">` + item.Mark + `</div>
                            </div>
                            <div class="reviews-list-item-top-experience-of-use">
                                <div class="reviews-list-item-top-experience-of-use-icon"></div>
                                <div class="reviews-list-item-top-experience-of-use-text">Experience of use:</div>
                                <div class="reviews-list-item-top-experience-of-use-number">` + item.ExperienceOfUse + `</div>
                            </div>
                            <div class="reviews-list-item-top-author">
                                <div class="reviews-list-item-top-author-avatar"></div>
                                <div class="reviews-list-item-top-author-name">` + item.Reviewer + `</div>
                            </div>
                        </div>
                        <div class="reviews-list-item-body">
                            <div class="reviews-list-item-body-advantages">
                                <p class="reviews-list-item-body-advantages-header">Advantages</p>
                                <p class="reviews-list-item-body-advantages-text">
                                    ` + item.Advantages + `
                                </p>
                            </div>
                            <div class="reviews-list-item-body-disadvantages">
                                <p class="reviews-list-item-body-disadvantages-header">Disadvantages</p>
                                <p class="reviews-list-item-body-disadvantages-text">
                                    ` + item.Disadvantages + `
                                </p>
                            </div>
                            <div class="reviews-list-item-body-comment">
                                <p class="reviews-list-item-body-comment-header">Comment</p>
                                <p class="reviews-list-item-body-comment-text">
                                    ` + item.Comment + `
                                </p>
                            </div>
                        </div>
                        <div class="reviews-list-item-bottom">
                            <div class="reviews-list-item-bottom-data">
                                <div class="reviews-list-item-bottom-data-icon"></div>
                                <div class="reviews-list-item-bottom-data-text">` + item.Date + `</div>
                            </div>
                            <div class="reviews-list-item-bottom-approval">
                                <div class="reviews-list-item-bottom-approval-like-icon"></div>
                                <div class="reviews-list-item-bottom-approval-like-number">` + item.LikesNumber + `</div>
                                <div class="reviews-list-item-bottom-approval-dislike-icon"></div>
                                <div class="reviews-list-item-bottom-approval-dislike-number">` + item.LikesNumber + `</div>
                            </div>
                        </div>
                    </div>
                `;
                $(review).insertBefore(".show-more-reviews");              
            }); 
            if (jsonResult[0] === "true") {
                $(".show-more-reviews").addClass("hidden");
                showedReviewsNumber = $('.reviews-list-item').length;

                $(".reviews-list").attr("showed-reviews", showedReviewsNumber);
            }
            else {
                showedReviewsNumber = Number(showedReviewsNumber) + Number(addedReviewsNumber);
                $(".reviews-list").attr("showed-reviews", showedReviewsNumber);
            }
        });             
    });

    //добавить похожий товар в корзину
    $(".similar-offers-list-item-add-to-cart-button").click(function () {
        var goodId = $(this).attr("good-id");

        if ($(this).hasClass("good-not-in-cart")) {
            $.get("/Cart/AddGoodToCart/" + goodId, function (goodIdList) {

            });
            //изменить отображение кнопки добавить/удалить товар
            $(this).addClass("good-in-cart").removeClass("good-not-in-cart");
            $(this).html("Remove from cart");
        }
        else if ($(this).hasClass("good-in-cart")) {
            $.get("/Cart/RemoveGoodFromdCart/" + goodId, function (goodIdList) {

            });
            //изменить отображение кнопки добавить/удалить товар
            $(this).addClass("good-not-in-cart").removeClass("good-in-cart");
            $(this).html("Add to cart");
        }
    });   

    //вывод сообщения о неработающем сервисе
    $(".category-goods-list-item-special-buttons-favorite, .category-goods-list-item-special-buttons-compare").click(function () {
        alert("Sorry, that service doesn't work...");
    });

    //вывод сообщения о неработающем сервисе
    $(".purchase-options-favorite, .purchase-options-compare").click(function () {
        alert("Sorry, that service doesn't work...");
    });

    //вывод сообщения о неработающем сервисе
    $(".go-to-favorite-page, .go-to-compare-page").click(function () {
        alert("Sorry, that service doesn't work...");
    });

    //перейти в панель методов оплаты
    $(".button-go-to-payment-methods").click(function () {
        $(".goods-in-cart-list-container").addClass("hidden").removeClass("visible");
        $(".payment-methods-container").addClass("visible").removeClass("hidden");
        $(".cart-icon").addClass("cart-menu-item-icon-block").removeClass("cart-menu-selected-item-icon-block");
        $(".payment-icon").addClass("cart-menu-selected-item-icon-block").removeClass("cart-menu-item-icon-block");
    });

    //вернуться в панель списка товаров корзины
    $(".button-back-to-cart").click(function () {
        $(".payment-methods-container").addClass("hidden").removeClass("visible");
        $(".goods-in-cart-list-container").addClass("visible").removeClass("hidden");
        $(".cart-icon").addClass("cart-menu-selected-item-icon-block").removeClass("cart-menu-item-icon-block");
        $(".payment-icon").addClass("cart-menu-item-icon-block").removeClass("cart-menu-selected-item-icon-block");
    });

    //вернуться в панель методов оплаты
    $(".button-back-to-payment-methods").click(function () {
        $(".delivery-methods-container").addClass("hidden").removeClass("visible");
        $(".payment-methods-container").addClass("visible").removeClass("hidden");
        $(".payment-icon").addClass("cart-menu-selected-item-icon-block").removeClass("cart-menu-item-icon-block");
        $(".delivery-icon").addClass("cart-menu-item-icon-block").removeClass("cart-menu-selected-item-icon-block");
    });

    //перейти в панель методов доставки
    $(".button-go-to-delivery-methods").click(function () {
        $(".payment-methods-container").addClass("hidden").removeClass("visible");
        $(".delivery-methods-container").addClass("visible").removeClass("hidden");
        $(".payment-icon").addClass("cart-menu-item-icon-block").removeClass("cart-menu-selected-item-icon-block");
        $(".delivery-icon").addClass("cart-menu-selected-item-icon-block").removeClass("cart-menu-item-icon-block");
    });    
    
    //перейти в панель методов доставки
    $(".payment-method").click(function () {
        $(".payment-method.active-bank-system").removeClass("active-bank-system");
        $(this).addClass("active-bank-system");
        $(".payment-method-input").val($(this).attr("bank-system"));
    });

    $('.delivery-methods-information-fourth-column-addressee-phone-number-input').change(function () {
        $('.card-owner-name-value').html($('.payment-methods-card-information-first-column-card-owner-name-input').val());
        $('.card-number-value').html($('.payment-methods-card-information-first-column-card-number-input').val());
        $('.card-cvv-value').html($('.payment-methods-card-information-second-column-card-cvv-input').val());
        $('.card-month-value').html($('.select-card-month').val());
        $('.card-year-value').html($('.select-card-year').val());
        $('.addressee-first-name-value').html($('.delivery-methods-information-first-column-addressee-first-name-input').val());
        $('.addressee-second-name-value').html($('.delivery-methods-information-first-column-addressee-second-name-input').val());
        $('.addressee-country-value').html($('.delivery-methods-information-second-column-addressee-country-input').val());
        $('.addressee-region-value').html($('.delivery-methods-information-second-column-addressee-region-input').val());
        $('.addressee-city-value').html($('.delivery-methods-information-second-column-addressee-city-input').val());
        $('.addressee-index-value').html($('.delivery-methods-information-third-column-addressee-index-input').val());
        $('.addressee-street-address-value').html($('.delivery-methods-information-third-column-addressee-street-address-input').val());
        $('.addressee-phone-number-value').text($('.delivery-methods-information-fourth-column-addressee-phone-number-input').val());
        $('.addressee-email-value').html($('.delivery-methods-information-fourth-column-addressee-email-input').val());    
    });

    //уменьшить количество комплектов товарова
    $(".goods-in-cart-list-item-quantity-minus").click(function () {
        var goodQuantity = $(this).parent().children(".goods-in-cart-list-item-quantity-number").text();
        if (Number(goodQuantity) > 1) {
            goodQuantity = Number(goodQuantity) - 1;
        }
        countGoodTotalPrice($(this), goodQuantity);
        countOrderPrice();
    });

    //увеличить количество комплектов товарова
    $(".goods-in-cart-list-item-quantity-plus").click(function () {
        var goodQuantity = $(this).parent().children(".goods-in-cart-list-item-quantity-number").text();
        if (Number(goodQuantity) < 100) {
            goodQuantity = Number(goodQuantity) + 1;
        }
        countGoodTotalPrice($(this), goodQuantity);
        countGoodsTotalPrice();
    });

    //Choose free delivery
    $(".delivery-methods-header-free-delivery").click(function () {
        $(".delivery-methods-header-free-delivery").addClass("active-delivery-type");
        $(".delivery-methods-header-express-delivery").removeClass("active-delivery-type");
        $(".delivery-methods-information-fifth-column").addClass("hidden");
        $(".delivery-price-input").val("0");
        $(".delivery-methods-input").val("Free delivery");

    });

    //Choose express delivery
    $(".delivery-methods-header-express-delivery").click(function () {
        $(".delivery-methods-header-express-delivery").addClass("active-delivery-type");
        $(".delivery-methods-header-free-delivery").removeClass("active-delivery-type");
        $(".delivery-methods-information-fifth-column").removeClass("hidden");
        if ($(".delivery-methods-information-fifth-column-ups-delivery").hasClass("selected-delivery-method")) {
            $(".delivery-price-input").val($(".delivery-methods-information-fifth-column-ups-delivery").attr("delivery-price"));
            $(".delivery-methods-input").val("UPS delivery");
        }
        else if ($(".delivery-methods-information-fifth-column-dhl-delivery").hasClass("selected-delivery-method")) {
            $(".delivery-price-input").val($(".delivery-methods-information-fifth-column-dhl-delivery").attr("delivery-price"));
            $(".delivery-methods-input").val("DHL delivery");
        }        
    });

    //Choose UPS express delivery
    $(".delivery-methods-information-fifth-column-ups-delivery").click(function () {
        $(".delivery-methods-information-fifth-column-ups-delivery").addClass("selected-delivery-method");
        $(".delivery-methods-information-fifth-column-dhl-delivery").removeClass("selected-delivery-method");
        $(".delivery-methods-information-fifth-column-ups-delivery-price-icon").addClass("selected-delivery-method-icon");
        $(".delivery-methods-information-fifth-column-dhl-delivery-price-icon").removeClass("selected-delivery-method-icon");
        $(".delivery-price-input").val($(".delivery-methods-information-fifth-column-ups-delivery").attr("delivery-price"));
        $(".delivery-methods-input").val("UPS delivery");
    });

    //Choose DHL express delivery
    $(".delivery-methods-information-fifth-column-dhl-delivery").click(function () {
        $(".delivery-methods-information-fifth-column-dhl-delivery").addClass("selected-delivery-method");
        $(".delivery-methods-information-fifth-column-ups-delivery").removeClass("selected-delivery-method");
        $(".delivery-methods-information-fifth-column-dhl-delivery-price-icon").addClass("selected-delivery-method-icon");
        $(".delivery-methods-information-fifth-column-ups-delivery-price-icon").removeClass("selected-delivery-method-icon");
        $(".delivery-price-input").val($(".delivery-methods-information-fifth-column-dhl-delivery").attr("delivery-price"));
        $(".delivery-methods-input").val("DHL delivery");
    });


    //Order form validation
    $('.order-form1').validate({
        rules: {
            cardOwnerName: {
                required: true,
                minlength: 3
            },
            cardNumber: {
                required: true,
                isCardNumberFormat: true
            },
            cardCvv: {
                required: true,
                minlength: 3,
                maxlength: 3,
                digits: true
            },
            addresseeFirstName: {
                required: true,
                minlength: 3
            },
            addresseeSecondName: {
                required: true,
                minlength: 3
            },
            addresseeCountry: {
                required: true,
                minlength: 3
            },
            addresseeRegion: {
                required: true,
                minlength: 3
            },
            addresseeCity: {
                required: true,
                minlength: 3
            },
            addresseeIndex: {
                required: true,
                minlength: 3,
                digits: true
            },
            addresseeStreetAddress: {
                required: true,
                minlength: 10
            },
            addresseePhoneNumber: {
                required: true,
                minlength: 3,
                isPhoneNumberFormat: true
            },
            addresseeEmail: {
                required: true
            }
        },
        messages: {
            cardOwnerName: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            cardNumber: {
                required: "Field is required",
                isCardNumberFormat: "Is not a card number format"
            },
            cardCvv: {
                required: "Field is required",
                minlength: "Enter 3 numbers",
                maxlength: "Enter 3 numbers",
                digits: "Only numbers are allowed"
            },
            addresseeFirstName: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            addresseeSecondName: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            addresseeCountry: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            addresseeRegion: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            addresseeCity: {
                required: "Field is required",
                minlength: "Enter at least 3 characters"
            },
            addresseeIndex: {
                required: "Field is required",
                minlength: "Enter at least 3 characters",
                digits: "Only numbers are allowed"                
            },
            addresseeStreetAddress: {
                required: "Field is required",
                minlength: "Enter at least 10 characters"
            },
            addresseePhoneNumber: {
                required: "Field is required",
                minlength: "Enter at least 3 characters",
                isPhoneNumberFormat: "Is not a phone number format"
            },
            addresseeEmail: {
                required: "Field is required",
                email: "Is not an email format" 
            }
        }
    });
    
    $.validator.addMethod('isPhoneNumberFormat', function (value) {
        return /^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$/.test(value);
    }, "Is not phone number format");

    $.validator.addMethod('isCardNumberFormat', function (value) {
        return /^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$/.test(value);
    }, "Is not card number format");

});

function countGoodTotalPrice(object, goodQuantity) {
    object.parent().children(".goods-in-cart-list-item-quantity-number").text(goodQuantity);
    object.parent().parent().children(".good-quantity-input").val(goodQuantity);
    var goodPrice = object.parent().parent().children(".good-price-input").val();
    var totalGoodPrice = Number(goodPrice) * Number(goodQuantity);
    object.parent().parent().children(".good-total-price-input").val(totalGoodPrice);
    object.parent().parent().children(".goods-in-cart-list-item-total").text("$ " + totalGoodPrice);
}

function countGoodsTotalPrice() {
    var goodsTotalPrice = 0;
    $('.goods-in-cart-list').children(".goods-in-cart-list-item").each(function () {
        var goodTotalPrice = $(this).children(".good-total-price-input").val();
        goodsTotalPrice += Number(goodTotalPrice); 
    });
    $('.goods-in-cart-list-footer-total-price-number').text("$ " + goodsTotalPrice);
}