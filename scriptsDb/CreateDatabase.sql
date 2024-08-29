/**
* DataBase Test
* Test for insert an image
*/

CREATE TABLE Books(
bookid serial primary key NOT NULL,
title varchar(512) NOT NULL,
publishyear smallint null,
picture bytea null
);
