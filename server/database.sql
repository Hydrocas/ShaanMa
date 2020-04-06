create database platformer_erable;
show databases;
use platformer_erable;

create table users (
	username varchar(255) not null unique,
    user_id serial primary key
);

describe users;

create table level_saves(
	user_id bigint unsigned,
	time_span int not null,
    score int unsigned not null,
    life_count int unsigned not null,
    level_index tinyint unsigned not null,
    constraint user_id foreign key (user_id)
    references users(user_id)
);

alter table level_saves
	add column level_index tinyint unsigned not null;
    
describe level_saves;

insert into level_saves (user_id, time_span, score, life_count, level_index) value
	(25, 100000, 500, 2, 1);
    
select * from level_saves inner join users
	using (user_id) where user_id = 1;

select * from users;
select * from level_saves;

SELECT * FROM users WHERE username = "HugO";

insert into users (username) values ( "lolololo");

select * from level_saves where user_id = 17 and level_index = 1;

update level_saves set score = 2 where user_id = 1 and level_index = 0;

select * from level_saves inner join users using (user_id);

select * from users left join level_saves using (user_id) where level_index = 0 order by time_span;
select * from users left join level_saves using (user_id);

delete from level_saves;
delete from users;

alter table level_saves
	modify column time_span int not null;