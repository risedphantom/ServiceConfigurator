CREATE proc [dbo].[IISSiteRestrictionAdd]
	@TypeName    varchar(250),
	@StartMoment datetime,
	@StopMoment  datetime,
	@Rule        xml,
	@IISSiteName varchar(250),
	@IISSiteHost varchar(250),
	@IISSiteGroup varchar(250),
	@ID			 int out
as
begin
	declare @IISSiteID bigint = null

	select	@IISSiteID = ID
	from	IISSite
	where	SiteName = @IISSiteName
			and Hostname = @IISSiteHost

	begin tran
		if @IISSiteID is null
			exec IISSiteAdd
				@IISSiteName = @IISSiteName
			,	@IISSiteHost = @IISSiteHost
			,	@IISSiteGroup = @IISSiteGroup 
			,	@ID = @IISSiteID out

		exec IISRestrictionAdd
			@TypeName = @TypeName
		,	@StartMoment = @StartMoment
		,	@StopMoment = @StopMoment
		,	@Rule = @Rule       
		,	@IISSiteID = @IISSiteID
		,	@ID = @ID out

	commit
end