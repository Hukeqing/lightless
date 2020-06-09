<?php

require '_mysql.php';

$scorePow = 1;

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$gameId = $arr_query['id'];

$sqlStr = 'select initiator, recipient from competition where id = '.$gameId.';';
$rs = querySQL($sqlStr);

$r = $rs->fetch_row();
$initiator = $r[0];
$recipient = $r[1];

$sqlStr = 'select player, score from game where id = '.$initiator.';';
$rs = querySQL($sqlStr);

$r = $rs->fetch_row();
$intId = $r[0];
$intSc = $r[1];

$sqlStr = 'select score from account where id = '.$intId.';';
$rs = querySQL($sqlStr);

$r = $rs->fetch_row();
$intScore = $r[0];

$sqlStr = 'select player, score from game where id = '.$recipient.';';
$rs = querySQL($sqlStr);

$r = $rs->fetch_row();
$recId = $r[0];
$recSc = $r[1];

$sqlStr = 'select score from account where id = '.$recId.';';
$rs = querySQL($sqlStr);

$r = $rs->fetch_row();
$recScore = $r[0];

$scoreChange = ($recSc - $intSc) * $scorePow;

$sqlStr = 'update competition set score_change = '.$scoreChange.' where id = '.$gameId.';';
$rs = querySQL($sqlStr);

$sqlStr = 'update account set score = '.($intScore - $scoreChange).' where id ='.$intId.';';
$rs = querySQL($sqlStr);

$sqlStr = 'update account set score = '.($recScore + $scoreChange).' where id ='.$recId.';';
$rs = querySQL($sqlStr);
?>
