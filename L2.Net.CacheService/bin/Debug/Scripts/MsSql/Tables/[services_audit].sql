if exists ( select * from sys.objects where object_id = object_id (N'[services_audit]') and type in (N'U') )
	drop table [services_audit]

go
	
create table [services_audit]
(
	[record_id] int not null identity(1,1),
	[fdt] datetime not null default getdate(),
	[service_id] tinyint not null,
	[service_type] tinyint not null,
	[action] tinyint not null,
	[args] text default null
);

go

alter table [services_audit] add constraint [PK_ServicesAudit] primary key clustered ( [record_id], [fdt] ) on [PRIMARY]