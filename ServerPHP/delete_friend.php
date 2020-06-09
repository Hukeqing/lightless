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
$sqlStr = 'delete from friends where a='.$from.' and b='.$to.' or a='.$to.' and b='.$from.';';
$rs = querySQL($sqlStr);

$ans = new res();

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
