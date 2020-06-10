<?php
header("Content-type: text/html; charset=utf-8");
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
$gameVersion = $arr_query['version'];

if ($gameVersion != "0.2") {
    $ans->errorId = 200;
    $ans->errorMsg = "Game version error!";
    echo json_encode($ans, JSON_UNESCAPED_UNICODE);
    return;
}

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
        $ans->accountNa = urldecode($ans->accountNa);
    } else {
        $ans->errorId = 200;
        $ans->errorMsg = "password error";
    }
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
