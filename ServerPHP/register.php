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
    public $errorId = 404;
    public $errorMsg  = "Account not find";
    public $accountId = -1;
    public $accountSc = 0;
    public $accountNa = "";
}

$arr_query = convertUrlQuery($_SERVER["QUERY_STRING"]);
$account = $arr_query['account'];
$pwd = $arr_query['pwd'];
$name = $arr_query['name'];
$gameVersion = $arr_query['version'];
if ($gameVersion != "0.2") {
    $ans->errorId = 200;
    $ans->errorMsg = "Game version error!";
    return;
}

$sqlStr = 'select * from account where email = "'.$account.'";';

$rs = querySQL($sqlStr);
$ans = new res();

if ($r = $rs->fetch_row()) {
    $ans->errorId = 404;
    $ans->errorMsg = "Email registered";
} else {
    $ans->errorId = 0;
    $ans->errorMsg = "";
    $sqlInsert = 'insert into account (email, password, registerTime, name) VALUES ("'.$account.'", "'.$pwd.'", now(), "'.$name.'");';
    $rs = querySQL($sqlInsert);
    $rs = querySQL($sqlStr);
    $r = $rs->fetch_row();
    $ans->accountId = $r[0];
    $ans->accountSc = $r[4];
    $ans->accountNa = $r[5];
}

echo json_encode($ans, JSON_UNESCAPED_UNICODE);
?>