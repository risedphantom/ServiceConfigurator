CREATE proc [dbo].[IISRestrictionAdd]
	@TypeName    varchar(250),
	@StartMoment datetime,
	@StopMoment  datetime,
	@Rule        xml,
	@IISSiteID   int,
	@ID			 int out
as
begin
	insert IISRestriction(TypeName, StartMoment, StopMoment, [Rule], [Enabled], IISSiteID)
	values (@TypeName, @StartMoment, @StopMoment, @Rule, 0, @IISSiteID)

	set @ID = scope_identity()
end