show databases;
use bqrtjszaqzhkoobbj2pn;

create table users (
	username varchar(255) not null unique,
    user_id serial primary key
);

describe users;

create table level_saves(
	user_id bigint unsigned,
	time_span time not null,
    score int unsigned not null,
    life_count int unsigned not null,
    level_index tinyint unsigned not null,
    constraint user_id foreign key (user_id)
    references users(user_id)
);

describe level_saves;

select * from users;
select * from level_saves;

delete from level_saves;
delete from users;