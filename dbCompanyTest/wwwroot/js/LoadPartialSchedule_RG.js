﻿//            <th>部門</th>
//            <th>簽核人員</th>
//            <th>簽核時間</th>
//            <th>輸入意見</th>
//            <th>簽核</th>
//            <td id="dpm_str"></td>
//            <td id="man_str"></td>
//            <td id="time_str"></td>
//            <td id="">com_str</td>
//            <td id="">sin_str</td>

//            <td id="dpm_ecu"></td>
//            <td id="man_ecu"></td>
//            <td id="time_ecu"></td>
//            <td id="com_ecu"></td>
//            <td id="sin_ecu"></td>
var split_str_dpm = $(`#inp_whostart`).text().indexOf(' ');
var str_dpm = $(`#inp_whostart`).text().substr(0, split);
var split_ecu_dpm = $(`#sele_executor`).text().indexOf(' ');
var str_ecu = $(`#sele_executor`).text().substr(0, split);