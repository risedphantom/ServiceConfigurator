CREATE proc [dbo].[IISRestrictionturnOff]
	@ID      int,
	@Enabled bit
as
begin	
	update	IISRestriction
	set		[Enabled] = @Enabled
	where	ID = @ID
end