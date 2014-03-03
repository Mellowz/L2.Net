if exists ( select * from sys.objects where object_id = object_id (N'[users]') and type in (N'U') )
	drop table [users]

go

create table [users]
(
	[uid] int not null identity(1,1),
	[login] varchar (16) not null,
	[password] varchar(47) not null,
	[access_level] tinyint not null default 0,
	[creation_date] datetime not null default getdate(),
	[used_time] bigint not null default 0,
	[last_world] tinyint not null default 1,
	[last_ip] varchar(15) default null,
	[last_login] datetime default null,
	[last_logout] datetime default null
);

go

alter table [users] add constraint [PK_Users] primary key clustered ( [uid], [login], [password] ) on [PRIMARY]