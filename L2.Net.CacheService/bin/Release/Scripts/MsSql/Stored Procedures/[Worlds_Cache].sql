if exists ( select * from sys.objects where object_id = object_id (N'[Worlds_Cache]') and type in (N'P', N'PC') )
	drop procedure [Worlds_Cache]

go

create procedure [Worlds_Cache]
as
begin
	select * from [worlds] with (nolock)
end