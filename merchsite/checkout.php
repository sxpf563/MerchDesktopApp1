<?php
session_start();
include "db.php";

if (!isset($_SESSION["cart"])) {
    $_SESSION["cart"] = [];
}

$message = "";
$error = "";

if ($_SERVER["REQUEST_METHOD"] === "POST") {
    $name = trim($_POST["name"]);
    $phone = trim($_POST["phone"]);
    $address = trim($_POST["address"]);
    $total = 0;

    if ($name === "" || $phone === "" || $address === "") {
        $error = "Заполните все поля";
    } elseif (empty($_SESSION["cart"])) {
        $error = "Корзина пуста";
    } else {
        foreach ($_SESSION["cart"] as $id => $qty) {
            $id = intval($id);
            $res = mysqli_query($conn, "SELECT * FROM products WHERE id=$id");

            if ($product = mysqli_fetch_assoc($res)) {
                $total += $product["price"] * $qty;
            }
        }

        $stmt = mysqli_prepare($conn, "INSERT INTO orders (customer_name, phone, address, total) VALUES (?, ?, ?, ?)");
        mysqli_stmt_bind_param($stmt, "sssi", $name, $phone, $address, $total);
        mysqli_stmt_execute($stmt);

        $orderId = mysqli_insert_id($conn);

        foreach ($_SESSION["cart"] as $id => $qty) {
            $id = intval($id);
            $res = mysqli_query($conn, "SELECT * FROM products WHERE id=$id");

            if ($product = mysqli_fetch_assoc($res)) {
                $stmt = mysqli_prepare($conn, "INSERT INTO order_items (order_id, product_id, product_name, price, quantity) VALUES (?, ?, ?, ?, ?)");
                mysqli_stmt_bind_param($stmt, "iisii", $orderId, $id, $product["name"], $product["price"], $qty);
                mysqli_stmt_execute($stmt);

                mysqli_query($conn, "UPDATE products SET quantity = quantity - $qty WHERE id=$id");
            }
        }

        $_SESSION["cart"] = [];
        $message = "Заказ успешно оформлен! Номер заказа: #" . $orderId;
    }
}

$cartCount = array_sum($_SESSION["cart"]);
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <title>Оформление заказа | YogurtStudio</title>
    <link rel="stylesheet" href="style.css">
</head>
<body>

<header class="header">
    <a class="logo" href="index.php">YogurtStudio</a>
    <nav>
        <a href="index.php#catalog">Каталог</a>
        <a href="cart.php">🛒 Корзина (<?= $cartCount ?>)</a>
    </nav>
</header>

<main class="page">
    <div class="section-title">
        <p>CHECKOUT</p>
        <h1>Оформление заказа</h1>
    </div>

    <?php if ($message): ?>
        <div class="success">
            <h2><?= $message ?></h2>
            <p>Данные заказа сохранены в базе данных.</p>
            <a class="btn" href="index.php">Вернуться в каталог</a>
        </div>
    <?php else: ?>
        <?php if ($error): ?>
            <div class="error"><?= $error ?></div>
        <?php endif; ?>

        <form class="order-form" method="post">
            <input type="text" name="name" placeholder="Ваше имя" required>
            <input type="text" name="phone" placeholder="Телефон" required>
            <textarea name="address" placeholder="Адрес доставки" required></textarea>
            <button type="submit">Подтвердить заказ</button>
        </form>
    <?php endif; ?>
</main>

</body>
</html>