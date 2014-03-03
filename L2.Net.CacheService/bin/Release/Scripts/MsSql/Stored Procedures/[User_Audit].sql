if exists ( select * from sys.objects where object_id = object_id (N'[User_Audit]') and type in (N'P', N'PC') )
	drop procedure [User_Audit]

go

create procedure [User_Audit]
@uid int,
@action tinyint,
@args text
as
begin
	insert into [users_audit] ( [uid], [action], [args] ) values ( @uid, @action, @args )
end