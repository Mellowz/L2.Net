if exists ( select * from sys.objects where object_id = object_id (N'[User_Logout]') and type in (N'P', N'PC') )
	drop procedure [User_Logout]

go

create procedure [User_Logout]
@uid int,
@session_start_time datetime,
@used_time bigint,
@last_world tinyint,
@last_ip varchar(15)
as
begin
	
if exists ( select [uid] from [users] where [uid] = @uid )
	begin
			update [users] set 
			[last_login] = @session_start_time, [last_logout] = getdate(), [last_ip] = @last_ip, [last_world] = @last_world,
			[used_time] = (select [used_time] from [users] where [uid] = @uid) + @used_time
			where [uid] = @uid;
	end
end