-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Май 30 2020 г., 09:23
-- Версия сервера: 10.3.13-MariaDB-log
-- Версия PHP: 7.1.32

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `lognet`
--

-- --------------------------------------------------------

--
-- Структура таблицы `accounts`
--

CREATE TABLE `accounts` (
  `p_id` int(11) NOT NULL,
  `p_socialclub` varchar(128) DEFAULT NULL,
  `p_login` varchar(32) DEFAULT NULL,
  `p_name` varchar(32) DEFAULT NULL,
  `p_ip` varchar(32) DEFAULT NULL,
  `p_lastip` varchar(32) DEFAULT NULL,
  `p_password` varchar(32) DEFAULT NULL,
  `p_mail` varchar(256) DEFAULT NULL,
  `p_lvl` int(12) NOT NULL DEFAULT 0,
  `p_datereg` varchar(64) DEFAULT NULL,
  `p_lastjoin` varchar(64) DEFAULT NULL,
  `p_birthday` date DEFAULT NULL,
  `p_sex` tinyint(1) DEFAULT NULL,
  `p_height` int(11) NOT NULL DEFAULT 0,
  `p_weight` int(11) NOT NULL DEFAULT 0,
  `p_age` int(3) NOT NULL DEFAULT 0,
  `p_satiety` int(3) NOT NULL DEFAULT 100,
  `p_thirst` int(3) NOT NULL DEFAULT 100,
  `p_money` double(32,2) NOT NULL DEFAULT 0.00,
  `p_bank` double(32,2) NOT NULL DEFAULT 0.00,
  `p_customize` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL,
  `p_clothes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL,
  `p_house` int(11) NOT NULL DEFAULT 0,
  `p_paycheck` double(32,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `accounts`
--

INSERT INTO `accounts` (`p_id`, `p_socialclub`, `p_login`, `p_name`, `p_ip`, `p_lastip`, `p_password`, `p_mail`, `p_lvl`, `p_datereg`, `p_lastjoin`, `p_birthday`, `p_sex`, `p_height`, `p_weight`, `p_age`, `p_satiety`, `p_thirst`, `p_money`, `p_bank`, `p_customize`, `p_clothes`, `p_house`, `p_paycheck`) VALUES
(1, 'Rams3ska', 'Ramses', 'Adolf Hietler', '127.0.0.1', '127.0.0.1', 'qweewq', 'responsetie@mail.ru', 0, '28.05.2020 23:41:09', '30.05.2020 4:48:37', NULL, 1, 0, 0, 32, 100, 100, 22.20, 0.00, '{\"mother\":25,\"father\":13,\"motherSkin\":0,\"fatherSkin\":0,\"parentsMix\":0,\"skinMix\":0,\"sex\":true,\"noseWidth\":0,\"noseHeigth\":0,\"noseLength\":0,\"noseBridge\":0,\"noseTip\":0,\"noseBridgeShift\":0,\"browHeigth\":0,\"browWidth\":0,\"cheekboneHeigth\":0,\"cheekboneWidth\":0,\"cheekWidth\":0,\"eyes\":0,\"lips\":0,\"jawWidth\":0,\"jawHeigth\":0,\"chinLength\":0,\"chinPosition\":0,\"chinWidth\":0,\"chinShape\":0,\"neckWidth\":0,\"hair\":[12,0,0],\"headOverlay\":{\"blemishes\":[0,255,1],\"facialHair\":[1,255,1],\"eyebrows\":[2,255,1],\"ageing\":[3,255,1],\"makeup\":[4,255,1],\"blush\":[5,255,1],\"complexion\":[6,255,1],\"sunDamage\":[7,255,1],\"lipStick\":[8,255,1],\"moles\":[9,255,1],\"chestHair\":[10,255,1],\"bodyBlemishes\":[11,255,1]}}', '{\"1\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"2\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"3\":{\"drawable\":-1,\"texture\":0,\"palette\":2},\"4\":{\"drawable\":3,\"texture\":0,\"palette\":2},\"5\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"6\":{\"drawable\":1,\"texture\":0,\"palette\":2},\"7\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"8\":{\"drawable\":15,\"texture\":0,\"palette\":2},\"9\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"10\":{\"drawable\":0,\"texture\":0,\"palette\":2},\"11\":{\"drawable\":9,\"texture\":0,\"palette\":2}}', 0, 0.00);

-- --------------------------------------------------------

--
-- Структура таблицы `house`
--

CREATE TABLE `house` (
  `h_id` int(11) NOT NULL DEFAULT -1,
  `h_owner` varchar(64) CHARACTER SET utf8 NOT NULL DEFAULT 'None',
  `h_days` int(11) NOT NULL DEFAULT 0,
  `h_lock` tinyint(1) NOT NULL DEFAULT 0,
  `h_price` int(11) NOT NULL DEFAULT 0,
  `h_class` int(11) NOT NULL DEFAULT -1,
  `h_interior` int(11) NOT NULL DEFAULT -1,
  `h_enterposX` float(8,4) NOT NULL DEFAULT 0.0000,
  `h_enterposY` float(8,4) NOT NULL DEFAULT 0.0000,
  `h_enterposZ` float(8,4) NOT NULL DEFAULT 0.0000,
  `h_enterposR` float(8,4) NOT NULL DEFAULT 0.0000
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `house`
--

INSERT INTO `house` (`h_id`, `h_owner`, `h_days`, `h_lock`, `h_price`, `h_class`, `h_interior`, `h_enterposX`, `h_enterposY`, `h_enterposZ`, `h_enterposR`) VALUES
(1, 'None', 0, 0, 300000, 4, 0, -214.8893, 6444.4790, 31.3135, 300.4662),
(2, 'None', 0, 0, 124000, 3, 0, -238.1144, 6423.4702, 31.4573, 219.9139),
(3, 'None', 0, 0, 130000, 2, 0, -188.8705, 6409.6655, 32.2968, 42.7428),
(4, 'None', 0, 0, 98900, 2, 0, -272.4327, 6400.6836, 31.5050, 206.9034),
(6, 'None', 0, 0, 160000, 4, 0, -367.9996, 6341.4473, 29.8437, 218.8562),
(7, 'None', 0, 0, 210000, 4, 0, -407.0733, 6313.9917, 28.9419, 219.1288),
(8, 'None', 0, 0, 127001, 3, 0, -447.7924, 6271.8232, 33.3300, 70.3017),
(9, 'None', 0, 0, 217600, 3, 0, -468.0394, 6206.6123, 29.5528, 272.4847),
(10, 'None', 0, 0, 87800, 2, 0, -384.7366, 6193.7021, 31.5354, 130.1723),
(5, 'None', 0, 0, 107800, 3, 0, -379.8027, 6252.9585, 31.8512, 316.8489),
(11, 'None', 0, 0, 150000, 3, 0, -213.9180, 6396.4087, 33.0851, 46.9100),
(12, 'None', 0, 0, 64200, 2, 0, -371.3474, 6267.3037, 31.6790, 44.5402),
(13, 'None', 0, 0, 95000, 2, 0, -227.4317, 6377.7002, 31.7592, 45.2882),
(14, 'None', 0, 0, 115000, 2, 0, -247.8429, 6370.2144, 31.8455, 84.7837),
(15, 'None', 0, 0, 91100, 3, 0, -347.0559, 6224.6865, 31.6865, 223.8649),
(16, 'None', 0, 0, 50000, 1, 0, -280.5943, 6350.8472, 32.6008, 45.4720),
(17, 'None', 0, 0, 59700, 2, 0, -356.7709, 6207.3892, 31.8423, 222.0281),
(18, 'None', 0, 0, 110000, 2, 0, -302.5208, 6327.3032, 32.8868, 40.1131),
(19, 'None', 0, 0, 150000, 3, 0, -332.7681, 6302.4233, 33.0882, 43.4838),
(20, 'None', 0, 0, 135000, 3, 0, -15.2296, 6557.5005, 33.2404, 311.0884),
(21, 'None', 0, 0, 110000, 3, 0, 4.4985, 6568.3052, 33.0759, 141.0268),
(22, 'None', 0, 0, 21900, 1, 0, -37.3890, 6578.9395, 32.3216, 315.1372),
(23, 'None', 0, 0, 110000, 3, 0, 31.1969, 6596.5850, 32.8222, 220.0669),
(24, 'None', 0, 0, 64200, 2, 0, -26.6336, 6597.4648, 31.8607, 35.8374),
(25, 'None', 0, 0, 88088, 2, 0, -41.5661, 6637.0459, 31.0875, 212.4074),
(26, 'None', 0, 0, 90000, 2, 0, 35.2949, 6662.9087, 32.1904, 160.4944),
(27, 'None', 0, 0, 130000, 3, 0, -9.4630, 6653.9131, 31.4797, 201.5931),
(28, 'None', 0, 0, 10000, 0, 0, 56.5637, 6646.3960, 32.2764, 142.1289),
(29, 'None', 0, 0, 35000, 1, 0, 1966.7946, 4634.3623, 41.1010, 30.2623),
(30, 'None', 0, 0, 55000, 2, 0, 1724.9092, 4642.2529, 43.8755, 116.2571),
(31, 'None', 0, 0, 6100, 2, 0, 1673.9102, 4658.0698, 43.3717, 264.9295),
(32, 'None', 0, 0, 45000, 2, 0, 1718.4937, 4677.2012, 43.6558, 85.4088),
(33, 'None', 0, 0, 44800, 2, 0, 1683.1399, 4689.4009, 43.0655, 263.5526),
(34, 'None', 0, 0, 45000, 2, 0, 1664.3378, 4739.8687, 42.0029, 278.5536),
(35, 'None', 0, 0, 35000, 2, 0, 1662.6146, 4776.2358, 42.0076, 272.6816),
(36, 'None', 0, 0, 22500, 1, 0, 1429.3518, 4377.5801, 44.5993, 47.4449),
(38, 'None', 0, 0, 17950, 1, 0, 1364.3298, 4315.7036, 37.6687, 336.8065),
(40, 'None', 0, 0, 27000, 1, 0, 1365.9055, 4358.2656, 44.5006, 353.3156),
(41, 'None', 0, 0, 12630, 1, 0, 1338.6665, 4359.6016, 44.3669, 306.2431),
(37, 'None', 0, 0, 5700, 0, 0, 1381.7426, 4381.6636, 45.0668, 183.5092),
(39, 'None', 0, 0, 5700, 0, 0, 1374.1523, 4380.9478, 45.0259, 183.5143),
(42, 'None', 0, 0, 2800, 0, 0, 1321.4224, 4314.4834, 38.3337, 74.8101),
(43, 'None', 0, 0, 96300, 2, 0, 775.9208, 4184.0796, 41.7808, 86.8426),
(44, 'None', 0, 0, 86900, 2, 0, 749.5795, 4184.1626, 41.0879, 74.5397),
(45, 'None', 0, 0, 94000, 2, 0, 723.0287, 4187.3247, 41.0826, 186.8061),
(46, 'None', 0, 0, 666, 0, 0, 727.1627, 4169.3003, 40.7092, 11.8533),
(47, 'None', 0, 0, 38000, 1, 0, -35.5608, 2871.3784, 59.5969, 153.7347),
(48, 'None', 0, 0, 47000, 1, 0, 470.9461, 2607.6687, 44.4772, 10.1054),
(49, 'None', 0, 0, 45000, 1, 0, 506.4840, 2610.3740, 43.9231, 13.3363),
(50, 'None', 0, 0, 18800, 1, 0, 564.4418, 2598.6162, 43.8515, 108.3942),
(52, 'None', 0, 0, 9600, 0, 0, 382.5330, 2576.5496, 44.3685, 106.8555),
(54, 'None', 0, 0, 9999, 0, 0, 348.2131, 2565.6594, 43.5196, 113.4939),
(55, 'None', 0, 0, 4200, 1, 0, 341.5933, 2615.1721, 44.6724, 28.0717),
(56, 'None', 0, 0, 4200, 1, 0, 347.0282, 2618.4512, 44.6787, 20.1333),
(57, 'None', 0, 0, 6700, 0, 0, 366.8887, 2571.3669, 44.4123, 113.3756),
(53, 'None', 0, 0, 4200, 1, 0, 354.1938, 2619.9895, 44.6724, 25.6537),
(51, 'None', 0, 0, 4200, 1, 0, 359.4973, 2623.1077, 44.6790, 27.4128),
(58, 'None', 0, 0, 7000, 0, 0, 404.0804, 2584.5322, 43.5195, 99.4804),
(59, 'None', 0, 0, 4200, 1, 0, 367.1077, 2624.8481, 44.6724, 25.0666),
(60, 'None', 0, 0, 4200, 1, 0, 372.3280, 2627.8860, 44.6782, 28.4416),
(61, 'None', 0, 0, 4200, 1, 0, 379.6731, 2629.4609, 44.6724, 22.4094),
(62, 'None', 0, 0, 4200, 1, 0, 384.9148, 2632.5168, 44.6776, 25.9288),
(63, 'None', 0, 0, 4200, 1, 0, 392.6155, 2634.4128, 44.6724, 22.8051),
(64, 'None', 0, 0, 4200, 1, 0, 397.8839, 2637.5303, 44.6791, 43.7875),
(65, 'None', 0, 0, 63200, 2, 0, 194.9754, 3031.0906, 43.8904, 274.1037),
(66, 'None', 0, 0, 48500, 2, 0, 191.3584, 3081.9834, 43.4729, 272.0031),
(67, 'None', 0, 0, 86000, 2, 0, 241.5334, 3107.7954, 42.4872, 97.9874),
(68, 'None', 0, 0, 12600, 1, 0, 189.7005, 3094.1814, 43.0742, 274.2772),
(69, 'None', 0, 0, 55000, 2, 0, 247.3141, 3169.3660, 42.7907, 89.0561),
(70, 'None', 0, 0, 1500, 0, 0, 163.3928, 3119.1274, 43.4260, 198.3469),
(71, 'None', 0, 0, 600, 0, 0, 54.6715, 3629.1292, 39.5980, 199.0623),
(72, 'None', 0, 0, 7700, 0, 0, 101.3441, 3652.5417, 40.4215, 266.8625),
(73, 'None', 0, 0, 8000, 0, 0, 97.8774, 3682.3081, 39.7366, 357.5584),
(74, 'None', 0, 0, 5100, 0, 0, 105.5493, 3728.5325, 40.2509, 118.4382),
(75, 'None', 0, 0, 3700, 1, 0, 15.4796, 3688.6265, 39.9925, 202.9625),
(76, 'None', 0, 0, 3500, 1, 0, 8.5855, 3686.2920, 39.9753, 199.7301),
(77, 'None', 0, 0, 5100, 0, 0, 84.6546, 3718.1960, 40.2390, 48.6911),
(78, 'None', 0, 0, 5100, 0, 0, 67.9704, 3693.1609, 40.6143, 48.2914),
(79, 'None', 0, 0, 4600, 1, 0, 76.6373, 3757.3811, 39.7547, 279.2588),
(80, 'None', 0, 0, 6500, 0, 0, 47.8828, 3702.1821, 40.5159, 332.7080),
(81, 'None', 0, 0, 5900, 1, 0, 78.3783, 3732.7156, 40.2785, 311.0185),
(82, 'None', 0, 0, 5800, 0, 0, 52.2054, 3741.9729, 40.0846, 189.1048),
(83, 'None', 0, 0, 700, 0, 0, 40.5160, 3715.5728, 39.6745, 149.5446);

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`p_id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `accounts`
--
ALTER TABLE `accounts`
  MODIFY `p_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
