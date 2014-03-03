if exists ( select * from sys.objects where object_id = object_id (N'[users_audit]') and type in (N'U') )
	drop table [users_audit]
	
go	
	
create table [users_audit]
(
	[record_id] int not null identity(1,1),
	[fdt] datetime not null default getdate(),
	[uid] int not null,
	[action] tinyint not null,
	[args] text default null
);

go

alter table [users_audit] add constraint [PK_UsersAudit] primary key clustered ( [record_id], [fdt] ) on [PRIMARY]