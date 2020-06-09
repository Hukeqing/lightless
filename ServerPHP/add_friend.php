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
$fromAccount = $arr_query['from'];
$sqlStr = 'select id from account where email = "'.$account.'";';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    if ($r[0] == $fromAccount) {
        $ans->errorId = 200;
        $ans->errorMsg = "Add Self";
    } else {
        $judge = 'select * from friends where a = '.$fromAccount.' and b = '.$r[0].' union select * from friends where a = '.$r[0].' and b = '.$fromAccount.';';
        $rs = querySQL($judge);
        if ($rr = $rs->fetch_row()) {
            $ans->errorId = 100;
            $ans->errorMsg = "Multi-send";
        } else {
            $add = 'insert into friends (a, b, relation) values ('.$fromAccount.', '.$r[0].', 1);';
            $rs = querySQL($add);
            $ans->errorId = 0;
            $ans->errorMsg = "";
        }
    }
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
