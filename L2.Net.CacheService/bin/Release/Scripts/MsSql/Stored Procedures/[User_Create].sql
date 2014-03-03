if exists ( select * from sys.objects where object_id = object_id (N'[User_Create]') and type in (N'P', N'PC') )
	drop procedure [User_Create]

go

-- Creates user account
-- Returns 
--		-2: error while adding user into [users] table
--		-1: user with same login already exists
--		>=0: user unique id
create procedure [User_Create]
@login varchar(16),
@password varchar(47),
@access_level tinyint,
@uid int output
as
begin
	set @uid = -1

	if not exists( select [uid] from [users] with (nolock) where [login] = @login )
		begin		

			insert into [users] ([login], [password], [access_level]) values ( @login, @password, @access_level )
			
			set @uid = ( select [uid] from [users] with (nolock) where [login] = @login )
			
			if (@@rowcount != 1)
				set @uid = -2

		end
end