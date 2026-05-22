<?php
include "db.php";

$products = mysqli_query($conn, "SELECT * FROM products ORDER BY id");
$orders = mysqli_query($conn, "SELECT * FROM orders ORDER BY id DESC");
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <title>Админка | YogurtStudio</title>
    <link rel="stylesheet" href="style.css">
</head>
<body>

<header class="header">
    <a class="logo" href="index.php">Admin Panel</a>
    <nav>
        <a href="index.php">На сайт</a>
        <a href="cart.php">Корзина</a>
    </nav>
</header>

<main class="admin-page">
    <div class="section-title">
        <p>ADMIN</p>
        <h1>Админ-панель</h1>
    </div>

    <h2>Товары</h2>
    <table>
        <tr>
            <th>ID</th>
            <th>Фото</th>
            <th>Название</th>
            <th>Категория</th>
            <th>Цена</th>
            <th>Остаток</th>
            <th>Описание</th>
        </tr>

        <?php while ($p = mysqli_fetch_assoc($products)): ?>
            <tr>
                <td><?= $p['id'] ?></td>
                <td><img class="table-img" src="img/<?= htmlspecialchars($p['image']) ?>"></td>
                <td><?= htmlspecialchars($p['name']) ?></td>
                <td><?= htmlspecialchars($p['category']) ?></td>
                <td><?= $p['price'] ?> ₽</td>
                <td><?= $p['quantity'] ?></td>
                <td><?= htmlspecialchars($p['description']) ?></td>
            </tr>
        <?php endwhile; ?>
    </table>

    <h2>Заказы</h2>
    <table>
        <tr>
            <th>ID</th>
            <th>Клиент</th>
            <th>Телефон</th>
            <th>Адрес</th>
            <th>Сумма</th>
            <th>Статус</th>
            <th>Дата</th>
        </tr>

        <?php while ($o = mysqli_fetch_assoc($orders)): ?>
            <tr>
                <td><?= $o['id'] ?></td>
                <td><?= htmlspecialchars($o['customer_name']) ?></td>
                <td><?= htmlspecialchars($o['phone']) ?></td>
                <td><?= htmlspecialchars($o['address']) ?></td>
                <td><?= $o['total'] ?> ₽</td>
                <td><span class="status"><?= htmlspecialchars($o['status']) ?></span></td>
                <td><?= $o['created_at'] ?></td>
            </tr>
        <?php endwhile; ?>
    </table>
</main>

</body>
</html>