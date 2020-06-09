<?php

require '_mysql.php';

class res {
    public $errorId = 200;
    public $errorMsg  = "Unknown Error";
    public $score = -1;
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$compe = $arr_query['id'];

$sqlStr = 'select initiator from competition where id = '.$compe.';';
$rs = querySQL($sqlStr);
$intId = $rs->fetch_row()[0];

$sqlStr = 'select score from game where id = '.$intId.';';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    $ans->errorId = 0;
    $ans->errorMsg = "";
    $ans->score = $r[0];
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
