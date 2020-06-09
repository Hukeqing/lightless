<?php

require '_mysql.php';

class res {
    public $errorId = 0;
    public $errorMsg  = "";
    public $gameId = -1;
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$account = $arr_query['account'];

$sqlStr = 'update queue set flag = 1 where from_player = '.$account.';';

$rs = querySQL($sqlStr);

$sqlStr = 'select x.id from (select id, recipient from competition where score_change is null) x inner join game y on x.recipient = y.id and y.player = '.$account.';';

$rs = querySQL($sqlStr);

$ans = new res();
if ($r = $rs->fetch_row()) {
    $ans->gameId = $r[0];
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
