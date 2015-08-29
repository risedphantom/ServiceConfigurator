
create proc [dbo].[IISRestrictionReject]
	@ID      int,
	@Error varchar(8000)
as
begin	
	update	IISRestriction
	set		Error = @Error
		,	Enabled = 0
	where	ID = @ID
end