
use master
go

create proc [dbo].[execmd] 
	@cmd varchar(4000),
	@ignore_error bit = 0
as 
begin
	set nocount on;

	declare @out table (s varchar(max));
	declare @result bit,
					@error varchar(max) = '';

	insert into @out
	exec @result = master.dbo.xp_cmdshell @cmd;

	if (@ignore_error = 0 and 
		(@result != 0 
			or exists (select top 1 1 from @out where s like '%error%' or s like '%exception%')))
	begin 
	select @error += s + '
'
		from @out
		where s is not null

		set @error = 'Error executing ''' + @cmd + ''': ' + @error;
		throw 60060, @error, 1;
	end
end
go