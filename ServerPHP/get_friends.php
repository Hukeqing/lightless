<?php

require '_mysql.php';

class res {
    public $cnt = 0;
    public $accountId = array();
    public $accountSc = array();
    public $accountNa = array();
    public $waitId = array();
    public $waitNa = array();
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$account = $arr_query['account'];
$sqlStr = '(select x.id, x.score, x.name from account x inner join (select b from friends where a='.$account.' and relation=0) y on x.id = y.b) union (select x.id, x.score, x.name from account x inner join (select a from friends where b='.$account.' and relation=0) y on x.id = y.a);';

$rs = querySQL($sqlStr);
$ans = new res();

while ($r = $rs->fetch_row()) {
    array_push($ans->accountId, $r[0]);
    array_push($ans->accountSc, $r[1]);
    array_push($ans->accountNa, $r[2]);
}

$sqlStr= 'select x.id, x.name from account x inner join (select a from friends where b='.$account.' and relation=1) y on x.id = y.a;';
$rs = querySQL($sqlStr);

while ($r = $rs->fetch_row()) {
    array_push($ans->waitId, $r[0]);
    array_push($ans->waitNa, $r[1]);
}

$ans->cnt = count($ans->accountId) + count($ans->waitId);
echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>
