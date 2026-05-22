<?php
session_start();
include "db.php";

if (!isset($_SESSION["cart"])) {
    $_SESSION["cart"] = [];
}

if (isset($_GET["remove"])) {
    $id = intval($_GET["remove"]);
    unset($_SESSION["cart"][$id]);
    header("Location: cart.php");
    exit;
}

if (isset($_GET["plus"])) {
    $id = intval($_GET["plus"]);
    $_SESSION["cart"][$id]++;
    header("Location: cart.php");
    exit;
}

if (isset($_GET["minus"])) {
    $id = intval($_GET["minus"]);
    $_SESSION["cart"][$id]--;

    if ($_SESSION["cart"][$id] <= 0) {
        unset($_SESSION["cart"][$id]);
    }

    header("Location: cart.php");
    exit;
}

$items = [];
$total = 0;

foreach ($_SESSION["cart"] as $id => $qty) {
    $id = intval($id);
    $res = mysqli_query($conn, "SELECT * FROM products WHERE id=$id");

    if ($product = mysqli_fetch_assoc($res)) {
        $product["cart_qty"] = $qty;
        $product["sum"] = $product["price"] * $qty;
        $total += $product["sum"];
        $items[] = $product;
    }
}

$cartCount = array_sum($_SESSION["cart"]);
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <title>Корзина | YogurtStudio</title>
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
        <p>CART</p>
        <h1>Корзина</h1>
    </div>

    <?php if (empty($items)): ?>
        <div class="empty-box">
            <h2>Корзина пуста</h2>
            <p>Добавьте товары из каталога.</p>
            <a class="btn" href="index.php#catalog">Перейти в каталог</a>
        </div>
    <?php else: ?>
        <div class="cart-list">
            <?php foreach ($items as $item): ?>
                <div class="cart-item">
                    <img src="img/<?= htmlspecialchars($item['image']) ?>" alt="<?= htmlspecialchars($item['name']) ?>">

                    <div>
                        <h3><?= htmlspecialchars($item['name']) ?></h3>
                        <p><?= htmlspecialchars($item['description']) ?></p>

                        <div class="qty">
                            <a href="cart.php?minus=<?= $item['id'] ?>">−</a>
                            <span><?= $item['cart_qty'] ?></span>
                            <a href="cart.php?plus=<?= $item['id'] ?>">+</a>
                        </div>
                    </div>

                    <div class="cart-price"><?= $item['sum'] ?> ₽</div>

                    <a class="remove" href="cart.php?remove=<?= $item['id'] ?>">Удалить</a>
                </div>
            <?php endforeach; ?>
        </div>

        <div class="total-box">
            <h2>Итого: <?= $total ?> ₽</h2>
            <a class="btn" href="checkout.php">Оформить заказ</a>
        </div>
    <?php endif; ?>
</main>

</body>
</html>