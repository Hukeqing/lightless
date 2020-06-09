<?php
function convertUrlQuery($query) {
    $queryParts = explode('&', $query);

    $params = array();
    foreach ($queryParts as $param)
    {
        $item = explode('=', $param);
        $params[$item[0]] = $item[1];
    }

    return $params;
}
function querySQL($sqlStr) {
    $dbhost = '127.0.0.1';  // mysql服务器主机地址
    $dbuser = 'root';            // mysql用户名
    $dbpass = 'MY53695a';        // mysql用户名密码
    $dbpower = 'lightless';
    $con = new MySQLi($dbhost, $dbuser, $dbpass, $dbpower);
    if(!$con)
        die("connect error:".mysqli_connect_error());
    $con->set_charset('utf-8');
    $rs = $con->query($sqlStr);
    $con->close();
    return $rs;
}

class res {
    public $errorId = 200;
    public $errorMsg  = "UnknowError";
    public $gameId = -1;
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$gameId = $arr_query['id'];
$account = $arr_query['player'];

$sqlStr = 'select score from account where id = '.$account.';';
$rs = querySQL($sqlStr);
$score = $rs->fetch_row()[0];
$scoreRange = 500;

$sqlStr = 'insert into queue (initiator, from_player, lower_score, high_score, flag) values ('.$gameId.', '.$account.', '.($score - $scoreRange).', '.($score + $scoreRange).', 0);';
$rs = querySQL($sqlStr);

$ans = new res();
$sqlStr = 'select max(id) from queue;';
$rs = querySQL($sqlStr);
if ($r = $rs->fetch_row()) {
    $ans->errorId = 0;
    $ans->errorMsg = "";
    $ans->gameId = $r[0];
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);

?>