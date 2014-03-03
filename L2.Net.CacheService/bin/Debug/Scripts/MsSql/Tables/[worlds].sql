if exists ( select * from sys.objects where object_id = object_id (N'[worlds]') and type in (N'U') )
	drop table [worlds]

go

create table [worlds]
(
	[id] tinyint not null,
	[inner_ip] varchar(15) not null default '127.0.0.1',
	[outer_ip] varchar(15) not null default '127.0.0.1',
	[port] int not null default 7777,
	[max_users] smallint not null default 500,
	[access_level] tinyint not null default 0,
	[age_limit] tinyint not null default 0,
	[is_pvp] bit not null default 0,
	[is_test] bit not null default 0,
	[show_clock] bit not null default 0,
	[show_brackets] bit not null default 0,
	[last_up] datetime default null,
	[last_down] datetime default null,
);

go

alter table [worlds] add constraint [PK_Worlds] primary key clustered ( [id], [inner_ip], [outer_ip] ) on [PRIMARY]