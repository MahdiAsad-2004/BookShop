 /**=====================
     Quantity js
==========================**/
 $('.qty-right-plus').click(function () {
     if ($(this).prev().val() < 9) {
         $(this).prev().val(+$(this).prev().val() + 1);
     }
 });
 $('.qty-left-minus').click(function () {
    //console.log('quantity minus');
    //console.log($(this).next().val())
    //var input = $(this).next()[0];
    //console.log(input);
    //console.log(input.value);
    //input.value = 10;
    if ($(this).next().val() > 1) {
        if ($(this).next().val() > 1) $(this).next().val(+$(this).next().val() - 1);
        }
    //input.value = 11;
    //input.setAttribute('value' , 15);
 });