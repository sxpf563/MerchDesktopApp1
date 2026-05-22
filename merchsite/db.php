<?php

mysqli_report(MYSQLI_REPORT_OFF);

$conn = mysqli_connect(
    "MySQL-8.0",
    "root",
    "",
    "merch_db"
);

if (!$conn) {
    die("Ошибка подключения");
}

mysqli_set_charset($conn, "utf8mb4");

?>