<?php

require '_mysql.php';

class res {
    public $errorId = 200;
    public $errorMsg  = "UnknowError";
    public $gameId = -1;
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$gameId = $arr_query['id'];
$gameScore = $arr_query['score'];
$sqlStr = 'update game set score= '.$gameScore.' where id = '.$gameId.';';
$rs = querySQL($sqlStr);

?>
