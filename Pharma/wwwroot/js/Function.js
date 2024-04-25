
function PhanTramGiam(Discount, Price)
{

    let Minus = Price - Discount;
    let ChiaHieu = Minus / Price;
    let Res = ChiaHieu * 100;

    return parseInt(Res);

}


function GetImageOne(images){
    let arrImage = images.split(',');
    return arrImage[0];
}