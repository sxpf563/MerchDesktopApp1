<?php
session_start();
include "db.php";

if (!isset($_SESSION["cart"])) {
    $_SESSION["cart"] = [];
}

$added = false;

if (isset($_GET["add"])) {
    $id = intval($_GET["add"]);

    if (!isset($_SESSION["cart"][$id])) {
        $_SESSION["cart"][$id] = 1;
    } else {
        $_SESSION["cart"][$id]++;
    }

    $added = true;
}

$result = mysqli_query($conn, "SELECT * FROM products");
$cartCount = array_sum($_SESSION["cart"]);
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <title>YogurtStudio Merch</title>
    <link rel="stylesheet" href="style.css">
</head>
<body>

<header class="header">
    <a class="logo" href="index.php">YogurtStudio</a>
    <nav>
        <a href="#catalog">Каталог</a>
        <a href="cart.php">🛒 Корзина (<?= $cartCount ?>)</a>
    </nav>
</header>

<section class="hero">
    <div class="hero-inner">
        <p class="label">BLACK & WHITE COLLECTION</p>
        <h1>Фирменный мерч YogurtStudio</h1>
        <p class="subtitle">Минималистичная коллекция: футболка, худи и кружка в едином стиле.</p>
        <a href="#catalog" class="btn">Смотреть коллекцию</a>
    </div>
</section>

<?php if ($added): ?>
    <div class="notice">Товар добавлен в корзину</div>
<?php endif; ?>

<section class="about">
    <h2>О коллекции</h2>
    <p>
        YogurtStudio Merch — это линейка фирменных товаров для клиентов и сотрудников.
        В коллекции представлены базовые вещи в черно-белом стиле с аккуратной надписью YogurtStudio.
    </p>
</section>

<main class="catalog" id="catalog">
    <div class="section-title">
        <p>SHOP</p>
        <h2>Каталог товаров</h2>
    </div>

    <div class="grid">
        <?php while ($product = mysqli_fetch_assoc($result)): ?>
            <div class="card">
                <?php if ($product['quantity'] < 10): ?>
                    <div class="badge">Осталось мало</div>
                <?php endif; ?>

                <div class="image-box">
                    <img src="img/<?= htmlspecialchars($product['image']) ?>" alt="<?= htmlspecialchars($product['name']) ?>">
                </div>

                <h3><?= htmlspecialchars($product['name']) ?></h3>
                <p class="desc"><?= htmlspecialchars($product['description']) ?></p>

                <div class="card-bottom">
                    <div>
                        <div class="price"><?= $product['price'] ?> ₽</div>
                        <div class="stock">В наличии: <?= $product['quantity'] ?></div>
                    </div>

                    <?php if ($product['quantity'] > 0): ?>
                        <a class="btn small" href="index.php?add=<?= $product['id'] ?>#catalog">В корзину</a>
                    <?php else: ?>
                        <span class="sold">Нет в наличии</span>
                    <?php endif; ?>
                </div>
            </div>
        <?php endwhile; ?>
    </div>
</main>

<footer>
    <div class="footer-logo">YogurtStudio Merch</div>
    <p>© 2026. Учебный проект интернет-магазина мерча.</p>
    <a class="admin-link" href="admin.php">Админка</a>
</footer>

</body>
</html>