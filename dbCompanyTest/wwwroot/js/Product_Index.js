//產品明細
function check_ProDetail_Edit() {
    let form = $("#Form_ProDe_E");
    let errorstr = "";
    //商品數量
    let 商品數量 = form.find("input[name='商品數量']");
    errorstr += check_null(商品數量, "商品數量")
    return errorstr;
}

//產品明細
function check_ProDetail_Create() {
    let form = $("#Form_ProDe_C");
    let errorstr = "";
    //商品數量
    let 商品數量 = form.find("input[name='數量']");
    errorstr += check_null(商品數量, "商品數量")
    //photo1
    let photo1 = form.find("input[name='photo1']");
    console.log(photo1);
    errorstr += check_file_null(photo1, "照片1")
    //photo2
    let photo2 = form.find("input[name='photo2']");
    errorstr += check_file_null(photo2, "照片2")
    //photo3
    let photo3 = form.find("input[name='photo3']");
    errorstr += check_file_null(photo3, "照片3")

    return errorstr;
}


//清空前端警告
function clear_check(obj) {
    obj.find("input").prop("class", "form-control");
    obj.find("span").html("");
}




function check_file_null(obj, mess) {
    let error = "";
    let file = obj.get(0);
    if (file.files.length == 0) {
        error += `請輸入${mess}`;
        obj.prop("class", "form-control is-invalid")
        obj.next(".invalid-feedback").find("span").html(`請輸入${mess}`);
    }
    else {
        obj.prop("class", "form-control is-valid");
        obj.next(".invalid-feedback").html("");
    }

}

//產品新增與修改
function check_ProC_Form() {
    let form01 = $("#Moda_P_C");
    let errorstr = "";

    let time = form01.find("input[name='time']");
    errorstr += check_null(time, "上架日期")
   
    //商品名稱
    let name = form01.find("input[name='name']");
    errorstr += check_null_lenth(name, "品名")

    //商品價格
    let price = form01.find("input[name='price']");
    errorstr += check_null(price, "價格")
    
    //商品介紹
    let introduce = form01.find("#_introduce");
    errorstr += check_null(introduce, "商品介紹")
    
    //商品材質   
    let material = form01.find("input[name='material']");
    errorstr += check_null_lenth(material, "材質")

    //商品重量
    let weight = form01.find("input[name='weight']");
    errorstr += check_null(weight, "重量")

    //商品成本
    let cost = form01.find("input[name='cost']");
    errorstr += check_null(cost, "成本")

    return errorstr;
}


function check_null(obj, mess) {
    let error = "";   
    if (obj.val() == "") {
        error += `請輸入${mess}`;   
        obj.prop("class", "form-control is-invalid")
        obj.next(".invalid-feedback ").find("span").html(`請輸入${mess}`);
    }
    else {
        obj.prop("class", "form-control is-valid");
        obj.next(".invalid-feedback").html("");
    }
    return error;
}



function check_null_lenth(obj, mess) {
    let error = "";   
    if (obj.val() == "") {
        error += `請輸入${mess}`;
        
        obj.prop("class", "form-control is-invalid")
        obj.next(".invalid-feedback").find("span").html(`請輸入${mess}`);
    }
    if (obj.val().length > 45) {
        error += "字數超出限制!";
      
        obj.prop("class", "form-control is-invalid")
        obj.next(".invalid-feedback").find("span").html("字數超出限制");
    }
 
    if (error.length == 0) {
        obj.prop("class", "form-control is-valid");
        obj.next(".invalid-feedback").find("span").html("");
    }

    return error;
}