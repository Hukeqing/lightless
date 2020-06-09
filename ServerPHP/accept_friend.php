<?php

require '_mysql.php';

class res {
    public $errorId = 0;
    public $errorMsg  = "";
    public $accountId = 0;
    public $accountSc = 0;
    public $accountNa = "";
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$from = $arr_query['from'];
$to = $arr_query['to'];
$sqlStr = 'update friends set relation = 0 where a='.$from.' and b='.$to.';';

$rs = querySQL($sqlStr);

$ans = new res();

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
