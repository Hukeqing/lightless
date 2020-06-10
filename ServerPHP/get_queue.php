<?php

require '_mysql.php';

class res {
    public $errorId = 200;
    public $errorMsg  = "No game on your score";
    public $gameId = -1;
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$account = $arr_query['account'];
$rec = $arr_query['re'];

$sqlStr = 'select score from account where id = '.$account.';';
$rs = querySQL($sqlStr);
$score = $rs->fetch_row()[0];

$sqlStr = 'select id, initiator from queue where from_player != '.$account.' and lower_score <= '.$score.' and high_score >= '.$score.' and flag = 1 ;';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    $sqlStr = 'delete from queue where id = '.$r[0].';';
    $rs = querySQL($sqlStr);
    $sqlStr = 'insert into competition (initiator, recipient) values ('.$r[1].', '.$rec.');';
    $rs = querySQL($sqlStr);
    $sqlStr = 'select max(id) from competition;';
    $rs = querySQL($sqlStr);
    $r = $rs->fetch_row();
    $ans->errorId = 0;
    $ans->errorMsg = "";
    $ans->gameId = $r[0];
} else {
    $sqlStr = 'delete from game where id = '.$rec.';';
    $rs = querySQL($sqlStr);
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
