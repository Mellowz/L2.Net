if exists (select * from sys.objects where object_id = object_id (N'[User_Auth]') and type in (N'P', N'PC'))
	drop procedure [User_Auth]

go

create procedure [User_Auth]
@login varchar(16),
@password varchar(47),
@uid int output,
@last_world tinyint output,
@access_level tinyint output
as
begin

	set @uid = -2
	set @last_world = 1
	set @access_level = 0

	if exists ( select * from [users] where [login] = @login and [password] = @password )
		begin
			set @uid = ( select [uid] from [users] with (nolock) where [login] = @login )
			set @last_world = ( select [last_world] from [users] with (nolock) where [uid] = @uid )
			set @access_level = ( select [access_level] from [users] with (nolock) where [uid] = @uid )
		end
	else
		if not exists ( select * from [users] where [login] = @login )
			set @uid = -1

end