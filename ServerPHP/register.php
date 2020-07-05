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
$name = $arr_query['name'];
$gameVersion = $arr_query['version'];
if ($gameVersion != "0.2") {
    $ans->errorId = 200;
    $ans->errorMsg = "Game version error!";
    return;
}

$sqlStr = 'select * from account where email = "'.$account.'";';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    $ans->errorId = 404;
    $ans->errorMsg = "Email registered";
} else {
    $ans->errorId = 0;
    $ans->errorMsg = "";
    $sqlInsert = 'insert into account (email, password, registerTime, name) VALUES ("'.$account.'", "'.$pwd.'", now(), "'.$name.'");';
    $rs = querySQL($sqlInsert);
    $rs = querySQL($sqlStr);
    $r = $rs->fetch_row();
    $ans->accountId = $r[0];
    $ans->accountSc = $r[4];
    $ans->accountNa = $r[5];
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>