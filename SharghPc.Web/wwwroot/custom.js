function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
};

//$('#quantity').on('change', function (e) {
//    var count = parseInt(e.target.value, 0);
//    console.log(count);
//    $('#add_product_to_order_count').val(count);
//});

$('#submit_product_to_Oders').on('click', function () {
    $('#addProductToOrderForm').submit();
});

function addProductToOrderList(Id) {
    $.get('/Cart/AddToCart?cartDto.ProductId=' + Id + '&cartDto.Count=1').then(res => {
        location.reload();
        
    });
};

function OnSuccessAddProductToOrder(res) {
    ShowMessage('', res.message);
    location.reload();
};


function RemoveProductFromCart(Id) {
    $.get('/Cart/RemoveProductFromCart/' + Id).then(res => {
        ShowMessage("", res.message, 'info');
        location.reload();
    });
};

function changeCartItemCount(Id) {
    console.log(Id);
    $.get('/CartPartial').then(res => {
        $('#user-open-cart').html(res);
    });
};
