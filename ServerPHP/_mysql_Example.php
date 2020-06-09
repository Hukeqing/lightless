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
    $dbpass = 'XXXXXXXX';        // mysql用户名密码
    $dbpower = 'lightless';
    $con = new MySQLi($dbhost, $dbuser, $dbpass, $dbpower);
    if(!$con)
        die("connect error:".mysqli_connect_error());
    $con->set_charset('utf-8');
    $rs = $con->query($sqlStr);
    $con->close();
    return $rs;
}
?>