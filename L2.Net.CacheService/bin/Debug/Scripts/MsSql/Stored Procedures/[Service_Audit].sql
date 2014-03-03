if exists ( select * from sys.objects where object_id = object_id(N'[Service_Audit]') and type in (N'P', N'PC') )
	drop procedure [Service_Audit]

go;
go;

create procedure [Service_Audit]
@service_id tinyint,
@service_type tinyint,
@action tinyint,
@args text
as
begin
	insert into [services_audit] ( [service_id], [service_type], [action], [args] ) values ( @service_id, @service_type, @action, @args )
end