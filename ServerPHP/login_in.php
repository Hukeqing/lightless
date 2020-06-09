<?php

require '_mysql.php';

class res {
    public $errorId = 404;
    public $errorMsg  = "Account not find";
    public $accountId = -1;
    public $accountSc = 0;
    public $accountNa = "";
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$account = $arr_query['account'];
$pwd = $arr_query['pwd'];
$sqlStr = 'select * from account where email = "'.$account.'";';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    if ($r[2] == $pwd) {
        $ans->errorId = 0;
        $ans->errorMsg = "";
        $ans->accountId = $r[0];
        $ans->accountSc = $r[4];
        $ans->accountNa = $r[5];
    } else {
        $ans->errorId = 200;
        $ans->errorMsg = "password error";
    }
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
